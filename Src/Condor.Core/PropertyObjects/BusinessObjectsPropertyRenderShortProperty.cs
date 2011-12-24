using System;
using System.Linq;

namespace Condor.Core.PropertyObjects
{
    /// <summary>
    /// Renders a property based on the column name with empty getters and setters.
    /// </summary>
    public class BusinessObjectsPropertyRenderShortProperty : Property
    {
        public BusinessObjectsPropertyRenderShortProperty(MyMeta.IColumn column, RequestContext context)
            :base(column, context)
        {
        }

        public override void Render()
        {
            if (ToPropertyName().ToLower() == _script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                _output.autoTabLn("public string " + this.Alias + " { get; set; }");
            }
            else
            {
                _output.autoTabLn(ToString());
            }
        }

        /// <summary>
        /// Returns a complete short property for the column name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "public " + LanguageType + " " + ToPropertyName() + " { get; set; }";
        }
    }
}
