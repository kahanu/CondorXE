using System;
using System.Linq;
using Condor.Core;
using Zeus;

namespace WebApi4.Components.BusinessObjects
{
    public class EntityClassBase
    {
        #region ctors
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;

        public EntityClassBase(RequestContext context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
        }

        #endregion

        /// <summary>
        /// This needs to be the first line of the derived class's Render method.
        /// </summary>
        public void RenderBefore()
        {
            _output.autoTabLn("[Key]");
            _output.autoTabLn("public int Id { get; set; }");
            _output.autoTabLn("");
        }

        /// <summary>
        /// This needs to be the last line of the derived class's Render method.
        /// </summary>
        public void RenderAfter()
        {
            _output.autoTabLn("[HiddenInput(DisplayValue = false)]");
            _output.autoTabLn("[Timestamp]");
            _output.autoTabLn("public byte[] rowversion { get; set; }");
        }
    }
}
