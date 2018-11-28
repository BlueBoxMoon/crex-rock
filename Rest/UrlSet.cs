using System;

namespace com.blueboxmoon.Crex.Rest
{
    /// <summary>
    /// Defines a URL set that contains multiple resolutions of the same URL.
    /// </summary>
    public class UrlSet
    {
        /// <summary>
        /// Gets or sets the HD url (720p).
        /// </summary>
        /// <value>
        /// The HD url (720p).
        /// </value>
        public string HD { get; set; }

        /// <summary>
        /// Gets or sets the FHD url (1080p).
        /// </summary>
        /// <value>
        /// The FHD url (1080p).
        /// </value>
        public string FHD { get; set; }

        /// <summary>
        /// Gets or sets the UHD url (2160p).
        /// </summary>
        /// <value>
        /// The UHD url (2160p).
        /// </value>
        public string UHD { get; set; }

        /// <summary>
        /// Creats a new URL set from the given BinaryFile guid by using the default image sizes.
        /// </summary>
        /// <param name="guid">The unique identifier of the binary file.</param>
        /// <returns>A UrlSet that identifies the image URLs.</returns>
        public static UrlSet FromBinaryImage( Guid guid )
        {
            return FromBinaryImage( guid, 1280, 1920, 3840 );
        }

        /// <summary>
        /// Creates a new URL set from the given BinaryFile guid at the specified image sizes.
        /// </summary>
        /// <param name="guid">The unique identifier of the binary file.</param>
        /// <param name="hdWidth">Width of the HD image.</param>
        /// <param name="fhdWidth">Width of the FHD image.</param>
        /// <param name="uhdWidth">Width of the UHD image.</param>
        /// <returns></returns>
        public static UrlSet FromBinaryImage( Guid guid, int hdWidth, int fhdWidth, int uhdWidth )
        {
            // TODO: We need to implement the UHD resolution instead of just
            // using the original. However, bug #3430 needs to be fixed first.
            return new UrlSet
            {
                HD = $"/GetImage.ashx?Guid={ guid }&width={ hdWidth }",
                FHD = $"/GetImage.ashx?Guid={ guid }&width={ fhdWidth }",
                UHD = $"/GetImage.ashx?Guid={ guid }"
            };
        }
    }
}
