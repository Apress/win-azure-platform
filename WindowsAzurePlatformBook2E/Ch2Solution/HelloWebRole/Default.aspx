<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HelloAzureCloud_WebRole._Default" MasterPageFile="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Hello Windows Azure.</title>
    <style type="text/css">
        .style1
        {
            width: 210px;
        }
        .style2
        {
            width: 140px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
    
       
    
        <asp:Button ID="btnWhere" runat="server" onclick="btnWhere_Click" 
            Text="Get Machine Info" />
        <br />
        <br />
       
                <table 
    style="padding: inherit; margin: inherit; border: thin solid #000000; width: 100%;">
                    <tr>
                        <td class="style1">
                            Cloud Machine Name</td>
                        <td>
                            <asp:Label ID="lblCloudMachineName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            User Host Address</td>
                        <td>
                            <asp:Label ID="lblUserHostAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            RoleId</td>
                        <td>
                            <asp:Label ID="lblRoleId" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Current Log Level</td>
                        <td>
                            <asp:Label ID="lblCurrentLogLevel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Local Storage Root Path</td>
                        <td>
                            <asp:Label ID="lblLocalStoragePath" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            Throw Exceptions</td>
                        <td>
                            <asp:Label ID="lblThrowExceptions" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            Can Access System Directory?</td>
                        <td>
                            <asp:Label ID="lblCanAccessSystemDirectory" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Can Access Windows Directory?</td>
                        <td>
                            <asp:Label ID="lblCanAccessWindowsDirectory" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            Can Access Local Storage?</td>
                        <td>
                            <asp:Label ID="lblCanAccessLocalStorage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            Upgrade Domain</td>
                        <td>
                            <asp:Label ID="lblUpgradeDomain" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            Fault Domain</td>
                        <td>
                            <asp:Label ID="lblFaultDomain" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            Worker Role Communication</td>
                        <td>
                            IPAddress&nbsp;
                            <asp:Label ID="lblWRIp" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td>
                            Host Name
                            <asp:Label ID="lblWRHostName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td>
                            Endpoint Address
                            <asp:Label ID="lblWREndpointAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td>
                            Upgrade Domain
                            <asp:Label ID="lblWRUpgradeDomain" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td>
                            Fault Domain
                            <asp:Label ID="lblWRFaultDomain" runat="server"></asp:Label>
                        </td>
                    </tr>
                    
                </table>
           
    
    </div>
    &nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <br />
    <table style="border: thin solid #000000; width: 100%;">
        <tr>
            <td>
    Upload File to Local Storage</td>
            <td>
    <asp:FileUpload ID="fuCache" runat="server" />
                            <asp:Button ID="btnUpload" runat="server" onclick="btnUpload_Click" 
                                Text="Upload" Width="99px" />
                                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="top">
                File List in Local Storage</td>
            <td>
                <asp:ListBox ID="lstFiles" runat="server" Height="163px" 
        Width="441px">
    </asp:ListBox>
    
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
    <table style="border: thin solid #000000; width: 100%;">
        <tr>
            <td class="style2" valign="top">
                <asp:Label ID="lblExceptionsTxt" runat="server" ForeColor="#CC0000" 
                    Text="Exceptions" Visible="False" Font-Names="Calibri"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblException" runat="server" ForeColor="#CC0000" 
                    Font-Names="Calibri"></asp:Label>
                                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2" bgcolor="#669999">
                </td>
            <td bgcolor="#669999">
                </td>
            <td bgcolor="#669999">
                </td>
        </tr>
        <tr>
            <td valign="top" class="style2">
                <asp:Label ID="lblLogsTxt" runat="server" ForeColor="#000099" Text="Logs" 
                    Visible="False" Font-Names="Calibri"></asp:Label>
            </td>
            <td bgcolor="White">
                <asp:Label ID="lblLogging" runat="server" ForeColor="#000099" BackColor="White" 
                    Font-Bold="True" Font-Names="Calibri"></asp:Label>
    
            </td>
            <td bgcolor="White">
                &nbsp;</td>
        </tr>
    </table>
                                <br />
    
    </form>
</body>
</html>

