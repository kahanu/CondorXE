using System.Collections;
using Condor.Core.Interfaces;

namespace Condor.Core
{
    public class RequestContext
    {
        public MyMeta.dbRoot MyMeta { get; set; }
        public Zeus.IZeusContext Zeus { get; set; }
        public ArrayList FileList { get; set; }
        public ScriptSettings ScriptSettings { get; set; }
        public ProgressDialogWrapper Dialog { get; set; }
        public MyMeta.IDatabase Database { get; set; }
        public CommonUtility Utility { get; set; }
    }
}
