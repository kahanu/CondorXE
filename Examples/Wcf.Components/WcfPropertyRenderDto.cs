using Condor.Core;

namespace Wcf.Components
{
    public class WcfPropertyRenderDto : Condor.Core.PropertyObjects.BusinessObjectsPropertyRenderShortProperty
    {
        public WcfPropertyRenderDto(MyMeta.IColumn column, RequestContext context)
            : base(column, context)
        {

        }

        public override void Render()
        {
            _output.autoTabLn("[DataMember]");
            if (ToPropertyName().ToLower() == "rowversion")
            {
                _output.autoTabLn("public string " + this.Alias + " { get; set; }");
            }
            else
            {
                _output.autoTabLn(ToString());
            }
        }
    }
}
