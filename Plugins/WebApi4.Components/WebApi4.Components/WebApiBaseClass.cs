using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace WebApi4.Components
{
    /// <summary>
    /// This is a base class so render the WebApi4 controllers and base controller.
    /// This is used by both the WebApi4 stand-alone project, and can be used in
    /// an ASP.NET MVC 4 application that has WebApi4 capabilities.
    /// </summary>
    public class WebApiBaseClass : RenderBase, IRenderObject
    {
        #region ctors

        private readonly RequestContext _context;

        private readonly string _namespaceName;

        /// <summary>
        /// A base class for rendering WebApi4 classes to any particular component.
        /// </summary>
        /// <param name="context">The RequestContext</param>
        /// <param name="namespaceName">The name of the component namespace passed in by the derived class, i.e., Services, UI, etc.</param>
        public WebApiBaseClass(RequestContext context, string namespaceName) : base(context.Zeus.Output)
        {
            this._namespaceName = namespaceName;
            this._context = context;
        }

        #endregion

        #region Interface Implementations

        public void Render()
        {
            _context.Dialog.InitDialog(_script.Tables.Count);
            _context.FileList.Add("");

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
            _output.autoTabLn("namespace " + _namespaceName + ".Controllers");
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
            SaveOutput(CreateFullPath(_namespaceName + "\\Controllers", StringFormatter.CleanUpClassName(table.Name) + "Controller.cs"), SaveActions.DontOverwrite);
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
            _output.autoTabLn("namespace " + _namespaceName + ".Controllers");
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
            _output.autoTabLn("/// Get all the entities in the repository.");
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
            _output.autoTabLn("/// Get the selected entity.");
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
            _output.autoTabLn("/// Insert a new model into the repository.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"model\">model</param>");
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
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.BadRequest);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Update the selected model.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"model\">model</param>");
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
            _output.autoTabLn("/// Alternate generic Put method with an Id argument.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"id\"></param>");
            _output.autoTabLn("/// <param name=\"model\"></param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public HttpResponseMessage Put(int id, T model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (ModelState.IsValid && id == model.Id)");
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



            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Delete the selected model by Id.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"id\">Id</param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public HttpResponseMessage Delete(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T model = _repository.GetById(id);");
            _output.autoTabLn("return Delete(model);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Delete the selected model via the model.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <param name=\"model\"></param>");
            _output.autoTabLn("/// <returns></returns>");
            _output.autoTabLn("public HttpResponseMessage Delete(T model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("T localModel = _repository.GetById(model.Id);");
            _output.autoTabLn("if (localModel == null)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.NotFound);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_repository.Delete(localModel);");
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
            _output.autoTabLn("return Request.CreateResponse(HttpStatusCode.OK, localModel);");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    BaseController.cs");
            SaveOutput(CreateFullPath(_namespaceName + "\\Controllers", "BaseController.cs"), SaveActions.Overwrite);
        }

        private void RenderUnityWebApiConfigurationClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("[assembly: WebActivator.PreApplicationStartMethod(typeof(" + _namespaceName + ".App_Start.UnityWebApi), \"Start\")]");
            _output.autoTabLn("[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(" + _namespaceName + ".App_Start.UnityWebApi), \"Stop\")]");
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _namespaceName + ".App_Start");
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
            SaveOutput(CreateFullPath(_namespaceName + "\\App_Start", "UnityWebApi.cs"), SaveActions.Overwrite);
        }

        #endregion
    }
}
