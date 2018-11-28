using System.Web.UI;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    public class ImagePreview : CrexPreview<Rest.UrlSet>
    {
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the server control content.</param>
        protected override void Render( HtmlTextWriter writer )
        {
            if ( Data != null && Visible )
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "crex-preview crex-image-preview" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                {
                    RenderImageElement( writer, "background-image", Data.UHD );
                }
                writer.RenderEndTag();
            }
        }
    }
}
