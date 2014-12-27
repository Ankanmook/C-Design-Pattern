/* 
 * Solution.cs 
 * 
 * Version:1.8
 * 
 * C# Program for generating Chained Hash Table
 * 
 * Revisions: 8 
 *  
 * @Author : Ankan Mookerjee & Jeremy Brown
 * @Date   : 09/09/2013                
 *           
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace RIT_CS

{
    /// <summary>
    /// An exception used to indicate a problem with how
    /// a HashTable instance is being accessed
    /// </summary>
    public class NonExistentKey<Key> : Exception
    {
        /// <summary>
        /// The key that caused this exception to be raised
        /// </summary>
        public Key BadKey { get; private set; }

        /// <summary>
        /// Create a new instance and save the key that
        /// caused the problem.
        /// </summary>
        /// <param name="k">
        /// The key that was not found in the hash table
        /// </param>
        public NonExistentKey(Key k) :
            base("Non existent key in HashTable: " + k)
        {
            BadKey = k;
        }

    }

    /// <summary>
    /// An associative (key-value) data structure.
    /// A given key may not appear more than once in the table,
    /// but multiple keys may have the same value associated with them.
    /// Tables are assumed to be of limited size are expected to automatically
    /// expand if too many entries are put in them.
    /// </summary>
    /// <param name="Key">the types of the table's keys (uses Equals())</param>
    /// <param name="Value">the types of the table's values</param>
    interface Table<Key, Value> : IEnumerable<Key>
    {
        /// <summary>
        /// Add a new entry in the hash table. If an entry with the
        /// given key already exists, it is replaced without error.
        /// put() always succeeds.
        /// (Details left to implementing classes.)
        /// </summary>
        /// <param name="k">the key for the new or existing entry</param>
        /// <param name="v">the (new) value for the key</param>
        void Put(Key k, Value v);

        /// <summary>
        /// Does an entry with the given key exist?
        /// </summary>
        /// <param name="k">the key being sought</param>
        /// <returns>true iff the key exists in the table</returns>
        bool Contains(Key k);

        /// <summary>
        /// Fetch the value associated with the given key.
        /// </summary>
        /// <param name="k">The key to be looked up in the table</param>
        /// <returns>the value associated with the given key</returns>
        /// <exception cref="NonExistentKey">if Contains(key) is false</exception>
        Value Get(Key k);
    }


    public class LinkedHashTable<Key,Value> : Table<Key,Value>
    {

        /// <summary>
        /// HashMap Container. 
        /// List of Tuple of Key and List of Value object
        /// Containes Tuple of key and Bucket
        /// Bucket is the list of values associated
        /// with tha key (To Avoid Collision )
        /// </summary>
        List<Tuple<Key,List<Value>>> hashMap = new List<Tuple<Key,List<Value>>>();


        /// <summary>
        /// List of Integer object
        /// Contains the count of 
        /// values in each bucket for a key
        /// </summary>
        List<int> countofBucket = new List<int>();

        /// <summary>
        /// Index of Value in Bucket which is to be Outputed
        /// List of Integer Object
        /// Contains the map of Key to the index of the output value of 
        /// bucket to used for output of hashmap during enumeration
        /// </summary>
        List<int>  indexbucketValue = new List<int>();

        
        /// <summary>
        /// Capacity
        /// Int Primitive
        /// Contains the original capacity of the Hash Table
        /// at the time of creating the Linked Hash List
        /// </summary>
        int capacity;

        /// <summary>
        /// Load Threshold of HashTable
        /// Double Primitive 
        /// Contains Load Threshold Value provided by user
        /// </summary>
        double loadThreshold;

        /// <summary>
        /// Number of Table Entries
        /// Int Primitive
        /// Containes the total number of Distinct Key Entries
        /// </summary>
        int numberofTableEntry;

        /// <summary>
        /// Key Count
        /// Int Primitive
        /// Containes the total number of keys in the Hash Table
        /// </summary>
        int keyCount;

        /// <summary>
        /// Custom Constructor
        /// Captures the value of capacity and Load Threshold
        /// from the user
        /// </summary>
        /// <param name="capacity">Int(DEFAULT VAL 100)</param>
        /// <param name="loadThreshold">Threshold(DEFAULT VAL 0.75)</param>
        public LinkedHashTable(int capacity = 100, double loadThreshold = 0.75)
        {
            this.loadThreshold = loadThreshold;
            this.capacity = capacity;

            keyCount= 0;
            numberofTableEntry = 0;
        }

        /// <summary>
        /// Add a new entry in the hash table. If an entry with the
        /// given key already exists, it is replaced without error.
        /// put() always succeeds.
        /// (Details left to implementing classes.)
        /// </summary>
        /// <param name="k">the key for the new or existing entry</param>
        /// <param name="v">the (new) value for the key</param>
        public void Put(Key k, Value v)
        {
            // If this key is already in the HashTable
            // then only add element to the bucket
            if (this.Contains(k))
            {
    
                //If this key value set is new to Hash Set
                //Otherwise do nothing
                if (this.ContainsVal(k,v))
                {

                //Reference of the bucket linked to the Key
                List<Value> newList = getBucket(k);

                //Adding New Element to the bucket
                //The bucket is already attached to the Hash Table Object
                newList.Add(v);

                int index = findIndexofValueinBucket(k);
                //Incrementing index value of bucket by one
                ++indexbucketValue[index];

                //Incrementing the table entry
                ++numberofTableEntry;
                }
            }

            // HashTable does not contain the key element
            //then add element to map and bucket
            else
            {
                //Creating a bucket for the Key
                List<Value> newList = new List<Value>(capacity);
                
                newList.Add(v);
                //Adding the bucket to the Hash Table 
                hashMap.Add(new Tuple<Key,List<Value>>(k,newList));

                //Initializing the bucket iterator to the 
                //Index Output Map which maps Key to index
                //This is added in the order of Key in HashMap
                this.indexbucketValue.Add(1);

                countofBucket.Add(0);

                //Incrementing key count
                ++keyCount;
                //Incrementing count of entry to linked hash table
                ++numberofTableEntry;   
            }

            //Expansion of 50% for the bucket if Rehashing Leeds to
            //increase in load capacity over the given Ration
            //This procedure checks each bucket and only individually
            //increases capacity of one bucket if that particular bucket
            //gets more rehashing than its load capacity
            foreach (var d in hashMap)
            {
                //Checking the count to capacity ration .It should be less
                //than load threshold ratio
                if (d.Item2.Count>= (d.Item2.Capacity * loadThreshold))
                {
                    //50% Expansion in the capacity of the bucket
                    d.Item2.Capacity = d.Item2.Capacity * 3 / 2;
                }    
            }                  
        }

        /// <summary>
        /// Finds the index of value in bucket
        /// </summary>
        /// <param name="k">Key</param>
        /// <returns>int</returns>
        public int findIndexofValueinBucket(Key k)
        {
            int index = 0;
            foreach (var h in hashMap)
            {
                if (h.Item1.Equals(k))
                {
                    return index;
                }

                index++;
            }

            return index;
        }


        /// <summary>
        /// Get Bucket
        /// Returns the full list of bucket values
        /// for the given key
        /// </summary>
        /// <param name="k">Key</param>
        /// <returns>List of Value</returns>
        public List<Value> getBucket(Key k)
        {
            foreach (var v in hashMap)
            {
                if (k.Equals(v.Item1))
                {
                    return v.Item2;
                }
            }

            return null;
        }


        /// <summary>
        /// Contains Value in bucket for a Key
        /// This function checks Redundancy of Same key-val pair 
        /// in Hash Table. Avoids the duplicacy of same key-value
        /// set to a Hash Table 
        /// </summary>
        /// <param name="k">The key and Value pair in Hash Table</param>
        /// <param name="v">boolean key-value pair is already in table</param>
        /// <returns></returns>
        public bool ContainsVal(Key k, Value v)
        {
            foreach (var t in hashMap)
            {
                if (t.Item1.Equals(k))
                {
                    foreach (var l in t.Item2)
                    {
                        if (l.Equals(v))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Does an entry with the given key exist?
        /// </summary>
        /// <param name="k">the key being sought</param>
        /// <returns>true if the key exists in the table</returns>
        public bool Contains(Key k)
        {
            foreach (var t in hashMap)
            {
                if (t.Item1.Equals(k))
                {
                    return true;
                }
            }
            return false;
        }

        
        /// <summary>
        /// Fetch the value associated with the given key.
        /// This entry returns the value in bucket in order
        /// which they were added in case of collision
        /// </summary>
        /// <param name="k">The key to be looked up in the table</param>
        /// <returns>the value associated with the given key</returns>
        /// <exception cref="NonExistentKey">if Contains(key) is false</exception>
        public Value Get(Key k)
        {
            int index = this.findIndexofValueinBucket(k);
            List<Value> bucket = getBucket(k);

            if (this.Contains(k))
            {
                if (countofBucket[index] < indexbucketValue[index])
                {
                    ++countofBucket[index];

                    //Increment the output index 
                    return bucket[countofBucket[index]-1];
                }

                else
                {
                    //Reset the output index to 0 
                    countofBucket[index] = 0;
                    return bucket[countofBucket[index]];
                }
            }

            //If there is no element then it throws non existent
            //key exception
            else
            {
                throw new NonExistentKey<Key>(k);
            }
        }
        
         
        /// <summary>
        /// Get Enumerator 
        /// Implemented with IEnumerable Inteface
        /// For doing enumerations across Hash Table 
        /// Used for Foreach loop enumerations
        /// </summary>
        /// <returns>IEnumerator object</returns>
        public IEnumerator<Key> GetEnumerator()
        {
            
            //Outputing the keys in order of bucket
            foreach (var h in hashMap)
            {
                foreach ( var l in h.Item2)
                {
                   //yield return key
                   yield return h.Item1;
                }
            }
        }
        

        /// <summary>
        /// IEnumerable GetEnumerator
        /// Implemented with IEnumerable Inteface
        /// For doing enumerations across Hash Table 
        /// Used for Foreach loop enumerations
        /// </summary>
        /// <returns>IEnumerator Object</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var h in hashMap)
            {
                    yield return h.Item1;
            }
        }


    }


    class TableFactory
    {
        /// <summary>
        /// Create a Table.
        /// (The student is to put a line of code in this method corresponding to
        /// the name of the Table implementor s/he has designed.)
        /// </summary>
        /// <param name="K">the key type</param>
        /// <param name="V">the value type</param>
        /// <param name="capacity">The initial maximum size of the table</param>
        /// <param name="loadThreshold">
        /// The fraction of the table's capacity that when
        /// reached will cause a rebuild of the table to a 50% larger size
        /// </param>
        /// <returns>A new instance of Table</returns>
        public static Table<K, V> Make<K, V>(int capacity = 100, double loadThreshold = 0.75)
        {
            return new LinkedHashTable<K, V>(capacity, loadThreshold);
        }
    }

    /// <summary>
    /// CLASS Test Table
    /// Tests the Linked Hash Table with various test case
    /// </summary>
    class TestTable
    {

        /// <summary>
        /// Static Test Method
        /// Runs 3 Test Cases
        /// Please have a file writes option to output test in text file
        /// because 2000 values test case is overshadowing the screen 
        /// not showing the first ones 1800 ones.
        /// Returns Void
        /// Parameters None
        /// </summary>
        public static void test()
        {

            //TEST CASE 1
            //2000 Replicated Test Case 

            
            Table<String, String> ht1 = TableFactory.Make<String, String>(5, 0.4);

            for (int iter = 0; iter < 2000; iter++)
            {
                ht1.Put("Ankan", "Mookherjee" + iter.ToString());
            }
            try
            {
                foreach (String first in ht1)
                {
                    Console.WriteLine(first + " -> " + ht1.Get(first));
                }
                Console.WriteLine("=========================");
            }
 

            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }
            

            //TEST CASE 2
            //12 Test Cases
            Table<String, String> ht2 = TableFactory.Make<String, String>(10, 0.6);
            
            ht2.Put("Joe", "Doe");
            ht2.Put("Joe", "Brain");
            ht2.Put("Joe", "Swiss");
            ht2.Put("Joe", "Doe");
            ht2.Put("Joe", "Ankan");
            ht2.Put("Joe", "Aurodeep");
            ht2.Put("Joe", "Brown");
            ht2.Put("Joe", "Ajay");
            ht2.Put("Joe", "Jeremy");
            ht2.Put("Joe", "Travis");
            ht2.Put("Joe", "Sujal");
            ht2.Put("Joe", "Patel");

            //Testing to see if Redundant key value entries get stored in bucket again
            ht2.Put("Joe", "Doe");
            ht2.Put("Joe", "Brain");
            ht2.Put("Joe", "Swiss");
            ht2.Put("Joe", "Doe");
            ht2.Put("Joe", "Ankan");
            ht2.Put("Joe", "Aurodeep");
            ht2.Put("Joe", "Brown");
            ht2.Put("Joe", "Ajay");
            ht2.Put("Joe", "Jeremy");
            ht2.Put("Joe", "Travis");
            ht2.Put("Joe", "Sujal");
            ht2.Put("Joe", "Patel");

            ht2.Put("Joe", "Doe");
            ht2.Put("Joe", "Brain");
            ht2.Put("Joe", "Swiss");
            ht2.Put("Joe", "Doe");
            ht2.Put("Joe", "Ankan");
            ht2.Put("Joe", "Aurodeep");
            ht2.Put("Joe", "Brown");
            ht2.Put("Joe", "Ajay");
            ht2.Put("Joe", "Jeremy");
            ht2.Put("Joe", "Travis");
            ht2.Put("Joe", "Sujal");
            ht2.Put("Joe", "Patel");

            try
            {
                foreach (String first in ht2)
                {
                    Console.WriteLine(first + " -> " + ht2.Get(first));
                }
                Console.WriteLine("=========================");
            }

            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }



            //TEST CASE 3
            //19 Test Cases
            Table<int,decimal> ht3 = TableFactory.Make<int, decimal>(4, 0.5);
            for (int iter = 0; iter < 20; iter++)
            {
                ht3.Put(iter, new decimal(iter));
            }

            try
            {
                foreach (int first in ht3)
                {
                    Console.WriteLine(first + " -> " + ht3.Get(first));
                }
                Console.WriteLine("=========================");
            }

            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

        }
    }



    class MainClass
    {
        public static void Main(string[] args)
        {
            //Calling the Test Case Class
            TestTable.test();

            Table<String, String> ht = TableFactory.Make<String, String>(4, 0.5);
            ht.Put("Joe", "Doe");
            ht.Put("Jane", "Brain");
            ht.Put("Chris", "Swiss"); 
            try
            {
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

                ht.Put("Wavy", "Gravy");
                ht.Put("Chris", "Bliss");
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

                Console.Write("Jane -> ");
                Console.WriteLine(ht.Get("Jane"));
                Console.Write("John -> ");
                Console.WriteLine(ht.Get("John"));
            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }

            Console.ReadLine();
        }
    }
}
