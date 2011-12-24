using System;
using System.Linq;
using MyMeta;
using Zeus;

namespace Condor.Core
{
    /// <summary>
    /// The abstract base Property object all concrete classes must implement.
    /// </summary>
    public abstract class Property
    {
        private readonly MyMeta.IColumn _column;
        private readonly MyMeta.ITable _table;
        protected RequestContext _context;
        protected ScriptSettings _script;
        protected IZeusOutput _output;
        private string _propertyName;

        public Property(MyMeta.IColumn column, RequestContext context)
        {
            this._context = context;
            this._table = column.Table;
            this._column = column;
            this._output = context.Zeus.Output;
            this._script = context.ScriptSettings;
        }

        public abstract void Render();

        /// <summary>
        /// The alias for the column.
        /// </summary>
        public string Alias
        {
            get
            {
                return this._column.Alias.Trim();
            }
        }

        /// <summary>
        /// The name for the column.
        /// </summary>
        public string Name
        {
            get
            {
                return this._column.Name.Trim();
            }
        }

        /// <summary>
        /// The column with all the properties.
        /// </summary>
        public IColumn Column
        {
            get
            {
                return this._column;
            }
        }

        /// <summary>
        /// The parent table for this column.
        /// </summary>
        public ITable Table
        {
            get
            {
                return this._table;
            }
        }

        /// <summary>
        /// The .Net language specific data type, such as: int, DateTime, string, etc.
        /// </summary>
        public string LanguageType
        {
            get
            {
                string nullable = string.Empty;
                if (this._column.IsNullable && (this._column.LanguageType != "string"))
                    nullable = "?";
                return this._column.LanguageType + nullable;
            }
        }

        /// <summary>
        /// Constructs a proper property name based on the pluralizer settings in CondorXE Common tab.
        /// </summary>
        /// <returns></returns>
        public virtual string ToPropertyName()
        {
            if (this.Alias.ToLower() == _script.Settings.DataOptions.VersionColumnName.ToLower())
            {
                return this.Alias;
            }
            else
            {
                if (!string.IsNullOrEmpty(_propertyName))
                {
                    return _propertyName;
                }
                ePluralizerTypes propertyType = EnumFactory.Parse<ePluralizerTypes>(_script.Settings.Pluralizer.PropertyNames.Selected);
                PluralizerFactory factory = new PluralizerFactory();
                _propertyName = factory.SetWord(this.Alias, propertyType);
                return _propertyName;
            }
        }
    }
}
