using System;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Core.PropertyObjects;
using MyMeta;
using WebApi4.Components.Interfaces;

namespace WebApi4.Components.BusinessObjects
{
    /// <summary>
    /// The WebApi base class is used by derived classes to create business objects that are
    /// good for ASP.NET MVC 4 or higher and WebApi uses equally.  It has an Entity base class
    /// that all business objects inherit that contains the primary key (id) property.  This 
    /// is available normally in MVC applications, but available in WebApi base CRUD class.
    /// </summary>
    public class WebApiBusinessObjectsBase : RenderBase, IBusinessObjects
    {
        #region ctors
        private readonly RequestContext _context;

        private readonly IEntityClass _entity;

        public WebApiBusinessObjectsBase(RequestContext context, IEntityClass entity)
            : base(context.Zeus.Output)
        {
            this._context = context;
            this._entity = entity;
        }

        public WebApiBusinessObjectsBase(RequestContext context)
            : this(context, new StandardEntityClass(context))
        {

        } 
        #endregion

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog();

            _context.FileList.Add("");
            _context.FileList.Add("Generated WebApi 4 or greater Business Objects");
            foreach (TableItem item in _context.ScriptSettings.Settings.Tables.Tables)
            {
                _context.Dialog.Display("Processing WebApi 4 or greater Business Objects for '" + item.Name + "'");
                ITable table = _context.Database.Tables[item.Name];
                RenderWebApiBusinessObjectsClass(table);
            }

            RenderEntityBaseClass();
        }
  
        private void RenderEntityBaseClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.ComponentModel;");
            _output.autoTabLn("using System.ComponentModel.DataAnnotations;");
            _output.autoTabLn("using System.Web.Mvc;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public abstract class Entity");
            _output.autoTabLn("{");
            _output.tabLevel++;

            
            _entity.Render();
            
            
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("   Entity.cs");
            SaveOutput(CreateFullPath(_script.Settings.BusinessObjects.BusinessObjectsNamespace, "_Entity.cs"), SaveActions.DontOverwrite);
        }

        private void RenderWebApiBusinessObjectsClass(ITable table)
        {
            try
            {
                _hdrUtil.WriteClassHeader(_output);

                /* Include any necessary using references here for this generated code. */
                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Collections.Generic;");
                _output.autoTabLn("using System.ComponentModel;");
                _output.autoTabLn("using System.ComponentModel.DataAnnotations;");
                _output.autoTabLn("using System.Web.Mvc;");
                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
                _output.autoTabLn("{");
                _output.tabLevel++;
                
                _output.autoTab("public class " + StringFormatter.CleanUpClassName(table.Name));

                if (!HasCompositeKey(table))
                {
                    _output.writeln(": Entity");
                }
                else
                {
                    _output.writeln("");
                }

                _output.autoTabLn("{");
                _output.tabLevel++;

                _output.autoTabLn("#region ctors");
                _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + "()");
                _output.autoTabLn("{");
                _output.tabLevel++;

                // Render any collection HashSets in the constructor for foreign key associations.
                BusinessObjectsPropertiesRenderForeignKeyConstructorForDbContext ctor = new BusinessObjectsPropertiesRenderForeignKeyConstructorForDbContext(table, _context);
                ctor.Render();

                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("#endregion");
                _output.autoTabLn("");

                _output.autoTabLn("#region Properties");
                RenderProperties(table);
                _output.autoTabLn("#endregion");

                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");

                _output.tabLevel--;
                _output.autoTabLn("}");

                _context.FileList.Add("   " + StringFormatter.CleanUpClassName(table.Name) + ".cs");
                SaveOutput(CreateFullPath(_script.Settings.BusinessObjects.BusinessObjectsNamespace, StringFormatter.CleanUpClassName(table.Name) + ".cs"), SaveActions.DontOverwrite);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RenderProperties(ITable table)
        {
            BusinessObjectsPropertyRenderDataAnnotationsForDbContext property = null;
            
            foreach (IColumn c in table.Columns)
            {
                property = new BusinessObjectsPropertyRenderDataAnnotationsForDbContext(c, _context, _entity.OmitList);
                property.Render();
            }

            BusinessObjectsPropertiesRenderForeignKeyForDbContext foreignKey = new BusinessObjectsPropertiesRenderForeignKeyForDbContext(table, _context);
            foreignKey.Render();
        }

        #endregion

        #region Helper Methods

        private bool HasCompositeKey(ITable table)
        {
            return table.PrimaryKeys.Count > 1;
        }

        #endregion
    }
}
