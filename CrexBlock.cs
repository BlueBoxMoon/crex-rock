using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using com.blueboxmoon.Crex.Rest;
using com.blueboxmoon.Crex.Web.UI.Controls;
using Rock;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace com.blueboxmoon.Crex
{
    public abstract class CrexBlock : RockBlock, ICrexBlock
    {
        #region Properties

        /// <summary>
        /// Gets the page parameters.
        /// </summary>
        /// <value>
        /// The page parameters.
        /// </value>
        public new IReadOnlyDictionary<string, string> PageParameters
        {
            get
            {
                if ( _pageParameters == null )
                {
                    var pageParameters = new Dictionary<string, string>( StringComparer.InvariantCultureIgnoreCase );
                    foreach ( string param in HttpContext.Current.Request.QueryString.Keys )
                    {
                        if ( param != null )
                        {
                            pageParameters.AddOrReplace( param, HttpContext.Current.Request.QueryString[param] );
                        }
                    }

                    _pageParameters = pageParameters;
                }

                return _pageParameters;
            }
        }
        private IReadOnlyDictionary<string, string> _pageParameters;

        /// <summary>
        /// Gets the entity context.
        /// </summary>
        /// <value>
        /// The entity context.
        /// </value>
        public EntityContextCollection EntityContext
        {
            get
            {
                if ( _entityContext == null )
                {
                    var entityContext = new EntityContextCollection();
                    foreach ( var contextType in PageCache.PageContexts )
                    {
                        int? contextId = HttpContext.Current.Request.QueryString[contextType.Value].AsIntegerOrNull();
                        var entityType = EntityTypeCache.Read( contextType.Key );

                        if ( entityType != null && contextId.HasValue )
                        {
                            entityContext.AddEntity( entityType, contextId.Value );
                        }
                    }

                    _entityContext = entityContext;
                }

                return _entityContext;
            }
        }
        private EntityContextCollection _entityContext;

        /// <summary>
        /// Gets the preview panel.
        /// </summary>
        /// <value>
        /// The preview panel.
        /// </value>
        protected abstract Panel PreviewPanel { get; }

        /// <summary>
        /// Gets the debug panel.
        /// </summary>
        /// <value>
        /// The debug panel.
        /// </value>
        protected abstract Panel DebugPanel { get; }

        /// <summary>
        /// Gets the menu preview.
        /// </summary>
        /// <value>
        /// The menu preview.
        /// </value>
        protected MenuPreview MenuPreview { get; private set; }

        /// <summary>
        /// Gets the poster list preview.
        /// </summary>
        /// <value>
        /// The poster list preview.
        /// </value>
        protected PosterListPreview PosterListPreview { get; private set; }

        /// <summary>
        /// Gets the image preview.
        /// </summary>
        /// <value>
        /// The image preview.
        /// </value>
        protected ImagePreview ImagePreview { get; private set; }

        /// <summary>
        /// Gets the video preview.
        /// </summary>
        /// <value>
        /// The video preview.
        /// </value>
        protected VideoPreview VideoPreview { get; private set; }

        /// <summary>
        /// Gets the redirect preview.
        /// </summary>
        /// <value>
        /// The redirect preview.
        /// </value>
        protected NotificationBox RedirectPreview { get; private set; }

        /// <summary>
        /// Gets the debug preview.
        /// </summary>
        /// <value>
        /// The debug preview.
        /// </value>
        protected Literal DebugPreview { get; private set; }

        #endregion

        #region Base Method Overrides

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage.AddCSSLink( "~/Plugins/com_blueboxmoon/Crex/Styles/crex.css" );
            RockPage.AddScriptLink( "~/Plugins/com_blueboxmoon/Crex/Scripts/crex.js" );

            MenuPreview = new MenuPreview
            {
                ID = "previewMenu",
                Visible = false
            };
            PreviewPanel.Controls.Add( MenuPreview );

            PosterListPreview = new PosterListPreview
            {
                ID = "previewPosterList",
                Visible = false
            };
            PreviewPanel.Controls.Add( PosterListPreview );

            ImagePreview = new ImagePreview
            {
                ID = "previewImage",
                Visible = false
            };
            PreviewPanel.Controls.Add( ImagePreview );

            VideoPreview = new VideoPreview
            {
                ID = "previewVideo",
                Visible = false
            };
            PreviewPanel.Controls.Add( VideoPreview );

            RedirectPreview = new NotificationBox
            {
                ID = "previewRedirect",
                NotificationBoxType = NotificationBoxType.Info,
                Visible = false
            };
            PreviewPanel.Controls.Add( RedirectPreview );

            DebugPreview = new Literal
            {
                ID = "previewDebug"
            };
            DebugPanel.Controls.Add( DebugPreview );
        }

        #endregion

        #region Methods

        /// <summary>
        /// Pages the parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected new string PageParameter( string key )
        {
            return PageParameters.ContainsKey( key ) ? PageParameters[key] : string.Empty;
        }

        /// <summary>
        /// Gets the common lava merge fields.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, object> GetCommonMergeFields()
        {
            var fields = new Dictionary<string, object>
            {
                { "PageParameters", PageParameters },
                { "Context", EntityContext },
                { "Page", PageCache },
                { "Block", BlockCache }
            };

            return fields;
        }

        /// <summary>
        /// Gets the UrlSet from the provided attributes.
        /// </summary>
        /// <param name="binaryFileAttribute">The attribute name that contains a binary file.</param>
        /// <param name="urlAttribute">The attribute name that contains a Lava enabled URL string.</param>
        /// <param name="mergeFields">The merge fields.</param>
        /// <returns>A UrlSet object that identifies the requested image.</returns>
        protected UrlSet GetUrlSetFromAttributes( string binaryFileAttribute, string urlAttribute, Dictionary<string, object> mergeFields )
        {
            if ( !string.IsNullOrWhiteSpace( binaryFileAttribute ) && GetAttributeValue( binaryFileAttribute ).AsGuidOrNull().HasValue )
            {
                return UrlSet.FromBinaryImage( GetAttributeValue( binaryFileAttribute ).AsGuid() );
            }

            if ( !string.IsNullOrWhiteSpace( urlAttribute ) )
            {
                var bgValue = GetAttributeValue( urlAttribute ).ResolveMergeFields( mergeFields, CurrentPerson );
                var bgGuid = bgValue.AsGuidOrNull();

                if ( bgGuid.HasValue )
                {
                    return UrlSet.FromBinaryImage( bgGuid.Value );
                }

                return new UrlSet
                {
                    HD = bgValue,
                    FHD = bgValue,
                    UHD = bgValue
                };
            }

            return new UrlSet();
        }

        /// <summary>
        /// Gets the data that identifies this Crex template and action data.
        /// </summary>
        /// <param name="isPreview">if set to <c>true</c> then this is for a preview.</param>
        /// <returns>A CrexAction object.</returns>
        public virtual CrexAction GetCrexAction( bool isPreview )
        {
            return new CrexAction();
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        protected abstract void ShowError( string message );

        /// <summary>
        /// Shows the preview.
        /// </summary>
        protected virtual void ShowPreview()
        {
            CrexAction action = null;

            try
            {
                action = GetCrexAction( true );
            }
            catch ( Exception e )
            {
                ShowError( e.Message );
                return;
            }

            //
            // Make all previews invisible until we know which one to show.
            //
            MenuPreview.Visible = false;
            PosterListPreview.Visible = false;
            ImagePreview.Visible = false;
            VideoPreview.Visible = false;
            RedirectPreview.Visible = false;

            //
            // Process the template and display the correct preview.
            //
            if ( action?.Template == "Menu" && action?.Data is Rest.Menu )
            {
                MenuPreview.Data = ( Rest.Menu ) action.Data;
                MenuPreview.Visible = true;
            }
            else if ( action?.Template == "PosterList" && action?.Data is PosterList )
            {
                PosterListPreview.Data = ( PosterList ) action.Data;
                PosterListPreview.Visible = true;
            }
            else if ( action?.Template == "Image" && action?.Data is UrlSet )
            {
                ImagePreview.Data = ( UrlSet ) action.Data;
                ImagePreview.Visible = true;
            }
            else if ( action?.Template == "Video" && action?.Data is string )
            {
                VideoPreview.Data = ( string ) action.Data;
                VideoPreview.Visible = true;
            }
            else if ( action?.Template == "Redirect" && action?.Data is string )
            {
                string url = ( string ) action.Data;

                if ( url.ToLower().StartsWith( "/api/crex/page/" ) )
                {
                    url = url.ToLower().Replace( "/api/crex", "" );
                }

                RedirectPreview.Text = $"If you were not an administrator you would have been redirected to <a href='{ url }'>{ url }</a>.";
                RedirectPreview.Visible = true;

                if ( !IsUserAuthorized( Authorization.ADMINISTRATE ) )
                {
                    Response.Redirect( url, false );
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            else
            {
                ShowError( "Could not render a preview, unknown template or invalid data." );
            }

            //
            // Populate the debug panel.
            //
            DebugPanel.Visible = IsUserAuthorized( Authorization.ADMINISTRATE );
            if ( DebugPanel.Visible )
            {
                var json = action.ToJson( Newtonsoft.Json.Formatting.Indented );
                var literal = new Literal
                {
                    ID = "literalDebug",
                    Text = $"<pre>{ json.EncodeHtml() }</pre>"
                };
                DebugPanel.Controls.Add( literal );
            }
        }

        #endregion
    }
}
