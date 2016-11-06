using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Samples;

namespace AzureSample
{

    public class ServiceBusLogger
    {
        public ILogChannel Channel { get; set; }

        public ServiceBusLogger(string endPoint, string issuerName, string issuerKey)
        {

            Uri uri = new Uri(endPoint);

            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);

            ChannelFactory<ILogChannel> channelFactory = new ChannelFactory<ILogChannel>();
            channelFactory.Endpoint.Address = new EndpointAddress(uri);
            channelFactory.Endpoint.Binding = new NetTcpRelayBinding();
            channelFactory.Endpoint.Contract.ContractType = typeof(ILogChannel);
            channelFactory.Endpoint.Behaviors.Add(sharedSecretServiceBusCredential);
            this.Channel = channelFactory.CreateChannel();
        }
    }

    public class CalculatorClient
    {
        public ICalculator Channel { get; set; }

        public CalculatorClient(string endPoint, string issuerName, string issuerKey)
        {

            Uri uri = new
            Uri(endPoint);
            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey);
            ChannelFactory<ICalculatorChannel> channelFactory = new
            ChannelFactory<ICalculatorChannel>();
            channelFactory.Endpoint.Address = new
            EndpointAddress(uri);
            channelFactory.Endpoint.Binding = new
            NetTcpRelayBinding();
            channelFactory.Endpoint.Contract.ContractType = typeof(ICalculatorChannel);
            channelFactory.Endpoint.Behaviors.Add(sharedSecretServiceBusCredential);
            this.Channel = channelFactory.CreateChannel();


        }


    }



}

