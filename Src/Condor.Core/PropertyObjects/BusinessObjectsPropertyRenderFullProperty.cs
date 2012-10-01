using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Condor.Core.PropertyObjects
{
    public class BusinessObjectsPropertyRenderFullProperty : Property
    {
        public BusinessObjectsPropertyRenderFullProperty(MyMeta.IColumn column, RequestContext context)
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
                _output.tabLevel++;
                _output.autoTabLn("public " + LanguageType + " " + ToPropertyName());
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("get");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return this._" + StringFormatter.CamelCasing(LanguageType) + ";");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("set");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("this._" + StringFormatter.CamelCasing(LanguageType) + " = value;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("}");
            }
        }
    }
}
