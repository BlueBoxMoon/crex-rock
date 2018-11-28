<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CrexContentChannelView.ascx.cs" Inherits="RockWeb.Plugins.com_blueboxmoon.Crex.CrexContentChannelView" %>
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
            <Rock:ModalDialog ID="mdEdit" runat="server" OnSaveClick="mdEdit_SaveClick" Title="Channel Configuration" ValidationGroup="EditSettings">
                <Content>
                    <asp:UpdatePanel ID="upnlEdit" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <Rock:RockDropDownList ID="ddlChannel" runat="server" Label="Content Channel" Help="The content channel whose items will be displayed." Required="true" ValidationGroup="EditSettings" DataValueField="Guid" DataTextField="Name" OnSelectedIndexChanged="ddlChannel_SelectedIndexChanged" CausesValidation="false" AutoPostBack="true" />
                                </div>

                                <div class="col-md-6">
                                    <Rock:RockCheckBoxList ID="cblStatus" runat="server" Label="Status" RepeatDirection="Horizontal"
                                        Help="Include items with the following status." />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <Rock:RockDropDownList ID="ddlLayout" runat="server" Label="Layout" Help="The screen layout to use on the TV." Required="true" ValidationGroup="EditSettings">
                                        <asp:ListItem />
                                        <asp:ListItem Value="Menu" Text="Menu" />
                                        <asp:ListItem Value="PosterList" Text="Poster List" />
                                    </Rock:RockDropDownList>
                                </div>

                                <div class="col-md-6">
                                    <Rock:PagePicker ID="ppDetailPage" runat="server" Label="Detail Page" Required="false" ValidationGroup="EditSettings" />
                                </div>
                            </div>

                            <Rock:RockTextBox ID="tbBackgroundImageUrl" runat="server" Label="Background Image Url" Help="The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>" Required="false" ValidationGroup="EditSettings" />

                            <CX:FileTypeAndImageUploader ID="ftBackgroundImage" runat="server" Label="Background Image" Help="The image to be displayed in the background. Suggested size is 3840x2160." Required="false" ValidationGroup="EditSettings" ControlCssClass="col-md-6" ThumbnailWidth="160" ThumbnailHeight="90" />

                            <div class="form-group">
                                <label class="control-label">
                                    Filter
                                </label>
                                <asp:HiddenField ID="hfDataFilterId" runat="server" />
                                <asp:PlaceHolder ID="phFilters" runat="server"></asp:PlaceHolder>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <Rock:KeyValueList ID="kvlOrder" runat="server" Label="Order Items By" KeyPrompt="Field" ValuePrompt="Direction"
                                        Help="The field value and direction that items should be ordered by." />
                                </div>

                                <div class="col-md-6">
                                </div>
                            </div>

                            <Rock:CodeEditor ID="ceTemplate" runat="server" Label="Template" Help="The lava template to use when generating the items." Required="true" ValidationGroup="EditSettings" EditorMode="Lava" EditorTheme="Rock" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </Rock:ModalDialog>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
