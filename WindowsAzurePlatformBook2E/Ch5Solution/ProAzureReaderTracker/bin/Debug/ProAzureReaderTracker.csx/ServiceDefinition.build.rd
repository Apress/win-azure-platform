<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ProAzureReaderTracker" generation="1" functional="0" release="0" Id="08708769-be73-4abf-bc50-c2a54ced37dd" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ProAzureReaderTrackerGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ProAzureReaderTracker_WebRole:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/LB:ProAzureReaderTracker_WebRole:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ProAzureReaderTracker_WebRole:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/MapProAzureReaderTracker_WebRole:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="ProAzureReaderTracker_WebRole:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/MapProAzureReaderTracker_WebRole:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="ProAzureReaderTracker_WebRole:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/MapProAzureReaderTracker_WebRole:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="ProAzureReaderTracker_WebRole:StorageAccountConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/MapProAzureReaderTracker_WebRole:StorageAccountConnectionString" />
          </maps>
        </aCS>
        <aCS name="ProAzureReaderTracker_WebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/MapProAzureReaderTracker_WebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ProAzureReaderTracker_WebRole:HttpIn">
          <toPorts>
            <inPortMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRole/HttpIn" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapProAzureReaderTracker_WebRole:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRole/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapProAzureReaderTracker_WebRole:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRole/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapProAzureReaderTracker_WebRole:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRole/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapProAzureReaderTracker_WebRole:StorageAccountConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRole/StorageAccountConnectionString" />
          </setting>
        </map>
        <map name="MapProAzureReaderTracker_WebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ProAzureReaderTracker_WebRole" generation="1" functional="0" release="0" software="C:\Tejaswi\Publications\Windows Azure Patterns\Code\Chapters2E\Ch5Solution\ProAzureReaderTracker\bin\Debug\ProAzureReaderTracker.csx\roles\ProAzureReaderTracker_WebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="768" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" portRanges="8080" />
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="StorageAccountConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ProAzureReaderTracker_WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ProAzureReaderTracker_WebRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRoleInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="ProAzureReaderTracker_WebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e2469d26-8bfd-4484-bfed-d2d6f79ef40a" ref="Microsoft.RedDog.Contract\ServiceContract\ProAzureReaderTrackerContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="85c36e1b-773a-47f6-b34d-66afca95514c" ref="Microsoft.RedDog.Contract\Interface\ProAzureReaderTracker_WebRole:HttpIn@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/ProAzureReaderTracker/ProAzureReaderTrackerGroup/ProAzureReaderTracker_WebRole:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>