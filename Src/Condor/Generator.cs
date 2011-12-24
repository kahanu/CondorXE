using Condor.Core;
using System;
using System.Collections;
using System.Reflection;
using Zeus;
using Condor.Factories;
using Condor.Interfaces;
using Condor.ORM;

namespace Condor
{
    /// <summary>
    /// This class is the only class needed to register in the 
    /// MyGeneration13\Settings\ZeusConfig.xml file.
    /// The entire template code Tab just needs to look like the snippet
    /// below and it should never have to change.
    /// 
    /// <%#REFERENCE System.Windows.Forms.dll, System.Xml.dll %><%
    ///public class GeneratedTemplate : DotNetScriptTemplate
    ///{
    ///	public GeneratedTemplate(ZeusContext context) : base(context) {}
    ///
    ///	//---------------------------------------------------
    ///	// Render() is where you want to write your logic    
    ///	//---------------------------------------------------
    ///	public override void Render()
    ///	{
    ///		Dnp.Utils.ProgressDialog progressDialog = new Dnp.Utils.ProgressDialog();
    ///		
    ///		Condor.Init(MyMeta, context, input, DnpUtils, progressDialog);
    ///		Condor.Execute();
    ///		Condor.Print();
    ///	}
    ///
    ///}
    ///%>
    /// </summary>
    public class Generator
    {
        #region Local Variables

        private ArrayList _fileList;
        private RequestContext _context;
        private ScriptSettings _scriptSettings;
        private string _version;

        #endregion

        #region ctors
        public Generator()
        {
            _fileList = new ArrayList();

            // Get the version of this assembly.
            Assembly assem = Assembly.GetExecutingAssembly();
            AssemblyName assemName = assem.GetName();
            Version ver = assemName.Version;
            _version = ver.ToString();
        }
        #endregion

        #region Public Properties

        public RequestContext RequestContext
        {
            get { return _context; }
        }

        public string Version
        {
            get { return _version; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Call this method first. This initializes everything that is
        /// necessary to run the application.
        /// </summary>
        /// <param name="MyMeta"></param>
        /// <param name="zeus"></param>
        public void Init(MyMeta.dbRoot MyMeta, Zeus.IZeusContext zeus, IZeusInput input, Dnp.Utils.Utils dnp, Dnp.Utils.ProgressDialog progressDialog)
        {
            // Initialize the ScriptSettings and save the input values from the MyGeneration form.
            try
            {
                _scriptSettings = ScriptSettings.InitInstance(input, MyMeta, dnp, _version);
                _scriptSettings.SaveSettings();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception("Error Saving ScriptSettings (inner): " + ex.InnerException.Message);
                }
                throw new Exception("Error Saving ScriptSettings: " + ex.Message + " - " + ex.StackTrace.ToString());
            }
            

            // Populate the RequestContext class with all the values
            // needed to be passed to every code generating class.
            RequestContext context = new RequestContext();
            context.Zeus = zeus;                                                    // Pass along the MyGeneration output context for saving files
            context.MyMeta = MyMeta;                                                // Pass along the Database meta properties
            context.FileList = _fileList;                                           // Start the ArrayList for printing the generated files
            context.ScriptSettings = _scriptSettings;                               // Store the saved form values
            context.Dialog = new ProgressDialogWrapper(progressDialog);             // Get instance of Dialog wrapper
            context.Database = MyMeta.Databases[_scriptSettings.DatabaseName];      // Get the selected database
            context.Utility = new CommonUtility();                                  // Get instance of helper utility
            
            _context = context;
        }

        /// <summary>
        /// Call this second.
        /// </summary>
        public void Execute()
        {
            try
            {
                /**************************************************************
                 * Begin Business Objects
                 * ***********************************************************/
                if (_scriptSettings.Settings.BusinessObjects.Use)
                {
                    IObjectFactory factory = new BusinessObjectsFactory(_context);
                    factory.Render(_scriptSettings.Settings.BusinessObjects.ClassName);
                }





                /**************************************************************
                 * Begin Data Objects
                 * ***********************************************************/

                string dataPattern = _scriptSettings.Settings.DataOptions.DataPattern.ClassName;
                string ormFramework = _scriptSettings.Settings.DataOptions.ORMFramework.ClassName;
                string dataStore = _scriptSettings.Settings.DataOptions.DataStore.ClassName;
                string dotNetFramework = _scriptSettings.Settings.DotNet.DotNetFramework.Selected;

                IValidator validator = new FrameworkValidator(ormFramework, dotNetFramework);

                try
                {
                    IDataObjectsFactory factory = new DataObjectsFactory(dataPattern, ormFramework, dataStore, validator, _context);
                    factory.Build();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error rendering DataObjectsFactory process - " + ex.Message);
                }





                /**************************************************************
                 * Begin Service Layer
                 * ***********************************************************/
                if (_scriptSettings.Settings.ServiceLayer.Use)
                {
                    IObjectFactory factory = new ServiceFactory(_context);
                    factory.Render(_scriptSettings.Settings.ServiceLayer.ClassName);
                }





                /**************************************************************
                 * Begin UI Layer
                 * ***********************************************************/
                if (_scriptSettings.Settings.UI.Use)
                {
                    IObjectFactory factory = new UIFactory(_context);
                    factory.Render(_scriptSettings.Settings.UI.ClassName);
                }







                /**************************************************************
                 * Begin IoC Layer
                 * ***********************************************************/
                if (_scriptSettings.Settings.IoC.Use)
                {
                    IObjectFactory factory = new IoCFactory(_context);
                    factory.Render(_scriptSettings.Settings.IoC.ClassName);
                }




            }
            catch (Exception ex)
            {
                _context.FileList.Add("Condor.Generator.Execute() error: " + ex.Message);
            }
            _context.Dialog.HideDialog();
        }

        /// <summary>
        /// Call this last to print the results.
        /// </summary>
        public void Print()
        {
            foreach (string txt in _context.FileList)
            {
                _context.Zeus.Output.writeln(txt);
            }
        }

        

        #endregion

    }
}
