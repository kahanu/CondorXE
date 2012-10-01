using System;
using Condor.Core;
using Condor.Core.Interfaces;
using MyMeta;

namespace GizmoBeach.Components.DataObjects
{
    public class EntityFrameworkDbContextRepositoryOrmFramework : RenderBase, IORMFramework
    {
        protected IDataStore _dataStore;
        protected RequestContext _context;
        protected IDatabase _database;
        protected ProgressDialogWrapper _dialog;

        public EntityFrameworkDbContextRepositoryOrmFramework(IDataStore dataStore, RequestContext context)
            : base(context.Zeus.Output)
        {
            this._dataStore = dataStore;
            this._context = context;
            this._database = context.Database;
            this._dialog = context.Dialog;
        }

        #region IORMFramework Members

        public string Name
        {
            get { return "Entity Framework DbContext"; }
        }

        public void Generate()
        {
            _dialog.InitDialog(7);
            _dialog.Display("Processing EF DbContext Classes");

            _context.FileList.Add("");
            _context.FileList.Add("Generated EF DbContext Classes:");

            _dialog.Display("Processing EF DbContext DataContext Class");
            RenderDataContextClass();

            _dialog.Display("Processing EF DbContext DataObjectFactory Class");
            RenderDataObjectFactory();

            _dialog.Display("Processing EF DbContext Disposable Class");
            RenderDisposableClass();

            _dialog.Display("Processing EF DbContext RepositoryBase Class");
            RenderRepositoryBaseClass();

            _dialog.Display("Processing EF DbContext IUnitOfWork Interface");
            RenderUnitOfWorkInterface();

            _dialog.Display("Processing EF DbContext UnitOfWork Class");
            RenderUnitOfWorkClass();

            _dialog.InitDialog(3);
            _dialog.Display("Processing EF DbContext Generated Classes");

            _context.FileList.Add("");
            _context.FileList.Add("Processing EF DbContext Generated Classes:");

            _dialog.Display("Processing EF DbContext DataObjectsBase Class");
            RenderDataObjectsBaseClass();

            _dialog.Display("Processing EF DbContext IRepository Interface");
            RenderIRepositoryInterface();

            _dialog.InitDialog();
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing EF DbContext Repository Interface for '" + tableName + "'");
                RenderConcreteInterface(table);
            }

            _dialog.InitDialog();
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                _dialog.Display("Processing EF DbContext Repository Class for '" + tableName + "'");
                RenderConcreteRepositoryClass(table);
            }
        }

        #endregion

        #region Private Methods

        private void RenderDataObjectsBaseClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Data.Entity;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Generated");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class DataObjectsBase : DbContext");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region Private Variables");
            _output.autoTabLn("");

            foreach (string tableName in _script.Tables)
            {
                _output.autoTabLn("private IDbSet<" + StringFormatter.CleanUpClassName(tableName) + "> _" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName, true)) + ";");
            }


            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("");
            _output.autoTabLn("public DataObjectsBase(string connectionString)");
            _output.tabLevel++;
            _output.autoTabLn(": base(connectionString)");
            _output.tabLevel--;
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Public Properties");

            foreach (string tableName in _script.Tables)
            {
                _output.autoTabLn("public IDbSet<" + StringFormatter.CleanUpClassName(tableName) + "> " + StringFormatter.CleanUpClassName(tableName, true) + "");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("get");
                _output.autoTabLn("{");
                _output.tabLevel++;
                _output.autoTabLn("return _" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName, true)) + " ?? (_" + StringFormatter.CamelCasing(StringFormatter.CleanUpClassName(tableName, true)) + " = Set<" + StringFormatter.CleanUpClassName(tableName) + ">());");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.tabLevel--;
                _output.autoTabLn("}");
                _output.autoTabLn("");
            }


            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    DataObjectsBase.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Generated", "DataObjectsBase.cs"), SaveActions.Overwrite);
        }

        private void RenderIRepositoryInterface()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Linq.Expressions;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Generated");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface IRepository<TEntity>");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("IQueryable<TEntity> GetAll();");
            _output.autoTabLn("");
            _output.autoTabLn("IEnumerable<TEntity> Get(");
            _output.tabLevel++;
            _output.autoTabLn("Expression<Func<TEntity, bool>> filter = null,");
            _output.autoTabLn("Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,");
            _output.autoTabLn("string includeProperties = \"\");");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("TEntity GetById(object id);");
            _output.autoTabLn("");
            _output.autoTabLn("void Insert(TEntity entity);");
            _output.autoTabLn("");
            _output.autoTabLn("void Delete(object id);");
            _output.autoTabLn("");
            _output.autoTabLn("void Delete(TEntity entityToDelete);");
            _output.autoTabLn("");
            _output.autoTabLn("void Update(TEntity entityToUpdate);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    IRepository.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Generated", "IRepository.cs"), SaveActions.Overwrite);
        }

        private void RenderConcreteInterface(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Generated;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : IRepository<" + StringFormatter.CleanUpClassName(table.Name) + ">");
            _output.autoTabLn("{");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Interfaces", "I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderConcreteRepositoryClass(ITable table)
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Interfaces;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace + ".Repositories");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + " : RepositoryBase<" + StringFormatter.CleanUpClassName(table.Name) + ">, I" + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + "(IUnitOfWork unitOfWork):base(unitOfWork)");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    " + StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace + "\\Repositories", StringFormatter.CleanUpClassName(table.Name) + _script.Settings.DataOptions.ClassSuffix.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderDataContextClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System.Data.Entity;");
            _output.autoTabLn("using System.Data.Entity.ModelConfiguration.Conventions;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using " + _script.Settings.BusinessObjects.BusinessObjectsNamespace + ";");
            _output.autoTabLn("using " + _script.Settings.DataOptions.DataObjectsNamespace + ".Generated;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class " + _script.Settings.DataOptions.DataContext.Name + " : DataObjectsBase");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.tabLevel++;
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("public " + _script.Settings.DataOptions.DataContext.Name + "(string connectionString)");
            _output.tabLevel++;
            _output.autoTabLn(": base(connectionString)");
            _output.tabLevel--;
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Public Methods");
            _output.autoTabLn("");
            _output.autoTabLn("public virtual void Commit()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("base.SaveChanges();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Overrides");
            _output.autoTabLn("");
            _output.autoTabLn("protected override void OnModelCreating(DbModelBuilder modelBuilder)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();");

            // Include mappings for entities with schema names.
            _output.autoTabLn("");
            foreach (string tableName in _script.Tables)
            {
                ITable table = _database.Tables[tableName];
                if (table.Schema.ToLower() != "dbo")
                {
                    _output.autoTabLn("modelBuilder.Entity<" + StringFormatter.CleanUpClassName(tableName) + ">().ToTable(\"" + StringFormatter.CleanUpClassName(tableName) + "\", \"" + table.Schema + "\");");
                }
            }

            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");

            _context.FileList.Add("    " + _script.Settings.DataOptions.DataContext.Name + ".cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace, _script.Settings.DataOptions.DataContext.Name + ".cs"), SaveActions.DontOverwrite);
        }

        private void RenderDataObjectFactory()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Configuration;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public static class DataObjectFactory");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("private static readonly string _connectionString;");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Static constructor. Reads the connectionstring from web.config just once.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("static DataObjectFactory()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("string connectionStringName = ConfigurationManager.AppSettings.Get(\"ConnectionStringName\");");
            _output.autoTabLn("_connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("/// <summary>");
            _output.autoTabLn("/// Creates the Context using the current connectionstring.");
            _output.autoTabLn("/// </summary>");
            _output.autoTabLn("/// <remarks>");
            _output.autoTabLn("/// Gof pattern: Factory method. ");
            _output.autoTabLn("/// </remarks>");
            _output.autoTabLn("/// <returns>DbContext.</returns>");
            _output.autoTabLn("public static " + _script.Settings.DataOptions.DataContext.Name + " CreateContext()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return new " + _script.Settings.DataOptions.DataContext.Name + "(_connectionString);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    DataObjectFactory.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace, "DataObjectFactory.cs"), SaveActions.DontOverwrite);
        }

        private void RenderRepositoryBaseClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Collections.Generic;");
            _output.autoTabLn("using System.Data;");
            _output.autoTabLn("using System.Data.Entity;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("using System.Linq.Expressions;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public abstract class RepositoryBase<TEntity> where TEntity : class");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("#region Locals");
            _output.autoTabLn("");
            _output.autoTabLn("internal " + _script.Settings.DataOptions.DataContext.Name + " _context;");
            _output.autoTabLn("internal IDbSet<TEntity> _dbSet;");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region ctors");
            _output.autoTabLn("private readonly IUnitOfWork _unitOfWork;");
            _output.autoTabLn("");
            _output.autoTabLn("public RepositoryBase(IUnitOfWork unitOfWork)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("this._unitOfWork = unitOfWork;");
            _output.autoTabLn("this._dbSet = " + _script.Settings.DataOptions.DataContext.Name + ".Set<TEntity>();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Public CRUD Methods");
            _output.autoTabLn("");
            _output.autoTabLn("public virtual IQueryable<TEntity> GetAll()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("IQueryable<TEntity> query = _dbSet;");
            _output.autoTabLn("");
            _output.autoTabLn("return query;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");

            _output.autoTabLn("public virtual IEnumerable<TEntity> Get(");
            _output.tabLevel++;
            _output.autoTabLn("Expression<Func<TEntity, bool>> filter = null,");
            _output.autoTabLn("Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,");
            _output.autoTabLn("string includeProperties = \"\")");
            _output.tabLevel--;
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("IQueryable<TEntity> query = _dbSet;");
            _output.autoTabLn("");
            _output.autoTabLn("if (filter != null)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("query = query.Where(filter);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("foreach (var includeProperty in includeProperties.Split");
            _output.tabLevel++;
            _output.autoTabLn("(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))");
            _output.tabLevel--;
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("query = query.Include(includeProperty);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("if (orderBy != null)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return orderBy(query).ToList();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("else");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return query.ToList();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public virtual TEntity GetById(object id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return _dbSet.Find(id);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public virtual void Insert(TEntity entity)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_dbSet.Add(entity);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public virtual void Delete(object id)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("TEntity entityToDelete = _dbSet.Find(id);");
            _output.autoTabLn("Delete(entityToDelete);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public virtual void Delete(TEntity entityToDelete)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (_context.Entry(entityToDelete).State == EntityState.Detached)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_dbSet.Attach(entityToDelete);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("_dbSet.Remove(entityToDelete);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public virtual void Update(TEntity entityToUpdate)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_dbSet.Attach(entityToUpdate);");
            _output.autoTabLn("_context.Entry(entityToUpdate).State = EntityState.Modified;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.autoTabLn("#region Protected Members");
            _output.autoTabLn("");
            _output.autoTabLn("protected " + _script.Settings.DataOptions.DataContext.Name + " " + _script.Settings.DataOptions.DataContext.Name + "");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("get");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return _context ?? (_context = _unitOfWork." + _script.Settings.DataOptions.DataContext.Name + ");");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("#endregion");
            _output.autoTabLn("");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    RepositoryBase.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace, "RepositoryBase.cs"), SaveActions.DontOverwrite);
        }

        private void RenderUnitOfWorkClass()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public class UnitOfWork : Disposable, IUnitOfWork");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("private " + _script.Settings.DataOptions.DataContext.Name + " _context;");
            _output.autoTabLn("");
            _output.autoTabLn("public " + _script.Settings.DataOptions.DataContext.Name + " " + _script.Settings.DataOptions.DataContext.Name + "");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("get");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("return _context ?? (_context = DataObjectFactory.CreateContext());");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public void Commit()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(_script.Settings.DataOptions.DataContext.Name + ".Commit();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("protected override void DisposeCore()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (_context != null)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("_context.Dispose();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    UnitOfWork.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace, "UnitOfWork.cs"), SaveActions.DontOverwrite);
        }

        private void RenderUnitOfWorkInterface()
        {
            _hdrUtil.WriteClassHeader(_output);

            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public interface IUnitOfWork");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn(_script.Settings.DataOptions.DataContext.Name + " " + _script.Settings.DataOptions.DataContext.Name + " { get; }");
            _output.autoTabLn("");
            _output.autoTabLn("void Commit();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    IUnitOfWork.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace, "IUnitOfWork.cs"), SaveActions.DontOverwrite);
        }

        private void RenderDisposableClass()
        {
            _hdrUtil.WriteClassHeader(_output);
            _output.autoTabLn("using System;");
            _output.autoTabLn("using System.Linq;");
            _output.autoTabLn("");
            _output.autoTabLn("namespace " + _script.Settings.DataOptions.DataObjectsNamespace);
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("public abstract class Disposable : IDisposable");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("private bool isDisposed;");
            _output.autoTabLn("");
            _output.autoTabLn("~Disposable()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("Dispose(false);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("protected virtual void DisposeCore()");
            _output.autoTabLn("{");
            _output.autoTabLn("");
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("private void Dispose(bool disposing)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("if (!isDisposed && disposing)");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("DisposeCore();");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("isDisposed = true;");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.autoTabLn("");
            _output.autoTabLn("public void Dispose()");
            _output.autoTabLn("{");
            _output.tabLevel++;
            _output.autoTabLn("Dispose(true);");
            _output.autoTabLn("GC.SuppressFinalize(this);");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");
            _output.tabLevel--;
            _output.autoTabLn("}");

            _context.FileList.Add("    Disposable.cs");
            SaveOutput(CreateFullPath(_script.Settings.DataOptions.DataObjectsNamespace, "Disposable.cs"), SaveActions.DontOverwrite);
        }

        #endregion
    }
}


