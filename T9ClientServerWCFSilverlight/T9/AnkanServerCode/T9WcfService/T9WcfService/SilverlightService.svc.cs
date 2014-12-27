/* 
 * SilverlightService.cs 
 * Cross domain Policy class
 * 
 * Version:1.8
 * 
 * Server side code
 * Revisions: 8
 *  
 * @Author : Ankan Mookerjee 
 * @Date   : 11/11/2013                
 *           
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;


namespace T9WcfService
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SilverlightService : ISilverlightService
    {

        /// <summary>
        /// Parses the string of result into Stream
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Stream StringToStream(string result)
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml";
            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        /// <summary>
        /// gets the client access policy
        /// </summary>
        /// <returns>Stream</returns>
        public Stream GetClientAccessPolicy()
        {

            string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<access-policy>
  <cross-domain-access>
    <policy>
      <allow-from http-request-headers=""SOAPAction"">
        <domain uri=""*""/>
      </allow-from>
      <grant-to>
        <resource path=""/"" include-subpaths=""true""/>
      </grant-to>
    </policy>
  </cross-domain-access>
</access-policy>";

            return StringToStream(result);
        }

        /// <summary>
        /// Gets the cross domain policies
        /// </summary>
        /// <returns></returns>
        public Stream GetFlashPolicy()
            {
                string result = @"<?xml version=""1.0"" ?>
<!DOCTYPE cross-domain-policy SYSTEM 
	""http://www.macromedia.com/xml/dtds/cross-domain-policy.dtd"">
<cross-domain-policy>
  <allow-http-request-headers-from domain=""*"" headers=""SOAPAction,Content-Type""/>
</cross-domain-policy>";

                return StringToStream(result);
            }

    }
}
