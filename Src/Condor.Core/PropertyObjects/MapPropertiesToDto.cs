using System;
using System.Linq;

namespace Condor.Core.PropertyObjects
{
    /// <summary>
    /// Renders property assignments for model-to-entity transformations. 
    /// Example: used for mapping business objects to DTOs.
    /// </summary>
    public class MapPropertiesToDto : Property
    {
        #region ctors

        private readonly string _modelName;

        private readonly string _dtoName;

        public MapPropertiesToDto(MyMeta.IColumn column, RequestContext context)
        : base(column, context)
        {

        }

        public MapPropertiesToDto(MyMeta.IColumn column, RequestContext context, string modelName, string dtoName):this(column, context)
        {
            if (string.IsNullOrEmpty(modelName))
                modelName = "model";

            if (string.IsNullOrEmpty(dtoName))
                dtoName = "dto";

            this._dtoName = dtoName;
            this._modelName = modelName;
        }

        #endregion

        public override void Render()
        {
            string str = "";
            // To BusinessObject loop

            if (Column.IsNullable && Column.LanguageType != "string")
            {
                if (Column.Name.ToLower() == _script.Settings.DataOptions.VersionColumnName.ToLower())
                {
                    _output.autoTabLn(_dtoName + "." + _context.Utility.CleanUpProperty(Column.Name, false) + " = " + _modelName + "." + _context.Utility.CleanUpProperty(Column.Name, false) + ";");
                }
                else
                {
                    str += _dtoName + "." + _context.Utility.CleanUpProperty(Column.Name, false) + " = " + _modelName + "." + _context.Utility.CleanUpProperty(Column.Name, false);
                    if (Column.LanguageType == "int")
                    {
                        str += " ?? 0";
                    }
                    if (Column.LanguageType == "decimal")
                    {
                        str += " ?? 0.0m";
                    }
                    _output.autoTabLn(str + ";");
                }
            }
            else
            {
                _output.autoTabLn(_dtoName + "." + _context.Utility.CleanUpProperty(Column.Name, false) + " = " + _modelName + "." + _context.Utility.CleanUpProperty(Column.Name, false) + ";");
            }
        }
    }
}
