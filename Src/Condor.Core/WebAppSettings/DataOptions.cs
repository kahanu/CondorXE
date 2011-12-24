using System;

namespace Condor.Core
{
    [Serializable]
    public class DataOptions
    {
        public DataOptions()
        {
        }

        public string DataObjectsNamespace;
        public string VersionColumnName;

        public ClassPrefix ClassPrefix;
        public ClassSuffix ClassSuffix;

        public DataContext DataContext;

        public DataStore DataStore;
        public ORMFramework ORMFramework;
        public DataPattern DataPattern;

        public string ConnectionString;
    }
}