<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableOperations.aspx.cs" Inherits="ProAzureReaderTracker_WebRole.TableOperations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            font-weight: bold;
            text-decoration: underline;
        }
        .style2
        {
            width: 107px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table style="border: thin solid #800080; width: 100%;">
            <tr>
                <td class="style1" colspan="2" style="border: thin solid #800080" valign="top">
                    Table Operations</td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Button ID="btnCreateTable" runat="server" BorderStyle="Solid" 
                        onclick="btnCreateTable_Click" Text="Create Table" Width="89px" />
                </td>
                <td>
                    <asp:TextBox ID="txtCreateTable" runat="server" BorderStyle="Solid" 
                        Width="180px">ProAzureReader</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td>
     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    List of Tables</td>
                <td valign="top">
                    <asp:ListBox ID="lbListTables" runat="server" AutoPostBack="True" 
                        BackColor="#CC99FF" Height="173px" 
                        onselectedindexchanged="ListBox1_SelectedIndexChanged" Width="183px">
                    </asp:ListBox>
                </td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    <asp:Button ID="btnDeleteTable" runat="server" BorderStyle="Solid" 
                        onclick="btnDeleteTable_Click" Text="Delete Table" Width="89px" />
                </td>
                <td>
                    <asp:TextBox ID="txtDeleteTable" runat="server" BorderStyle="Solid" 
                        Width="179px">ProAzureReader</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2" valign="top">
                    &nbsp;</td>
                <td>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
