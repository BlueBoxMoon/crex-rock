using System.Collections.Generic;

namespace com.blueboxmoon.Crex.Rest
{
    public class Menu
    {
        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>
        /// The background image URL.
        /// </value>
        public UrlSet BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the menu buttons.
        /// </summary>
        /// <value>
        /// The menu buttons.
        /// </value>
        public List<MenuButton> Buttons { get; set; }

        /// <summary>
        /// Gets or sets the notifications that should be displayed.
        /// </summary>
        /// <value>The notifications that should be displayed.</value>
        public List<Notification> Notifications { get; set; }
    }
}
