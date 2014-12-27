Programming Skills (Design Patterns and C#) CSCI-641

HASHTABLE
Overview

This class that implements a chained hash table. The class conform to the Table interface provided. 



Design

Hash tables are one way of implementing a simple associative storage table.  Associative means that each value stored is associated with a key – in our case, a unique key. Specifically, hash tables accomplish this by providing an array, and then using a hash function on the key to decide at what location the value should be. We are calling that location a bucket.  Collisions happen when more than one needed key hashes to the same bucket.  I am handling collisions by attaching some kind of collection data  structure to each bucket  (chaining)  rather than  searching  for an alternative bucket  (open addressing)  in which to store the value.

Assume that the key class has implemented Equals and GetHashCode appropriately. To  realize  this  property of making sure that Table.put is always successful and  still maintain good performance,  code is expected  to be able to rehash,  i.e., expand  the table  and reassign the keys to new buckets  when the load gets above a certain  fraction.  Load is defined as the ratio of the number of entries in the table (not number of buckets) to the size of the array. It is supposed to indicate the likelihood of a collision. The more likely a collision, the harder it is to claim that table operations occur in O(1) time. For the sake of this exercise, expansion is fixed at +50% for each time rehashing is done.



Testing

The  Table interface,  along  with  an  exception  definition,  a  table  factory,  and  a  simple  test program  is provided  in Main.cs.  
Implementation

My implementation class is LinkedHashTable. Test code in a class called Test- Table that has a method

p u b l i c   s t a t i c   v o i d   t e s t ( )

