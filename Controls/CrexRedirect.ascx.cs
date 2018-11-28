using System;
using System.ComponentModel;

using com.blueboxmoon.Crex;
using com.blueboxmoon.Crex.Rest;
using Rock;
using Rock.Attribute;
using Rock.Web.UI;
using System.Web.UI.WebControls;

namespace RockWeb.Plugins.com_blueboxmoon.Crex
{
    [DisplayName( "Crex Redirect" )]
    [Category( "Blue Box Moon > Crex" )]
    [Description( "Redirects the TV application to another page." )]
    [CodeEditorField( "Url Template", "The URL should be in the format of '/api/crex/page/#PageId#'.", Rock.Web.UI.Controls.CodeEditorMode.Lava, Rock.Web.UI.Controls.CodeEditorTheme.Rock, defaultValue: "", order: 0 )]
    [LavaCommandsField( "Enabled Lava Commands", "The lava commands to make available when parsing the templates.", false, order: 1 )]
    [ContextAware]
    public partial class CrexRedirect : CrexBlock
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

            BlockUpdated += CrexRedirect_BlockUpdated;
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

            var url = GetAttributeValue( "UrlTemplate" ).ResolveMergeFields( mergeFields, CurrentPerson, GetAttributeValue( "EnabledLavaCommands" ) ).Trim();

            return new CrexAction( "Redirect", url );
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the BlockUpdated event of the CrexRedirect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CrexRedirect_BlockUpdated( object sender, EventArgs e )
        {
            ShowPreview();
        }

        #endregion
    }
}
