using System;
using System.Linq;
using Condor.Interfaces;
using Condor.ORM;
using Condor.Core;
using Condor.Core.Interfaces;

namespace Condor.Factories
{
    public class DataObjectsFactory : IDataObjectsFactory
    {
        #region ctors

        private readonly string _dataPatternName;
        private readonly string _ormFrameworkName;
        private readonly string _dataStoreName;
        private readonly IValidator _validator;
        private readonly RequestContext _context;
        private IDataPattern _dataPattern;

        public DataObjectsFactory(string dataPatternName, string ormFrameworkName, string dataStoreName, IValidator validator, RequestContext requestContext)
        {
            if (string.IsNullOrEmpty(dataPatternName))
                throw new ApplicationException("dataPattern");

            if (string.IsNullOrEmpty(ormFrameworkName))
                throw new ApplicationException("ormFramework");

            if (string.IsNullOrEmpty(dataStoreName))
                throw new ApplicationException("dataStore");

            if (requestContext.Database == null)
                throw new ApplicationException("database");

            if (validator == null)
                throw new ApplicationException("validator");

            this._context = requestContext;
            this._validator = validator;
            this._dataStoreName = dataStoreName;
            this._ormFrameworkName = ormFrameworkName;
            this._dataPatternName = dataPatternName;
        }

        #endregion

        #region IDataObjectsFactory Members

        public void Build()
        {
            if (!_validator.IsValid())
                throw new ApplicationException(_validator.Message);

            IDataStore dataStore = null;
            IORMFramework ormFramework = null;

            /*****************************************************************
             * 1 - Create a DataStore object
             * This creates the logic for the CRUD methods that are injected
             * into the classes created with the ORM Framework.
             * **************************************************************/
            try
            {
                object[] parms = new object[1]
                {
                    _context
                };
                dataStore = (IDataStore)Activator.CreateInstance(Type.GetType(_dataStoreName), parms);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating instance of " + _dataStoreName + " - " + ex.Message);
            }


            /*****************************************************************
             * 2 - Create a ORMFramework object
             * **************************************************************/
            try
            {
                object[] ormParms = new object[2]
                {
                    dataStore,
                    _context
                };
                ormFramework = (IORMFramework)Activator.CreateInstance(Type.GetType(_ormFrameworkName), ormParms);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating instance of " + _ormFrameworkName + " - " + ex.Message);
            }


            /*****************************************************************
             * 3 - Create a DataPattern object
             * **************************************************************/
            try
            {
                object[] patternParms = new object[2]
                {
                    ormFramework,
                    _context
                };
                _dataPattern = (IDataPattern)Activator.CreateInstance(Type.GetType(_dataPatternName), patternParms);

                _dataPattern.Render();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating instance of " + _dataPatternName + " - " + ex.Message);
            }

        }

        #endregion
    }
}
