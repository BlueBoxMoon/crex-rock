<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CrexContentChannelItemChildren.ascx.cs" Inherits="RockWeb.Plugins.com_blueboxmoon.Crex.CrexContentChannelItemChildren" %>
<%@ Register Namespace="com.blueboxmoon.Crex.Web.UI.Controls" Assembly="com.blueboxmoon.Crex" TagPrefix="CX" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlContent" runat="server">
            <Rock:NotificationBox ID="nbError" runat="server" NotificationBoxType="Danger" />

            <CX:TabPanel ID="tpContent" runat="server" CssClass="margin-b-md">
                <CX:Tab ID="tPreview" runat="server" Title="Preview" />
                <CX:Tab Id="tDebug" runat="server" Title="Debug" />
            </CX:TabPanel>
        </asp:Panel>

        <asp:Panel ID="pnlEditModal" runat="server" Visible="true">
            <Rock:ModalDialog ID="mdEdit" runat="server" OnSaveClick="mdEdit_SaveClick" Title="Menu Configuration" ValidationGroup="EditSettings">
                <Content>
                    <asp:UpdatePanel ID="upnlEdit" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <Rock:RockDropDownList ID="ddlLayout" runat="server" Label="Layout" Help="The screen layout to use on the TV." Required="true" ValidationGroup="EditSettings">
                                        <asp:ListItem />
                                        <asp:ListItem Value="Menu" Text="Menu" />
                                        <asp:ListItem Value="PosterList" Text="Poster List" />
                                    </Rock:RockDropDownList>
                                </div>

                                <div class="col-md-6">
                                    <Rock:RockDropDownList ID="ddlSortItemsBy" runat="server" Label="Sort Items By" Help="How to sort the child items." Required="true" ValidationGroup="EditSettings">
                                        <asp:ListItem />
                                        <asp:ListItem Value="Ascending" Text="Ascending" />
                                        <asp:ListItem Value="Descending" Text="Descending" />
                                    </Rock:RockDropDownList>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <Rock:PagePicker ID="ppDetailPage" runat="server" Label="Detail Page" Required="false" ValidationGroup="EditSettings" />
                                </div>
                            </div>

                            <Rock:RockCheckBoxList ID="cblAllowedChannelTypes" runat="server" Label="Allowed Channel Types" Help="Limits the content channel types that can be viewed." Required="false" ValidationGroup="EditSettings" DataTextField="Name" DataValueField="Guid" RepeatDirection="Horizontal" />

                            <Rock:RockTextBox ID="tbBackgroundImageUrl" runat="server" Label="Background Image Url" Help="The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>" Required="false" ValidationGroup="EditSettings" />

                            <CX:FileTypeAndImageUploader ID="ftBackgroundImage" runat="server" Label="Background Image" Help="The image to be displayed in the background. Suggested size is 3840x2160." Required="false" ValidationGroup="EditSettings" ControlCssClass="col-md-6" ThumbnailWidth="160" ThumbnailHeight="90" />

                            <Rock:CodeEditor ID="ceTemplate" runat="server" Label="Template" Help="The lava template to use when generating the items." Required="true" ValidationGroup="EditSettings" EditorMode="Lava" EditorTheme="Rock" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </Rock:ModalDialog>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
