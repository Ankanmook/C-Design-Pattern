﻿/* 
 * Model.cs 
 * 
 * Version:1.8
 * 
 * C# Program for T9 Dictionary Server
 * 
 * 
 * Server Side Model 
 * Revisions: 8 
 *  
 * @Author : Ankan Mookerjee 
 * @Date   : 11/10/2013                
 *           
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace T9WcfService
{
    public class Model
    {

        /// <summary>
        /// File to read from the given file 
        /// </summary>
        private StreamReader file;

        /// <summary>
        /// Stores set of predictive keywords for entered text
        /// </summary>
        private Dictionary<String, SortedSet<String>> predict = new Dictionary<String, SortedSet<String>>();

        /// <summary>
        /// Stores Look up value of character with integer
        /// </summary>
        private Dictionary<char, int> lookup = new Dictionary<char, int>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Model()
        {
            //file = new StreamReader(@"F:\EBOOKS & ASSIGNMENTS\C#\C# A\HW2\T9\T9\english-words.txt");
            //file = new StreamReader(@"F:\\EBOOKS & ASSIGNMENTS\\C#\\C# A\\HW3\\Part1\\T9WcfService\\T9WcfService\\bin\\english-words.txt");
            file = new StreamReader(@"english-words.txt");

            //string[] file = System.IO.File.ReadAllLines("english-words.txt");
            
            setLookUpTables();
            read();
        }


        /// <summary>
        /// Sets the Look up tables for checking predictive
        /// and non predictive text
        /// </summary>
        public void setLookUpTables()
        {
            lookup.Add('a', 2); lookup.Add('b', 2); lookup.Add('c', 2);
            lookup.Add('d', 3); lookup.Add('e', 3); lookup.Add('f', 3);
            lookup.Add('g', 4); lookup.Add('h', 4); lookup.Add('i', 4);
            lookup.Add('j', 5); lookup.Add('k', 5); lookup.Add('l', 5);
            lookup.Add('m', 6); lookup.Add('n', 6); lookup.Add('o', 6);
            lookup.Add('p', 7); lookup.Add('q', 7); lookup.Add('r', 7);
            lookup.Add('s', 7); lookup.Add('t', 8); lookup.Add('u', 8);
            lookup.Add('v', 8); lookup.Add('w', 9); lookup.Add('x', 9);
            lookup.Add('y', 9); lookup.Add('z', 9);
           
        }


        /// <summary>
        /// Read Method stores all data given in
        /// dictionary text file to our data structure
        /// </summary>
        public void read()
        {
            string word;

            while ((word = this.file.ReadLine()) != null)
            {
                Int64 predictivekey = findkey(word);
                addpredictiveDictionary(predictivekey, word);
            }
        }


        /// <summary>
        /// Adds a word to predictive dictionary
        /// </summary>
        /// <param name="predictivekey"></param>
        public void addpredictiveDictionary(Int64 predictivekey, string word)
        {
            if (predict.ContainsKey(predictivekey.ToString()))
            {
                SortedSet<String> set = predict[predictivekey.ToString()];
                set.Add(word);
            }

            else
            {
                SortedSet<String> set = new SortedSet<string>();
                set.Add(word);
                predict.Add(predictivekey.ToString(), set);
            }
        }


        /// <summary>
        /// Find a predictive key set for string
        /// Entered
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Int64 findkey(String value)
        {
            Int64 key = 0;
            int len = value.Length;
            char indChar;
            Int64 indVal;

            for (int i = 0; i < len; i++)
            {
                indChar = value[i];

                indVal = lookup[indChar];
                key = (key * 10) + indVal;
            }

            return key;
        }




        /// <summary>
        /// Returns the predictive dictionary collection 
        /// of the data
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, SortedSet<String>> GetPredictiveDictionary()
        {
            return predict;
        }

    }
}