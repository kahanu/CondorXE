using System;
using System.Collections;
using System.Xml;
using Condor.Core;
using Zeus;

namespace Condor.Core.Interfaces
{
    public interface IScriptSettings
    {
        string Version { get; }

        Dnp.Utils.Utils DnpUtils { get; }

        MyMeta.dbRoot Database { get; }

        string DatabaseName { get; }

        ArrayList Tables { get; }

        string OutputPath { get; }

        WebAppSettings Settings { get; }

        void SaveSettings();

        /// <summary>
        /// This gets the "value" attribute from the CondorXELoader.xml file
        /// for the selected "class" attribute.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        string GetClassValue(string className);

        /// <summary>
        /// From the MyGeneration connection string, chop off the leading
        /// 'Provider=[provider name];'.
        /// </summary>
        /// <param name="fullConnString"></param>
        /// <returns></returns>
        string ChopProvider(string fullConnString);
    }
}