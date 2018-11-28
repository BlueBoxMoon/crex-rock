using System.Web.UI;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    public class VideoPreview : CrexPreview<string>
    {
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the server control content.</param>
        protected override void Render( HtmlTextWriter writer )
        {
            if ( Data != null && Visible )
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "crex-preview crex-video-preview" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                {
                    writer.AddAttribute( "controls", "controls" );
                    writer.AddAttribute( "autoplay", "autoplay" );
                    writer.AddAttribute( HtmlTextWriterAttribute.Src, Data );
                    writer.RenderBeginTag( "video" );
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
        }
    }
}
