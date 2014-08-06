using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.UI
{
    /// <summary>
    /// This class will render the code templates for the MVC 5 project.  This is necessary because
    /// MVC 5 uses all new T4 templates to generate their controllers, views and WebApi classes.
    /// </summary>
    public class KingsMvc5ServiceLibraryCodeTemplatesForDbContext : RenderBase, ICodeTemplateBuilder
    {

        #region ctors
        private readonly RequestContext _context;

        private readonly UICriteria _criteria;

        /// <summary>
        /// This class will render the code templates for the MVC 5 project.  This is necessary because
        /// MVC 5 uses all new T4 templates to generate their controllers, views and WebApi classes.
        /// </summary>
        public KingsMvc5ServiceLibraryCodeTemplatesForDbContext(RequestContext context)
            :this(context, null)
        {
            
        }

        public KingsMvc5ServiceLibraryCodeTemplatesForDbContext(RequestContext context, UICriteria criteria)
            : base(context.Zeus.Output)
        {
            this._context = context;
            this._criteria = criteria;
        }

        #endregion

        #region Implemented Methods

        public void RenderAspNetViewTemplates()
        {
            
        }

        public void RenderControllerTemplate()
        {
            _output.autoTabLn("<#@ template language=\"C#\" HostSpecific=\"True\" #>");
            _output.autoTabLn("<#@ output extension=\"cs\" #>");
            _output.autoTabLn("<#@ import namespace=\"System\" #>");
            _output.autoTabLn("<#@ parameter type=\"System.String\" name=\"ControllerName\" #>");
            _output.autoTabLn("<#@ parameter type=\"System.String\" name=\"ControllerRootName\" #>");
            _output.autoTabLn("<#@ parameter type=\"System.String\" name=\"Namespace\" #>");
            _output.autoTabLn("<#@ parameter type=\"System.String\" name=\"AreaName\" #>");
            _output.autoTabLn("<#");
            _output.autoTabLn("");
            _output.autoTabLn("string camelCaseServiceName = CamelCaseString(ControllerRootName);");
            _output.autoTabLn("string camelCaseServiceNameWithPrefix = CamelCaseString(ControllerRootName, \"_\");");
            _output.autoTabLn("#>");
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Web;");
            _output.autoTabLn("using System.Web.Mvc;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".ViewData;");
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".Controllers;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using Kendo.Mvc.Extensions;");
            _output.autoTabLn("using Kendo.Mvc.UI;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace <#= Namespace #>");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("[Authorize]");
            _output.autoTabLn("public class <#= ControllerName #> : BaseController");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("");
            _output.autoTabLn("private readonly I<#= ControllerRootName #>Service <#= camelCaseServiceNameWithPrefix #>Service;");
            _output.autoTabLn("private readonly IUnitOfWork _unitOfWork;");
            _output.autoTabLn("");
            _output.autoTabLn("public <#= ControllerName #>(I<#= ControllerRootName #>Service <#= camelCaseServiceName #>Service, IUnitOfWork unitOfWork)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this._unitOfWork = unitOfWork;");
            _output.autoTabLn("this.<#= camelCaseServiceNameWithPrefix #>Service = <#= camelCaseServiceName #>Service;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region MVC Actions");
            _output.autoTabLn("");
            _output.autoTabLn("public ActionResult Index()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= ControllerRootName #>ViewData>(\"<#= ControllerRootName #> List\");");
            _output.autoTabLn("viewData.<#= ControllerRootName #>List = <#= camelCaseServiceNameWithPrefix #>Service.Get().ToList();");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public ActionResult Details(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= ControllerRootName #>ViewData>(\"<#= ControllerRootName #> Details\");");
            _output.autoTabLn("viewData.<#= ControllerRootName #> = <#= camelCaseServiceNameWithPrefix #>Service.GetById(id);");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public ActionResult Create()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= ControllerRootName #>ViewData>(\"Create <#= ControllerRootName #>\");");
            _output.autoTab("viewData.<#= ControllerRootName #> = new <#= ControllerRootName #>() { ");
            
            if (_criteria.UseTracking)
	        {
                _output.write("UpdatedBy = User.Identity.Name, UpdatedOn = DateTime.Now");
	        }
            _output.writeln("};");
            
        
        
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("} ");
            _output.autoTabLn("");
            _output.autoTabLn("[HttpPost]");
            _output.autoTabLn("[ValidateAntiForgeryToken]");
            _output.autoTabLn("public ActionResult Create([Bind(Exclude=\"Id,rowversion\")]<#= ControllerRootName #> model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (!ModelState.IsValid)");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(\"Create\");");
            _output.tabLevel--;
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("<#= camelCaseServiceNameWithPrefix #>Service.Insert(model);");
            _output.autoTabLn("_unitOfWork.Commit();");
            _output.autoTabLn("");
            _output.autoTabLn("return RedirectToAction(\"Index\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception ex)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("ModelState.AddModelError(\"\", ex.Message);");
            _output.autoTabLn("return RedirectToAction(\"Create\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public ActionResult Edit(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= ControllerRootName #>ViewData>(\"Edit <#= ControllerRootName #>\");");
            _output.autoTabLn("viewData.<#= ControllerRootName #> = <#= camelCaseServiceNameWithPrefix #>Service.GetById(id);");
            if (_criteria.UseTracking)
            {
                _output.autoTabLn("viewData.<#= ControllerRootName #>.UpdatedOn = DateTime.Now;");
            }
            _output.autoTabLn("");
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("[HttpPost]");
            _output.autoTabLn("[ValidateAntiForgeryToken]");
            _output.autoTabLn("public ActionResult Edit(" + ((_criteria.UseTracking) ? "[Bind(Prefix=\"<#= ControllerRootName #>\")]" : "") + "<#= ControllerRootName #> model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (!ModelState.IsValid)");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(\"Edit\", new { id = model.Id });");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("<#= camelCaseServiceNameWithPrefix #>Service.Update(model);");
            _output.autoTabLn("_unitOfWork.Commit();");
            _output.autoTabLn("");
            _output.autoTabLn("return RedirectToAction(\"Index\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception ex)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("ModelState.AddModelError(\"\", ex.Message);");
            _output.autoTabLn("return RedirectToAction(\"Edit\", new { id = model.Id });");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("public ActionResult Delete(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= ControllerRootName #>ViewData>(\"Delete <#= ControllerRootName #>\");");
            _output.autoTabLn("viewData.<#= ControllerRootName #> = <#= camelCaseServiceNameWithPrefix #>Service.GetById(id);");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("[HttpPost]");
            _output.autoTabLn("[ValidateAntiForgeryToken]");
            _output.autoTabLn("public ActionResult Delete([Bind(Prefix=\"<#= ControllerRootName #>\")]<#= ControllerRootName #> model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("");
            _output.autoTabLn("<#= camelCaseServiceNameWithPrefix #>Service.Delete(model.Id);");
            _output.autoTabLn("_unitOfWork.Commit();");
            _output.autoTabLn("");
            _output.autoTabLn("return RedirectToAction(\"Index\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(\"Delete\", new { id = model.Id });");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Ajax Grid Paging Methods");
            _output.autoTabLn("");
            _output.autoTabLn("//Used with KendoMvc.UI components");
            _output.autoTabLn("//public ActionResult _Get<#= ControllerRootName #>List([DataSourceRequest] DataSourceRequest request)");
            _output.autoTabLn("//{");
            _output.autoTabLn("//	return Json(<#= camelCaseServiceNameWithPrefix #>Service.Get<#= ControllerRootName #>ListForIndex().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);");
            _output.autoTabLn("//}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("<#+");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("public string CamelCaseString(string input)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return CamelCaseString(input, \"\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public string CamelCaseString(string input, string prefix)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("string firstChar = input.Substring(0, 1);");
            _output.autoTabLn("string remaining = input.Substring(1);");
            _output.autoTabLn("");
            _output.autoTabLn("string result = firstChar.ToLower() + remaining;");
            _output.autoTabLn("");
            _output.autoTabLn("if (!string.IsNullOrEmpty(prefix))");
            _output.tabLevel++;
            _output.autoTabLn("result = prefix + result;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return result;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("#>");

            _context.FileList.Add("    MvcControllerWithActions\\Controller.cs.t4");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\CodeTemplates\\MvcControllerWithActions", "Controller.cs.t4"), SaveActions.DontOverwrite);	
        }

        public void RenderRazorViewTemplates()
        {
            
        } 

        public void RenderWebApiTemplates()
        {


            _output.autoTabLn("<#@ template language=\"C#\" HostSpecific=\"True\" #>");
            _output.autoTabLn("<#@ output extension=\"cs\" #>");
            _output.autoTabLn("<#@ parameter type=\"System.String\" name=\"ControllerName\" #>");
            _output.autoTabLn("<#@ parameter type=\"System.String\" name=\"ControllerRootName\" #>");
            _output.autoTabLn("<#@ parameter type=\"System.String\" name=\"Namespace\" #>");
            _output.autoTabLn("<#");
            _output.autoTabLn("");
            _output.autoTabLn("string camelCaseServiceName = CamelCaseString(ControllerRootName);");
            _output.autoTabLn("string camelCaseServiceNameWithPrefix = CamelCaseString(ControllerRootName, \"_\");");
            _output.autoTabLn("#>");
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Net;");
            _output.autoTabLn("using System.Net.Http;");
            _output.autoTabLn("using System.Web.Http;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace <#= Namespace #>");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class <#= ControllerName #> : BaseController<<#= ControllerRootName #>>");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("");
            _output.autoTabLn("private readonly I<#= ControllerRootName #>Service <#= camelCaseServiceNameWithPrefix #>Service;");
            _output.autoTabLn("");
            _output.autoTabLn("private readonly IUnitOfWork _unitOfWork;");
            _output.autoTabLn("");
            _output.autoTabLn("public <#= ControllerName #>(I<#= ControllerRootName #>Service <#= camelCaseServiceName #>Service, IUnitOfWork unitOfWork)");
            _output.tabLevel++;
            _output.autoTabLn(":base(<#= camelCaseServiceName #>Service, unitOfWork)");
            _output.tabLevel--;
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this.<#= camelCaseServiceNameWithPrefix #>Service = <#= camelCaseServiceName #>Service;");
            _output.autoTabLn("this._unitOfWork = unitOfWork;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Public Methods");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("<#+");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("public string CamelCaseString(string input)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return CamelCaseString(input, \"\");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public string CamelCaseString(string input, string prefix)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("string firstChar = input.Substring(0, 1);");
            _output.autoTabLn("string remaining = input.Substring(1);");
            _output.autoTabLn("");
            _output.autoTabLn("string result = firstChar.ToLower() + remaining;");
            _output.autoTabLn("");
            _output.autoTabLn("if (!string.IsNullOrEmpty(prefix))");
            _output.tabLevel++;
            _output.autoTabLn("result = prefix + result;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("return result;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("#>");

            _context.FileList.Add("    ApiControllerEmpty\\Controller.cs.t4");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\CodeTemplates\\ApiControllerEmpty", "Controller.cs.t4"), SaveActions.DontOverwrite);	
        }
        #endregion
    }
}
