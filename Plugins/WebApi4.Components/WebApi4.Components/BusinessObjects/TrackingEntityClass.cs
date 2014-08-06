using System;
using System.Linq;
using Condor.Core;
using WebApi4.Components.Interfaces;
using Zeus;

namespace WebApi4.Components.BusinessObjects
{
    /// <summary>
    /// This derived Entity class adds two extra properties for tracking purposes.
    /// </summary>
    public class TrackingEntityClass : EntityClassBase, IEntityClass
    {
        #region ctors

        private readonly RequestContext _context;
        private readonly IZeusOutput _output;

        public TrackingEntityClass(RequestContext context):base(context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
        }

        #endregion

        public string OmitList { get { return "Id,UpdatedBy,UpdatedOn,rowversion"; } }

        public void Render()
        {
            base.RenderBefore();
            
            _output.autoTabLn("[DisplayName(\"Updated By\")]");
            _output.autoTabLn("public string UpdatedBy { get; set; }");
            _output.autoTabLn("");
            _output.autoTabLn("[DisplayName(\"Updated On\")]");
            _output.autoTabLn("public DateTime? UpdatedOn { get; set; }");
            _output.autoTabLn("");

            base.RenderAfter();
        }
    }
}
