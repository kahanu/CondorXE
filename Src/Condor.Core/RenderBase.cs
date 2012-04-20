using System;
using System.Linq;
using Condor.Core.Interfaces;

namespace Condor.Core
{
    /// <summary>
    /// Each code generating class should inherit from this class.
    /// </summary>
    public class RenderBase
    {
        protected Zeus.IZeusOutput _output;
        protected ScriptSettings _script;
        protected HeaderUtility _hdrUtil;

        public RenderBase(Zeus.IZeusOutput output)
        {
            this._output = output;
            this._script = ScriptSettings.GetInstance();
            this._hdrUtil = new HeaderUtility(_script.Version);
        }

        /***************************************************************
        *** In MyGeneration, there are several saving options.
        *** The two "actions" that I use are:
        ***   "d" = don't overwrite
        ***   "o" = overwrite
        *** Check the Zeus chm file for more information.
        *** Use the easy SaveActions static class for this parameter.
        ***************************************************************/
        public void SaveOutput(string filePath, object action)
        {
            SaveOutput(filePath, action, true);
        }

        public void SaveOutput(string filePath, object action, bool clearOutput)
        {
            if (_script.Settings.Common.SaveToFile)
            {
                try
                {
                    _output.save(filePath, action);
                    if (clearOutput) 
                    { 
                        _output.clear();
                        _output.tabLevel = 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("SaveOutput exception: " + ex.Message);
                }
            }
        }

        public string CreateFullPath(string middlePath, string fileName)
        {
            string outputFilePath = GetOutputPath();
            string fullPath = string.Empty;

            if (middlePath != "")
            {
                if (!middlePath.EndsWith("\\")) middlePath += "\\";
            }

            fullPath = outputFilePath + middlePath + fileName;
            return fullPath;
        }

        public string GetOutputPath()
        {
            if (_script == null)
                throw new NullReferenceException("_script");

            string outputFilePath = _script.OutputPath;
            if (!outputFilePath.EndsWith("\\"))
                outputFilePath += "\\";

            return outputFilePath;
        }
    }
}
