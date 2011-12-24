using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;

namespace GizmoBeach.Components.UI
{
    /// <summary>
    /// This class renders the T4 controller template to use the ViewData classes
    /// that communicates with either a library service or WCF services.
    /// </summary>
    public class KingsMvcServiceLibraryCodeTemplates : RenderBase, ICodeTemplateBuilder
    {

        #region ctors
        private readonly RequestContext _context;

        private readonly bool _useWebServices;

        private readonly bool _useUIDtos;

        /// <summary>
        /// This class renders the T4 controller template to use the ViewData classes
        /// that communicates with either a library service or WCF services.
        /// </summary>
        /// <param name="context">RequestContext object</param>
        /// <param name="useWebServices">Whether this template uses WCF of a service library</param>
        public KingsMvcServiceLibraryCodeTemplates(RequestContext context, bool useWebServices)
            : this(context, useWebServices, false)
        {
        }

        /// <summary>
        /// This class renders the T4 controller template to use the ViewData classes
        /// that communicates with either a library service or WCF services.
        /// </summary>
        /// <param name="context">RequestContext object</param>
        /// <param name="useWebServices">Whether this template uses WCF of a service library</param>
        /// <param name="useUIDtos">Is this application using DTOs?</param>
        public KingsMvcServiceLibraryCodeTemplates(RequestContext context, bool useWebServices, bool useUIDtos)
            : base(context.Zeus.Output)
        {
            this._useUIDtos = useUIDtos;
            this._useWebServices = useWebServices;
            this._context = context;
        } 
        #endregion

        #region ICodeTemplateBuilder Members

        public void RenderControllerTemplate()
        {
            _output.autoTabLn("/*");
            _output.autoTabLn("!!!!! NOTICE !!!!!");
            _output.autoTabLn("This code template uses the T4MVC framework.  To install using NuGet (http://nuget.org),");
            _output.autoTabLn("search for \"T4MVC\" and install that package.  It makes your applications stronger ");
            _output.autoTabLn("in that it's easier to find errors in your controller references, and it prevents");
            _output.autoTabLn("the use of Magic Strings.");
            _output.autoTabLn("");
            _output.autoTabLn("Go here for information: http://mvccontrib.codeplex.com/wikipage?title=T4MVC");
            _output.autoTabLn("*/");
            _output.autoTabLn("<#@ template language=\"C#\" HostSpecific=\"True\" #>");
            _output.autoTabLn("<#");
            _output.autoTabLn("MvcTextTemplateHost mvcHost = (MvcTextTemplateHost)(Host);");
            _output.autoTabLn("#>");
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Web.Mvc;");
            _output.autoTabLn("");
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".Models;");
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".ViewData;");
            _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".Controllers;");
            if (_useWebServices)
            {
                _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".Repositories.Core;");
                _output.autoTabLn("using " + _script.Settings.UI.UINamespace + ".ActionServiceReference;");
            }
            else
            {
                _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces;");
                _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            }
            _output.autoTabLn("");
            _output.autoTabLn("namespace <#= mvcHost.Namespace #>");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class <#= mvcHost.ControllerName #> : BaseController");
            _output.autoTabLn("{");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("");
            _output.autoTabLn("private I<#= mvcHost.ControllerRootName #>Service <#= mvcHost.ControllerRootName #>Service;");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("public <#= mvcHost.ControllerName #>(I<#= mvcHost.ControllerRootName #>Service <#= mvcHost.ControllerRootName #>Service)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this.<#= mvcHost.ControllerRootName #>Service = <#= mvcHost.ControllerRootName #>Service;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("public ActionResult Index()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= mvcHost.ControllerRootName #>ViewData>(\"<#= mvcHost.ControllerRootName #> List\");");
            _output.autoTabLn("viewData.<#= mvcHost.ControllerRootName #>List = <#= mvcHost.ControllerRootName #>Service.GetAll()" + WriteToModel() + ";");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("<#");
            _output.autoTabLn("if(mvcHost.AddActionMethods) {");
            _output.autoTabLn("#>");
            _output.tabLevel++;
            _output.autoTabLn("public ActionResult Details(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= mvcHost.ControllerRootName #>ViewData>(\"<#= mvcHost.ControllerRootName #> Details\");");
            _output.autoTabLn("viewData.<#= mvcHost.ControllerRootName #> = <#= mvcHost.ControllerRootName #>Service.GetById(id)" + WriteToModel() + ";");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("public ActionResult Create()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= mvcHost.ControllerRootName #>ViewData>(\"Create <#= mvcHost.ControllerRootName #>\");");
            _output.autoTabLn("viewData.<#= mvcHost.ControllerRootName #> = new <#= mvcHost.ControllerRootName #>()" + WriteToModel() + ";");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("} ");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("[HttpPost]");
            _output.autoTabLn("public ActionResult Create([Bind(Exclude=\"Id,rowversion\")]<#= mvcHost.ControllerRootName #>" + ((_useUIDtos) ? "Model" : "") + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (!ModelState.IsValid)");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(Actions.Create());");
            _output.tabLevel--;
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn(((_useUIDtos) ? "var localModel = model.FromModel();" : ""));
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>Service.Insert(" + ((_useUIDtos) ? "localModel" : "model") + ");");
            _output.autoTabLn("");
            _output.autoTabLn("return RedirectToAction(Actions.Index());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(Actions.Create());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public ActionResult Edit(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= mvcHost.ControllerRootName #>ViewData>(\"Edit <#= mvcHost.ControllerRootName #>\");");
            _output.autoTabLn("viewData.<#= mvcHost.ControllerRootName #> = <#= mvcHost.ControllerRootName #>Service.GetById(id)" + WriteToModel() + ";");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("[HttpPost]");
            _output.autoTabLn("public ActionResult Edit(<#= mvcHost.ControllerRootName #>" + ((_useUIDtos) ? "Model" : "") + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (!ModelState.IsValid)");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(Actions.Edit(model.Id));");
            _output.tabLevel--;
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn(((_useUIDtos) ? "var localModel = model.FromModel();" : ""));
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>Service.Update(" + ((_useUIDtos) ? "localModel" : "model") + ");");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(Actions.Index());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(Actions.Edit(model.Id));");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("public ActionResult Delete(int id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>ViewData viewData = ViewDataFactory.CreateBaseViewData<<#= mvcHost.ControllerRootName #>ViewData>(\"Delete <#= mvcHost.ControllerRootName #>\");");
            _output.autoTabLn("viewData.<#= mvcHost.ControllerRootName #> = <#= mvcHost.ControllerRootName #>Service.GetById(id)" + WriteToModel() + ";");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("return View(viewData);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("[HttpPost]");
            _output.autoTabLn("public ActionResult Delete(<#= mvcHost.ControllerRootName #>" + ((_useUIDtos) ? "Model" : "") + " model)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("try");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(((_useUIDtos) ? "var localModel = model.FromModel();" : ""));
            _output.autoTabLn("<#= mvcHost.ControllerRootName #>Service.Delete(" + ((_useUIDtos) ? "localModel" : "model") + ");");
            _output.autoTabLn("return RedirectToAction(Actions.Index());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("catch (Exception)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return RedirectToAction(Actions.Delete(model.Id));");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("<#");
            _output.autoTabLn("}");
            _output.autoTabLn("#>");
            
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    Controller.tt");
            SaveOutput(CreateFullPath(_script.Settings.UI.UINamespace + "\\CodeTemplates\\AddController", "Controller.tt"), SaveActions.DontOverwrite);	
        }

        public void RenderAspNetViewTemplates()
        {
            
        }

        public void RenderRazorViewTemplates()
        {
            
        }

        #endregion

        #region Private Methods

        private string WriteToModel()
        {
            string result = string.Empty;
            if (_useUIDtos)
            {
                result = ".ToModel()";
            }

            return result;
        }

        #endregion
    }
}
