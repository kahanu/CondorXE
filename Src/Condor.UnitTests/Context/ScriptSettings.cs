using System;
using System.Linq;
using Condor.Core;

namespace Condor.UnitTests.Context
{
    class ScriptSettings
    {
        public WebAppSettings Settings
        {
            get
            {
                return CreateWebAppSettings();
            }
        }

        #region Private Methods

        private WebAppSettings CreateWebAppSettings()
        {
            WebAppSettings settings = new WebAppSettings();
            settings.DataOptions = new DataOptions();
            settings.DataOptions.DataObjectsNamespace = "BlogApp.DataObjects";
            settings.DataOptions.VersionColumnName = "rowversion";

            settings.DataOptions.ORMFramework = new ORMFramework();
            settings.DataOptions.ORMFramework.Selected = "EntityFramework";

            return settings;
        }

        #endregion

        public string ChopProvider(string fullConnString)
        {
            throw new NotImplementedException();
        }

        public MyMeta.dbRoot Database
        {
            get { throw new NotImplementedException(); }
        }

        public string DatabaseName
        {
            get { throw new NotImplementedException(); }
        }

        public Dnp.Utils.Utils DnpUtils
        {
            get { throw new NotImplementedException(); }
        }

        public string GetClassValue(string className)
        {
            throw new NotImplementedException();
        }

        public string OutputPath
        {
            get { throw new NotImplementedException(); }
        }

        public void SaveSettings()
        {
            throw new NotImplementedException();
        }

        public System.Collections.ArrayList Tables
        {
            get { throw new NotImplementedException(); }
        }

        public string Version
        {
            get { throw new NotImplementedException(); }
        }
    }

}
