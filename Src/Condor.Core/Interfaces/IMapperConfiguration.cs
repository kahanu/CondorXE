namespace Condor.Core.Interfaces
{
    /// <summary>
    /// Implement this interface to generate the configuration methods for the mapping class.
    /// </summary>
    public interface IMapperConfiguration
    {
        /// <summary>
        /// Render the configuration methods for the mapping class. Default model name is 'Model'.
        /// </summary>
        void Render();

        /// <summary>
        /// Render the configuration methods for the mapping class.
        /// </summary>
        /// <param name="modelName">Enter the custom model name.</param>
        void Render(string modelName);
    }
}