﻿using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.ServiceLayer
{
    public class ServiceClassesForDbContextServiceLayerObjects : RenderBase, IServiceObjects
    {
        #region ctors
        private readonly RequestContext _context;

        public ServiceClassesForDbContextServiceLayerObjects(RequestContext context)
            : base(context.Zeus.Output)
        {
            this._context = context;
        }
        #endregion

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog(_context.Database.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated ServiceClassesForDbContext Service Layer interfaces: ");
            RenderServiceCRUDInterface();
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding I" + tableName + "Service Interface");
                ITable table = _context.Database.Tables[tableName];
                RenderInterface(table);
            }

            _context.Dialog.InitDialog(_context.Database.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated ServiceClassesForDbContext Service Layer Base classes: ");
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding " + tableName + "ServiceBase class");
                ITable table = _context.Database.Tables[tableName];
                RenderServiceBaseClass(table);
            }

            _context.Dialog.InitDialog(_context.Database.Tables.Count);
            _context.FileList.Add("");
            _context.FileList.Add("Generated ServiceClassesForDbContext Service Layer classes: ");
            foreach (string tableName in _script.Tables)
            {
                _context.Dialog.Display("Adding " + tableName + "Service class");
                ITable table = _context.Database.Tables[tableName];
                RenderConcreteClass(table);
            }
        }


  
        

        #endregion

        #region Private Methods

        private void RenderServiceCRUDInterface()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Linq.Expressions;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".Generated");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface IService<TEntity>");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("IEnumerable<TEntity> Get(");
            _output.tabLevel++;
            _output.autoTabLn("Expression<Func<TEntity, bool>> filter = null,");
            _output.autoTabLn("Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,");
            _output.autoTabLn("string includeProperties = \"\");");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("TEntity GetById(object id);");
            _output.autoTabLn("void Insert(TEntity entity);");
            _output.autoTabLn("void Delete(object id);");
            _output.autoTabLn("void Delete(TEntity entityToDelete);");
            _output.autoTabLn("void Update(TEntity entityToUpdate);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    IService.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Generated", "IService.cs"), SaveActions.Overwrite);
        }

        private void RenderInterface(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Generated;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface I" + StringFormatter.CleanUpClassName(table.Name) + "Service : IService<" + StringFormatter.CleanUpClassName(table.Name) + ">");
            _output.autoTabLn("{");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    I" + StringFormatter.CleanUpClassName(table.Name) + "Service.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Interfaces", "I" + StringFormatter.CleanUpClassName(table.Name) + "Service.cs"), SaveActions.DontOverwrite);
        }

        private void RenderServiceBaseClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace +".Generated");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase : IService<" + _context.Utility.BuildModelClassWithNameSpace(table.Name) + ">");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region Constructors");
            _output.autoTabLn("protected readonly I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " _" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
            _output.autoTabLn("");
            _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase(I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ")");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this._" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + " = " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ";");
            _output.tabLevel--;
            _output.autoTabLn("} ");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region CRUD Methods");
            _output.autoTabLn("public IEnumerable<" + StringFormatter.CleanUpClassName(table.Name) + "> Get(System.Linq.Expressions.Expression<Func<" + StringFormatter.CleanUpClassName(table.Name) + ", bool>> filter = null, Func<IQueryable<" + StringFormatter.CleanUpClassName(table.Name) + ">, IOrderedQueryable<" + StringFormatter.CleanUpClassName(table.Name) + ">> orderBy = null, string includeProperties = \"\")");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return _" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Get(filter, orderBy, includeProperties);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + " GetById(object id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return _" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetById(id);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public void Insert(" + StringFormatter.CleanUpClassName(table.Name) + " entity)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Insert(entity);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public void Delete(object id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(StringFormatter.CleanUpClassName(table.Name) + " entity = _" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".GetById(id);");
            _output.autoTabLn("_" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Delete(entity);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public void Delete(" + StringFormatter.CleanUpClassName(table.Name) + " entityToDelete)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Delete(entityToDelete);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public void Update(" + StringFormatter.CleanUpClassName(table.Name) + " entityToUpdate)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ".Update(entityToUpdate);");
            _output.tabLevel--;
            _output.autoTabLn("} ");
            _output.autoTabLn("#endregion");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("");
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace + "\\Generated", StringFormatter.CleanUpClassName(table.Name) + "ServiceBase.cs"), SaveActions.Overwrite);

        }

        private void RenderConcreteClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Generated;");
            _output.autoTabLn("using " + _script.Settings.ServiceLayer.ServiceNamespace + ".Interfaces;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.ServiceLayer.ServiceNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + "Service : " + StringFormatter.CleanUpClassName(table.Name) + "ServiceBase, I" + StringFormatter.CleanUpClassName(table.Name) + "Service");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + "Service(I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " " + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ")");
            _output.tabLevel++;
            _output.autoTabLn(": base(" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(table.Name)) + _script.Settings.DataOptions.ClassSuffix.Name + ")");
            _output.tabLevel--;
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + "Service.cs");
            SaveOutput(CreateFullPath(_script.Settings.ServiceLayer.ServiceNamespace, StringFormatter.CleanUpClassName(table.Name) + "Service.cs"), SaveActions.DontOverwrite);
        }
        #endregion
    }
}

	
