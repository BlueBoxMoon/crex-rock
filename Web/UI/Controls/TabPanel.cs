using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using com.blueboxmoon.Crex.Extensions;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    [ParseChildren( false )]
    [PersistChildren( true )]
    public class TabPanel : WebControl
    {
        #region Fields

        HiddenField _hfSelectedTab;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected tab title.
        /// </summary>
        /// <value>
        /// The selected tab title.
        /// </value>
        public string SelectedTab { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabPanel"/> class.
        /// </summary>
        public TabPanel()
            : base( HtmlTextWriterTag.Div )
        {
        }

        #endregion

        #region Base Method Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            EnsureChildControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( Page.IsPostBack )
            {
                SelectedTab = _hfSelectedTab.Value;
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            _hfSelectedTab = new HiddenField
            {
                ID = ClientID + "_hfSelectedTab"
            };
            Controls.Add( _hfSelectedTab );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender( EventArgs e )
        {
            var tabs = new List<Tab>();

            //
            // Find all the tab controls.
            //
            foreach ( var c in Controls )
            {
                if ( c is Tab tab && tab.Visible )
                {
                    tabs.Add( tab );
                }
            }

            if ( tabs.Any() )
            {
                //
                // Mark them all as inactive.
                //
                tabs.ForEach( t => t.AddCssClass( "hidden" ) );

                //
                // Find the selected tab or pick the first one.
                //
                var selectedTab = tabs.FirstOrDefault( t => t.Title == SelectedTab );
                if ( selectedTab == null )
                {
                    selectedTab = tabs.First();
                    SelectedTab = selectedTab.Title;
                }

                //
                // Make sure the selected tab is active.
                //
                if ( selectedTab != null )
                {
                    selectedTab.RemoveCssClass( "hidden" );
                }
            }

            _hfSelectedTab.Value = SelectedTab;

            RegisterScripts();

            base.OnPreRender( e );
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        protected override void Render( HtmlTextWriter writer )
        {
            var tabs = new List<Tab>();

            foreach ( var c in Controls )
            {
                if ( c is Tab tab && tab.Visible )
                {
                    tabs.Add( tab );
                }
            }

            writer.AddAttribute( HtmlTextWriterAttribute.Class, CssClass );
            writer.AddAttribute( HtmlTextWriterAttribute.Id, ClientID );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "nav nav-tabs" );
                writer.RenderBeginTag( HtmlTextWriterTag.Ul );
                {
                    tabs.ForEach( t => RenderTabHeader( writer, t ) );
                }
                writer.RenderEndTag();

                writer.AddAttribute( HtmlTextWriterAttribute.Style, "padding: 15px; border-left: 1px solid #ddd; border-right: 1px solid #ddd; border-bottom: 1px solid #ddd; border-radius: 0px 0px 4px 4px;" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                {
                    RenderChildren( writer );
                }
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        protected virtual void RegisterScripts()
        {
            string script = $@"
;Sys.Application.add_load(function () {{
    $('#{ ClientID } > ul.nav > li > a').on('click', function (e) {{
        e.preventDefault();

        $('#{ ClientID } > div > div').addClass('hidden');
        $('#{ ClientID } > ul > li').removeClass('active');

        $('#' + $(this).data('target')).removeClass('hidden');
        $(this).closest('li').addClass('active');

        $('#{ _hfSelectedTab.ClientID }').val($(this).text());
    }});
}});";

            ScriptManager.RegisterStartupScript( this, GetType(), $"{ ClientID }_Events", script, true );
        }

        /// <summary>
        /// Renders the tab header.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="tab">The tab.</param>
        protected virtual void RenderTabHeader( HtmlTextWriter writer, Tab tab )
        {
            writer.AddAttribute( "role", "presentation" );
            if ( !tab.HasCssClass( "hidden" ) )
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "active" );
            }

            writer.RenderBeginTag( HtmlTextWriterTag.Li );
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Href, "#" );
                writer.AddAttribute( "data-target", tab.ClientID );
                writer.RenderBeginTag( HtmlTextWriterTag.A );
                {
                    writer.WriteEncodedText( tab.Title );
                }
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        #endregion
    }
}
