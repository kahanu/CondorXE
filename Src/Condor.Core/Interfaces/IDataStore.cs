using System;
using System.Linq;
using MyMeta;

namespace Condor.Core.Interfaces
{
    public interface IDataStore
    {
        // For Base classes
        void GetAll(ITable table);
        void Insert(ITable table);
        void Update(ITable table);
        void Delete(ITable table);

        // For Concrete classes
        void GetById(ITable table);
        void GetAllWithSortingAndPaging(ITable table);

        // For Mapper classes
        void Mapper(ITable table);

        // For Interfaces
        void Interface(ITable table);

        // For CRUDInterface
        void CRUDInterface();
    } 
}
