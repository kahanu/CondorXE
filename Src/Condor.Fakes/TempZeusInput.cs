using System;
using System.Collections;

namespace Condor.Fakes
{
    class TempZeusInput : Zeus.IZeusInput
    {
        private Hashtable _invars;

        public TempZeusInput()
        {
            this._invars = new Hashtable();
        }

        #region IZeusInput Members

        public void AddItems(object collection)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeys(object[] keys)
        {
            throw new NotImplementedException();
        }

        public System.Collections.ICollection Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public System.Collections.ICollection Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string __Variables
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object this[string key]
        {
            get
            {
                return this._invars[key];
            }
            set
            {
                this._invars[key] = value;
            }
        }
        #endregion
    }
}