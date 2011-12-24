namespace Condor.Core.Interfaces
{
    /// <summary>
    /// Implement this interface for model mapping classes to generate the MVC extension methods.
    /// </summary>
    public interface IMapperExtensions
    {
        /// <summary>
        /// Overload that generates the ToModel and FromModel extensions methods. Default model name is 'Model'.
        /// </summary>
        void Render();

        /// <summary>
        /// Overload that generates the ToModel and FromModel extensions methods.
        /// </summary>
        /// <param name="modelName">Enter your custom model name.</param>
        void Render(string modelName);
    }
}