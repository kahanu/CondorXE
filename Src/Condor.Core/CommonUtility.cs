using MyMeta;

namespace Condor.Core
{
    /// <summary>
    /// This class helps encapsulate globally needed functions.
    /// </summary>
    public class CommonUtility
    {
        protected ScriptSettings script;
        private string[] primaryKeyTypes;
        private string[] primaryKeyNames;

        public CommonUtility()
        {
            script = ScriptSettings.GetInstance();
        }

        /// <summary>
        /// Build a complete namespace for Entity classes.
        /// Example: DataObjectsNamespace.OrmFrameworkName.Classname
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public string BuildEntityClassWithNameSpace(string className)
        {
            string fullClassNamespace = string.Empty;
            fullClassNamespace = script.Settings.DataOptions.DataObjectsNamespace + "." +
                script.Settings.DataOptions.ORMFramework.Selected + "." + className;

            return fullClassNamespace;
        }

        /// <summary>
        /// Build complete namespace for business objects.
        /// Example: BusinessObjectsNamespace.Classname
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public string BuildModelClassWithNameSpace(string className)
        {
            string fullClassNamespace = string.Empty;
            fullClassNamespace = script.Settings.BusinessObjects.BusinessObjectsNamespace + "." + className;

            return fullClassNamespace;
        }

        /// <summary>
        /// Build the parameters for concrete classes that contain primary key values that are placed
        /// inside the parentheses of the method.
        /// Example: public void SampleMethod(int Id) -- 'int Id' is what is injected into the method signature.
        /// This method can create a comma-separated set of parameters and their data types.
        /// </summary>
        /// <param name="m_ITable"></param>
        /// <returns></returns>
        public string RenderConcreteMethodParameters(MyMeta.ITable m_ITable)
        {
            string result = string.Empty;
            GetPrimaryKeys(m_ITable);

            for (int i = 0; i < primaryKeyTypes.Length; i++)
            {
                result += primaryKeyTypes[i] + " ";
                result += primaryKeyNames[i] + ", ";
            }

            // Trim the trailing comma.
            result = result.Substring(0, result.Length - 2);

            return result;
        }

        /// <summary>
        /// Builds the parameters for a calling method that contain only the variable, and not the data type.
        /// Example: someClassInstance.MyMethod(injectedParameter) -- 'injectedParameter' is the value created.
        /// </summary>
        /// <param name="m_ITable"></param>
        /// <returns></returns>
        public string RenderCallingMethodParameters(MyMeta.ITable m_ITable)
        {
            string result = string.Empty;
            GetPrimaryKeys(m_ITable);

            for (int i = 0; i < primaryKeyTypes.Length; i++)
            {
                result += primaryKeyNames[i] + ", ";
            }

            // Trim the trailing comma.
            result = result.Substring(0, result.Length - 2);

            return result;
        }

        /// <summary>
        /// This should be called before the parameter generating methods in 
        /// order to initialize the keyTypes and keyValues.
        /// </summary>
        /// <param name="m_ITable"></param>
        public void GetPrimaryKeys(MyMeta.ITable m_ITable)
        {
            int i = 0;

            foreach (Column c in m_ITable.Columns)
            {
                if (c.IsInPrimaryKey)
                {
                    primaryKeyTypes = new string[i + 1];
                    primaryKeyNames = new string[i + 1];
                    i++;
                }
            }

            i = 0;
            foreach (Column c in m_ITable.Columns)
            {
                if (c.IsInPrimaryKey)
                {
                    primaryKeyTypes[i] = c.LanguageType;
                    primaryKeyNames[i] = c.Alias;
                    i++;
                }
            }
        }

        /// <summary>
        /// This does some rudimentary property name cleanup.  
        /// Removal of spaces, dashes, underscores and periods. 
        /// No capitalization.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string CleanUpProperty(string property)
        {
            return CleanUpProperty(property, false);
        }

        /// <summary>
        /// This does some rudimentary property name cleanup.  
        /// Removal of spaces, dashes, underscores and periods. 
        /// The option to capitalize the property name (Pascal casing).
        /// </summary>
        /// <param name="property"></param>
        /// <param name="capitalize"></param>
        /// <returns></returns>
        public string CleanUpProperty(string property, bool capitalize)
        {
            property = property.Replace(" ", "");
            property = property.Replace("-", "");
            property = property.Replace("_", "");
            property = property.Replace(".", "");

            if (capitalize)
            {
                property = StringFormatter.PascalCasing(property);
            }
            return property;
        }
    }
}
