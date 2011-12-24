using System;
using System.Linq;
using MyMeta;

namespace Condor.Core.PropertyObjects
{
    public class BusinessObjectsPropertyRenderEntLibValidation : Property
    {
        protected CommonUtility _util;

        public BusinessObjectsPropertyRenderEntLibValidation(MyMeta.IColumn column, RequestContext context)
            : base(column, context)
        {
            this._util = context.Utility;
        }

        public override void Render()
        {
            IColumn column = Column;
            if (column.Name.ToLower() != _script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                if (column.LanguageType.ToString().ToLower() == "string")
                {
                    if (!column.IsNullable)
                    {
                        _output.autoTabLn("[NotNullValidator(MessageTemplate = \"" + StringFormatter.NormalizePropertyName(column.Name) + " is required.\")]");
                        if (column.Name.ToLower().Contains("phone"))
                        {
                            _output.autoTabLn("[StringLengthValidator(14, " + column.CharacterMaxLength + ", MessageTemplate=\"" + column.Name + " is invalid. Format: (###) ###-####.\")]");
                        }
                        else
                        {
                            if (!column.DataTypeName.Contains("text") && !column.DataTypeNameComplete.ToLower().Contains("nvarchar(max)"))
                            {
                                _output.autoTabLn("[StringLengthValidator(1, " + column.CharacterMaxLength + ", MessageTemplate=\"" + StringFormatter.NormalizePropertyName(column.Name) + " must be between 1 and " + column.CharacterMaxLength + " characters.\")]");
                            }
                            if (column.DataTypeName.Contains("text") || column.DataTypeNameComplete.ToLower().Contains("varchar(max)"))
                            {
                                //output.autoTabLn("[DataType(DataType.MultilineText)]");
                            }
                        }
                    }
                    else
                    {
                        if (column.DataTypeName.Contains("text") || column.DataTypeNameComplete.ToLower().Contains("varchar(max)"))
                        {
                            //output.autoTabLn("[DataType(DataType.MultilineText)]");
                        }
                    }
                    _output.autoTabLn(StringFormatter.CreateDisplayName(column.Name));
                    _output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                    _output.autoTabLn("");
                }
                else if (column.IsInPrimaryKey)
                {
                    //output.autoTabLn("[ScaffoldColumn(false)]");
                    _output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                    _output.autoTabLn("");
                }
                else
                {
                    //output.autoTabLn(StringFormatter.CreateDisplayName(column.Name));
                    if (!column.IsNullable)
                    {
                        _output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        _output.autoTabLn("");
                    }
                    else
                    {
                        if (column.DataTypeName == "bit")
                        {
                            _output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            _output.autoTabLn("");
                        }
                        else
                        {
                            _output.autoTabLn("public " + column.LanguageType + "? " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            _output.autoTabLn("");
                        }
                    }
                }
            }
            else
            {
                if (!column.IsNullable)
                {
                    if (column.LanguageType == "byte[]")
                    {
                        if (column.Name.ToLower() == _script.Settings.DataOptions.VersionColumnName.ToLower())
                        {
                            //output.autoTabLn("[HiddenInput(DisplayValue=false)]");
                            _output.autoTabLn("public string " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            _output.autoTabLn("");
                        }
                        else
                        {
                            _output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            _output.autoTabLn("");
                        }
                    }
                    else
                    {
                        _output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        _output.autoTabLn("");
                    }
                }
                else
                {
                    if (column.LanguageType == "byte[]")
                    {
                        if (column.Name.ToLower() == _script.Settings.DataOptions.VersionColumnName.ToLower())
                        {
                            //output.autoTabLn("[HiddenInput(DisplayValue=false)]");
                            _output.autoTabLn("public string " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            _output.autoTabLn("");
                        }
                        else
                        {
                            _output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            _output.autoTabLn("");
                        }
                    }
                    else
                    {
                        _output.autoTabLn("public " + column.LanguageType + "? " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        _output.autoTabLn("");
                    }
                }
            }
        }
    }
}
