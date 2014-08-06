using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.UI
{
    public class AspNetMvcUILayerObjectsForDbContextBase : RenderBase, IUIObjects
    {
        #region ctors

        private readonly UICriteria _criteria;

        private readonly IAutoMapperFramework _autoMapperFramework;
        private readonly ProgressDialogWrapper _dialog;
        private readonly IDatabase _database;
        private readonly RequestContext _context;

        public AspNetMvcUILayerObjectsForDbContextBase(RequestContext context)
            : this(context, null)
        {

        }
        
        public AspNetMvcUILayerObjectsForDbContextBase(RequestContext context, IAutoMapperFramework autoMapperFramework)
            :this(context, autoMapperFramework, new UICriteria())
        {
        }

        public AspNetMvcUILayerObjectsForDbContextBase(RequestContext context, IAutoMapperFramework autoMapperFramework, UICriteria criteria)
            : base(context.Zeus.Output)
        {
            this._criteria = criteria;
            this._autoMapperFramework = autoMapperFramework;
            this._context = context;
            this._dialog = context.Dialog;
            this._database = context.Database;
        }

        #endregion

        #region IRenderObject Members

        public void Render()
        {
            _dialog.InitDialog();
            _context.FileList.Add("");
            _context.FileList.Add("Generated UI MVC ViewData classes:");

            bool useUIDtos = false;
            if (_autoMapperFramework != null)
                useUIDtos = true;

            foreach (string tableName in _script.Tables)
            {
                MyMeta.ITable table = _database.Tables[tableName];
                BuildViewDataClass(table, useUIDtos);
            }

            _dialog.InitDialog(3);
            _dialog.Display("Processing UI MVC BaseViewData class.");
            BuildBaseViewDataClass();

            _dialog.Display("Processing UI MVC BaseViewDataBuilder class.");
            BuildBaseViewDataBuilderClass();

            _dialog.Display("Processing UI MVC ViewDataFactory class.");
            BuildViewDataFactory();

            CreateCommonClass();

            CreateConfigSettingsClass();

            CreateJsonResponseClass();

            BuildControllerCodeTemplate();

            BuildBaseController();

            BuildAutoMapperFramework();
        }

        private void BuildAutoMapperFramework()
        {
            if (_autoMapperFramework != null)
            {
                bool useWebService = false;
                _autoMapperFramework.RenderAutoMapperExtensionClass(useWebService);
                _autoMapperFramework.RenderAutoMapperConfiguration(useWebService);
                _autoMapperFramework.RenderAutoMapperAppStart();

                foreach (string tableName in _script.Tables)
                {
                    ITable table = _database.Tables[tableName];
                    _autoMapperFramework.BuildModelClass(table, useWebService);
                }
            }
        }

        private void BuildBaseController()
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web.Mvc;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".Controllers");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class BaseController : Controller");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#region DI stuff");

            foreach (string tableName in _script.Tables)
            {
                if (tableName == "SiteSetting")
                {
                    _output.autoTabLn("private ISiteSettingService siteSettingService;");
                    _output.autoTabLn("");
                    _output.autoTabLn("public BaseController(ISiteSettingService siteSettingService)");
                    _output.autoTabLn("{");
                    _output.tabLevel++;
                    _output.autoTabLn("this.siteSettingService = siteSettingService;");
                    _output.tabLevel--;
                    _output.autoTabLn("}");
                }
            }



            _output.autoTabLn("");
            _output.autoTabLn("public BaseController()");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");

            foreach (string tableName in _script.Tables)
            {
                if (tableName == "SiteSetting")
                {
                    _output.autoTabLn("#region Public Methods");
                    _output.autoTabLn("");
                    _output.autoTabLn("/// <summary>");
                    _output.autoTabLn("/// Get the value of a SiteSetting.");
                    _output.autoTabLn("/// </summary>");
                    _output.autoTabLn("/// <param name=\"key\"></param>");
                    _output.autoTabLn("/// <returns></returns>");
                    _output.autoTabLn("public string GetSetting(string key)");
                    _output.autoTabLn("{");
                    _output.tabLevel++;
                    _output.autoTabLn("string result = string.Empty;");
                    _output.autoTabLn("");
                    _output.autoTabLn("var query = siteSettingService.GetByKey(key);");
                    _output.autoTabLn("");
                    _output.autoTabLn("return result;");
                    _output.tabLevel--;
                    _output.autoTabLn("}");
                    _output.autoTabLn("");
                    _output.autoTabLn("#endregion");
                    _output.autoTabLn("");
                }
            }



            _output.autoTabLn("#region Overrides");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// This provides simple feedback to the modelstate in the case of errors.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"filterContext\"></param>");
            _output.autoTabLn("protected override void OnActionExecuted(ActionExecutedContext filterContext)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (filterContext.Result is RedirectToRouteResult)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("// put the ModelState into TempData");
            _output.autoTabLn("TempData.Add(\"_MODELSTATE\", ModelState);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("else if (filterContext.Result is ViewResult && TempData.ContainsKey(\"_MODELSTATE\"))");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("// merge modelstate from TempData");
            _output.autoTabLn("var modelState = TempData[\"_MODELSTATE\"] as ModelStateDictionary;");
            _output.autoTabLn("foreach (var item in modelState)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (!ModelState.ContainsKey(item.Key))");
            _output.tabLevel++;
            _output.autoTabLn("ModelState.Add(item);");
            _output.tabLevel--;
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("base.OnActionExecuted(filterContext);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    BaseController.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\Controllers", "BaseController.cs"), SaveActions.DontOverwrite);


            // Now create the WebApi base class
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Net;");
            _output.autoTabLn("using System.Net.Http;");
            _output.autoTabLn("using System.Web.Http;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Generated;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".Controllers.api");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// This is the common base ApiController used for all controllers.");
            _output.autoTabLn("/// It will be used primarily for GET operations, even though it has");
            _output.autoTabLn("/// the other CRUD operations enabled.  Other operations such as POST,");
            _output.autoTabLn("/// PUT, DELETE, etc., should be used only where absolutely necessary,");
            _output.autoTabLn("/// and only in the Admin Area, not the public site.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <typeparam name=\"T\">a business object type</typeparam>");
            _output.autoTabLn("public class BaseController<T> : ApiController where T : class");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("private readonly IService<T> _service;");
            _output.autoTabLn("private readonly IUnitOfWork _unitOfWork;");
            _output.autoTabLn("");
            _output.autoTabLn("public BaseController(IService<T> service, IUnitOfWork unitOfWork)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this._unitOfWork = unitOfWork;");
            _output.autoTabLn("this._service = service;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Basic CRUD");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Get all the Products in the repository.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public IEnumerable<T> Get()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return _service.Get();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Get the selected Entity.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"id\">Id</param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public T Get(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T model = _service.GetById(id);");
            _output.autoTabLn("if (model == null)");
            _output.tabLevel++;
            _output.autoTabLn("throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return model;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Insert a new Product into the repository.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"model\">Product</param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("/// ");
            _output.autoTabLn("[Authorize]");
            _output.autoTabLn("public HttpResponseMessage Post(T model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (ModelState.IsValid)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_service.Insert(model);");
            _output.autoTabLn("_unitOfWork.Db.Commit();");
            _output.autoTabLn("");
            _output.autoTabLn("var response = Request.CreateResponse(HttpStatusCode.Created, model);");
            _output.autoTabLn("//response.Headers.Location = new Uri(Url.Link(\"DefaultApi\", new { controller = \"" + " id = model.Id }));");
            _output.autoTabLn("");
            _output.autoTabLn("return response;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("else");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Update the selected Product.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"id\">Id</param>");
            _output.autoTabLn("/// <param name=\"model\">Product</param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("/// ");
            _output.autoTabLn("[Authorize]");
            _output.autoTabLn("public HttpResponseMessage Put(T model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (ModelState.IsValid)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_service.Update(model);");
            _output.autoTabLn("_unitOfWork.Db.Commit();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.NotFound);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.OK, model);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("else");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.BadRequest);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Delete the selected Product.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"id\">Id</param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("/// ");
            _output.autoTabLn("[Authorize]");
            _output.autoTabLn("public HttpResponseMessage Delete(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T model = _service.GetById(id);");
            _output.autoTabLn("if (model == null)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.NotFound);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_service.Delete(model);");
            _output.autoTabLn("_unitOfWork.Db.Commit();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.NotFound);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.OK, model);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("}");

            _context.FileList.Add("    api\\BaseController.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\Controllers\\api", "BaseController.cs"), SaveActions.DontOverwrite);
        }

        private void BuildControllerCodeTemplate()
        {
            // Am I building a PIA patterned code template, no. (false)
            bool useDtos = false;
            if (_autoMapperFramework != null)
                useDtos = true;

            

            ICodeTemplateBuilder builder;
            if (_criteria == null)
            {
                builder = new KingsMvcServiceLibraryCodeTemplatesForDbContext(_context, false, useDtos);
                builder.RenderControllerTemplate();
                builder.RenderAspNetViewTemplates();
                builder.RenderRazorViewTemplates();
                builder.RenderWebApiTemplates();
            }
            else
            {
                if (_criteria.MvcVersion >= 5.0)
                {
                    builder = new KingsMvc5ServiceLibraryCodeTemplatesForDbContext(_context, _criteria);
                    builder.RenderControllerTemplate();
                    builder.RenderAspNetViewTemplates();
                    builder.RenderRazorViewTemplates();
                    builder.RenderWebApiTemplates();    
                }
                else
                {
                    builder = new KingsMvcServiceLibraryCodeTemplatesForDbContext(_context, false, useDtos);
                    builder.RenderControllerTemplate();
                    builder.RenderAspNetViewTemplates();
                    builder.RenderRazorViewTemplates();
                    builder.RenderWebApiTemplates();
                }
            }



        }

        private void CreateJsonResponseClass()
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".ViewData");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class JsonResponse");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public bool Success { get; set; }");
            _output.autoTabLn("public string Message { get; set; }");
            _output.autoTabLn("public bool Exists { get; set; }");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    JsonResponse.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\ViewData", "JsonResponse.cs"), SaveActions.DontOverwrite);
        }

        private void CreateConfigSettingsClass()
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Configuration;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.Namespace + ".Core");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class ConfigSettings");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static string SiteTitle { get { return ConfigurationManager.AppSettings[\"SiteTitle\"]; } }");
            _output.autoTabLn("public static string MetaKeywords { get { return ConfigurationManager.AppSettings[\"MetaKeywords\"]; } }");
            _output.autoTabLn("public static string MetaDescription { get { return ConfigurationManager.AppSettings[\"MetaDescription\"]; } }");
            _output.autoTabLn("public static string FromEmail { get { return ConfigurationManager.AppSettings[\"FromEmail\"]; } }");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    ConfigSettings.cs");
            SaveOutput(CreateFullPath(_script.Settings.Namespace + ".Core", "ConfigSettings.cs"), SaveActions.DontOverwrite);
        }

        private void CreateCommonClass()
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System.Web;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.Namespace + ".Core");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class Common");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static string GetSiteUrl()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("HttpContext current = HttpContext.Current;");
            _output.autoTabLn("string str = current.Request.ServerVariables[\"SERVER_PORT\"];");
            _output.autoTabLn("switch (str)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("case null:");
            _output.autoTabLn("case \"80\":");
            _output.autoTabLn("case \"443\":");
            _output.tabLevel++;
            _output.autoTabLn("str = \"\";");
            _output.autoTabLn("break;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("default:");
            _output.tabLevel++;
            _output.autoTabLn("str = \":\" + str;");
            _output.autoTabLn("break;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("string str2 = current.Request.ServerVariables[\"SERVER_PORT_SECURE\"];");
            _output.autoTabLn("switch (str2)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("case null:");
            _output.autoTabLn("case \"0\":");
            _output.tabLevel++;
            _output.autoTabLn("str2 = \"http://\";");
            _output.autoTabLn("break;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("default:");
            _output.tabLevel++;
            _output.autoTabLn("str2 = \"https://\";");
            _output.autoTabLn("break;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("string applicationPath = current.Request.ApplicationPath;");
            _output.autoTabLn("if (applicationPath == \"/\")");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("applicationPath = \"\";");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("return (str2 + current.Request.ServerVariables[\"SERVER_NAME\"] + str + applicationPath);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    Common.cs");
            SaveOutput(CreateFullPath(_script.Settings.Namespace + ".Core", "Common.cs"), SaveActions.DontOverwrite);
        }

        private void BuildViewDataFactory()
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".ViewData");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class ViewDataFactory : BaseViewDataBuilder");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static T CreateBaseViewData<T>(string pageTitle) where T : BaseViewData, new()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T viewData = CreateViewData<T>(pageTitle);");
            _output.autoTabLn("");
            _output.autoTabLn("return viewData;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    ViewDataFactory.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\ViewData", "ViewDataFactory.cs"), SaveActions.DontOverwrite);
        }

        private void BuildBaseViewDataBuilderClass()
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using " + _script.Settings.Namespace + ".Core;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".ViewData");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class BaseViewDataBuilder");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static T CreateViewData<T>(string pageTitle) where T : BaseViewData, new()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T viewData = new T");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("SiteTitle = ConfigSettings.SiteTitle,");
            _output.autoTabLn("MetaKeywords = ConfigSettings.MetaKeywords,");
            _output.autoTabLn("MetaDescription = ConfigSettings.MetaDescription,");
            _output.autoTabLn("PageTitle = pageTitle");
            _output.tabLevel--;
            _output.autoTabLn("};");
            _output.autoTabLn("return viewData;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    BaseViewDataBuilder.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\ViewData", "BaseViewDataBuilder.cs"), SaveActions.DontOverwrite);
        }

        private void BuildBaseViewDataClass()
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".ViewData");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public abstract class BaseViewData");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public string SiteTitle { get; set; }");
            _output.autoTabLn("public string MetaKeywords { get; set; }");
            _output.autoTabLn("public string MetaDescription { get; set; }");
            _output.autoTabLn("public string PageTitle { get; set; }");
            _output.autoTabLn("public bool IsAdmin { get; set; }");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    BaseViewData.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\ViewData", "BaseViewData.cs"), SaveActions.DontOverwrite);
        }

        private void BuildViewDataClass(MyMeta.ITable table, bool useUIDtos)
        {

            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web;");
            _output.autoTabLn("");
            if (useUIDtos)
            {
                _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".Models;");
            }
            else
            {
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            }
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.UI.UINamespace + ".ViewData");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "ViewData : BaseViewData");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + ((useUIDtos) ? "Model" : "") + " " + StringFormatter.CleanUpClassName(table.Name) + " { get; set; }");
            _output.autoTabLn("public List<" + StringFormatter.CleanUpClassName(table.Name) + ((useUIDtos) ? "Model" : "") + "> " + StringFormatter.CleanUpClassName(table.Name) + "List { get; set; }");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "ViewData.cs");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\ViewData", StringFormatter.CleanUpClassName(table.Name) + "ViewData.cs"), SaveActions.DontOverwrite);
        }

        #endregion
    }
}
