using System.Web.UI;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    public class PosterListPreview : CrexPreview<Rest.PosterList>
    {
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the server control content.</param>
        protected override void Render( HtmlTextWriter writer )
        {
            if ( Data != null && Visible )
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "crex-preview crex-posterlist-preview" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                {
                    RenderImageElement( writer, "background-image", Data.BackgroundImage?.UHD );
                    RenderImageElement( writer, "poster-image", Data.Items.Count > 0 ? Data.Items[0].Image?.UHD : string.Empty );
                    RenderTextElement( writer, "title", Data.Title );
                    RenderTextElement( writer, "detail-left", Data.Items.Count > 0 ? Data.Items[0].DetailLeft : string.Empty );
                    RenderTextElement( writer, "detail-right", Data.Items.Count > 0 ? Data.Items[0].DetailRight : string.Empty );
                    RenderTextElement( writer, "description", Data.Items.Count > 0 ? Data.Items[0].Description : string.Empty );

                    writer.AddAttribute( HtmlTextWriterAttribute.Class, "list" );
                    writer.RenderBeginTag( HtmlTextWriterTag.Div );
                    {
                        bool isFirst = true;

                        foreach ( var item in Data.Items )
                        {
                            string url = item.ActionUrl;

                            if ( url.ToLower().StartsWith( "/api/crex/page/" ) )
                            {
                                url = url.ToLower().Replace( "/api/crex", "" );
                            }

                            if ( isFirst )
                            {
                                writer.AddAttribute( HtmlTextWriterAttribute.Class, "active" );
                                isFirst = false;
                            }
                            writer.AddAttribute( HtmlTextWriterAttribute.Href, url );
                            writer.AddAttribute( "data-image", item.Image != null ? item.Image.UHD : string.Empty, true );
                            writer.AddAttribute( "data-detail-left", item.DetailLeft, true );
                            writer.AddAttribute( "data-detail-right", item.DetailRight, true );
                            writer.AddAttribute( "data-description", item.Description, true );
                            writer.RenderBeginTag( HtmlTextWriterTag.A );
                            {
                                writer.WriteEncodedText( item.Title );
                            }
                            writer.RenderEndTag();
                        }
                    }
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
        }
    }
}
