using System.Web.UI;
using System.Web.UI.WebControls;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    [ParseChildren( false )]
    [PersistChildren( true )]
    public class Tab : Panel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
    }
}
