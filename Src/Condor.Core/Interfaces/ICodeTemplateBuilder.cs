using System;
using System.Linq;

namespace Condor.Core.Interfaces
{
    public interface ICodeTemplateBuilder : ICodeTemplateBuilderV2
    {
        void RenderControllerTemplate();
        void RenderAspNetViewTemplates();
        void RenderRazorViewTemplates();
    }
}
