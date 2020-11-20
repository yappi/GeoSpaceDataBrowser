namespace GeospaceDataBrowser
{
    /// <summary>
    /// Represents a limit object.
    /// </summary>
    /// <typeparam name="T">The type of the limit values.</typeparam>
    public class Limit<T>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Limit"/> class.
        /// </summary>
        private Limit()
        { }

        /// <summary>
        /// Initializes an instance of the <see cref="Limit"/> class.
        /// </summary>
        /// <param name="minValue">The minimum limit value.</param>
        /// <param name="maxValue">The maximum limit value.</param>
        public Limit(T minValue, T maxValue)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        /// <summary>
        /// Gets or sets a minimum limit value.
        /// </summary>
        public T MinValue { get; private set; }

        /// <summary>
        /// Gets or sets a minimum limit value.
        /// </summary>
        public T MaxValue { get; private set; } 
    }
}
