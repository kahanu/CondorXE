using System;
using System.Linq;

namespace WebApi4.Components.Interfaces
{
    /// <summary>
    /// All different versions of the Entity Class must implement this interface.
    /// </summary>
    public interface IEntityClass
    {
        /// <summary>
        /// Output the code.
        /// </summary>
        void Render();

        /// <summary>
        /// Tell the component that will render the business objects properties, if there
        /// are any properties to omit.  The reason for this is so you can put them into 
        /// a base class, such as an Entity Class.
        /// </summary>
        /// <example>
        /// public class Tag : Entity
        /// {
        ///     [Required(ErrorMessage = "Tag Name is required.")]
        ///     [StringLength(50, ErrorMessage = "Tag Name must be between 1 and 50 characters.")]
        ///     [DisplayName("Tag Name")]
        ///     public string TagName { get; set; }
        /// }
        /// 
        /// The Omitted properties would be: Id and rowversion.
        /// public abstract class Entity
        /// {
        ///    [Key]
        ///    public int Id { get; set; }
	    ///	
        ///    [HiddenInput(DisplayValue = false)]
        ///    [Timestamp]
        ///    public byte[] rowversion { get; set; }
        /// }
        /// </example>
        string OmitList {get; }
    }
}
