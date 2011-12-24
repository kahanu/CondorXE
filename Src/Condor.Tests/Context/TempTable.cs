using System;
using MyMeta;

namespace Condor.Tests.Context
{
    class TempTable : MyMeta.ITable
    {
        private readonly Columns columns;

        public TempTable()
        {
        }

        public TempTable(Columns columns)
        {
            this.columns = columns;
        }

        #region ITable Members
        private string _alias = string.Empty;
        public string Alias
        {
            get
            {
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        public IPropertyCollection AllProperties
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IColumns Columns
        {
            get
            {
                return columns;
            }
        }

        public IDatabase Database
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object DatabaseSpecificMetaData(string key)
        {
            throw new NotImplementedException();
        }

        public DateTime DateCreated
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTime DateModified
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IForeignKeys ForeignKeys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPropertyCollection GlobalProperties
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid Guid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IIndexes Indexes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                return "Customer";
            }
        }

        public IColumns PrimaryKeys
        {
            get
            {
                return null;
            }
        }

        public int PropID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPropertyCollection Properties
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Schema
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Type
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string UserDataXPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}