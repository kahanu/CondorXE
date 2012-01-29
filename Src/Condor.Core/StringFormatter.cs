using System;

namespace Condor.Core
{
    /// <summary>
    /// A string formatting helper class.
    /// </summary>
    public class StringFormatter
    {
        private static ScriptSettings script;

        private static WebAppSettings Settings
        {
            get
            {
                script = ScriptSettings.GetInstance();
                return script.Settings;
            }
        }

        /// <summary>
        /// Camel-case the input string.  
        /// Before: MyPropertyName - After: myPropertyName
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CamelCasing(string str)
        {
            if (str.Length > 0)
            {
                if (str.Length == 1)
                {
                    return str.ToLower();
                }
                else
                {
                    return Char.ToLower(str[0]).ToString() + str.Substring(1);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Pascal-case the input string. 
        /// Before: somevariable - After: Somevariable
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PascalCasing(string str)
        {
            if (str.Length > 0)
            {
                if (str.Length == 1)
                {
                    return str.ToUpper();
                }
                else
                {
                    return Char.ToUpper(str[0]).ToString() + str.Substring(1);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Create the DisplayName attribute for properties.
        /// Example: [DisplayName("First Name")]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CreateDisplayName(string input)
        {
            string result = CheckAndRemoveID(input);

            return string.Format("[DisplayName(\"{0}\")]", result);
        }

        /// <summary>
        /// Inserts spaces into variable strings before each capital letter.
        /// Before: FirstName - After: First Name 
        /// Before: Firstname - After: Firstname
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NormalizePropertyName(string input)
        {
            char[] chars = input.ToCharArray();

            string result = string.Empty;

            foreach (char item in chars)
            {
                if (item.ToString().Equals(item.ToString().ToUpper()))
                {
                    result += " " + item.ToString();
                }
                else
                {
                    result += item.ToString();
                }
            }
            return result.Trim();
        }

        /// <summary>
        /// Removes the string "id" from an input string.  Could be used
        /// as labels on drop down lists as a many-to-one selection.
        /// Before: Blog Id or Blog ID - After: Blog
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CheckAndRemoveID(string input)
        {
            string result = SpacePascalCase(input);

            if (result.ToLower().Contains(" id"))
            {
                int pos = result.ToLower().IndexOf(" id");
                result = result.Substring(0, pos);
            }
            return result;
        }

        /// <summary>
        /// Properly space input strings with Pascal-casing based
        /// on capitalization.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SpacePascalCase(string input)
        {
            string splitString = String.Empty;

            for (int idx = 0; idx < input.Length; idx++)
            {
                char c = input[idx];

                if (Char.IsUpper(c)
                    // keeps abbreviations together like "Number HEI"
                    // instead of making it "Number H E I"
                    && ((idx < input.Length - 1
                            && !Char.IsUpper(input[idx + 1]))
                        || (idx != 0
                            && !Char.IsUpper(input[idx - 1])))
                    && splitString.Length > 0)
                {
                    splitString += " ";
                }

                splitString += c;
            }

            return splitString;
        }

        /// <summary>
        /// Convert a physical path string to a namespace.
        /// Before: somefolder\somedirectory\somefile - After: somefolder.somedirectory.somefile
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConvertPathToNamespace(string path)
        {
            string result = path.Replace(@"\", ".");
            return result;
        }

        /// <summary>
        /// Set camel-casing on an input string using the 
        /// MyGeneration DnpUtils.
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public static string CheckAndSetCamelCasing(string myString)
        {
            ScriptSettings m_UserScriptSettings;
            m_UserScriptSettings = ScriptSettings.GetInstance();
            return m_UserScriptSettings.DnpUtils.SetCamelCase(myString);
        }

        /// <summary>
        /// Set pascal-casing on an input string using the 
        /// MyGeneration DnpUtils.
        /// </summary>
        /// <param name="myString"></param>
        /// <returns></returns>
        public static string CheckAndSetPascalCasing(string myString)
        {
            ScriptSettings m_UserScriptSettings;
            m_UserScriptSettings = ScriptSettings.GetInstance();
            return m_UserScriptSettings.DnpUtils.SetPascalCase(myString);
        }

        /// <summary>
        /// Fixes tablenames that will be used as class names to make sure
        /// they are valid classnames.
        /// </summary>
        /// <param name="tableName">The table name to fix.</param>
        /// <returns></returns>
        public static string CleanUpClassName(string tableName)
        {
            return CleanUpClassName(tableName, false);
        }

        /// <summary>
        /// Fixes tablenames that will be used as class names to make sure
        /// they are valid classnames.
        /// </summary>
        /// <param name="tableName">The table name to fix.</param>
        /// <param name="isEntitySet">Should this be rendered as an EntitySet. Determines the pluralization.</param>
        /// <returns></returns>
        public static string CleanUpClassName(string tableName, bool isEntitySet)
        {
            string result = tableName;

            ePluralizerTypes classType = EnumFactory.Parse<ePluralizerTypes>(Settings.Pluralizer.ClassNames.Selected);

            PluralizerFactory factory = new PluralizerFactory();
            result = factory.SetWord(tableName, classType, isEntitySet);

            return result;
        }

        

        // Old implementation
        /*
        public static string CleanUpClassName(string tableName, bool makeSingular){
            tableName = tableName.Replace(" ", "");
            //tableName = tableName.Replace("_", "");
            //tableName = tableName.Replace(".", "");
		
            if(makeSingular){
                return MakeSingular(tableName);
            }
            else
            {
                return tableName;
            }
        }
        */

        public static string MakeSingular(string tableName)
        {
            string strLastThree = string.Empty;
            string strShortTableName = string.Empty;
            string newTableName = string.Empty;

            string strLastChar = tableName.Substring(tableName.Length - 1);

            if (tableName.ToLower().Contains("news"))
                return tableName;

            if (strLastChar == "s")
            {
                strLastThree = tableName.Substring(tableName.Length - 3);
                if (strLastThree != "ess")
                {
                    if (strLastThree == "ies")
                    {
                        strShortTableName = tableName.Substring(0, tableName.Length - 3);
                        newTableName = strShortTableName + "y";
                    }
                    else
                    {
                        if (!tableName.ToLower().Contains("status"))
                        {
                            strShortTableName = tableName.Substring(0, tableName.Length - 1);
                            newTableName = strShortTableName;
                        }
                        else
                        {
                            newTableName = tableName;
                        }
                    }
                }
                else
                {
                    newTableName = tableName;
                }

            }
            else
            {
                newTableName = tableName;
            }
            return newTableName;
        }

        public static string MakePlural(string word)
        {
            if (TestIsPlural(word) == true)
            {
                return word; //it's already a plural
            }
            /*
            else if (_dictionary.ContainsKey(word.ToLower())) 
            //it's an irregular plural, use the word from the dictionary
            {
                return _dictionary[word.ToLower()];
            }
            */
            if (word.Length <= 2)
            {
                return word; //not a word that can be pluralised!
            }
            ////1. If the word ends in a consonant plus -y, change the -y into
            ///-ie and add an -s to form the plural 
            ///e.g. enemy--enemies baby--babies
            switch (word.Substring(word.Length - 2))
            {
                case "by":
                case "cy":
                case "dy":
                case "fy":
                case "gy":
                case "hy":
                case "jy":
                case "ky":
                case "ly":
                case "my":
                case "ny":
                case "py":
                case "ry":
                case "sy":
                case "ty":
                case "vy":
                case "wy":
                case "xy":
                case "zy":
                    {
                        return word.Substring(0, word.Length - 1) + "ies";
                    }
                //2. For words that end in -is, change the -is to -es to make the plural form.
                //synopsis--synopses 
                //thesis--theses 
                case "is":
                    {
                        return word.Substring(0, word.Length - 1) + "es";
                    }
                //3. For words that end in a "hissing" sound (s,z,x,ch,sh), add an -es to form the plural.
                //box--boxes 
                //church--churches
                case "ch":
                case "sh":
                case "us":
                case "ss":
                    {
                        return word + "es";
                    }
                default:
                    {
                        switch (word.Substring(word.Length - 1))
                        {
                            case "s":
                            case "z":
                            case "x":
                                {
                                    return word + "es";
                                }
                            default:
                                {
                                    //4. Assume add an -s to form the plural of most words.
                                    return word + "s";
                                }
                        }
                    }
            }
        }

        public static string GetFirstChar(string str)
        {
            if (str.Length > 0)
            {
                return Char.ToLower(str[0]).ToString();
            }
            return string.Empty;
        }

        public static bool TestIsPlural(string word)
        {
            word = word.ToLower();
            if (word.Length <= 2)
            {
                return false; // not a word that can be made singular if only two letters!
            }
            /*
            if (_dictionary.ContainsValue(word.ToLower()))
            {
                return true; //it's definitely already a plural
            }
            */
            if (word.Length >= 4)
            {
                //1. If the word ends in a consonant plus -y, change the -y into -ie and add an -s to form the plural 
                // e.g. enemy--enemies baby--babies family--families
                switch (word.Substring(word.Length - 4))
                {
                    case "bies":
                    case "cies":
                    case "dies":
                    case "fies":
                    case "gies":
                    case "hies":
                    case "jies":
                    case "kies":
                    case "lies":
                    case "mies":
                    case "nies":
                    case "pies":
                    case "ries":
                    case "sies":
                    case "ties":
                    case "vies":
                    case "wies":
                    case "xies":
                    case "zies":
                    case "ches":
                    case "shes":
                        {
                            return true;
                        }
                }
            }

            if (word.Length >= 3)
            {
                switch (word.Substring(word.Length - 3))
                {
                    //box--boxes 
                    case "ses":
                    case "zes":
                    case "xes":
                        {
                            return true;
                        }
                }
            }
            if (word.Length >= 3)
            {
                switch (word.Substring(word.Length - 2))
                {
                    case "es":
                        {
                            return true;
                        }
                        
                    case "us":
                    case "ss":
                        {
                            return false;
                        }
                }
            }
            if (word.Substring(word.Length - 1) != "s")
            {
                return false; // not a plural word if it doesn't end in S
            }
            return true;
        }

    }
}
