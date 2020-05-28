using System;
using System.Collections;
using System.Data;
using System.Net.NetworkInformation;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.RuntimePublic.Db;

namespace OutSystems.NssPing_extension {

	public class CssPing_extension: IssPing_extension {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ssipaddress"></param>
		/// <param name="sspingable"></param>
		public void MssPingHost(string ssipaddress, out bool sspingable) {
			sspingable = false;
            using (Ping pinger = new Ping())
            {
                try
                {
                    PingReply reply = pinger.Send(ssipaddress);
                    sspingable = reply.Status == IPStatus.Success;
                }
                catch (PingException e)
                {
                    //if (throwExceptionOnError) throw e;
                    sspingable = false;
                }
            }
            //return sspingable;
        } // MssPingHost


	} // CssPing_extension

} // OutSystems.NssPing_extension

