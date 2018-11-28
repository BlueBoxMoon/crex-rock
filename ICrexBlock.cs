using com.blueboxmoon.Crex.Rest;

namespace com.blueboxmoon.Crex
{
    public interface ICrexBlock
    {
        /// <summary>
        /// Gets the data that identifies this Crex template and action data.
        /// </summary>
        /// <param name="isPreview">if set to <c>true</c> then this is for a preview.</param>
        /// <returns>A CrexAction object.</returns>
        CrexAction GetCrexAction( bool isPreview );
    }
}
