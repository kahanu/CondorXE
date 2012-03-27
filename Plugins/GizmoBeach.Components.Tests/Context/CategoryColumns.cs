using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyMeta;

namespace GizmoBeach.Components.Tests.Context
{
    public class CategoryColumns : IColumns
    {
        private ArrayList _array;

        public CategoryColumns()
        {
            _array = new ArrayList();
            _array.Add(new IdentityColumn());
            _array.Add(new CategoryNameColumn());
            _array.Add(new RowversionColumn());
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _array.Count; }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public string UserDataXPath
        {
            get { throw new NotImplementedException(); }
        }

        public IColumn this[object index]
        {
            get { return (IColumn)_array[(int)index]; }
        }

        public int Add(object value)
        {
            _array.Add(value);
            return _array.Count;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get
            {
                return (IColumn)_array[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return new ColumnEnumerator<IColumn>(_array);
        }

        System.Collections.Generic.IEnumerator<IColumn> System.Collections.Generic.IEnumerable<IColumn>.GetEnumerator()
        {
            return new ColumnEnumerator<IColumn>(_array);
        }
    }

    class ColumnEnumerator<T> : IEnumerator<T>
    {
        IColumn[] _columnList;

        int _position = -1;

        public ColumnEnumerator(ArrayList columns)
        {
            _columnList = new IColumn[columns.Count];
            for (int i = 0; i < columns.Count; i++)
            {
                _columnList[i] = (IColumn)columns[i];
            }
        }

        public object Current
        {
            get
            {
                try
                {
                    return _columnList[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _columnList.Length);
        }

        public void Reset()
        {
            _position = -1;
        }

        T IEnumerator<T>.Current
        {
            get
            {
                try
                {
                    return (T)_columnList[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose()
        {
            
        }
    }
}
