using System.Web.UI;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    public class MenuPreview : CrexPreview<Rest.Menu>
    {
        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        protected override void Render( HtmlTextWriter writer )
        {
            if ( Data != null && Visible )
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "crex-preview crex-menu-preview" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                {
                    RenderImageElement( writer, "background-image", Data.BackgroundImage?.UHD );

                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "menu-buttons" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    {
                        if ( Data.Buttons != null )
                        {
                            bool isFirst = true;

                            foreach ( var button in Data.Buttons )
                            {
                                string url = button.ActionUrl;

                                if ( url.ToLower().StartsWith("/api/crex/page/") )
                                {
                                    url = url.ToLower().Replace( "/api/crex", "" );
                                }

                                if ( isFirst )
                                {
                                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "active" );
                                    isFirst = false;
                                }
                                writer.AddAttribute( HtmlTextWriterAttribute.Href, url );
                                writer.RenderBeginTag( HtmlTextWriterTag.A );
                                {
                                    writer.WriteEncodedText( button.Title );
                                }
                                writer.RenderEndTag();
                            }
                        }
                    }
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
        }
    }
}
