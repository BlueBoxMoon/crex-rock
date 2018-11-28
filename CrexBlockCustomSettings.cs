using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace com.blueboxmoon.Crex
{
    public abstract class CrexBlockCustomSettings : CrexBlock
    {
        /// <summary>
        /// Gets the settings tool tip.
        /// </summary>
        /// <value>
        /// The settings tool tip.
        /// </value>
        public virtual string SettingsToolTip
        {
            get { return "Settings"; }
        }

        /// <summary>
        /// Adds icons to the configuration area of a <see cref="Rock.Model.Block" /> instance.  Can be overridden to
        /// add additional icons
        /// </summary>
        /// <param name="canConfig">A <see cref="System.Boolean" /> flag that indicates if the user can configure the <see cref="Rock.Model.Block" /> instance.
        /// This value will be <c>true</c> if the user is allowed to configure the <see cref="Rock.Model.Block" /> instance; otherwise <c>false</c>.</param>
        /// <param name="canEdit">A <see cref="System.Boolean" /> flag that indicates if the user can edit the <see cref="Rock.Model.Block" /> instance.
        /// This value will be <c>true</c> if the user is allowed to edit the <see cref="Rock.Model.Block" /> instance; otherwise <c>false</c>.</param>
        /// <returns>
        /// A <see cref="System.Collections.Generic.List{Control}" /> containing all the icon <see cref="System.Web.UI.Control">controls</see>
        /// that will be available to the user in the configuration area of the block instance.
        /// </returns>
        public override List<Control> GetAdministrateControls( bool canConfig, bool canEdit )
        {
            List<Control> configControls = new List<Control>();

            if ( canEdit )
            {
                LinkButton lbEdit = new LinkButton();
                lbEdit.CssClass = "edit";
                lbEdit.ToolTip = SettingsToolTip;
                lbEdit.Click += lbEdit_Click;
                configControls.Add( lbEdit );
                HtmlGenericControl iEdit = new HtmlGenericControl( "i" );
                lbEdit.Controls.Add( iEdit );
                lbEdit.CausesValidation = false;
                iEdit.Attributes.Add( "class", "fa fa-pencil-square-o" );

                // will toggle the block config so they are no longer showing
                lbEdit.Attributes["onclick"] = "Rock.admin.pageAdmin.showBlockConfig()";

                ScriptManager.GetCurrent( this.Page ).RegisterAsyncPostBackControl( lbEdit );
            }

            configControls.AddRange( base.GetAdministrateControls( canConfig, canEdit ) );

            return configControls;
        }

        /// <summary>
        /// Handles the Click event of the lbEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbEdit_Click( object sender, EventArgs e )
        {
            ShowSettings();
        }

        /// <summary>
        /// Shows the settings.
        /// </summary>
        protected abstract void ShowSettings();
    }
}
