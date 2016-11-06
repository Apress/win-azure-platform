<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manager.aspx.cs" Inherits="EventPoint_WebRole.Manager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EventPoint Messages</title>
    <style type="text/css">
        body { font-family: Tahoma, Verdana; font-size: 10pt; }
        h1 { font-size: 16pt; color: #666}
        h2 { font-size: 12pt; margin-bottom:0.4em}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Event Message Listing from Azure Table Storage</h1>
        <i><%=strConnectionString%></i>
        <asp:GridView AutoGenerateColumns="False"
            ID="Messages" runat="server" CellPadding="8" EnableModelValidation="True" 
            ForeColor="#333333" GridLines="None" Width="90%"
            EmptyDataText="No messages available" OnRowDeleted="Messages_RowDeleted" 
            DataSourceID="EventMessageDataSource"
            DataKeyNames="PartitionKey,RowKey" 
            onselectedindexchanged="Messages_SelectedIndexChanged">
            <AlternatingRowStyle BackColor="White" HorizontalAlign="Right" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" />
                <asp:BoundField DataField="Priority" HeaderText="Priority" 
                    HeaderStyle-HorizontalAlign = "Right"
                    SortExpression="Priority" />
                <asp:BoundField DataField="Originator" HeaderText="Originator" 
                    HeaderStyle-HorizontalAlign = "Right"
                    SortExpression="Originator" />
                <asp:BoundField DataField="Title" HeaderText="Title" 
                    HeaderStyle-HorizontalAlign = "Right"
                    SortExpression="Title" />
                <asp:BoundField DataField="Message" HeaderText="Message"
                    HeaderStyle-HorizontalAlign = "Right" 
                    SortExpression="Message">
                </asp:BoundField>
                <asp:BoundField DataField="Link" HeaderText="Link"
                    HeaderStyle-HorizontalAlign = "Right" 
                    SortExpression="Link" >
                </asp:BoundField>
                <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" 
                    HeaderStyle-HorizontalAlign = "Right"
                    SortExpression="Timestamp" >
                </asp:BoundField>
                <asp:BoundField DataField="PartitionKey" HeaderText="PartitionKey"
                    HeaderStyle-HorizontalAlign = "Right" 
                    SortExpression="PartitionKey" />
                <asp:BoundField DataField="RowKey" HeaderText="RowKey" 
                    HeaderStyle-HorizontalAlign = "Right"
                    SortExpression="RowKey" />
            </Columns>
            <EditRowStyle BackColor="#7C6F57" HorizontalAlign="Right" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"  HorizontalAlign="Right"/>
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Right" />
            <RowStyle BackColor="#E3EAEB" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            
        </asp:GridView>
        <asp:ObjectDataSource 
            ID="EventMessageDataSource" 
            runat="server" 
            SelectMethod="GetEventMessageEntries" 
            TypeName="EventPoint.Data.EventMessageDataSource" 
            DeleteMethod="DeleteEventMessage" 
            DataObjectTypeName="EventPoint.Data.EventMessage">
            <DeleteParameters>
                <asp:Parameter Name="PartitionKey" Type="String" />
                <asp:Parameter Name="RowKey" Type="String" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>

