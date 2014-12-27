/* 
 * Solution.cs 
 * 
 * Version:1.2
 * 
 * C# Program for Hosting T9 DictionaryService
 *
 * Revisions: 2
 *  
 * @Author : Ankan Mookerjee 
 * @Date   : 10/25/2013                
 *           
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Description;
using T9WcfService;


namespace T9Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(T9WcfService.T9Service));
            ServiceHost cross = new ServiceHost(typeof(T9WcfService.SilverlightService));
            Console.WriteLine("Ankans service Running");
            Console.WriteLine("url:http://localhost:3355/T9Service.svc/");
            host.Open();
            cross.Open();
            Console.WriteLine("Server Running...press any key to close");
            Console.ReadKey();
            host.Close();
        }




  
    }
}

