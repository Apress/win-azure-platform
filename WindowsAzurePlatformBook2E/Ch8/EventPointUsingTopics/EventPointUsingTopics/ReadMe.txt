This code and information are provided "as is" without warranty of any kind, either expressed or implied.

Much thanks and credit to Brian Loegsen, Principal Architect Evangelst at Microsoft. This sample code is a rewritten version of his original EventPoint demonstration,
that uses Service Bus with WCF and NetEventRelayBindings. 
You can find that code and more useful information about Windows Azure at http://www.brianloesgen.com.

This sample code demonstrates how to use Service Bus Topics as a publish/subscribe mechanism. In addition to the standard SDK, it uses the Windows Azure Samples Push programming model,
which can be found at http://nuget.org/List/Packages/WindowsAzure.ServiceBus.Samples.PushProgrammingModel.

To run the application locally in the Azure emulator:
1. Run the database script in the EventPoint.Install folder to create the database tables.
2. Change the database configuration string "SqlConnectionString" in EventPoint.Common\app.config to point to a local SQL instance
3. Update "ServiceNamespace", "issuerName", and "IssuerKey" in EventPoint.Common\app.config to reflect your Service Bus Namespace
4. Right click the Client applications and choose Debug --> Start New Instance
5. Right click the Cloud Project and select Debug --> Start new instance
6. In the DataGenerator application, choose a number of messages to send to the queue in bulk, or single critical messages.
7. Critical messages should show up in the ConsoleApp and Monitor applications. The should also be stored inthe SQL database via CriticalPersister.
8. All messages should be sent to Azure Table storage, and displayed in the Web Role application

NOTE - if you want to use cloud storage instead of emulator, simple change the connection strings in the ServiceConfiguration.

To run the application in Windows Azure:
1. Run the database script in the EventPoint.Install folder against your SQL Azure instance to create the database tables.
2. Change the database configuration string "SqlConnectionString" in EventPoint.Common\app.config to point to your SQL Azure instance
3. Update "ServiceNamespace", "issureName", and "IssuerKey" in EventPoint.Common\app.config to reflect your Service Bus Namespace
4. Update the connection strings in Service configuration to point to your Windows Azure storage configuration
5. Deploy Cloud project to your Windows Azure environment
6. Run the client applications
7. In the DataGenerator application, choose a number of messages to send to the queue in bulk, or single critical messages.
8. Critical messages should show up in the ConsoleApp and Monitor applications. The should also be stored inthe SQL database via CriticalPersister.
9. All messages should be sent to Azure Table storage, and displayed in the Web Role application


Additionally, I strongly recommend downloading the latest version of the Windows Azure AppFabric Samples for additional samples. The latest release at the time of writing can be found at:

http://msdn.microsoft.com/en-us/library/ee706741

There are mnay useful samples on all aspects of the Service Bus.