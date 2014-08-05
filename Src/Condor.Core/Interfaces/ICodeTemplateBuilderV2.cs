using System;
using System.Linq;

namespace Condor.Core.Interfaces
{
    /// <summary>
    /// This interface contains all the members from the original ICodeTemplateBuilder interface,
    /// but it includes a new member for WebApi templates.  Generally used for MVC 5 applications.
    /// </summary>
    public interface ICodeTemplateBuilderV2
    {
        void RenderWebApiTemplates();
    }
}
