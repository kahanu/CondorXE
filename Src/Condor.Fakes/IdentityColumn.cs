﻿using System;
using System.Linq;

namespace Condor.Fakes
{
    class IdentityColumn : MyMeta.IColumn
    {
        //private readonly ITable table;
        //public IdentityColumn(ITable table)
        //{
        //    this.table = table;
        //}
        #region IColumn Members
        private string _alias = string.Empty;
        public string Alias
        {
            get
            {
                return "Id";
            }
            set
            {
                _alias = value;
            }
        }

        public MyMeta.IPropertyCollection AllProperties
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int AutoKeyIncrement
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int AutoKeySeed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int CharacterMaxLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int CharacterOctetLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CharacterSetCatalog
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CharacterSetName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CharacterSetSchema
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int CompFlags
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int DataType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DataTypeName
        {
            get
            {
                return "nvarchar";
            }
        }

        public string DataTypeNameComplete
        {
            get
            {
                return "nvarchar(50)";
            }
        }

        public object DatabaseSpecificMetaData(string key)
        {
            throw new NotImplementedException();
        }

        public int DateTimePrecision
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DbTargetType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Default
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

        public MyMeta.IDomain Domain
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DomainCatalog
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DomainName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DomainSchema
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Flags
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MyMeta.IForeignKeys ForeignKeys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MyMeta.IPropertyCollection GlobalProperties
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

        public bool HasDefault
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasDomain
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAutoKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsComputed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInForeignKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInPrimaryKey
        {
            get
            {
                return true;
            }
        }

        public bool IsNullable
        {
            get
            {
                return false;
            }
        }

        public int LCID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string LanguageType
        {
            get
            {
                return "int";
            }
        }

        public string Name
        {
            get
            {
                return "Id";
            }
        }

        public int NumericPrecision
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int NumericScale
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Ordinal
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PropID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MyMeta.IPropertyCollection Properties
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int SortID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public byte[] TDSCollation
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MyMeta.ITable Table
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid TypeGuid
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

        public MyMeta.IView View
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        public bool Equals(MyMeta.IColumn other)
        {
            throw new NotImplementedException();
        }
    }
}
