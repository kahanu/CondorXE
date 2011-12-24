using System;
using System.Linq;
using MyMeta;
using System.Collections;

namespace Condor.Core
{
    public class GenTable
    {
        private readonly ITable _table;
        protected string[] primaryKeyTypes;
        protected string[] primaryKeyNames;
        protected ArrayList _primaryKeys = null;
        private readonly RequestContext _context;

        public GenTable(ITable table, RequestContext context)
        {
            this._context = context;
            this._table = table;
            MakePrimaryKeyList();
        }

        public ITable Table
        {
            get { return _table; }
        }

        public string[] GetPrimaryKeyTypes
        {
            get { return primaryKeyTypes; }
        }

        public string[] GetPrimaryKeyNames
        {
            get { return primaryKeyNames; }
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

        public void MakePrimaryKeyList()
        {
            int i = 0;
            bool blnHasKey = false;

            try
            {
                // Make sure there's at least 1 primary key column
                foreach (Column c in _table.Columns)
                {
                    if (c.IsInPrimaryKey)
                    {
                        primaryKeyTypes = new string[i + 1];
                        primaryKeyNames = new string[i + 1];
                        blnHasKey = true;
                        i++;
                    }
                }

                if (blnHasKey)
                {
                    i = 0;
                    foreach (Column c in _table.Columns)
                    {
                        if (c.IsInPrimaryKey)
                        {
                            primaryKeyTypes[i] = c.LanguageType;
                            primaryKeyNames[i] = c.Alias;
                            i++;
                        }
                    }
                }
                else
                {
                    throw new Exception("GenTable: " + _table.Name + " does not have a Primary Key column!");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error making GetTable Primary Key List - " + ex.Message);
            }
        }
    }
}
