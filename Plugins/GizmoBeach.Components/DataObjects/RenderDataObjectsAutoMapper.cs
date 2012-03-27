using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Infrastructure;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.DataObjects
{
    public class RenderDataObjectsAutoMapper : RenderBase, IAutoMapperFramework
    {

        #region ctors

        private readonly RequestContext _context;

        private readonly IMapperConfiguration _mapperConfiguration;

        private readonly IMapperExtensions _mapperExtensions;

        public RenderDataObjectsAutoMapper(RequestContext context, IMapperConfiguration mapperConfiguration,
            IMapperExtensions mapperExtensions) : base(context.Zeus.Output)
        {
            this._mapperExtensions = mapperExtensions;
            this._mapperConfiguration = mapperConfiguration;
            this._context = context;
        }

        #endregion

        public void BuildModelClass(MyMeta.ITable table, bool useWebService)
        {
            
        }

        public void RenderAutoMapperAppStart()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".ModelMappers;");
            _output.autoTabLn("");
            _output.autoTabLn("[assembly: WebActivator.PreApplicationStartMethod(typeof(" + _script.Settings.UI.UINamespace + ".App_Start.AutoMapperDataObjects), \"Start\")]");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".App_Start");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class AutoMapperDataObjects");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static void Start()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("DataObjectsConfiguration.Configure();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    AutoMapperDataObjects.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\App_Start", "AutoMapperDataObjects.cs"), SaveActions.DontOverwrite); 
        }

        public void RenderAutoMapperConfiguration(bool useWebService)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".ModelMappers");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class DataObjectsConfiguration");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static void Configure()");
            _output.autoTabLn("{");
            _output.tabLevel++;

            //IMapperConfiguration mapper = new DataObjectsAutoMapperConfiguration(_context);
            //mapper.Render();

            _mapperConfiguration.Render();
            
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    DataObjectsConfiguration.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\ModelMappers", "DataObjectsConfiguration.cs"), SaveActions.Overwrite);
        }

        public void RenderAutoMapperExtensionClass(bool useWebService)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".EntityMapper");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class DataObjectsModelMappersExtensions");
            _output.autoTabLn("{");
            _output.autoTabLn("");

            //IMapperExtensions mapper = new DataObjectsAutoMapperExtensions(_context);
            //mapper.Render();

            _mapperExtensions.Render();

            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    DataObjectsModelMappersExtensions.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\EntityMapper", "DataObjectsModelMappersExtensions.cs"), SaveActions.Overwrite);
        }
    }
}
