using System.Collections.Generic;

namespace com.blueboxmoon.Crex.Rest
{
    public class PosterList
    {
        /// <summary>
        /// Gets or sets the screen title.
        /// </summary>
        /// <value>
        /// The screen title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>
        /// The background image.
        /// </value>
        public UrlSet BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the items to be displayed.
        /// </summary>
        /// <value>
        /// The items to be displayed.
        /// </value>
        public List<PosterListItem> Items { get; set; }
    }
}
