using System;
using System.Linq;
using Condor.Core.Interfaces;
using Zeus;

namespace Condor.Core
{
    /// <summary>
    /// You can inherit from this class for your DataStore, or
    /// simply implement IDataStore. Subclassing from this class
    /// generates the standard GetById(int Id) and 
    /// GetAll(string sortExpression, int startRowIndex, int maximumRows) 
    /// interface members for you.
    /// </summary>
    public class DefaultDataStoreFactory : IDataStore
    {
        protected IZeusOutput _output;
        protected CommonUtility _util;
        protected RequestContext _context;
        protected ScriptSettings _script;

        public DefaultDataStoreFactory(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
            this._util = context.Utility;
            this._script = context.ScriptSettings;
        }

        #region IDataStore Members

        public void GetAll(MyMeta.ITable table)
        {
            
        }

        public void Insert(MyMeta.ITable table)
        {
            
        }

        public void Update(MyMeta.ITable table)
        {
            
        }

        public void Delete(MyMeta.ITable table)
        {
            
        }

        public void GetById(MyMeta.ITable table)
        {
            
        }

        public void GetAllWithSortingAndPaging(MyMeta.ITable table)
        {
            
        }

        public void Mapper(MyMeta.ITable table)
        {
            
        }

        public void Interface(MyMeta.ITable table)
        {
            string tableName = table.Name;
            string args = "";
            GenTable genTable = new GenTable(table, _context);
            string str = "";
            str += _util.BuildModelClassWithNameSpace(tableName) + " GetById(";

            for (int i = 0; i < genTable.GetPrimaryKeyNames.Length; i++)
            {
                // type + " " + key name
                args += genTable.GetPrimaryKeyTypes[i] + " ";
                args += StringFormatter.CamelCasing(genTable.GetPrimaryKeyNames[i]) + ", ";
            }
            // trim the trailing ", "

            args = args.Substring(0, (args.Length - 2));

            str += args;
            str += ");";
            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn(str);
            _output.tabLevel--;
            _output.tabLevel--;

            str = "";
            str += "List<" + _util.BuildModelClassWithNameSpace(tableName) + "> GetAll(string sortExpression, int startRowIndex, int maximumRows);";
            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn(str);
            _output.tabLevel--;
            _output.tabLevel--;
        }

        public void CRUDInterface()
        {
            
        }

        #endregion
    }
}
