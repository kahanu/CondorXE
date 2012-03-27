using MyMeta;

namespace Condor.Core.Interfaces
{
    public interface ICommonGenerators
    {
        void RenderDaoCRUDInterface();

        void RenderDaoInterface(ITable table);

        void RenderServiceCRUDInterface();

        void RenderServiceInterface(ITable table);
    }
}