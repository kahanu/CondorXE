using MyMeta;
using System.Linq;

namespace GizmoBeach.Condor.Core
{
    public class GenColumn
    {
        protected MyMeta.IColumn m_IColumn;
        protected ScriptSettings script;
        protected Zeus.IZeusOutput output;
        protected CommonUtility _util;
        protected MyMeta.ITable _table;
        private Dnp.Utils.Utils m_DnpUtils;


        //public GenColumn(MyMeta.IColumn column, Zeus.IZeusOutput outputDest, MyMeta.ITable table, Dnp.Utils.Utils dnp)
        public GenColumn(MyMeta.IColumn column, MyMeta.ITable table, RequestContext context)
        {
            m_IColumn = column;
            script = ScriptSettings.GetInstance();
            output = context.Zeus.Output;
            _util = new CommonUtility();
            _table = table;
            m_DnpUtils = context.ScriptSettings.DnpUtils;
        }


        public IColumn MyMetaColumn
        {
            get
            {
                return m_IColumn;
            }
        }

        public string Alias
        {
            get
            {
                return m_IColumn.Alias.Trim();
            }
        }

        public string Name
        {
            get
            {
                return m_IColumn.Name.Trim();
            }
        }

        public string LanguageType
        {
            get
            {
                string nullable = string.Empty;
                if (m_IColumn.IsNullable && (m_IColumn.LanguageType != "string"))
                    nullable = "?";
                return m_IColumn.LanguageType + nullable;
            }
        }

        public string ToPropertyName()
        {
            if (this.Alias.ToLower() == script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                return this.Alias;
            }
            else
            {
                ////return script.DnpUtils.SetPascalCase(this.Alias);
                //return this.Alias;

                ePluralizerTypes propertyType = EnumFactory.Parse<ePluralizerTypes>(script.Settings.Pluralizer.PropertyNames.Selected);
                PluralizerFactory factory = new PluralizerFactory();
                string result = factory.SetWord(this.Alias, propertyType);
                return result;

            }
        }

        public string GetFirstStringColumnName()
        {
            string str = string.Empty;
            foreach (IColumn column in _table.Columns)
            {
                if (column.LanguageType.ToLower() == "string")
                {
                    str = column.Name;
                    break;
                }
            }
            return str;
        }

        public string CastNullableEntityProperties()
        {
            string result = "entity." + ToPropertyName();
            if (m_IColumn.LanguageType != "string")
            {
                if (m_IColumn.Name.ToLower() != script.Settings.DataOptions.VersionColumnName.ToLower())
                {
                    if (m_IColumn.IsNullable)
                    {
                        result = "(" + m_IColumn.LanguageType + ")entity." + ToPropertyName();
                    }
                    else
                    {
                        result = "entity." + ToPropertyName();
                    }
                }
                else
                {
                    result = "entity.rowversion";
                }
            }
            return result;
        }

        //********************************************************************
        //*** For business objects properties
        //********************************************************************

        public void RenderAsBusinessObjectsProperty()
        {
            if (ToPropertyName().ToLower() == script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                output.autoTabLn("public string " + this.Alias + " { get; set; }");
            }
            else
            {
                output.autoTabLn("public " + LanguageType + " " + ToPropertyName() + " { get; set; }");
            }
        }

        public void RenderForeignKeyBusinessObjectsProperties()
        {

            string tableName = _table.Name;
            foreach (IForeignKey key in _table.ForeignKeys)
            {
                if (script.Tables.Contains(key.ForeignTable.Name))
                {
                    if (key.PrimaryTable.Name == tableName)
                        output.autoTabLn("public List<" + StringFormatter.CleanUpClassName(key.ForeignTable.Name) + "> " + StringFormatter.CleanUpClassName(key.ForeignTable.Name) + "List { get; set; }");
                }

                if (script.Tables.Contains(key.PrimaryTable.Name))
                {
                    if (key.PrimaryTable.Name != tableName)
                        output.autoTabLn("public " + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + " " + StringFormatter.CleanUpClassName(key.PrimaryTable.Name) + " { get; set; }");
                }
            }
            output.writeln("");
        }

        public void RenderAsDtoProperty()
        {
            output.autoTabLn("[DataMember]");
            if (ToPropertyName().ToLower() == "rowversion")
            {
                output.autoTabLn("public string " + this.Alias + " { get; set; }");
            }
            else
            {
                output.autoTabLn("public " + LanguageType + " " + ToPropertyName() + " { get; set; }");
            }
        }

        public void RenderForeignKeyDtoProperties()
        {
            string tableName = _table.Name;

            foreach (IForeignKey key in _table.ForeignKeys)
            {
                if (script.Tables.Contains(key.ForeignTable.Name))
                {
                    if (key.PrimaryTable.Name == tableName)
                    {
                        output.autoTabLn("[DataMember]");
                        output.autoTabLn("public " + key.ForeignTable.Name + "Dto[] " + key.ForeignTable.Name + "List { get; set; }");
                    }
                }

                if (script.Tables.Contains(key.PrimaryTable.Name))
                {
                    if (key.PrimaryTable.Name != tableName)
                    {
                        output.autoTabLn("[DataMember]");
                        output.autoTabLn("public " + key.PrimaryTable.Name + "Dto " + key.PrimaryTable.Name + " { get; set; }");
                    }
                }
            }
            output.writeln("");
        }





        public void RenderAsProperty()
        {
            RenderAsProperty(false);
        }

        public void RenderAsProperty(bool isDto)
        {
            if (ToPropertyName().ToLower() == script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                output.autoTabLn("public string " + this.Alias + " { get; set; }");
            }
            else
            {
                if (m_IColumn.IsInPrimaryKey)
                {
                    output.autoTabLn("public " + LanguageType + " " + ToPropertyName() + " { get; set; }");
                }
                else if (m_IColumn.IsInForeignKey && !m_IColumn.IsInPrimaryKey)
                {
                    // Build a Typed property for the foreign key
                    if (isDto)
                        SetForeignTableProperty(isDto);
                }
                else
                {
                    output.autoTabLn("public " + LanguageType + " " + ToPropertyName() + " { get; set; }");
                }
            }
        }

        private void SetForeignTableProperty(bool isDto)
        {
            for (int j = 0; j < m_IColumn.ForeignKeys.Count; j++)
            {
                if (m_IColumn.ForeignKeys[j].PrimaryTable.Name == _table.Name)
                {
                    if (isDto)
                    {
                        output.autoTabLn("public " + m_IColumn.ForeignKeys[j].ForeignTable.Name + "Dto[] " + StringFormatter.MakePlural(StringFormatter.CleanUpClassName(m_IColumn.ForeignKeys[j].ForeignTable.Name)) + " { get; set; }");
                    }
                    else
                    {
                        output.autoTabLn("public List<" + m_IColumn.ForeignKeys[j].ForeignTable.Name + "> " + StringFormatter.MakePlural(StringFormatter.CleanUpClassName(m_IColumn.ForeignKeys[j].ForeignTable.Name)) + " { get; set; }");
                    }
                }

                if (m_IColumn.ForeignKeys[j].PrimaryTable.Name != _table.Name)
                {
                    if (isDto)
                    {
                        output.autoTabLn("public " + m_IColumn.ForeignKeys[j].PrimaryTable.Name + "Dto " + m_IColumn.ForeignKeys[j].PrimaryTable.Name + " { get; set; }");
                    }
                    else
                    {
                        output.autoTabLn("public " + m_IColumn.ForeignKeys[j].PrimaryTable.Name + " " + m_IColumn.ForeignKeys[j].PrimaryTable.Name + " { get; set; }");
                    }
                }
            }
        }

        //********************************************************************
        //*** For Request class properties
        //********************************************************************
        public void RenderAsField()
        {

        }



        //********************************************************************
        //*** For WCF Criteria classes 
        //********************************************************************
        public void RenderPrimaryKeyProperty()
        {
            if (m_IColumn.IsInPrimaryKey)
            {
                RenderAsProperty();
            }
        }


        public void RenderPrimaryKeyPropertyForCriteria()
        {
            if (m_IColumn.IsInPrimaryKey)
            {
                output.tabLevel++;
                output.tabLevel++;
                output.autoTabLn("[DataMember]");
                RenderAsProperty();
                output.tabLevel--;
                output.tabLevel--;
            }
        }


        public void RenderPropertyWithDataAnnotations()
        {
            IColumn column = m_IColumn;
            if (column.Name.ToLower() != script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                if (column.LanguageType.ToString().ToLower() == "string")
                {
                    if (!column.IsNullable)
                    {
                        output.autoTabLn("[Required(ErrorMessage = \"" + StringFormatter.NormalizePropertyName(column.Name) + " is required.\")]");
                        if (column.Name.ToLower().Contains("phone"))
                        {
                            output.autoTabLn("[StringLength(14, ErrorMessage=\"" + column.Name + " is invalid. Format: (###) ###-####.\")]");
                        }
                        else
                        {
                            if (!column.DataTypeName.Contains("text") && !column.DataTypeNameComplete.ToLower().Contains("nvarchar(max)"))
                            {
                                output.autoTabLn("[StringLength(" + column.CharacterMaxLength + ", ErrorMessage=\"" + StringFormatter.NormalizePropertyName(column.Name) + " must be between 1 and " + column.CharacterMaxLength + " characters.\")]");
                            }
                            if (column.DataTypeName.Contains("text") || column.DataTypeNameComplete.ToLower().Contains("varchar(max)"))
                            {
                                output.autoTabLn("[DataType(DataType.MultilineText)]");
                            }
                        }
                    }
                    else
                    {
                        if (column.DataTypeName.Contains("text") || column.DataTypeNameComplete.ToLower().Contains("varchar(max)"))
                        {
                            output.autoTabLn("[DataType(DataType.MultilineText)]");
                        }
                    }
                    output.autoTabLn(StringFormatter.CreateDisplayName(column.Name));
                    output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                    output.autoTabLn("");
                }
                else if (column.IsInPrimaryKey)
                {
                    output.autoTabLn("[ScaffoldColumn(false)]");
                    output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                    output.autoTabLn("");
                }
                else
                {
                    output.autoTabLn(StringFormatter.CreateDisplayName(column.Name));
                    if (!column.IsNullable)
                    {
                        output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        output.autoTabLn("");
                    }
                    else
                    {
                        if (column.DataTypeName == "bit")
                        {
                            output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                        else
                        {
                            output.autoTabLn("public " + column.LanguageType + "? " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
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
                        if (column.Name.ToLower() == script.Settings.DataOptions.VersionColumnName.ToLower())
                        {
                            output.autoTabLn("[HiddenInput(DisplayValue=false)]");
                            output.autoTabLn("public string " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                        else
                        {
                            output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                    }
                    else
                    {
                        output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        output.autoTabLn("");
                    }
                }
                else
                {
                    if (column.LanguageType == "byte[]")
                    {
                        if (column.Name.ToLower() == script.Settings.DataOptions.VersionColumnName.ToLower())
                        {
                            output.autoTabLn("[HiddenInput(DisplayValue=false)]");
                            output.autoTabLn("public string " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                        else
                        {
                            output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                    }
                    else
                    {
                        output.autoTabLn("public " + column.LanguageType + "? " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        output.autoTabLn("");
                    }
                }
            }
        }


        public void RenderPropertyWithEntLibValidationAttributes()
        {
            IColumn column = m_IColumn;
            if (column.Name.ToLower() != script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                if (column.LanguageType.ToString().ToLower() == "string")
                {
                    if (!column.IsNullable)
                    {
                        output.autoTabLn("[NotNullValidator(MessageTemplate = \"" + StringFormatter.NormalizePropertyName(column.Name) + " is required.\")]");
                        if (column.Name.ToLower().Contains("phone"))
                        {
                            output.autoTabLn("[StringLengthValidator(14, " + column.CharacterMaxLength + ", MessageTemplate=\"" + column.Name + " is invalid. Format: (###) ###-####.\")]");
                        }
                        else
                        {
                            if (!column.DataTypeName.Contains("text") && !column.DataTypeNameComplete.ToLower().Contains("nvarchar(max)"))
                            {
                                output.autoTabLn("[StringLengthValidator(1, " + column.CharacterMaxLength + ", MessageTemplate=\"" + StringFormatter.NormalizePropertyName(column.Name) + " must be between 1 and " + column.CharacterMaxLength + " characters.\")]");
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
                    output.autoTabLn(StringFormatter.CreateDisplayName(column.Name));
                    output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                    output.autoTabLn("");
                }
                else if (column.IsInPrimaryKey)
                {
                    //output.autoTabLn("[ScaffoldColumn(false)]");
                    output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                    output.autoTabLn("");
                }
                else
                {
                    //output.autoTabLn(StringFormatter.CreateDisplayName(column.Name));
                    if (!column.IsNullable)
                    {
                        output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        output.autoTabLn("");
                    }
                    else
                    {
                        if (column.DataTypeName == "bit")
                        {
                            output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                        else
                        {
                            output.autoTabLn("public " + column.LanguageType + "? " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
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
                        if (column.Name.ToLower() == script.Settings.DataOptions.VersionColumnName.ToLower())
                        {
                            //output.autoTabLn("[HiddenInput(DisplayValue=false)]");
                            output.autoTabLn("public string " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                        else
                        {
                            output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                    }
                    else
                    {
                        output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        output.autoTabLn("");
                    }
                }
                else
                {
                    if (column.LanguageType == "byte[]")
                    {
                        if (column.Name.ToLower() == script.Settings.DataOptions.VersionColumnName.ToLower())
                        {
                            //output.autoTabLn("[HiddenInput(DisplayValue=false)]");
                            output.autoTabLn("public string " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                        else
                        {
                            output.autoTabLn("public " + column.LanguageType + " " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                            output.autoTabLn("");
                        }
                    }
                    else
                    {
                        output.autoTabLn("public " + column.LanguageType + "? " + _util.CleanUpProperty(column.Name) + " { get; set; }");
                        output.autoTabLn("");
                    }
                }
            }
        }


    }
}
