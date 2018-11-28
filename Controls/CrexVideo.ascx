<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CrexVideo.ascx.cs" Inherits="RockWeb.Plugins.com_blueboxmoon.Crex.CrexVideo" %>
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
    </ContentTemplate>
</asp:UpdatePanel>
