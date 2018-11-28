using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace com.blueboxmoon.Crex.Extensions
{
    public static class WebControlExtensions
    {
        public static bool HasCssClass( this WebControl control, string className )
        {
            return control.CssClass.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries ).Any( c => c == className );
        }
    }
}
