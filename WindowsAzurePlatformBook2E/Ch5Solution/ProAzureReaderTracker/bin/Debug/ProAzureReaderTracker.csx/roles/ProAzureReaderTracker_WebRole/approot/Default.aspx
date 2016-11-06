<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProAzureReaderTracker_WebRole._Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Pro Azure Services Platform</title>
    <style type="text/css">
        .style1
        {
            font-weight: bold;
            text-decoration: underline;
        }
         body { font-family: Verdana; font-size: 9pt; }
        h1 { font-size: 12pt; color: #555555; }
        li { list-style-type: none; }
        form { background-color: #eeeeff; width: 50%; margin: 0 auto; padding: 1em; border: solid 1px #333333; }
        .style2
        {
        }
        .style3
        {
            font-weight: bold;
            text-decoration: underline;
            width: 303px;
        }
        .style4
        {
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     <div>
    
         <asp:ScriptManager ID="ScriptManager1" runat="server">
         </asp:ScriptManager>
    
        <table style="border: thin solid #800000; width:100%;">
            <tr>
                <td bgcolor="Black">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Larger" 
                        ForeColor="Yellow" Text="Pro Azure Reader Tracker"></asp:Label>
                    </td>
              
            </tr>
        </table>
    </div>
    <div>
    
        <table style="border: thin none #800000; width:100%;">
            <tr>
                <td class="style1" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style1" colspan="2">
                    Please enter your details and click Submit</td>
            </tr>
            <tr>
                <td class="style3">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style4">
                    Your Name/Email</td>
                <td valign="top">
                    <asp:TextBox ID="txtName" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtName" ErrorMessage="required" InitialValue="*" 
                        ValidationGroup="t">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Country</td>
                <td valign="top">
                    <asp:TextBox ID="txtCountry" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="txtCountry" ErrorMessage="required" InitialValue="*" 
                        ValidationGroup="t">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    City</td>
                <td valign="top">
                    <asp:TextBox ID="txtCity" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="txtCity" ErrorMessage="required" InitialValue="*" 
                        ValidationGroup="t">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    State</td>
                <td>
                    <asp:TextBox ID="txtState" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ControlToValidate="txtState" ErrorMessage="required" InitialValue="*" 
                        ValidationGroup="t">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Zip</td>
                <td>
                    <asp:TextBox ID="txtZip" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                        ControlToValidate="txtZip" ErrorMessage="required" InitialValue="*" 
                        ValidationGroup="t">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Book purchase location</td>
                <td>
                    <asp:TextBox ID="txtPurchaseLocation" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="100" Width="228px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                        ControlToValidate="txtPurchaseLocation" ErrorMessage="required" 
                        InitialValue="*" ValidationGroup="t">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                                        Purchase Type</td>
                <td>
                    <asp:DropDownList ID="ddlPurchaseType" runat="server">
                        <asp:ListItem Selected="True">New</asp:ListItem>
                        <asp:ListItem>Used</asp:ListItem>
                        <asp:ListItem>Library</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style4">
           Purchase Date</td>
                <td valign="top">
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                        TargetControlID="txtPurchaseDate" >
                    </cc1:CalendarExtender>
                    <asp:TextBox ID="txtPurchaseDate" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                        ControlToValidate="txtPurchaseDate" ErrorMessage="required" 
                        InitialValue="*" ValidationGroup="t">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Your Url 
                             <br />
                                        (e.g. Facebook, LinkedIn, etc.)</td>
                <td valign="middle">
                    <asp:TextBox ID="txtUrl" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="500" Width="289px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Any feedback?</td>
                <td>
                    <asp:TextBox ID="txtFeedback" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="500" Height="85px" TextMode="MultiLine" 
                        Width="289px">Good Book :). But don&#39;t write again.</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style4">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" BorderStyle="Outset" Text="Submit" 
                        Width="95px" onclick="btnSubmit_Click" ValidationGroup="t" />
                </td>
            </tr>
            <tr>
                <td class="style4">
                    Filter Text</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style4">
                    <asp:TextBox ID="txtFilter" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    </td>
                <td>
&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td class="style2" colspan="2">
                    <asp:LinkButton ID="lbFilterByCity" runat="server" 
                        onclick="lbFilterByCity_Click">By City</asp:LinkButton>
&nbsp;&nbsp; <asp:LinkButton ID="lbFilterByState" runat="server" onclick="lbFilterByState_Click">By State</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton ID="lbFilterByCountry" runat="server" onclick="lbFilterByCountry_Click">By Country</asp:LinkButton>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:LinkButton ID="lbToday" runat="server" onclick="lbToday_Click">Today&#39;s Entries</asp:LinkButton>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:LinkButton ID="lbTop50" runat="server" onclick="lbTop50_Click">Top N (50)</asp:LinkButton>
                    </td>
            </tr>
            <tr>
                <td class="style4" colspan="2">
                    <asp:TextBox ID="txtPurchaseDateFilter" runat="server" BackColor="White" 
                        BorderStyle="Solid" MaxLength="20"></asp:TextBox>
                    &nbsp;
                    <asp:LinkButton ID="lbFilterByPurchaseDate" runat="server" 
                        onclick="lbFilterByPurchaseDate_Click">By Purchase Date</asp:LinkButton>
                    <cc1:CalendarExtender ID="txtPurchaseDateFilter_CalendarExtender" runat="server" 
                        TargetControlID="txtPurchaseDateFilter" >
                    </cc1:CalendarExtender>
                    </td>
            </tr>
        </table>
    
    </div>
    
     <div>
    
        <table style="border: thin solid #800000; width:100%;"">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblStatus" runat="server" Font-Bold="True" ForeColor="Red" />

                            <asp:DataList ID="dlReaders" runat="server" 
                                onitemcommand="dlReaders_ItemCommand">
                                <ItemTemplate>
                                    
                                    <table style="border: thin solid #008080; width:100%;">
                                        <tr>
                                            <td>
                                              <b>  <%# Eval("ReaderName")%> </b>from <%# Eval("Country")%>, <%# Eval("State")%>, <%# Eval("City")%>, <%# Eval("Zip")%></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Url: <%# Eval("ReaderUrl")%></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                purchased a <%# Eval("PurchaseType")%> book at <%# Eval("PurchaseLocation")%> on <%# Eval("PurchaseDate")%></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Has the following Feedback:<br />
                                                <%# Eval("Feedback")%><br />
                                                <br />
                                                <asp:TextBox ID="txtUrl" runat="server" Text='<%# Eval("ReaderUrl")%>'></asp:TextBox><br />

                                                <asp:Button ID="btnUpdateUrl" runat="server" Text="UpdateUrl" CommandArgument='<%# Eval("PartitionKey")%>' CommandName='<%# Eval("RowKey")%>'/><br />

                                            </td>
                                            
                                        </tr>
                                    </table>
                                    <br />
                                </ItemTemplate>
                            </asp:DataList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbFilterByCity" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbFilterByCountry" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbFilterByState" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbFilterByPurchaseDate" 
                                EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbToday" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lbTop50" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
              
            </tr>
            </table>
    
    </div>
     <p>
     &nbsp;</p>
    </form>
</body>
</html>
