/* 
 * ISilverlightService.cs 
 * Inteface 
 * 
 * Version:1.8
 * 
 * Server side code
 * for making policy accessible to silverlight client
 * 
 * Revisions: 8
 *  
 * @Author : Ankan Mookerjee 
 * @Date   : 11/11/2013                
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace T9WcfService
{
    
    [ServiceContract]
    public interface ISilverlightService
    {
        [OperationContract, WebGet(UriTemplate = "/clientaccesspolicy.xml")]
        Stream GetClientAccessPolicy();

        [OperationContract, WebGet(UriTemplate = "/crossdomain.xml")]
        Stream GetFlashPolicy();


    }
}
