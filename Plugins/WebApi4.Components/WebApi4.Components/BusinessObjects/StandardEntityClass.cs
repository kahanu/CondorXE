using System;
using System.Linq;
using Condor.Core;
using WebApi4.Components.Interfaces;
using Zeus;

namespace WebApi4.Components.BusinessObjects
{
    /// <summary>
    /// This class renders the primary key for the class only, with the 
    /// [Key] DataAnnotations attribute.
    /// </summary>
    public class StandardEntityClass : EntityClassBase, IEntityClass
    {
        private readonly RequestContext _context;
        private readonly IZeusOutput _output;

        public StandardEntityClass(RequestContext context):base(context)
        {
            this._context = context;
            this._output = context.Zeus.Output;
        }

        public void Render()
        {
            base.RenderBefore();
            base.RenderAfter();
        }

        public string OmitList { get { return "Id,rowversion"; } }
    }
}
