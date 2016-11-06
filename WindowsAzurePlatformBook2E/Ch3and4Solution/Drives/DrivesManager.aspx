<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DrivesManager.aspx.cs" Inherits="SLWebService.silverlining.DrivesManger" %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="server">
    <div id="adminedit" style="font-family: Arial; font-size: xx-small">
        <fieldset>
            <!-- add H2 here and hide it with css since you can not put h2 inside a legend tag -->
            <h2 class="none">
                &nbsp;</h2>
            <legend>Drives
            </legend>
            
            <br />
          

                 <table style="width: 100%;">
                <tr>
                    <td style="width: 306px" valign="top">
                        &nbsp;
                        <asp:TreeView ID="TreeView1" runat="server" BorderColor="Black" 
                            BorderStyle="Solid" ImageSet="XPFileExplorer" NodeIndent="15" 
                            Width="304px" BorderWidth="1px" Height="748px" NodeWrap="True" 
                            Font-Size="X-Small">
                            <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                            <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" 
                                HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                            <ParentNodeStyle Font-Bold="False" />
                            <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" 
                                HorizontalPadding="0px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </td>
                    <td>
                       <table style="width:89%; height: 901px;">
                <tr>
                    <td style="width: 145px">
                        <b>Container Name</b></td>
                    <td>
                        <asp:TextBox ID="txtContainerName" runat="server" CssClass="textboxentry" 
                            ValidationGroup="drive" Width="180px" Font-Size="X-Small" Height="22px">inputfiles</asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ErrorMessage="Container Name is required" ValidationGroup="drive" 
                            ControlToValidate="txtContainerName" Display="Dynamic" 
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px">
                        <b>Page Blob Name</b></td>
                    <td>
                        <asp:TextBox ID="txtPageBlobName" runat="server" CssClass="textboxentry" 
                            ValidationGroup="drive" Width="180px" Font-Size="X-Small" Height="22px">test1.vhd</asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Page Blob Name is required" ValidationGroup="drive" 
                            ControlToValidate="txtPageBlobName" Display="Dynamic" 
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px">
                        <b>Storage Account Name</b></td>
                    <td>
                        <asp:TextBox ID="txtStorageAccountName" runat="server" CssClass="textboxentry" 
                            ValidationGroup="drive" Width="180px" Font-Size="X-Small" Height="22px">devstoreaccount1</asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtStorageAccountName" ErrorMessage="Required" 
                            ValidationGroup="drive" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px">
                        <b>Storage Account Key<br />
                        <br />
                        </b> <i>
                        (Not needed when using dev storage)</i></td>
                    <td>
                        <asp:TextBox ID="txtStorageAccountKey" runat="server" TextMode="Password" 
                            CssClass="textboxentry" ValidationGroup="drive" Width="180px" 
                            Font-Size="X-Small">Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==</asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px">
                        <b>Drive size in MB<br />
                        <br />
                        </b> <i>
                        (Only for new drives to be created)</i></td>
                    <td>
                        <asp:TextBox ID="txtDriveSize" runat="server" CssClass="textboxentry" 
                            ValidationGroup="drive" Width="180px" Font-Size="X-Small">25</asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ControlToValidate="txtDriveSize" ErrorMessage="Required" 
                            ValidationGroup="drive" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px">
                        <b>Cache size in MB</b></td>
                    <td>
                        <asp:TextBox ID="txtCacheSize" runat="server" CssClass="textboxentry" 
                            ValidationGroup="drive" Width="180px" Font-Size="X-Small">20</asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                            ControlToValidate="txtCacheSize" ErrorMessage="Required" 
                            ValidationGroup="drive" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px">
                        <b>Cache directory name</b></td>
                    <td>
                        <asp:TextBox ID="txtCacheDirName" runat="server" CssClass="textboxentry" 
                            ValidationGroup="drive" Width="180px" Font-Size="X-Small">silverliningcache</asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                            ControlToValidate="txtCacheDirName" ErrorMessage="Required" 
                            ValidationGroup="drive" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px">
                        &nbsp;</td>
                    <td>
                        <asp:Button ID="btnCreate" runat="server" onclick="btnCreate_Click" 
                            Text="Mount Drive" CssClass="standard-text" Font-Size="X-Small" />
                                                <br />
                                                <asp:CheckBox ID="cbCreateSnapshot" runat="server" 
                                                    Text="Create Snapshot before mount" />
                        <br />
                        <asp:CheckBox ID="cbCreateTestFile" runat="server" Text="Create a test file" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px; height: 4px;">
                        </td>
                    <td style="height: 4px">
                        <asp:Label ID="lblMsg" runat="server" CssClass="errorlabel"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 145px" valign="top">
                        <b>Mounted Drives</b></td>
                    <td valign="top">
           
                        &nbsp;&nbsp;&nbsp;
                        <table style="width:100%;">
                            <tr>
                                <td style="width: 495px" valign="top">
                                    <asp:ListBox ID="lstDrives" runat="server" Height="131px" Width="300px" 
                                        Font-Size="X-Small">
                                    </asp:ListBox>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnUnmount" runat="server" CssClass="standard-text" 
                                        onclick="btnUnmount_Click" Text="Unmount Drive" Width="116px" 
                                        Font-Size="X-Small" />
                                &nbsp;
                                    <asp:Button ID="btnViewFiles" runat="server" CssClass="standard-text" 
                                        onclick="btnViewFiles_Click" Text=" View Directories" Width="119px" 
                                        Font-Size="X-Small" />
                                    &nbsp;
                                    <asp:Button ID="btnSnapshot" runat="server" CssClass="standard-text" 
                                        onclick="btnSnapshot_Click" Text="Snapshot" Width="106px" 
                                        Font-Size="X-Small" />
                                    &nbsp;
                                    <asp:Button ID="btnDelete" runat="server" CssClass="standard-text" 
                                        onclick="btnDelete_Click" Text="Delete" Width="106px" 
                                        Font-Size="X-Small" />
                                    <br />
                                </td>
                            </tr>
                          
                            <tr>
                                <td style="width: 495px" valign="top">
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="205px" 
                                        Font-Size="X-Small" Height="23px" />
&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                          
                            <tr>
                                <td style="width: 495px" valign="top">
                                    <asp:Button ID="btnUploadFileToDrive" runat="server" CssClass="standard-text" 
                                        onclick="btnUploadFileToDrive_Click" Text="Upload File to Drive" 
                                        Width="139px" Font-Size="X-Small" Height="22px" />
                                </td>
                            </tr>
                          
                        </table>
                     <br />
                        
                     <br />
                     <br />
                
           
                    </td>
                </tr>
             
            </table>
                    </td>
                  
                </tr>
               
                
            </table>
            
       
            <br />
            <br />
           
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
    .textboxentry
    {}
    .standard-text
    {}
</style>
</asp:Content>

