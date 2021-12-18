using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;

namespace WebLib 
{


    public interface IAjaxViewInfoProvider
    {
        AjaxViewInfo GetViewInfo(AjaxRequest AjaxRequest, AjaxPacket Packet);
    }


    public class AjaxViewInfoProvider : IAjaxViewInfoProvider
    {
        public virtual AjaxViewInfo GetViewInfo(AjaxRequest AjaxRequest, AjaxPacket Packet)
        {
            return null;
        }
    }
}
