namespace com.blueboxmoon.Crex.Rest
{
    public class PosterListItem
    {
        /// <summary>
        /// Gets or sets the title of this item.
        /// </summary>
        /// <value>
        /// The title of this item.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the detail left text.
        /// </summary>
        /// <value>
        /// The detail left text.
        /// </value>
        public string DetailLeft { get; set; }

        /// <summary>
        /// Gets or sets the detail right text.
        /// </summary>
        /// <value>
        /// The detail right text.
        /// </value>
        public string DetailRight { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public CrexAction Action { get; set; }

        /// <summary>
        /// Gets or sets the action URL.
        /// </summary>
        /// <value>The action URL.</value>
        public string ActionUrl { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public UrlSet Image { get; set; }
    }
}
