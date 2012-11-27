using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace WebApi4.Components.ServiceLayer
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
            _context.Dialog.InitDialog(_script.Tables.Count);
            _context.FileList.Add("");

            //RenderFormatterClass();
            //RenderRouteConfigClass();
            //RenderGlobalAsaxClass();
            RenderUnityWebApiConfigurationClass();
            RenderBaseControllerClass();

            _context.Dialog.InitDialog(_script.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi4 Controller classes: ");
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding " + tableName + "Controller class");
                ITable table = _context.Database.Tables[tableName];
                RenderApiControllerClass(table);
            }
        }


        #endregion

        #region Private Methods

        private void RenderApiControllerClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Linq.Expressions;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".Controllers");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// This is the WebApi controller for the " + StringFormatter.CleanUpClassName(table.Name) + " entity.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "Controller : BaseController<" + StringFormatter.CleanUpClassName(table.Name) + ">");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("private readonly I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " _" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
            _output.autoTabLn("private readonly IUnitOfWork _unitOfWork;");
            _output.autoTabLn("");
            _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + "Controller(I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ", IUnitOfWork unitOfWork) : base(" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ", unitOfWork)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this._unitOfWork = unitOfWork;");
            _output.autoTabLn("this._" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + " = " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Custom Actions");
            _output.autoTabLn("");
            _output.autoTabLn("///// <summary>");
            _output.autoTabLn("///// This is a custom action that returns Product by category.");
            _output.autoTabLn("///// Check how the route is created in the RouteConfig.cs file in the App_Start folder.");
            _output.autoTabLn("///// </summary>");
            _output.autoTabLn("///// <param name=\"category\">Category Name</param>");
            _output.autoTabLn("///// <returns></returns>");
            _output.autoTabLn("//public List<Product> GetProductsByCategory(string category)");
            _output.autoTabLn("//{");
            _output.autoTabLn("//	Expression<Func<Product, bool>> exp = null;");
            _output.autoTabLn("//	if (!string.IsNullOrEmpty(category))");
            _output.autoTabLn("//		exp = c => c.Category.CategoryName.ToLower() == category.ToLower();");
            _output.autoTabLn("");
            _output.autoTabLn("//	return _productRepository.Get(exp).ToList();");
            _output.autoTabLn("//}");
            _output.autoTabLn("");
            _output.autoTabLn("///// <summary>");
            _output.autoTabLn("///// Get a Product by name.  Check the RouteConfig.cs file for routing.");
            _output.autoTabLn("///// </summary>");
            _output.autoTabLn("///// <param name=\"name\">Product Name</param>");
            _output.autoTabLn("///// <returns></returns>");
            _output.autoTabLn("//public Product GetProductByName(string name)");
            _output.autoTabLn("//{");
            _output.autoTabLn("");
            _output.autoTabLn("//	Product product = _productRepository.Get(p => p.Name.ToLower() == name.ToLower()).SingleOrDefault();");
            _output.autoTabLn("//	if (product == null)");
            _output.autoTabLn("//		throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));");
            _output.autoTabLn("");
            _output.autoTabLn("//	return product;");
            _output.autoTabLn("//}");
            _output.autoTabLn("");
            _output.autoTabLn("///// <summary>");
            _output.autoTabLn("///// Calculate the tax for a product. (Example PUT method.)");
            _output.autoTabLn("///// </summary>");
            _output.autoTabLn("///// <param name=\"name\">Product model</param>");
            _output.autoTabLn("///// <returns></returns>");
            _output.autoTabLn("//[HttpPut]");
            _output.autoTabLn("//public Product CalculateTax(Product model)");
            _output.autoTabLn("//{");
            _output.autoTabLn("//	if (ModelState.IsValid)");
            _output.autoTabLn("//	{");
            _output.autoTabLn("//		try");
            _output.autoTabLn("//		{");
            _output.autoTabLn("//			var tax = model.Amount * _taxRate;");
            _output.autoTabLn("//			_groupDao.UpdateTax(tax);");
            _output.autoTabLn("//			_unitOfWork.ActionEntities.Commit();");
            _output.autoTabLn("//			");
            _output.autoTabLn("//		}");
            _output.autoTabLn("//		catch (Exception)");
            _output.autoTabLn("//		{");
            _output.autoTabLn("//			return Request.CreateResponse(HttpStatusCode.NotFound);");
            _output.autoTabLn("//		}");
            _output.autoTabLn("//		return Request.CreateResponse(HttpStatusCode.OK, model);");
            _output.autoTabLn("//	}");
            _output.autoTabLn("//	else");
            _output.autoTabLn("//	{");
            _output.autoTabLn("//		return Request.CreateResponse(HttpStatusCode.BadRequest);");
            _output.autoTabLn("//	}");
            _output.autoTabLn("//}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Controller.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Controllers", StringFormatter.CleanUpClassName(table.Name) + "Controller.cs"), SaveActions.DontOverwrite);
        }

        private void RenderBaseControllerClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Net;");
            _output.autoTabLn("using System.Net.Http;");
            _output.autoTabLn("using System.Web.Http;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Generated;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".Controllers");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// This is the common base ApiController used for all controllers.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <typeparam name=\"T\"></typeparam>");
            _output.autoTabLn("public class BaseController<T> : ApiController where T : Entity");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("private readonly IRepository<T> _repository;");
            _output.autoTabLn("private readonly IUnitOfWork _unitOfWork;");
            _output.autoTabLn("");
            _output.autoTabLn("public BaseController(IRepository<T> repository, IUnitOfWork unitOfWork)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this._unitOfWork = unitOfWork;");
            _output.autoTabLn("this._repository = repository;");
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
            _output.autoTabLn("return _repository.Get();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Get the selected Product.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"id\">Id</param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public T Get(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T model = _repository.GetById(id);");
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
            _output.autoTabLn("public HttpResponseMessage Post(T model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (ModelState.IsValid)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_repository.Insert(model);");
            _output.autoTabLn("_unitOfWork." + _script.Settings.DataOptions.DataContext.Name + ".Commit();");
            _output.autoTabLn("");
            _output.autoTabLn("var response = Request.CreateResponse(HttpStatusCode.Created, model);");
            _output.autoTabLn("response.Headers.Location = new Uri(Url.Link(\"DefaultApi\", new { id = model.Id }));");
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
            _output.autoTabLn("public HttpResponseMessage Put(T model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (ModelState.IsValid)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_repository.Update(model);");
            _output.autoTabLn("_unitOfWork." + _script.Settings.DataOptions.DataContext.Name + ".Commit();");
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
            _output.autoTabLn("public HttpResponseMessage Delete(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T model = _repository.GetById(id);");
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
            _output.autoTabLn("_repository.Delete(model);");
            _output.autoTabLn("_unitOfWork." + _script.Settings.DataOptions.DataContext.Name + ".Commit();");
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
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    BaseController.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Controllers", "BaseController.cs"), SaveActions.Overwrite);
        }

        private void RenderGlobalAsaxClass()
        {
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web.Http;");
            _output.autoTabLn("using System.Web.Routing;");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class Global : System.Web.HttpApplication");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("protected void Application_Start(object sender, EventArgs e)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("RouteConfig.RegisterRoutes(RouteTable.Routes);");
            _output.autoTabLn("FormattersConfig.RegisterFormatters(GlobalConfiguration.Configuration);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    Global.asax.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace, "Global.asax.cs"), SaveActions.DontOverwrite);

            _output.writeln("<%@ Application Codebehind=\"Global.asax.cs\" Inherits=\"" + _script.Settings.ServiceLayer.ServiceNamespace + ".Global\" Language=\"C#\" %>");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace, "Global.asax"), SaveActions.DontOverwrite);
        }

        private void RenderUnityWebApiConfigurationClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("[assembly: WebActivator.PreApplicationStartMethod(typeof(" + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start.UnityWebApi), \"Start\")]");
            _output.autoTabLn("[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(" + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start.UnityWebApi), \"Stop\")]");
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web.Http;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Repositories;");
            _output.autoTabLn("using Microsoft.Practices.Unity;");
            _output.autoTabLn("");
            _output.autoTabLn("public static class UnityWebApi");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("private static IUnityContainer container;");
            _output.autoTabLn("");
            _output.autoTabLn("#region Public Methods");
            _output.autoTabLn("public static void Start()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("Initialise();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public static void Stop()");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("} ");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Private Methods");
            _output.autoTabLn("private static void Initialise()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("container = BuildUnityContainer();");
            _output.autoTabLn("");
            _output.autoTabLn("GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("private static IUnityContainer BuildUnityContainer()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("var container = new UnityContainer();");
            _output.autoTabLn("");

            foreach (string tableName in _script.Tables)
            {
                ITable table = _context.Database.Tables[tableName];
                _output.autoTabLn("container.RegisterType<I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ", " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ">();");
            }
            _output.autoTabLn("container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());");

            _output.autoTabLn("");
            _output.autoTabLn("return container;");
            _output.tabLevel--;
            _output.autoTabLn("} ");
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    UnityWebApi.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\App_Start", "UnityWebApi.cs"), SaveActions.Overwrite);
        }

        private void RenderRouteConfigClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web.Http;");
            _output.autoTabLn("using System.Web.Routing;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Register your routes here.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("public class RouteConfig");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static void RegisterRoutes(RouteCollection routes)");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("// This is the default route.");
            _output.autoTabLn("routes.MapHttpRoute(");
            _output.tabLevel++;
            _output.autoTabLn("name: \"DefaultApi\",");
            _output.autoTabLn("routeTemplate: \"api/{controller}/{id}\",");
            _output.autoTabLn("defaults: new { id = RouteParameter.Optional }");
            _output.tabLevel--;
            _output.autoTabLn(");");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    RouteConfig.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\App_Start", "RouteConfig.cs"), SaveActions.DontOverwrite);
        }

        private void RenderFormatterClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web.Http;");
            _output.autoTabLn("using WebApiContrib.Formatting.Jsonp;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".App_Start");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Register your formatters here.");
            _output.autoTabLn("/// For the Jsonp formatter, install the NuGet package: install-package WebApiContrib.Formatting.Jsonp");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("public class FormattersConfig");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static void RegisterFormatters(HttpConfiguration config)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("config.Formatters.Insert(0, new JsonpMediaTypeFormatter());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    FormattersConfig.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\App_Start", "FormattersConfig.cs"), SaveActions.DontOverwrite);
        }


        #endregion
    }
}

