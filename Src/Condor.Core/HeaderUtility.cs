using System;
using System.Linq;

namespace Condor.Core
{
    public class HeaderUtility
    {
        private string _version = string.Empty;

        public HeaderUtility(string version)
        {
            _version = version;
        }

        public void WriteClassHeader(Zeus.IZeusOutput output)
        {
            string hdr = "/*" + Environment.NewLine;
            hdr += "===============================================================" + Environment.NewLine;
            hdr += "            CondorXE Code Generator - ver " + _version + Environment.NewLine;
            hdr += "       Generated using MyGeneration Software - ver 1.3.1.1" + Environment.NewLine;
            hdr += "Created By King Wilder - https://www.github.com/kahanu/CondorXE" + Environment.NewLine;
            hdr += "                   " + DateTime.Now.ToString() + Environment.NewLine;
            hdr += "===============================================================" + Environment.NewLine;
            hdr += "*/";
            output.writeln(hdr);
        }
    }
}
