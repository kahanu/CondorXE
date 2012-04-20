using System;
using System.Collections;
using Condor.Core.Interfaces;
using Zeus;
using System.Xml;

namespace Condor.Core
{
    /// <summary>
    /// This class was inspired by the Gentle.NET template.
    /// </summary>
    public class ScriptSettings
    {
        private readonly IZeusInput _input;
        private readonly MyMeta.dbRoot _database;
        private readonly Dnp.Utils.Utils _dnpUtils;
        private string _version;
        private string _fileName = @"\settings.xml";

        static private ScriptSettings _instance = null;

        #region ctors
        public ScriptSettings(IZeusInput input, MyMeta.dbRoot database, Dnp.Utils.Utils dnp, string version)
        {
            this._input = input;
            this._database = database;
            this._version = version;
            this._dnpUtils = dnp;
        }
        #endregion

        #region Static Methods
        public static ScriptSettings GetInstance()
        {
            return _instance;
        }

        public static ScriptSettings InitInstance(IZeusInput input, MyMeta.dbRoot database, Dnp.Utils.Utils dnp, string version)
        {
            if (_instance == null)
            {
                if (input == null)
                    throw new ApplicationException("InitInstance input");

                if (database == null)
                    throw new ApplicationException("InitInstance database");

                if (dnp == null)
                    throw new ApplicationException("InitInstance dnp");

                if (string.IsNullOrEmpty(version))
                    throw new ApplicationException("InitInstance version");

                _instance = new ScriptSettings(input, database, dnp, version);
            }
            return _instance;
        }
        #endregion

        #region Public Properties
        public string Version
        {
            get
            {
                return _version;
            }
        }

        public Dnp.Utils.Utils DnpUtils
        {
            get
            {
                return _dnpUtils;
            }
        }

        public MyMeta.dbRoot Database
        {
            get
            {
                return _database;
            }
        }

        public string DatabaseName
        {
            get
            {
                return _input["cmbDatabase"].ToString();
            }
        }

        public ArrayList Tables
        {
            get
            {
                return _input["lstTablesViews"] as ArrayList;
            }
        }

        public string OutputPath
        {
            get
            {
                return _input["defaultOutputPath"].ToString();
            }
        }

        public WebAppSettings Settings
        {
            get
            {
                return Serializer<WebAppSettings>.Deserialize(OutputPath + _fileName);
            }
        }
        #endregion

        #region Public Methods

        public void SaveSettings()
        {
            string filePath = OutputPath + _fileName;

            WebAppSettings s = new WebAppSettings();
            s.Version = _version;
            s.Namespace = _input["txtNamespace"].ToString();

            s.Tables = new TableObject();
            foreach (string table in Tables)
            {
                s.Tables.AddItem(new TableItem(table));
            }

            s.BusinessObjects = new BusinessObjects();
            s.BusinessObjects.BusinessObjectsNamespace = _input["txtBusinessObjectsNamespace"].ToString();
            s.BusinessObjects.Use = Convert.ToBoolean(_input["chkCreateBusinessObjects"].ToString());
            s.BusinessObjects.ClassName = _input["ddlChooseBusinessObjects"].ToString();
            s.BusinessObjects.Selected = GetClassValue(s.BusinessObjects.ClassName);

            s.ServiceLayer = new ServiceLayer();
            s.ServiceLayer.Use = Convert.ToBoolean(_input["chkEnableServices"].ToString());
            s.ServiceLayer.ServiceNamespace = _input["txtServiceNamespace"].ToString();
            s.ServiceLayer.DataContract = _input["txtDataContract"].ToString();
            s.ServiceLayer.ClassName = _input["ddlChooseServiceType"].ToString();
            s.ServiceLayer.Selected = GetClassValue(s.ServiceLayer.ClassName);
            s.ServiceLayer.SupportWCF = true;

            s.DataOptions = new DataOptions();
            s.DataOptions.DataObjectsNamespace = _input["txtDataObjectsNamespace"].ToString();
            s.DataOptions.VersionColumnName = _input["txtVersionColumnName"].ToString();
            s.DataOptions.ConnectionString = ChopProvider(Database.ConnectionString);

            s.DataOptions.DataPattern = new DataPattern();
            s.DataOptions.DataPattern.ClassName = _input["ddlChooseDataPattern"].ToString();
            s.DataOptions.DataPattern.Selected = GetClassValue(s.DataOptions.DataPattern.ClassName);
            s.DataOptions.DataContext = new DataContext();

            s.DataOptions.DataStore = new DataStore();
            s.DataOptions.DataStore.ClassName = _input["ddlChooseDataStore"].ToString();
            s.DataOptions.DataStore.Selected = GetClassValue(s.DataOptions.DataStore.ClassName);

            s.DataOptions.ORMFramework = new ORMFramework();
            s.DataOptions.ORMFramework.ClassName = _input["ddlChooseOrmFramework"].ToString();
            s.DataOptions.ORMFramework.Selected = GetClassValue(s.DataOptions.ORMFramework.ClassName);

            bool isDefault = true;
            if ((_input["txtDataContextName"].ToString() != "ActionDataContext") && (_input["txtDataContextName"].ToString() != "ActionEntities"))
                isDefault = false;

            s.DataOptions.DataContext.IsDefault = isDefault;
            s.DataOptions.DataContext.Name = _input["txtDataContextName"].ToString();

            isDefault = true;
            s.DataOptions.ClassPrefix = new ClassPrefix();

            if (s.DataOptions.DataStore.Selected == "SQLServer")
            {
                if (_input["txtClassPrefix"].ToString() != "SQLServer")
                    isDefault = false;
            }

            s.DataOptions.ClassPrefix.IsDefault = isDefault;
            s.DataOptions.ClassPrefix.Name = s.DataOptions.DataStore.Selected;
            //s.DataOptions.ClassPrefix.Name = m_Input["txtClassPrefix"].ToString();

            isDefault = true;
            s.DataOptions.ClassSuffix = new ClassSuffix();

            if (_input["txtClassSuffix"].ToString() != "Dao")
                isDefault = false;

            s.DataOptions.ClassSuffix.IsDefault = isDefault;
            s.DataOptions.ClassSuffix.Name = _input["txtClassSuffix"].ToString();

            s.Common = new Common();
            s.Common.SaveToFile = Convert.ToBoolean(_input["chkSaveToFile"].ToString());
            s.UI = new UI();
            s.UI.Use = Convert.ToBoolean(_input["chkRenderUI"].ToString());
            s.UI.UINamespace = _input["txtUINamespace"].ToString().Trim();
            s.UI.ClassName = _input["ddlChooseUI"].ToString();
            s.UI.UIWcfServiceNamespace = _input["txtWCFServiceNamespace"].ToString();
            s.UI.UIFramework = new UIFramework();
            s.UI.UIFramework.Selected = _input["ddlChooseUI"].ToString();

            s.IoC = new IoC();
            s.IoC.Use = Convert.ToBoolean(_input["chkUseIoC"].ToString());
            s.IoC.ClassName = _input["ddlChooseIoCProvider"].ToString();
            s.IoC.IoCProvider = new IoCProvider();
            s.IoC.IoCProvider.Selected = _input["ddlChooseIoCProvider"].ToString();

            s.DotNet = new DotNet();
            s.DotNet.DotNetFramework = new DotNetFramework();
            s.DotNet.DotNetFramework.Selected = _input["ddlChooseDotNetFramework"].ToString();

            s.Pluralizer = new Pluralizer();
            s.Pluralizer.ClassNames = new ClassNames();
            s.Pluralizer.ClassNames.Selected = _input["ddlClassNamePluralizer"].ToString();

            s.Pluralizer.FieldNames = new FieldNames();
            s.Pluralizer.FieldNames.Selected = _input["ddlFieldNamePluralizer"].ToString();

            s.Pluralizer.PropertyNames = new PropertyNames();
            s.Pluralizer.PropertyNames.Selected = _input["ddlPropertyNamePluralizer"].ToString();

            try
            {
                Serializer<WebAppSettings>.Serialize(s, filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// This gets the "value" attribute from the CondorXELoader.xml file
        /// for the selected "class" attribute.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public string GetClassValue(string className)
        {
            string valueName = string.Empty;

            if (!string.IsNullOrEmpty(className))
            {
                UILoader loader = new UILoader();

                XmlNode node = loader.GetNode("//add[@class='" + className + "']");
                valueName = node.Attributes["value"].Value;
            }

            return valueName;
        }

        /// <summary>
        /// From the MyGeneration connection string, chop off the leading
        /// 'Provider=[provider name];'.
        /// </summary>
        /// <param name="fullConnString"></param>
        /// <returns></returns>
        public string ChopProvider(string fullConnString)
        {
            int providerPos = fullConnString.IndexOf("Provider=");
            string connString = "";

            if (providerPos == 0)
            {
                int semiColon = fullConnString.IndexOf(';', 0);
                connString = fullConnString.Substring((semiColon + 1), (fullConnString.Length - (semiColon + 1)));
            }
            return connString;
        }
        #endregion
    }
}
