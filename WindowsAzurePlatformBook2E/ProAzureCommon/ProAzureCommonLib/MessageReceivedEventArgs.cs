using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace ProAzureCommonLib
{
    /// <summary>
    /// Queues in the storage client library expose a functionality for listening for incoming messages. 
    /// If a message is put into a queue, a corresponding event is issued and this delegate is called. This functionality
    /// is implemented internally in this library by periodically polling for incoming messages.
    /// </summary>
    /// <param name="sender">The queue that has received a new event.</param>
    /// <param name="e">The event argument containing the message.</param>
    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);

    /// <summary>
    /// The argument class for the MessageReceived event.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The message itself.
        /// </summary>
        private CloudQueueMessage _msg;

        /// <summary>
        /// Constructor for creating a message received argument.
        /// </summary>
        /// <param name="msg"></param>
        public MessageReceivedEventArgs(CloudQueueMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException("msg");
            }
            _msg = msg;
        }

        /// <summary>
        /// The message received by the queue.
        /// </summary>
        public CloudQueueMessage Message
        {
            get
            {
                return _msg;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _msg = value;
            }
        }
    }
}
