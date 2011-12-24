using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeWrapper
{
    public class Wrapper
    {
        public string Process(string textBox)
        {
            StringReader reader = new StringReader(textBox);
            string input = null;
            string buffer = string.Empty;
            int tabCounter = 0;

            while ((input = reader.ReadLine()) != null)
            {
                int localCounter = 0;
                int diffCounter = 0;
                string thisline = input;
                string tabBuffer = string.Empty;

                while (thisline.StartsWith("\t"))
                {
                    localCounter++;
                    thisline = thisline.Substring(1);
                }
                diffCounter = localCounter - tabCounter;
                tabCounter += diffCounter;

                for (int i = 0; i < tabCounter; i++)
                {
                    tabBuffer += "\t";
                }

                if (diffCounter > 0)
                {
                    //buffer += tabBuffer + "_output.tabLevel++;\n";
                    buffer += tabBuffer + "_output.tabLevel++;" + Environment.NewLine;
                }
                else if (diffCounter < 0)
                {
                    //buffer += tabBuffer + "_output.tabLevel--;\n";
                    buffer += tabBuffer + "_output.tabLevel--;" + Environment.NewLine;
                }
                
                string pattern = "\\\"";
                thisline = Regex.Replace(thisline, pattern, "\\\"");

                //buffer += tabBuffer + "_output.autoTabLn(\"" + thisline + "\");\n";
                buffer += tabBuffer + "_output.autoTabLn(\"" + thisline + "\");" + Environment.NewLine;
            }
            return buffer;
        }
    }
}
