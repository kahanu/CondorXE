using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Condor.Core.PropertyObjects
{
    public class MapPropertiesFromDto : Property
    {
        #region ctors

        private readonly string _modelName;

        private readonly string _dtoName;

        public MapPropertiesFromDto(MyMeta.IColumn column, RequestContext context):base(column, context)
        {
        }

        public MapPropertiesFromDto(MyMeta.IColumn column, RequestContext context, string modelName, string dtoName):this(column, context)
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
            _output.autoTabLn(_modelName + "." + _context.Utility.CleanUpProperty(Column.Name, false) + " = " + _dtoName + "." + _context.Utility.CleanUpProperty(Column.Name, false) + ";");

        }
    }
}
