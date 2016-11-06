<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="DemResWorker" generation="1" functional="0" release="0" Id="b97789f2-c5a3-45fe-a285-f15c6d8a9aa0" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="DemResWorkerGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="DemResWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRoleInstances" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:DiagnosticsConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:DiagnosticsConnectionString" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:UserName" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:UserName" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:Password" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:Password" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:LogEndpoint" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:LogEndpoint" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:DemResEndpoint" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:DemResEndpoint" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:SQLAzure-ServerName" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:SQLAzure-ServerName" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:SQLAzure-UserName" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:SQLAzure-UserName" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:SQLAzure-Password" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:SQLAzure-Password" />
          </maps>
        </aCS>
        <aCS name="DemResWorkerRole:SQLAzure-DatabaseName" defaultValue="">
          <maps>
            <mapMoniker name="/DemResWorker/DemResWorkerGroup/MapDemResWorkerRole:SQLAzure-DatabaseName" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapDemResWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRoleInstances" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:DiagnosticsConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/DiagnosticsConnectionString" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:UserName" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/UserName" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:Password" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/Password" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:LogEndpoint" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/LogEndpoint" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:DemResEndpoint" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/DemResEndpoint" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:SQLAzure-ServerName" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/SQLAzure-ServerName" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:SQLAzure-UserName" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/SQLAzure-UserName" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:SQLAzure-Password" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/SQLAzure-Password" />
          </setting>
        </map>
        <map name="MapDemResWorkerRole:SQLAzure-DatabaseName" kind="Identity">
          <setting>
            <aCSMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRole/SQLAzure-DatabaseName" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="DemResWorkerRole" generation="1" functional="0" release="0" software="C:\Tejaswi\Publications\Azure Services\Content\Code\9\Ch8Solution\DemResWorker\obj\Debug\DemResWorkerRole\" entryPoint="base\x64\WaWorkerHost.exe" parameters="" memIndex="1792" hostingEnvironment="consolerolefulltrust" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="DiagnosticsConnectionString" defaultValue="" />
              <aCS name="UserName" defaultValue="" />
              <aCS name="Password" defaultValue="" />
              <aCS name="LogEndpoint" defaultValue="" />
              <aCS name="DemResEndpoint" defaultValue="" />
              <aCS name="SQLAzure-ServerName" defaultValue="" />
              <aCS name="SQLAzure-UserName" defaultValue="" />
              <aCS name="SQLAzure-Password" defaultValue="" />
              <aCS name="SQLAzure-DatabaseName" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;DemResWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;DemResWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/DemResWorker/DemResWorkerGroup/DemResWorkerRoleInstances" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyID name="DemResWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>