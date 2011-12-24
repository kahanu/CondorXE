using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace WebApi.Components.ServiceLayer
{
    public class WebApiServiceLayerObjects : RenderBase, IServiceObjects
    {
        #region ctors
        private readonly RequestContext _context;

        public WebApiServiceLayerObjects(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }
        #endregion

        #region IRenderObject Members

        public void Render()
        {

            _context.Dialog.InitDialog(5);
            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi Service Layer App_Start classes: ");

            _context.Dialog.Display("Adding NinjectLoader() class");
            RenderNinjectLoaderClass();

            _context.Dialog.Display("Adding NinjectWebApiConfiguration() class");
            RenderNinjectWebApiConfigurationClass();

            _context.Dialog.Display("Adding RouteTableLoader() class");
            RenderRouteTableLoaderClass();

            _context.Dialog.Display("Adding WebApiStart() class");
            RenderWebApiStartClass();

            _context.Dialog.Display("Adding Extensions class");
            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi Extensions classes: ");
            RenderExtensionsClass();


            _context.Dialog.InitDialog(_script.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi Service base classes: ");

            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding " + tableName + "Service Base class");
                ITable table = _context.Database.Tables[tableName];
                RenderServiceBaseClass(table);
            }

            _context.Dialog.InitDialog(_script.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi Service Layer concrete classes: ");
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding " + tableName + "Service Custom class");
                ITable table = _context.Database.Tables[tableName];
                RenderConcreteClass(table);
            }

            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi configuration: ");
            RenderWebConfig();
        }

        private void RenderWebConfig()
        {
            string connectionStringName = _script.Settings.DataOptions.ORMFramework.Selected + "." + _script.Settings.DataOptions.DataStore.Selected;

            bool isEF = connectionStringName.ToLower().Contains("entityframework");

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<configuration>" +
                  "<appSettings>" +
                    "<!-- The ClientTag is for use with Patterns In Action 4.0 applications, otherwise ignored. -->" + 
                    "<add key=\"ClientTag\" value=\"" + _script.Settings.Namespace + "Tag\" />" +
                    "<add key=\"ConnectionStringName\" value=\"" + connectionStringName + "\" />" +
                    "<add key=\"DataProvider\" value=\"System.Data.SqlClient\" />" +
                  "</appSettings>" +
                  "<connectionStrings>" +
                    BuildConnectionString(connectionStringName, isEF) +
                  "</connectionStrings>" +
                  "<system.web>" + 
                    "<compilation debug=\"true\" targetFramework=\"4.0\"/>" + 
                  "</system.web>" + 
                  "<system.serviceModel>" + 
                    "<serviceHostingEnvironment aspNetCompatibilityEnabled=\"true\"/>" + 
                  "</system.serviceModel>" + 
                  "<system.webServer>" + 
                    "<modules runAllManagedModulesForAllRequests=\"true\">" + 
                    "</modules>" + 
                  "</system.webServer>" + 
                  "</configuration>";

            _output.write(xml);

            _context.FileList.Add("    web.config");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace, "web.config"), SaveActions.DontOverwrite);
        }

        private string BuildConnectionString(string connectionStringName, bool isEF)
        {
            string efConnectionString = "metadata=res://*/" + _script.Settings.DataOptions.ORMFramework.Selected + "." + _script.Settings.DataOptions.DataContext.Name + ".csdl|res://*/" + _script.Settings.DataOptions.ORMFramework.Selected + "." + _script.Settings.DataOptions.DataContext.Name + ".ssdl|res://*/" + _script.Settings.DataOptions.ORMFramework.Selected + "." + _script.Settings.DataOptions.DataContext.Name + ".msl;provider=System.Data.SqlClient;provider connection string=&quot;";
            //string connectionString = string.Format("Data Source=localhost;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True", _script.DatabaseName);
            string connectionString = _script.Settings.DataOptions.ConnectionString + "MultipleActiveResultSets=True";

            string addNode = "<add name=\"{0}\" connectionString=\"{1}\" {2}/>";

            if (!isEF)
            {
                // Linq-To-Sql connection string
                addNode = string.Format(addNode, connectionStringName, connectionString, "");
            }
            else
            {
                // Entity Framework connection string
                addNode = string.Format(addNode, connectionStringName, efConnectionString + connectionString + "&quot;", "providerName=\"System.Data.EntityClient\"");
            }

            return addNode;
        }

        private void RenderExtensionsClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".Extensions");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class StringExtensions");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static string GetPrefix(this string source, string stringToChop)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return source.Replace(stringToChop, \"\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    StringExtensions.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Extensions", "StringExtensions.cs"), SaveActions.DontOverwrite);
        }

        private void RenderWebApiStartClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web.Routing;");
            _output.autoTabLn("using Microsoft.ApplicationServer.Http.Activation;");
            _output.autoTabLn("");
            _output.autoTabLn("[assembly: WebActivator.PreApplicationStartMethod(typeof(" + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start.WebApi), \"Start\")]");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start {");
            _output.tabLevel++;
            _output.autoTabLn("public static class WebApi {");
            _output.tabLevel++;
            _output.autoTabLn("");
            _output.autoTabLn("public static void Start()");
            _output.autoTabLn("{			");
            _output.tabLevel++;
            _output.autoTabLn("HttpServiceHostFactory _factory = null;");
            _output.autoTabLn("");
            _output.autoTabLn("var config = new NinjectWebApiConfiguration();");
            _output.autoTabLn("_factory = new HttpServiceHostFactory() { Configuration = config };");
            _output.autoTabLn("");
            _output.autoTabLn("RouteTable.Routes.ServicesLoader(_factory);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    WebApi.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\App_Start", "WebApi.cs"), SaveActions.DontOverwrite);
        }

        private void RenderRouteTableLoaderClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Reflection;");
            _output.autoTabLn("using System.ServiceModel;");
            _output.autoTabLn("using System.ServiceModel.Activation;");
            _output.autoTabLn("using System.Web.Routing;");
            _output.autoTabLn("using Microsoft.ApplicationServer.Http.Activation;");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Extensions;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class RouteTableLoader");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// This static class loads the services dynamically based on naming conventions.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("public static void ServicesLoader(this RouteCollection routes, HttpServiceHostFactory factory)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()");
            _output.tabLevel++;
            _output.autoTabLn(".Where(type => type.GetCustomAttributes(typeof(ServiceContractAttribute), false).Length > 0);");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("foreach (var item in types)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("string preFix = item.Name.GetPrefix(\"Service\");");
            _output.autoTabLn("string routePreFix = string.Format(\"{0}/\", preFix.ToLower());");
            _output.autoTabLn("");
            _output.autoTabLn("routes.Add(new ServiceRoute(routePreFix, factory, item));");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    RouteTableLoader.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\App_Start", "RouteTableLoader.cs"), SaveActions.DontOverwrite);
        }

        private void RenderNinjectWebApiConfigurationClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using Microsoft.ApplicationServer.Http;");
            _output.autoTabLn("using Ninject;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class NinjectWebApiConfiguration : WebApiConfiguration");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("private IKernel kernel = new StandardKernel(new NinjectLoader());");
            _output.autoTabLn("");
            _output.autoTabLn("public NinjectWebApiConfiguration()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("CreateInstance = (serviceType, context, request) => kernel.Get(serviceType);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    NinjectWebApiConfiguration.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\App_Start", "NinjectWebApiConfiguration.cs"), SaveActions.DontOverwrite);
        }

        private void RenderNinjectLoaderClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".SQLServer;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class NinjectLoader: Ninject.Modules.NinjectModule");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public override void Load()");
            _output.autoTabLn("{");
            _output.tabLevel++;

            foreach (string tableName in _script.Tables)
            {
                ITable table = _context.Database.Tables[tableName];
                _output.autoTabLn("Bind<I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ">().To<" + _script.Settings.DataOptions.DataStore.Selected + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ">();");
            }


            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    NinjectLoader.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\App_Start", "NinjectLoader.cs"), SaveActions.Overwrite);
        }


        #endregion

        #region Private Methods

        private void RenderConcreteClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            try
            {
                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Linq;");
                _output.autoTabLn("using System.Net;");
                _output.autoTabLn("using System.Net.Http;");
                _output.autoTabLn("using System.ServiceModel;");
                _output.autoTabLn("using System.ServiceModel.Web;");
                _output.autoTabLn("using Microsoft.ApplicationServer.Http.Dispatcher;");
                _output.autoTabLn("using System.Text;");
                _output.autoTabLn("");
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
                _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("[ServiceContract(Namespace = \"" + _script.Settings.ServiceLayer.DataContract + "\")]");
                _output.autoTabLn("public partial class " + StringFormatter.CleanUpClassName(table.Name) + "Service ");
                _output.autoTabLn("{");
                _output.autoTabLn("");
                _output.tabLevel++;
                _output.autoTabLn("#region ctors");
                _output.autoTabLn("private readonly I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
                _output.autoTabLn("");
                _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + "Service(I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ")");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("this." + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + " = " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
                _output.tabLevel--;
                _output.autoTabLn("} ");
                _output.autoTabLn("#endregion");
                _output.autoTabLn("");
                _output.autoTabLn("");
                _output.autoTabLn("//[WebGet(UriTemplate = \"/title/{title}\")]");
                _output.autoTabLn("//public HttpResponseMessage<" + StringFormatter.CleanUpClassName(table.Name) + "> GetByTitle(string title)");
                _output.autoTabLn("//{");
                _output.autoTabLn("//	if (string.IsNullOrEmpty(title))");
                _output.autoTabLn("//		throw new HttpResponseException(HttpStatusCode.NotFound);");
                _output.tabLevel++;
                _output.autoTabLn("");
                _output.tabLevel--;
                _output.autoTabLn("//	var " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + " = " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetAll().Where(b => b.Title.ToLower() == title.ToLower()).SingleOrDefault();");
                _output.autoTabLn("//	if (" + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + " == null)");
                _output.autoTabLn("//	{");
                _output.autoTabLn("//		var response = new HttpResponseMessage();");
                _output.autoTabLn("//		response.StatusCode = HttpStatusCode.NotFound;");
                _output.autoTabLn("//		response.Content = new StringContent(\"" + StringFormatter.CleanUpClassName(table.Name) + " not found\");");
                _output.autoTabLn("//		throw new HttpResponseException(response);");
                _output.autoTabLn("//	}");
                _output.autoTabLn("//	var " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + "Response = new HttpResponseMessage<" + StringFormatter.CleanUpClassName(table.Name) + ">(" + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + ");");
                _output.tabLevel++;
                _output.autoTabLn("");
                _output.tabLevel--;
                _output.autoTabLn("//	//set it to expire in 5 minutes");
                _output.autoTabLn("//	" + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + "Response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(30));");
                _output.autoTabLn("//	return " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + "Response;");
                _output.autoTabLn("//}");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("}");

                _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Service.cs");
                SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\api\\v1.0", StringFormatter.CleanUpClassName(table.Name) + "Service.cs"), SaveActions.DontOverwrite);
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering ServiceLayer Concrete class - " + ex.Message);
            }
        }

        private void RenderServiceBaseClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            try
            {
                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Linq;");
                _output.autoTabLn("using System.Net;");
                _output.autoTabLn("using System.Net.Http;");
                _output.autoTabLn("using System.ServiceModel.Web;");
                _output.autoTabLn("using Microsoft.ApplicationServer.Http.Dispatcher;");
                _output.autoTabLn("");
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("");
                _output.autoTabLn("public partial class " + StringFormatter.CleanUpClassName(table.Name) + "Service");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("");
                _output.autoTabLn("#region " + StringFormatter.CleanUpClassName(table.Name) + " Members");
                _output.autoTabLn("");
                _output.autoTabLn("[WebGet(UriTemplate = \"{id}\")]");
                _output.autoTabLn("public HttpResponseMessage<BusinessObjects." + StringFormatter.CleanUpClassName(table.Name) + "> GetById(int id)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("var " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + " = " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetById(id);");
                _output.autoTabLn("if (" + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + " == null)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("var response = new HttpResponseMessage();");
                _output.autoTabLn("response.StatusCode = HttpStatusCode.NotFound;");
                _output.autoTabLn("response.Content = new StringContent(\"Contact not found\");");
                _output.autoTabLn("throw new HttpResponseException(response);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("var " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + "Response = new HttpResponseMessage<" + StringFormatter.CleanUpClassName(table.Name) + ">(" + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + ");");
                _output.autoTabLn("");
                _output.autoTabLn("//set it to expire in 5 minutes");
                _output.autoTabLn(StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + "Response.Content.Headers.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(30));");
                _output.autoTabLn("return " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + "Response;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("[WebGet(UriTemplate=\"\")]");
                _output.autoTabLn("public IQueryable<BusinessObjects." + StringFormatter.CleanUpClassName(table.Name) + "> Get()");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetAll().AsQueryable();");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("[WebInvoke(UriTemplate = \"\", Method = \"POST\")]");
                _output.autoTabLn("public HttpResponseMessage<BusinessObjects." + StringFormatter.CleanUpClassName(table.Name) + "> Post(BusinessObjects." + StringFormatter.CleanUpClassName(table.Name) + " model)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("if (model == null)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("throw new HttpResponseException(HttpStatusCode.NotFound);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn(StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Insert(model);");
                _output.autoTabLn("");
                _output.autoTabLn("var response = new HttpResponseMessage<" + StringFormatter.CleanUpClassName(table.Name) + ">(model);");
                _output.autoTabLn("response.StatusCode = HttpStatusCode.Created;");
                _output.autoTabLn("return response;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("[WebInvoke(UriTemplate = \"{id}\", Method = \"PUT\")]");
                _output.autoTabLn("public BusinessObjects." + StringFormatter.CleanUpClassName(table.Name) + " Put(int id, BusinessObjects." + StringFormatter.CleanUpClassName(table.Name) + " model)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("if (id <= 0)");
                _output.tabLevel++;
                _output.autoTabLn("throw new HttpResponseException(\"id is missing\");");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.autoTabLn("if (model == null)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("var response = new HttpResponseMessage();");
                _output.autoTabLn("response.StatusCode = HttpStatusCode.NotFound;");
                _output.autoTabLn("response.Content = new StringContent(\"Contact not found\");");
                _output.autoTabLn("throw new HttpResponseException(response);");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn(StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Update(model);");
                _output.autoTabLn("model = " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetById(model.Id);");
                _output.autoTabLn("");
                _output.autoTabLn("return model;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("[WebInvoke(UriTemplate = \"{id}\", Method = \"DELETE\")]");
                _output.autoTabLn("public BusinessObjects." + StringFormatter.CleanUpClassName(table.Name) + " Delete(int id)");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("if (id <= 0)");
                _output.tabLevel++;
                _output.autoTabLn("throw new HttpResponseException(\"id is missing\");");
                _output.tabLevel--;
                _output.autoTabLn("");
                _output.autoTabLn("var model = " + StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetById(id);");
                _output.autoTabLn(StringFormatter.CleanUpClassName(StringFormatter.CamelCasing(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Delete(model);");
                _output.autoTabLn("");
                _output.autoTabLn("return model;");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
                _output.autoTabLn("#endregion");
                _output.autoTabLn("");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("}");

                _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Service.cs");
                SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Generated", StringFormatter.CleanUpClassName(table.Name) + "Service.cs"), SaveActions.Overwrite);
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering ServiceBase class - " + ex.Message);
            }

        }


        #endregion
    }
}

