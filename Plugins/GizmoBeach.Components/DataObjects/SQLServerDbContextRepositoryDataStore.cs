using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Zeus;
using MyMeta;

namespace GizmoBeach.Components.DataObjects
{
    public class SQLServerDbContextRepositoryDataStore : IDataStore
    {
        #region ctors
        protected IZeusOutput _output;
        protected ScriptSettings _script;
        protected CommonUtility _util;
        protected RequestContext _context;

        public SQLServerDbContextRepositoryDataStore(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
            this._script = context.ScriptSettings;
            this._util = context.Utility;
        }
        #endregion

        #region IDataStore Members

        public void GetAll(ITable table)
        {

        }

        public void Insert(ITable table)
        {

        }

        public void Update(ITable table)
        {

        }

        public void Delete(ITable table)
        {

        }

        public void GetById(ITable table)
        {

        }

        public void GetAllWithSortingAndPaging(ITable table)
        {

        }

        public void Mapper(ITable table)
        {

        }

        public void Interface(ITable table)
        {

        }

        public void CRUDInterface()
        {

        }

        #endregion
    }
}

