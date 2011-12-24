using System;
using System.Linq;

namespace Condor.Core.Interfaces
{
    public interface ICodeTemplateBuilder
    {
        void RenderControllerTemplate();
        void RenderAspNetViewTemplates();
        void RenderRazorViewTemplates();
    }
}
