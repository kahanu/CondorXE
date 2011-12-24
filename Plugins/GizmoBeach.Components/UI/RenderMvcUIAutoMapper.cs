using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Infrastructure;
using Condor.Core.Interfaces;
using Condor.Core.PropertyObjects;
using MyMeta;

namespace GizmoBeach.Components.UI
{
    /// <summary>
    /// This class renders ASP.NET MVC AutoMapper classes in the UI.
    /// </summary>
    public class RenderMvcUIAutoMapper : RenderBase, IAutoMapperFramework
    {
        #region ctors

        private readonly RequestContext _context;

        public RenderMvcUIAutoMapper(RequestContext context):base(context.Zeus.Output)
        {
            this._context = context;
        }

        #endregion

        #region IAutoMapperFramework Members

        public void RenderAutoMapperExtensionClass(bool useWebService)
        {
            
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("");
            if (useWebService)
            {
                _output.autoTabLn("using " + _script.Settings.UI.UIWcfServiceNamespace + ";");
            }
            else
            {
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            }
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".Models");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class MapperExtensions");
            _output.autoTabLn("{");
            _output.autoTabLn("");

            IMapperExtensions mapper = new AutoMapperExtensions(_context);
            mapper.Render();

            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}	");

            _context.FileList.Add("    MapperExtensions.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\Models", "_MapperExtensions.cs"), SaveActions.Overwrite);
        }

        public void RenderAutoMapperConfiguration(bool useWebService)
        {
            
            _output.tabLevel = 0;
            _hdrUtil.WriteClassHeader(_output);

            if (useWebService)
            {
                _output.autoTabLn("using " + _script.Settings.UI.UIWcfServiceNamespace + ";");
            }
            else
            {
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            }
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".Models;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".ModelMappers");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class Configuration");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static void Configure()");
            _output.autoTabLn("{");

            IMapperConfiguration mapper = new AutoMapperConfiguration(_context);
            mapper.Render();
            
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    Configuration.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\ModelMappers", "Configuration.cs"), SaveActions.Overwrite);
        }

        public void BuildModelClass(ITable table, bool useWebService)
        {
            
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Web.Mvc;");
            _output.autoTabLn("using System.ComponentModel;");
            _output.autoTabLn("using System.ComponentModel.DataAnnotations;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("");
            if (useWebService)
            {
                _output.autoTabLn("using " + _script.Settings.UI.UIWcfServiceNamespace + ";");
            }
            else
            {
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            }
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".Models");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "Model");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("");

            Condor.Core.Property prop = null;
            foreach (IColumn c in table.Columns)
            {
                prop = new BusinessObjectsPropertyRenderDataAnnotations(c, _context);
                prop.Render();
            }
            BusinessObjectsPropertiesRenderForeignKey foreignKey = new BusinessObjectsPropertiesRenderForeignKey(table, _context);
            foreignKey.Render();


            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Model.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\Models", StringFormatter.CleanUpClassName(table.Name) + "Model.cs"), SaveActions.DontOverwrite);
        }

        public void RenderAutoMapperAppStart()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".ModelMappers;");
            _output.autoTabLn("");
            _output.autoTabLn("[assembly: WebActivator.PreApplicationStartMethod(typeof(" + _script.Settings.UI.UINamespace + ".App_Start.AutomapperMVC3), \"Start\")]");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".App_Start");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class AutomapperMVC3");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static void Start()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("Configuration.Configure();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    AutomapperMVC3.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\App_Start", "AutomapperMVC3.cs"), SaveActions.DontOverwrite);
        }

        #endregion
    }
}
