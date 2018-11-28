using System;

namespace com.blueboxmoon.Crex.Rest
{
    public class Notification
    {
        /// <summary>
        /// Gets or sets the message to be displayed.
        /// </summary>
        /// <value>The message to be displayed.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail image.
        /// </summary>
        /// <value>The thumbnail image.</value>
        public UrlSet Image { get; set; }

        /// <summary>
        /// Gets or sets the date and time this notification should start
        /// showing up.
        /// </summary>
        /// <value>The date and time this notification should start showing up.</value>
        public DateTime StartDateTime { get; set; }
    }
}
