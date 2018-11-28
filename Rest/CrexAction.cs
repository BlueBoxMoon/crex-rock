namespace com.blueboxmoon.Crex.Rest
{
    public class CrexAction
    {
        /// <summary>
        /// Gets or sets the name of the template for this action.
        /// </summary>
        /// <value>
        /// The name of the template for this action.
        /// </value>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the data that the template will use to render itself.
        /// </summary>
        /// <value>
        /// The data that the template will use to render itself.
        /// </value>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the required crex version.
        /// </summary>
        /// <value>
        /// The required crex version.
        /// </value>
        public int? RequiredCrexVersion { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrexAction"/> class.
        /// </summary>
        public CrexAction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrexAction"/> class.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="data">The action data.</param>
        /// <param name="requiredCrexVersion">The required crex version.</param>
        public CrexAction( string template, object data, int? requiredCrexVersion = null )
        {
            Template = template;
            Data = data;
            RequiredCrexVersion = requiredCrexVersion;
        }

    }
}
