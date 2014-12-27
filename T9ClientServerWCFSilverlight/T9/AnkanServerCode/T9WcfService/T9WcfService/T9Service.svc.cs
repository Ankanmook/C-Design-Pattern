/* 
 * T9Service.cs 
 * 
 * Version:1.8
 * 
 * C# Program for T9 Dictionary Service
 * Now using for building client side code 
 * for my service
 * 
 * Model Module
 * Revisions: 16
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
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.IO;
using System.Text;

namespace T9WcfService
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class T9Service : IT9Service
    {

        /// <summary>
        /// Predictive Dictionary
        /// </summary>
        Dictionary<String, SortedSet<String>> predict;

        /// <summary>
        /// A model object
        /// </summary>
        Model md;


        /// <summary>
        /// This is the set that we will return
        /// </summary>
        SortedSet<string> returnset;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public T9Service()
        {
            md = new Model();
            predict = md.GetPredictiveDictionary();
        }


        /// <summary>
        /// Get Data Method
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>List of String</returns>
        public List<string> GetData(Int64 key)
        {
            
            String textappend = "";
            List<string> strLst = new List<string>();

            if (predict.ContainsKey(key.ToString()))
            {
                returnset = predict[key.ToString()];
            }

            else
            {
                for (int i = 0; i < key.ToString().Length; i++)
                {
                    textappend = textappend + "-";
                }

                returnset = new SortedSet<string>();
                returnset.Add(textappend);
            }

            foreach (string var in returnset)
            {
                strLst.Add(var);
            }

            return strLst;
        }
    }
}
