/* 
 * Solution.cs 
 * 
 * Version:1.8
 * 
 * C# Program for T9 dictionary
 * MVC Design Pattern
 * Controller Module
 * 
 * Revisions: 8 
 *  
 * @Author : Ankan Mookerjee
 * @Date   : 09/24/2013                
 *           
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace T9
{

    public class Controller
    {

        /// <summary>
        /// Predictive Dictionary
        /// </summary>
        Dictionary<String, SortedSet<String>> predict;
        
        /// <summary>
        /// Non Predictive Dictionary
        /// </summary>
        Dictionary<Int64, char> nonpredictLookup;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Controller()
        {
            setGetDictionary();
        }


        /// <summary>
        /// Invoke values from Model
        /// </summary>
        public void setGetDictionary()
        {
            T9.Model md = new Model();
            md.read();
            predict = md.GetPredictiveDictionary();
            nonpredictLookup = md.GetNonPredictiveDictionary();
        }

        /// <summary>
        /// Gets a Text of non predictive text 
        /// from a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public char getNonPredictiveText(String key)
        {
            return nonpredictLookup[Convert.ToInt64(key)];
        }

        /// <summary>
        /// Gets a set of Predictive Set
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SortedSet<string> getPredictiveSet(String key)
        {
            String textappend = "";
            SortedSet<string> returnset;

            if (predict.ContainsKey(key))
            {
                returnset = predict[key];

                return returnset;
            }

            else
            {

                for (int i = 0; i < key.Length; i++)
                {
                    textappend = textappend + "_";
                }



                returnset = new SortedSet<string>();
                returnset.Add(textappend);

                return returnset;
            }

        }

    }

}
