using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Threading;
using Microsoft.WindowsAzure;

namespace ProAzureCommonLib
{
    public class QueueListener
    {
        private AutoResetEvent _evStarted;
        private AutoResetEvent _evStopped;
        private AutoResetEvent _evQuit;
        private bool _run;
        private Thread _receiveThread;
        private int _internalPollInterval;
        private int _pollInterval = DefaultPollInterval;
        private CloudQueue _queue;

        

        /// <summary>
        /// The default time interval between polling the queue for messages. 
        /// Polling is only enabled if the user has called StartReceiving().
        /// </summary>
        public static readonly int DefaultPollInterval = 5000;

        public QueueListener(CloudQueue queue)
        {
            this._queue = queue;
        }

        public event MessageReceivedEventHandler MessageReceived;

        // // Summary:
        ////     Initializes a new instance of the Microsoft.WindowsAzure.StorageClient.CloudQueue
        ////     class with the given relative address and credentials.
        ////
        //// Parameters:
        ////   address:
        ////     The relative address.
        ////
        ////   credentials:
        ////     The storage account credentials.
        //public QueueListener(string address, StorageCredentials credentials) : base(address, credentials) { }

        // automatic receiving of messages
        public int PollInterval
        {
            get
            {
                return _pollInterval;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The send threshold must be a positive value.");
                }
                if (_run)
                {
                    throw new ArgumentException("You cannot set the poll interval while the receive thread is running");
                }
                _pollInterval = value;
            }
        }

        public CloudQueue Queue
        {
            get { return _queue; }
            set { _queue = value; }
        }

        private void PeriodicReceive()
        {
            CloudQueueMessage msg;

            _evStarted.Set();
            _internalPollInterval = PollInterval;
            while (!_evQuit.WaitOne(_internalPollInterval, false))
            {
                // time is up, so we get the message and continue
                msg = GetMessage();
                if (msg != null)
                {
                    MessageReceived(this, new MessageReceivedEventArgs(msg));
                    // continue receiving fast until we get no message
                    _internalPollInterval = 10;
                }
                else
                {
                    // we got no message, so we can fall back to the normal speed
                    _internalPollInterval = PollInterval;
                }
            }
            _evStopped.Set();
        }

        public bool StartReceiving()
        {
            lock (this)
            {
                if (_run)
                {
                    return true;
                }
                _run = true;
            }
            if (_evStarted == null)
            {
                _evStarted = new AutoResetEvent(false);
            }
            if (_evStopped == null)
            {
                _evStopped = new AutoResetEvent(false);
            }
            if (_evQuit == null)
            {
                _evQuit = new AutoResetEvent(false);
            }
            _receiveThread = new Thread(new ThreadStart(this.PeriodicReceive));
            _receiveThread.Start();
            if (!_evStarted.WaitOne(10000, false))
            {
                _receiveThread.Abort();
                CloseEvents();
                _run = false;
                return false;
            }
            return true;
        }

        public void StopReceiving()
        {
            _evQuit.Set();
            if (!_evStopped.WaitOne(10000, false))
            {
                _receiveThread.Abort();
            }
            CloseEvents();
            _run = false;
        }

        private void CloseEvents()
        {
            if (_evStarted != null)
            {
                _evStarted.Close();
            }
            if (_evStopped != null)
            {
                _evStopped.Close();
            }
            if (_evQuit != null)
            {
                _evQuit.Close();
            }
        }

        public CloudQueueMessage GetMessage()
        {
            return _queue.GetMessage();
            //IEnumerable<CloudQueueMessage> result = GetMessages(1);
            //if (result == null || result.Count() == 0)
            //{
            //    return null;
            //}
            //return result.First();
        }        
    }
}
