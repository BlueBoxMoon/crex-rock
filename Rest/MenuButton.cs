namespace com.blueboxmoon.Crex.Rest
{
    public class MenuButton
    {
        /// <summary>
        /// Gets or sets the title of the button.
        /// </summary>
        /// <value>
        /// The title of the button.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the action URL.
        /// </summary>
        /// <value>The action URL.</value>
        public string ActionUrl { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public CrexAction Action { get; set; }
    }
}
