using System;
using System.Globalization;

using DotLiquid;
using Rock;
using Rock.Utility;

namespace com.blueboxmoon.Crex
{
    public class LavaFilters : IRockStartup
    {
        #region Initialization

        /// <summary>
        /// All IRockStartup classes will be run in order by this value. If class does not depend on an order, return zero.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int StartupOrder { get { return 0; } }

        /// <summary>
        /// Method that will be run at Rock startup
        /// </summary>
        public virtual void OnStartup()
        {
            Template.RegisterFilter( GetType() );
        }

        #endregion

        /// <summary>
        /// Makes a string safe for inclusion inside a JSON string
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A string that can safely be included inside a JSON quoted string.</returns>
        public static string ToJSONSafeString( object input )
        {
            var json = input.ToString().ToJson();

            return json.Substring( 1, json.Length - 2 );
        }

        /// <summary>
        /// Convert the date to an ISO 8601 format.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ToISO8601( object input )
        {
            if ( input == null )
            {
                return null;
            }

            if ( input.ToString() == "Now" )
            {
                input = RockDateTime.Now;
            }

            var inputDateTime = input.ToString().AsDateTime();

            return inputDateTime.Value.ToUniversalTime().ToString( "yyyy-MM-dd'T'HH:mm:ssK", CultureInfo.InvariantCulture );
        }
    }
}
