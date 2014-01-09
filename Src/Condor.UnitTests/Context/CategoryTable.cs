using System;
using System.Linq;
using MyMeta;

namespace Condor.UnitTests.Context
{
    class CategoryTable : MyMeta.ITable
    {
        private readonly IColumns _columns;

        private readonly IDatabase _database;

        public CategoryTable(IColumns columns, IDatabase database)
        {
            this._database = database;
            this._columns = columns;
        }

        public string Alias
        {
            get
            {
                return "Category";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MyMeta.IPropertyCollection AllProperties
        {
            get { throw new NotImplementedException(); }
        }

        public MyMeta.IColumns Columns
        {
            get { return _columns; }
        }

        public MyMeta.IDatabase Database
        {
            get { return _database; }
        }

        public object DatabaseSpecificMetaData(string key)
        {
            throw new NotImplementedException();
        }

        public DateTime DateCreated
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime DateModified
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public MyMeta.IForeignKeys ForeignKeys
        {
            get { throw new NotImplementedException(); }
        }

        public MyMeta.IPropertyCollection GlobalProperties
        {
            get { throw new NotImplementedException(); }
        }

        public Guid Guid
        {
            get { throw new NotImplementedException(); }
        }

        public MyMeta.IIndexes Indexes
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return "Category"; }
        }

        public MyMeta.IColumns PrimaryKeys
        {
            get { throw new NotImplementedException(); }
        }

        public int PropID
        {
            get { throw new NotImplementedException(); }
        }

        public MyMeta.IPropertyCollection Properties
        {
            get { throw new NotImplementedException(); }
        }

        public string Schema
        {
            get { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { throw new NotImplementedException(); }
        }

        public string UserDataXPath
        {
            get { throw new NotImplementedException(); }
        }

        public bool Equals(MyMeta.ITable other)
        {
            throw new NotImplementedException();
        }
    }

}
