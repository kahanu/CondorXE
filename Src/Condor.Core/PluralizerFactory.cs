using System;
using System.Linq;

namespace Condor.Core
{
    public enum ePluralizerTypes
    {
        /// <summary>
        /// The type remains unchanged.
        /// </summary>
        Unchanged,

        /// <summary>
        /// The type is forced to singular.
        /// </summary>
        Singular,

        /// <summary>
        /// The type is forced to plural.
        /// </summary>
        Plural
    }

    public class PluralizerFactory
    {
        /// <summary>
        /// Modify the input based on the settings on the Common tab.
        /// </summary>
        /// <param name="input">Class name, field name, or property name to change.</param>
        /// <param name="type">Enum: Unchanged, Singular, Plural</param>
        /// <returns></returns>
        public string SetWord(string input, ePluralizerTypes type)
        {
            return SetWord(input, type, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">Class name, field name, or property name to change.</param>
        /// <param name="type">Enum: Unchanged, Singular, Plural</param>
        /// <param name="isEntitySet">If EntitySet, then will check for Underscore.  EF designer does not pluralize EntitySets with underscores.</param>
        /// <returns></returns>
        public string SetWord(string input, ePluralizerTypes type, bool isEntitySet)
        {
            string result = input;
            bool containsUnderscore = false;

            if (isEntitySet)
            {
                containsUnderscore = input.Contains("_");
                type = ePluralizerTypes.Plural;
            }

            if (containsUnderscore)
            {
                return result;
            }


            switch (type)
            {
                case ePluralizerTypes.Unchanged:
                    break;
                case ePluralizerTypes.Singular:
                    result = MakeSingular(input);
                    break;
                case ePluralizerTypes.Plural:
                    result = MakePlural(input);
                    break;
                default:
                    break;
            }
            return result;
        }

        private string MakeSingular(string tableName)
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

        private string MakePlural(string word)
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
                }
            }
            if (word.Substring(word.Length - 2) == "ss")
            {
                return false;
            }
            if (word.Substring(word.Length - 1) != "s")
            {
                return false; // not a plural word if it doesn't end in S
            }
            return true;
        }

    }
}
