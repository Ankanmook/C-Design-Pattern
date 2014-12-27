/* 
 * Model.cs 
 * 
 * Version:1.11
 * 
 * C# Program for T9 Dictionary Server
 * 
 * 
 * Client Side Model 
 * Revisions: 11 
 *  
 * @Author : Ankan Mookerjee 
 * @Date   : 11/15/2013                
 *           
 */
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections;

namespace T9Client
{
    public class Model
    {


        /// <summary>
        /// Stores Look up value of character with integer
        /// </summary>
        public Dictionary<Int64, char> nonpredictlookup;

        /// <summary>
        /// Default Contructor
        /// </summary>
        public Model()
        {
            nonpredictlookup = new Dictionary<Int64, char>();
            setLookUpTables();
        }

        /// <summary>
        /// Sets the Look up tables for checking predictive
        /// and non predictive text
        /// </summary>
        public void setLookUpTables()
        {
            nonpredictlookup.Add(2, 'a'); nonpredictlookup.Add(22, 'b');
            nonpredictlookup.Add(222, 'c'); nonpredictlookup.Add(3, 'd');
            nonpredictlookup.Add(33, 'e'); nonpredictlookup.Add(333, 'f');
            nonpredictlookup.Add(4, 'g'); nonpredictlookup.Add(44, 'h');
            nonpredictlookup.Add(444, 'i'); nonpredictlookup.Add(5, 'j');
            nonpredictlookup.Add(55, 'k'); nonpredictlookup.Add(555, 'l');
            nonpredictlookup.Add(6, 'm'); nonpredictlookup.Add(66, 'n');
            nonpredictlookup.Add(666, 'o'); nonpredictlookup.Add(7, 'p');
            nonpredictlookup.Add(77, 'q'); nonpredictlookup.Add(777, 'r');
            nonpredictlookup.Add(7777, 's'); nonpredictlookup.Add(8, 't');
            nonpredictlookup.Add(88, 'u'); nonpredictlookup.Add(888, 'v');
            nonpredictlookup.Add(9, 'w'); nonpredictlookup.Add(99, 'x');
            nonpredictlookup.Add(999, 'y'); nonpredictlookup.Add(9999, 'z');
        }


        /// <summary>
        /// Gets a Text of non predictive text 
        /// from a given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public char getNonPredictiveText(String key)
        {
            if (nonpredictlookup.ContainsKey(Convert.ToInt64(key)))
            {
                return nonpredictlookup[Convert.ToInt64(key)];
            }

            else
            {
                //Only choose last 4 characters of the key
                key = key.Substring(key.Length-5,4);
                return nonpredictlookup[Convert.ToInt64(key)];
            }
        }
        


    }
}
