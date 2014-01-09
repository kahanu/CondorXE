using System;
using System.Linq;
using Condor.Core;
using Condor.Core.Interfaces;
using Condor.Core.PropertyObjects;
using GizmoBeach.Components.CodeWriters;
using MyMeta;

namespace GizmoBeach.Components.BusinessObjects
{
    public class DataAnnotationsBusinessObjectsForDbContext : RenderBase, IBusinessObjects
    {
        private readonly RequestContext _context;

        public DataAnnotationsBusinessObjectsForDbContext(RequestContext context)
        : base(context.Zeus.Output)
        {
            this._context = context;
        }

        #region IRenderObject Members

        public void Render()
        {
            _context.Dialog.InitDialog();

            _context.FileList.Add("");
            _context.FileList.Add("Generated DataAnnotations Business Objects");
            try
            {
                foreach (TableItem item in _context.ScriptSettings.Settings.Tables.Tables)
                {
                    try
                    {
                        //_context.FileList.Add("Writing '" + item.Name + "...");
                        _context.Dialog.Display("Processing DataAnnotations Business Objects for '" + item.Name + "'");
                        ITable table = _context.Database.Tables[item.Name];
                        //_context.FileList.Add("   Table: '" + table.Name);
                        RenderDataAnnotationsBusinessObjectsClass(table);
                    }
                    catch (Exception ex)
                    {
                        string msg = _output.text;
                        throw new Exception("Foreach exception: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Render method exception: " + ex.Message);
            }

            //foreach (string tableName in _script.Tables)
            //{
            //    _context.Dialog.Display("Processing DataAnnotations Business Objects for '" + tableName + "'");
            //    ITable table = _database.Tables[tableName];
            //    RenderDataAnnotationsBusinessObjectsClass(table);
            //}
        }
   
        private void RenderDataAnnotationsBusinessObjectsClass(ITable table)
        {
            

            try
            {
                _hdrUtil.WriteClassHeader(_output);

                _output.autoTabLn("using System;");
                _output.autoTabLn("using System.Collections.Generic;");
                _output.autoTabLn("using System.ComponentModel.DataAnnotations;");
                _output.autoTabLn("using System.ComponentModel;");
                _output.autoTabLn("using System.Web.Mvc;");

                _output.autoTabLn("");
                _output.autoTabLn("namespace " + _script.Settings.BusinessObjects.BusinessObjectsNamespace);
                _output.autoTabLn("{");
                _output.tabLevel++;

                _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name));
                _output.autoTabLn("{");
                _output.tabLevel++;

                _output.autoTabLn("#region Properties");
                RenderProperties(table);
                _output.autoTabLn("#endregion");

                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");

                _output.tabLevel--;
                _output.autoTabLn("}");
                
                _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + ".cs");
                SaveOutput(CreateFullPath(_script.Settings.BusinessObjects.BusinessObjectsNamespace, StringFormatter.CleanUpClassName(table.Name) + ".cs"), SaveActions.DontOverwrite);
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RenderProperties(ITable table)
        {
            //_context.FileList.Add("      Inside RenderProperties method...");
            Condor.Core.Property prop = null;
            foreach (IColumn c in table.Columns)
            {
                //_context.FileList.Add("      Processing column '" + c.Name + "'");
                prop = new BusinessObjectsPropertyRenderDataAnnotationsForDbContext(c, _context);
                prop.Render();
            }
            BusinessObjectsPropertiesRenderForeignKeyForDbContext foreignKey = new BusinessObjectsPropertiesRenderForeignKeyForDbContext(table, _context);
            foreignKey.Render();
        }
        #endregion
    }
}
