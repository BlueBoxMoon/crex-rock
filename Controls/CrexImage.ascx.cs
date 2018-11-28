using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

using com.blueboxmoon.Crex;
using com.blueboxmoon.Crex.Rest;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI;

namespace RockWeb.Plugins.com_blueboxmoon.Crex
{
    [DisplayName( "Crex Image" )]
    [Category( "Blue Box Moon > Crex" )]
    [Description( "Displays an image that can be specified with Lava." )]
    [TextField( "Background Image", "The image to be displayed in the background. Suggested size is 3840x2160.", false, "", "CustomSetting" )]
    [TextField( "Background Image Url", "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>", false, "", "CustomSetting" )]
    [ContextAware]
    public partial class CrexImage : CrexBlockCustomSettings
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

            BlockUpdated += CrexImage_BlockUpdated;
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
            var binaryFile = new BinaryFileService( rockContext ).Get( GetAttributeValue( "BackgroundImage" ).AsGuid() );
            ftBackgroundImage.BinaryFileId = binaryFile != null ? ( int? ) binaryFile.Id : null;
            tbBackgroundImageUrl.Text = GetAttributeValue( "BackgroundImageUrl" );
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
            UrlSet urlSet = GetUrlSetFromAttributes( "BackgroundImage", "BackgroundImageUrl", mergeFields );

            return new CrexAction( "Image", urlSet );
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

            SetAttributeValue( "BackgroundImage", newBinaryFile != null ? newBinaryFile.Guid.ToString() : string.Empty );
            SetAttributeValue( "BackgroundImageUrl", tbBackgroundImageUrl.Text );

            SaveAttributeValues();

            mdEdit.Hide();

            ShowPreview();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the CrexImage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CrexImage_BlockUpdated( object sender, EventArgs e )
        {
            ShowPreview();
        }

        #endregion
    }
}
