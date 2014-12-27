/* 
 * IT9Service.cs 
 * Interface 
 * 
 * Version:1.2
 * 
 * Server Side Code
 * 
 * Revisions: 2
 *  
 * @Author : Ankan Mookerjee 
 * @Date   : 11/11/2013                
 */           

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace T9WcfService
{
    /// <summary>
    /// Service Contract
    /// </summary>
    [ServiceContract]
    public interface IT9Service
    {
        /// <summary>
        /// Operation contract
        /// The only method I intend to expose
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetData(Int64 key);
    }

}
