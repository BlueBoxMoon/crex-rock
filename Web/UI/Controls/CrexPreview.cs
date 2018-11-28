using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    public abstract class CrexPreview<DType> : WebControl
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public DType Data { get; set; }

        /// <summary>
        /// Restores view-state information from a previous page request that was saved by the <see cref="M:System.Web.UI.Control.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            Data = ( ( string ) ViewState["Data"] ).FromJsonOrNull<DType>();
        }

        /// <summary>
        /// Saves any server control view-state changes that have occurred since the time the page was posted back to the server.
        /// </summary>
        /// <returns>
        /// Returns the server control's current view state. If there is no view state associated with the control, this method returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            ViewState["Data"] = Data?.ToJson();

            return base.SaveViewState();
        }

        /// <summary>
        /// Renders the text element.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="cssClass">The CSS class to use for the containing control.</param>
        /// <param name="content">The content.</param>
        protected virtual void RenderTextElement( HtmlTextWriter writer, string cssClass, string content )
        {
            writer.AddAttribute( HtmlTextWriterAttribute.Class, cssClass );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            {
                writer.WriteEncodedText( content ?? string.Empty );
            }
            writer.RenderEndTag();
        }

        /// <summary>
        /// Renders the image element.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="cssClass">The CSS class to identify the containing element.</param>
        /// <param name="url">The URL of the image.</param>
        protected virtual void RenderImageElement( HtmlTextWriter writer, string cssClass, string url )
        {
            writer.AddAttribute( HtmlTextWriterAttribute.Class, $"centered-image { cssClass }" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Src, url ?? string.Empty );
                writer.RenderBeginTag( HtmlTextWriterTag.Img );
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
    }
}
