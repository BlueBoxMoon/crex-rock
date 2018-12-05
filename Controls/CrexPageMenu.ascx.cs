using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;

using com.blueboxmoon.Crex;
using com.blueboxmoon.Crex.Rest;
using Rock;
using Rock.Data;
using Rock.Attribute;
using Rock.Web.Cache;
using System.Web.UI;
using Rock.Model;

namespace RockWeb.Plugins.com_blueboxmoon.Crex
{
    [DisplayName( "Crex Page Menu" )]
    [Category( "Blue Box Moon > Crex" )]
    [Description( "Displays a menu of child pages." )]
    [TextField( "Layout", "The screen layout to use on the TV.", true, "Menu", "CustomSetting" )]
    [TextField( "Background Image", "The image to be displayed in the background. Suggested size is 3840x2160.", false, "", "CustomSetting" )]
    [TextField( "Background Image Url", "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>", false, "", "CustomSetting" )]
    [TextField( "Template", "The lava template to use when generating the menu items.", true, "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/PageMenu.lava' %}", "CustomSetting" )]
    [TextField( "Notification Channel", "The content channel to use for posting notifications on the menu (only applies to Menu layout).", false, "", "CustomSetting" )]
    [TextField( "Notification Template", "The lava template to use when generating the notification items.", true, "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/Notifications.lava' %}", "CustomSetting" )]
    [LavaCommandsField( "Enabled Lava Commands", "The lava commands to make available when parsing the template.", false, order: 0 )]
    public partial class CrexPageMenu : CrexBlockCustomSettings
    {
        #region Properties

        /// <summary>
        /// Gets the preview panel.
        /// </summary>
        /// <value>
        /// The preview panel.
        /// </value>
        protected override Panel PreviewPanel { get { return tPreview; } }

        /// <summary>
        /// Gets the debug panel.
        /// </summary>
        /// <value>
        /// The debug panel.
        /// </value>
        protected override Panel DebugPanel { get { return tDebug; } }

        #endregion

        #region Base Method Overrides

        /// <summary>
        /// Raises the <see cref="E:Init" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            BlockUpdated += CrexPageMenu_BlockUpdated;
        }

        /// <summary>
        /// Raises the <see cref="E:Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            nbError.Text = string.Empty;

            if ( !IsPostBack )
            {
                ShowPreview();
            }
        }

        /// <summary>
        /// Gets the data that identifies this Crex template and action data.
        /// </summary>
        /// <param name="isPreview">if set to <c>true</c> then this is for a preview.</param>
        /// <returns>A CrexAction object.</returns>
        public override CrexAction GetCrexAction( bool isPreview )
        {
            var mergeFields = GetCommonMergeFields();
            var layout = GetAttributeValue( "Layout" );
            var lavaTemplate = GetAttributeValue( "Template" );

            using ( var rockContext = new RockContext() )
            {
                var pageCache = PageCache.Read( BlockCache.PageId.Value, rockContext );

                mergeFields.AddOrReplace( "Pages", pageCache.GetPages( rockContext ) );

                //
                // Process the template to generate the JSON data.
                //
                var json = lavaTemplate.ResolveMergeFields( mergeFields, CurrentPerson, GetAttributeValue( "EnabledLavaCommands" ) );

                if ( layout == "Menu" )
                {
                    var menuData = new com.blueboxmoon.Crex.Rest.Menu
                    {
                        BackgroundImage = GetUrlSetFromAttributes( "BackgroundImage", "BackgroundImageUrl", mergeFields ),
                        Notifications = GetNotifications( rockContext, mergeFields )
                    };

                    menuData.Buttons = json.FromJsonOrNull<List<MenuButton>>();

                    return new CrexAction( layout, menuData );
                }
                else if ( layout == "PosterList" )
                {
                    var posterListData = new PosterList
                    {
                        BackgroundImage = GetUrlSetFromAttributes( "BackgroundImage", "BackgroundImageUrl", mergeFields ),
                        Title = PageCache.PageTitle
                    };

                    posterListData.Items = json.FromJsonOrNull<List<PosterListItem>>();

                    return new CrexAction( layout, posterListData );
                }
            }

            return new CrexAction();
        }

        /// <summary>
        /// Shows the settings.
        /// </summary>
        protected override void ShowSettings()
        {
            mdEdit.Show();

            var rockContext = new RockContext();
            ddlNotificationChannel.DataSource = new ContentChannelService( rockContext ).Queryable()
                .OrderBy( c => c.Name )
                .Select( c => new { c.Guid, c.Name } )
                .ToList();
            ddlNotificationChannel.DataBind();
            ddlNotificationChannel.Items.Insert( 0, new ListItem( "", "" ) );
            ddlNotificationChannel.SetValue( GetAttributeValue( "NotificationChannel" ) );

            ddlLayout.SetValue( GetAttributeValue( "Layout" ) );

            var binaryFile = new BinaryFileService( rockContext ).Get( GetAttributeValue( "BackgroundImage" ).AsGuid() );
            ftBackgroundImage.BinaryFileId = binaryFile != null ? ( int? ) binaryFile.Id : null;
            tbBackgroundImageUrl.Text = GetAttributeValue( "BackgroundImageUrl" );

            ceTemplate.Text = GetAttributeValue( "Template" );

            ceNotificationTemplate.Text = GetAttributeValue( "NotificationTemplate" );
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void ShowError( string message )
        {
            nbError.Text = message;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="mergeFields">The merge fields.</param>
        /// <returns></returns>
        private List<com.blueboxmoon.Crex.Rest.Notification> GetNotifications( RockContext rockContext, Dictionary<string, object> mergeFields )
        {
            var contentChannelGuid = GetAttributeValue( "NotificationChannel" ).AsGuidOrNull();

            if ( !contentChannelGuid.HasValue )
            {
                return null;
            }

            var contentChannel = new Rock.Model.ContentChannelService( rockContext ).Get( contentChannelGuid.Value );
            if ( contentChannel == null )
            {
                return null;
            }

            //
            // Filter only items that are approved and should already be displayed.
            //
            var now = RockDateTime.Now;
            var items = contentChannel.Items.AsQueryable()
                .Where( i => i.StartDateTime < now );

            //
            // Filter any non-approved items.
            //
            if ( contentChannel.RequiresApproval )
            {
                items = items.Where( i => i.ApprovedDateTime.HasValue );
            }

            //
            // Filter any expired items out.
            //
            if ( contentChannel.ContentChannelType.DateRangeType == Rock.Model.ContentChannelDateType.DateRange )
            {
                items = items.Where( i => !i.ExpireDateTime.HasValue || i.ExpireDateTime > now );
            }

            items = items.OrderBy( i => i.StartDateTime );

            mergeFields.AddOrReplace( "Items", items.ToList() );

            var json = GetAttributeValue( "NotificationTemplate" ).ResolveMergeFields( mergeFields, CurrentPerson );

            return json.FromJsonOrNull<List<com.blueboxmoon.Crex.Rest.Notification>>();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the SaveClick event of the mdEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mdEdit_SaveClick( object sender, EventArgs e )
        {
            if ( !Page.IsValid )
            {
                return;
            }

            var rockContext = new RockContext();

            //
            // Delete the old file and persist the new file - if they are different.
            //
            var binaryFileService = new BinaryFileService( rockContext );
            var oldBinaryFileGuid = GetAttributeValue( "BackgroundImage" ).AsGuidOrNull();
            var newBinaryFile = binaryFileService.Get( ftBackgroundImage.BinaryFileId ?? 0 );
            if ( oldBinaryFileGuid.HasValue && ( newBinaryFile == null || oldBinaryFileGuid.Value != newBinaryFile.Guid ) )
            {
                var oldBinaryFile = binaryFileService.Get( oldBinaryFileGuid.Value );
                if ( oldBinaryFile != null )
                {
                    oldBinaryFile.IsTemporary = true;
                }
            }
            if ( newBinaryFile != null && ( !oldBinaryFileGuid.HasValue || newBinaryFile.Guid != oldBinaryFileGuid.Value ) )
            {
                newBinaryFile.IsTemporary = false;
            }

            rockContext.SaveChanges();

            SetAttributeValue( "Layout", ddlLayout.SelectedValue );
            SetAttributeValue( "BackgroundImage", newBinaryFile != null ? newBinaryFile.Guid.ToString() : string.Empty );
            SetAttributeValue( "BackgroundImageUrl", tbBackgroundImageUrl.Text );
            SetAttributeValue( "Template", ceTemplate.Text );
            SetAttributeValue( "NotificationChannel", ddlNotificationChannel.SelectedValue );
            SetAttributeValue( "NotificationTemplate", ceNotificationTemplate.Text );

            SaveAttributeValues();

            mdEdit.Hide();

            ShowPreview();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the CrexPageMenu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CrexPageMenu_BlockUpdated( object sender, EventArgs e )
        {
            ShowPreview();
        }

        #endregion
    }
}
