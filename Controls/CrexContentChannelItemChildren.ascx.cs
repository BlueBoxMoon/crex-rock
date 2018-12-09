using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using com.blueboxmoon.Crex;
using com.blueboxmoon.Crex.Rest;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Field.Types;
using Rock.Model;
using Rock.Security;

namespace RockWeb.Plugins.com_blueboxmoon.Crex
{
    [DisplayName( "Crex Content Channel Item Children" )]
    [Category( "Blue Box Moon > Crex" )]
    [Description( "Displays a list of content channel items that are children of the passed content channel item." )]
    [TextField( "Layout", "The screen layout to use on the TV.", true, "Menu", "CustomSetting" )]
    [TextField( "Detail Page", "The page to navigate to for details.", false, "", "CustomSetting" )]
    [TextField( "Sort Items By", "How to sort the child items.", true, "Descending", "CustomSetting" )]
    [TextField( "Allowed Channel Types", "Limits the content channel types that can be viewed.", false, "", "CustomSetting" )]
    [TextField( "Background Image", "The image to be displayed in the background. Suggested size is 3840x2160.", false, "", "CustomSetting" )]
    [TextField( "Background Image Url", "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>", false, "", "CustomSetting" )]
    [TextField( "Template", "The lava template to use when generating the items.", true, "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/PosterListContentChannelChildren.lava' %}", "CustomSetting" )]
    [LavaCommandsField( "Enabled Lava Commands", "The lava commands to make available when parsing the template.", false, "", "Advanced", order: 6 )]
    public partial class CrexContentChannelItemChildren : CrexBlockCustomSettings
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

            BlockUpdated += CrexContentChannelItemChildren_BlockUpdated;
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
        /// Shows the settings.
        /// </summary>
        protected override void ShowSettings()
        {
            mdEdit.Show();

            var rockContext = new RockContext();
            cblAllowedChannelTypes.DataSource = new ContentChannelTypeService( rockContext ).Queryable()
                .OrderBy( c => c.Name )
                .Select( c => new { c.Guid, c.Name } )
                .ToList();
            cblAllowedChannelTypes.DataBind();
            
            ddlLayout.SetValue( GetAttributeValue( "Layout" ) );
            var ppFieldType = new PageReferenceFieldType();
            ppFieldType.SetEditValue( ppDetailPage, null, GetAttributeValue( "DetailPage" ) );
            cblAllowedChannelTypes.SetValues( GetAttributeValues( "AllowedChannelTypes" ) );
            ddlSortItemsBy.SetValue( GetAttributeValue( "SortItemsBy" ) );

            var binaryFile = new BinaryFileService( rockContext ).Get( GetAttributeValue( "BackgroundImage" ).AsGuid() );
            ftBackgroundImage.BinaryFileId = binaryFile != null ? ( int? ) binaryFile.Id : null;
            tbBackgroundImageUrl.Text = GetAttributeValue( "BackgroundImageUrl" );

            ceTemplate.Text = GetAttributeValue( "Template" );
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void ShowError( string message )
        {
            nbError.Text = message;
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
            var allowedChannelTypes = GetAttributeValues( "AllowedChannelTypes" ).AsGuidList();
            int? contentItemId = HttpContext.Current.Request.QueryString["ContentItemId"].AsIntegerOrNull();

            if ( !contentItemId.HasValue )
            {
                return new CrexAction();
            }

            var detailPage = Rock.Web.Cache.PageCache.Get( GetAttributeValue( "DetailPage" ).AsGuid() );
            var linkedPages = new Dictionary<string, object>
            {
                { "DetailPageId", detailPage != null ? detailPage.Id.ToString() : string.Empty }
            };
            mergeFields.Add( "LinkedPages", linkedPages );

            using ( var rockContext = new RockContext() )
            {
                var contentItem = new ContentChannelItemService( rockContext ).Get( contentItemId.Value );

                //
                // Verify the content item is one we can display.
                //
                if ( allowedChannelTypes.Any() && !allowedChannelTypes.Contains( contentItem.ContentChannelType.Guid ) )
                {
                    throw new Exception( "Content channel item is not in an allowed channel type." );
                }

                //
                // Perform sorting on the content item.
                //
                var contentChannelItems = GetContentItems( contentItem );

                //
                // Get the JSON from the lava template.
                //
                mergeFields.AddOrReplace( "ParentItem", contentItem );
                mergeFields.AddOrReplace( "Items", contentChannelItems.ToList() );
                var json = lavaTemplate.ResolveMergeFields( mergeFields, CurrentPerson, GetAttributeValue( "EnabledLavaCommands" ) );

                //
                // Get the background image.
                //
                UrlSet backgroundImage = GetUrlSetFromAttributes( "BackgroundImage", "BackgroundImageUrl", mergeFields );

                //
                // Final layout configuration.
                //
                if ( layout == "Menu" )
                {
                    var menuData = new com.blueboxmoon.Crex.Rest.Menu
                    {
                        BackgroundImage = backgroundImage
                    };

                    menuData.Buttons = json.FromJsonOrNull<List<MenuButton>>();

                    return new CrexAction( layout, menuData );
                }
                else if ( layout == "PosterList" )
                {
                    var posterListData = new PosterList
                    {
                        BackgroundImage = backgroundImage,
                        Title = contentItem.Title
                    };

                    posterListData.Items = json.FromJsonOrNull<List<PosterListItem>>();

                    return new CrexAction( layout, posterListData );
                }
            }

            return new CrexAction();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the content items.
        /// </summary>
        /// <param name="contentItem">The content item.</param>
        /// <returns></returns>
        private IQueryable<ContentChannelItem> GetContentItems( ContentChannelItem contentItem )
        {
            var sortMethod = GetAttributeValue( "SortItemsBy" );

            var contentChannelItems = contentItem.ChildItems.Select( c => c.ChildContentChannelItem ).AsQueryable();

            if ( sortMethod == "Ascending")
            {
                if ( contentItem.ContentChannel.ChildItemsManuallyOrdered )
                {
                    contentChannelItems = contentChannelItems.OrderBy( i => i.Order );
                }
                else
                {
                    contentChannelItems = contentChannelItems.OrderBy( i => i.StartDateTime );
                }
            }
            else
            {
                if ( contentItem.ContentChannel.ChildItemsManuallyOrdered )
                {
                    contentChannelItems = contentChannelItems.OrderByDescending( i => i.Order );
                }
                else
                {
                    contentChannelItems = contentChannelItems.OrderByDescending( i => i.StartDateTime );
                }
            }

            return contentChannelItems;
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
            SetAttributeValue( "AllowedChannelTypes", cblAllowedChannelTypes.SelectedValues.AsDelimited( "," ) );
            SetAttributeValue( "SortItemsBy", ddlSortItemsBy.SelectedValue );
            var ppFieldType = new PageReferenceFieldType();
            SetAttributeValue( "DetailPage", ppFieldType.GetEditValue( ppDetailPage, null ) );
            SetAttributeValue( "BackgroundImage", newBinaryFile != null ? newBinaryFile.Guid.ToString() : string.Empty );
            SetAttributeValue( "BackgroundImageUrl", tbBackgroundImageUrl.Text );
            SetAttributeValue( "Template", ceTemplate.Text );

            SaveAttributeValues();

            mdEdit.Hide();

            ShowPreview();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the CrexContentChannelView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CrexContentChannelItemChildren_BlockUpdated( object sender, EventArgs e )
        {
            ShowPreview();
        }

        #endregion
    }
}
