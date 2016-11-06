<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="EventPoint" generation="1" functional="0" release="0" Id="6e9f2512-ff3c-4c40-add9-51e86147f074" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="EventPointGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="EventPoint_WebRole:HttpIn" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/EventPoint/EventPointGroup/LB:EventPoint_WebRole:HttpIn" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="EventPoint.CriticalPersister:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint.CriticalPersister:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="EventPoint.CriticalPersister:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint.CriticalPersister:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="EventPoint.CriticalPersister:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint.CriticalPersister:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="EventPoint.CriticalPersister:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint.CriticalPersister:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="EventPoint.CriticalPersisterInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint.CriticalPersisterInstances" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WebRole:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WebRole:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WebRole:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WebRole:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WebRole:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WebRole:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WebRole:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WebRole:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WebRole:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WebRole:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WebRoleInstances" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WorkerRole:?IsSimulationEnvironment?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WorkerRole:?IsSimulationEnvironment?" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WorkerRole:?RoleHostDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WorkerRole:?RoleHostDebugger?" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WorkerRole:?StartupTaskDebugger?" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WorkerRole:?StartupTaskDebugger?" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WorkerRole:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WorkerRole:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WorkerRole:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WorkerRole:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="EventPoint_WorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/EventPoint/EventPointGroup/MapEventPoint_WorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:EventPoint_WebRole:HttpIn">
          <toPorts>
            <inPortMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRole/HttpIn" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:EventPoint.CriticalPersister:SBEndpoint">
          <toPorts>
            <inPortMoniker name="/EventPoint/EventPointGroup/EventPoint.CriticalPersister/SBEndpoint" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapEventPoint.CriticalPersister:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint.CriticalPersister/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapEventPoint.CriticalPersister:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint.CriticalPersister/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapEventPoint.CriticalPersister:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint.CriticalPersister/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapEventPoint.CriticalPersister:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint.CriticalPersister/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapEventPoint.CriticalPersisterInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/EventPoint/EventPointGroup/EventPoint.CriticalPersisterInstances" />
          </setting>
        </map>
        <map name="MapEventPoint_WebRole:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRole/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapEventPoint_WebRole:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRole/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapEventPoint_WebRole:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRole/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapEventPoint_WebRole:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRole/DataConnectionString" />
          </setting>
        </map>
        <map name="MapEventPoint_WebRole:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRole/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapEventPoint_WebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRoleInstances" />
          </setting>
        </map>
        <map name="MapEventPoint_WorkerRole:?IsSimulationEnvironment?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WorkerRole/?IsSimulationEnvironment?" />
          </setting>
        </map>
        <map name="MapEventPoint_WorkerRole:?RoleHostDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WorkerRole/?RoleHostDebugger?" />
          </setting>
        </map>
        <map name="MapEventPoint_WorkerRole:?StartupTaskDebugger?" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WorkerRole/?StartupTaskDebugger?" />
          </setting>
        </map>
        <map name="MapEventPoint_WorkerRole:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WorkerRole/DataConnectionString" />
          </setting>
        </map>
        <map name="MapEventPoint_WorkerRole:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/EventPoint/EventPointGroup/EventPoint_WorkerRole/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapEventPoint_WorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/EventPoint/EventPointGroup/EventPoint_WorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="EventPoint.CriticalPersister" generation="1" functional="0" release="0" software="C:\Projects\EventPoint\EventPoint\bin\Release\EventPoint.csx\roles\EventPoint.CriticalPersister" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="SBEndpoint" protocol="http" portRanges="80" />
              <outPort name="EventPoint.CriticalPersister:SBEndpoint" protocol="http">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/EventPoint/EventPointGroup/SW:EventPoint.CriticalPersister:SBEndpoint" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;EventPoint.CriticalPersister&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;EventPoint.CriticalPersister&quot;&gt;&lt;e name=&quot;SBEndpoint&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EventPoint_WebRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EventPoint_WorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/EventPoint/EventPointGroup/EventPoint.CriticalPersisterInstances" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="EventPoint_WebRole" generation="1" functional="0" release="0" software="C:\Projects\EventPoint\EventPoint\bin\Release\EventPoint.csx\roles\EventPoint_WebRole" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="HttpIn" protocol="http" portRanges="8080" />
              <outPort name="EventPoint.CriticalPersister:SBEndpoint" protocol="http">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/EventPoint/EventPointGroup/SW:EventPoint.CriticalPersister:SBEndpoint" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;EventPoint_WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;EventPoint.CriticalPersister&quot;&gt;&lt;e name=&quot;SBEndpoint&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EventPoint_WebRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EventPoint_WorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRoleInstances" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="EventPoint_WorkerRole" generation="1" functional="0" release="0" software="C:\Projects\EventPoint\EventPoint\bin\Release\EventPoint.csx\roles\EventPoint_WorkerRole" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <outPort name="EventPoint.CriticalPersister:SBEndpoint" protocol="http">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/EventPoint/EventPointGroup/SW:EventPoint.CriticalPersister:SBEndpoint" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="?IsSimulationEnvironment?" defaultValue="" />
              <aCS name="?RoleHostDebugger?" defaultValue="" />
              <aCS name="?StartupTaskDebugger?" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;EventPoint_WorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;EventPoint.CriticalPersister&quot;&gt;&lt;e name=&quot;SBEndpoint&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EventPoint_WebRole&quot;&gt;&lt;e name=&quot;HttpIn&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;EventPoint_WorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/EventPoint/EventPointGroup/EventPoint_WorkerRoleInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="EventPoint.CriticalPersisterInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="EventPoint_WebRoleInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="EventPoint_WorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="44073e59-4b1d-404d-954b-de8afb3c05cf" ref="Microsoft.RedDog.Contract\ServiceContract\EventPointContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="564578b7-6b0c-49bf-80af-285ac04c4f0f" ref="Microsoft.RedDog.Contract\Interface\EventPoint_WebRole:HttpIn@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/EventPoint/EventPointGroup/EventPoint_WebRole:HttpIn" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>