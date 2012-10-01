using System;
using System.Linq;

namespace Condor.Core.PropertyObjects
{
    /// <summary>
    /// Renders a property based on the column name with empty getters and setters.
    /// </summary>
    public class BusinessObjectsPropertyRenderShortProperty : Property
    {
        private readonly string[] _omitList;

        /// <summary>
        /// This constructor takes the column and RequestContext.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="context"></param>
        public BusinessObjectsPropertyRenderShortProperty(MyMeta.IColumn column, RequestContext context)
            :base(column, context)
        {
        }

        /// <summary>
        /// This constructor takes the column and RequestContext and a comma-delimited string of properties to omit.
        /// </summary>
        /// <param name="column">The IColumn variable</param>
        /// <param name="context">The RequestContext</param>
        /// <param name="omitList">The comma-delimited list of properties that will not be rendered.</param>
        public BusinessObjectsPropertyRenderShortProperty(MyMeta.IColumn column, RequestContext context, string omitList)
            :base(column, context)
        {
            this._omitList = omitList.ToLower().Split(',');
        }

        public override void Render()
        {
            if (!_omitList.Where(o => o == this.Alias.ToLower()).Any())
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
