<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CrexImage.ascx.cs" Inherits="RockWeb.Plugins.com_blueboxmoon.Crex.CrexImage" %>
<%@ Register Namespace="com.blueboxmoon.Crex.Web.UI.Controls" Assembly="com.blueboxmoon.Crex" TagPrefix="CX" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbError" runat="server" NotificationBoxType="Danger" />

        <asp:Panel ID="pnlContent" runat="server">
            <CX:TabPanel ID="tpContent" runat="server" CssClass="margin-b-md">
                <CX:Tab ID="tPreview" runat="server" Title="Preview" />
                <CX:Tab Id="tDebug" runat="server" Title="Debug" />
            </CX:TabPanel>
        </asp:Panel>

        <asp:Panel ID="pnlEditModal" runat="server" Visible="true">
            <Rock:ModalDialog ID="mdEdit" runat="server" OnSaveClick="mdEdit_SaveClick" Title="Image Configuration" ValidationGroup="EditSettings">
                <Content>
                    <asp:UpdatePanel ID="upnlEdit" runat="server">
                        <ContentTemplate>
                            <Rock:RockTextBox ID="tbBackgroundImageUrl" runat="server" Label="Background Image Url" Help="The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>" Required="false" ValidationGroup="EditSettings" />

                            <CX:FileTypeAndImageUploader ID="ftBackgroundImage" runat="server" Label="Background Image" Help="The image to be displayed in the background. Suggested size is 3840x2160." Required="false" ValidationGroup="EditSettings" ControlCssClass="col-md-6" ThumbnailWidth="160" ThumbnailHeight="90" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </Rock:ModalDialog>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
