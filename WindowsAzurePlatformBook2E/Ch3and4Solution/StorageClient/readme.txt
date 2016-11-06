StorageClient
--------------

The StorageClient sample implements a library that programmers can use to access the following three
storage services:
	- blob storage service
	- queue service
	- table storage service

The sample consists of:

(i) The library itself.

(ii) A simple sample program that exercises the library. This program is a
standard console application and NOT a service by itself. The sample is thus not run inside 
a development fabric. But it shows the API of how to access storage services. Storage service
enpoints can be configured for Development Storage services or for storage services running in the Windows Azure fabric.


How to run
-----------
	
To run the sample with Visual Studio 2008:

Preparation:

	- Install .NET Framework 3.5 SP1 if you haven't done so already.
	- Open the Microsoft Windows Azure Command Prompt (Start | Programs | 
	  Windows Azure SDK (March 2009 CTP) | Windows Azure SDK Command Prompt).
	- Go to the directory where you unpacked the samples. 
	- Run development storage by calling the 'rundevstore.cmd' command script.
	- Make sure that the Table Storage service is running in development storage. Also make sure that 
	  the database called 'ServiceHostingSDKSamples' is currently selected for the 
	  table service; choose Development Storage -> Tools -> Table service properties -> 
          ServiceHostingSDKSamples.

Run within Visual Studio 2008:

	- Open the Windows Azure SDK Command Prompt (Start | Programs | 
	  Windows Azure SDK (March 2009 CTP) | Windows Azure SDK Command Prompt). 
	- Go to the directory where you unpacked the samples. 
	- Change to the StorageClient directory.
	- Type StorageClient.sln at the command prompt.
	- Make sure StorageClientAPISample is selected as the startup project (if it is not set up as 
	  the startup project, right-click the StorageClientAPISample project in the solution explorer and select 
	  "Set as StartUp Project"
	- start the sample directly from within VS 2008

Run from command prompt:
	- Open the Windows Azure SDK Command Prompt (Start | Programs | 
	  Windows Azure SDK (March 2009 CTP) | Windows Azure SDK Command Prompt).
	- Go to the directory where you unpacked the samples. 
	- Change to the StorageClient directory. 
	- Call the buildme.cmd script in the StorageClient directory.
	- Change directory to StorageClientAPISample\bin\Debug.
	- Run StorageClientAPISample.exe.

Please refer to the Windows Azure SDK documentation if you need help starting Development Storage.

To configure development storage to run against a local instance of SQL Server, rather than against 
SQL Express, call DSInit with the /sqlInstance parameter, passing in the name of the target SQL Server 
instance. Use the name of the SQL Server instance without the server qualifier (e.g., MSSQL instead 
of .\MSSQL) to refer to a named instance. Use "." to denote an unnamed or default 
instance of SQL Server. You can call dsInit /sqlInstance at any time to configure development storage 
to point to a different instance of SQL Server.


Configuring service endpoints
-----------------------------

Storage endpoints for running the API samples can be configured in the App.config file. 

Here is an example of the configuration:

  <appSettings>

    <add key = "AccountName" value="devstoreaccount1"/>
    <add key = "AccountSharedKey" value="Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="/>   
    
    <!-- Change this to point to the base URIs of the storage services against which this sample is run -->
    <!-- When using production-style URIs within this sample, make sure that the HTTP  endpoints do not contain 
         the account name, as the library code adds the account name internally and there is another configuration
         setting for the account name. An example of a valid production-style URI for this sample's 
         configuration file is:
            <add key="StorageEndpoint" value="http://blob.core.windows.net/"/>
         Note that it is NOT http://***accountname***.blob.core.windows.net/. 
    -->
         
    <add key="BlobStorageEndpoint" value="http://127.0.0.1:10000"/>

    <add key="QueueStorageEndpoint" value="http://127.0.0.1:10001"/>

    <add key="TableStorageEndpoint" value="http://127.0.0.1:10002"/>
    
    <!-- For production servers, set this to false, otherwise to true.
         For example, it must be true for and endpoint such as http://127.0.0.1:10000 and false
         for and endpoint such as http://blob.core.windows.net.
         The implementation of the storage client library
         derives this setting automatically.
         Please set this value only if you are sure the derived value is wrong. 
    -->
    <!--<add key="UsePathStyleUris" value="true" />-->


    <!-- Change this if you want to write to a different container to avoid clashing with others using this
    sample against the same instance of the storage service -->
    <add key="ContainerName" value="storagesamplecontainer"/>
    
  </appSettings>



When running the API sample, you see an output similar to the following:


Creating blob hello.txt
Creating blob goodbye.txt
Getting hello.txt and goodbye.txt
hello.txt: Hello world Metadata = m1 = v1; m2 = v2
goodbye.txt Goodbye world
Uploading a large blob
Successfully uploaded blob LargeBlob.txt at time 6/2/2008 10:09:43 PM
Downloading large blob to file LargeBlob.txt
Downloaded blob LargeBlob.txt to file <filepath>\LargeBlob.txt
hello.txt not refreshed
goodbye.txt refreshed Goodbye again world
hello.txt updated Hello again world Metadata = m1 = v1; m2 = v2
goodbye.txt not updated because it has been changed
Enumerating blobs
http://<IP:port>/devstoreaccount1/testcontainer/LargeBlob.txt
http://<IP:port>/devstoreaccount1/testcontainer/goodbye.txt
http://<IP:port>/devstoreaccount1/testcontainer/hello.txt

...
