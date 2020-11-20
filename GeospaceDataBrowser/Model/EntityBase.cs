namespace GeospaceDataBrowser.Model
{
    /// <summary>
    /// Represents a base class for model objects.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets an id of the object.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets an object short name.
        /// </summary>
        public string ShortName { get; protected set; }

        /// <summary>
        /// Gets an object full name.
        /// </summary>
        public string FullName { get; protected set; }

        /// <summary>
        /// Gets an object description.
        /// </summary>
        public string Description { get; protected set; }
    }
}