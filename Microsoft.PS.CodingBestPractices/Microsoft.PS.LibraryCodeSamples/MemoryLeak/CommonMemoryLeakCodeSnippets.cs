using System.Collections.Generic;
using System.IO;

namespace Microsoft.PS.LibraryCodeSamples.MemoryLeak
{
    #region Streams are not closed properly

    public class StreamReaderMemoryLeaks
    {
        //****Possible Memory Leak****//
        //We create a StreamReader instance that is not closed
        //Internally, every Reader or Writer instance has some sort of Stream object that must be closed or disposed of after usage using the .Close() or .Dispose() methods
        //In this example, the internal Stream is not properly disposed which leads to its internal resources not being properly released
        public static string ReadAll()
        {
            var filePath = @"C:UtilsLog.txt";
            var sr = new StreamReader(filePath);
            return sr.ReadToEnd();
        }

        //****Recommended Solution****//
        public static string ReadAllWithUsing()
        {
            var filePath = @"C:UtilsLog.txt";
            using (var sr = new StreamReader(filePath))
            {
                return sr.ReadToEnd();
            }
        }

        //OR
        public static string ReadAllWithClose()
        {
            var filePath = @"C:UtilsLog.txt";
            var sr = new StreamReader(filePath);
            var result = sr.ReadToEnd();
            sr.Close();
            return result;
        }
       
    }
    #endregion

    #region Static References 

    //We have a global static instance of an object – e.g. a Singleton – and its lifetime is “forever”
    //So if a method of the static instance allocates some memory, this memory will never be reclaimed unless you stop the application
    //The real problems here are the chained references - When the static instance references another object, that, in turn, references another object, and so on and so forth    
    public class Singleton
    {
        private static Singleton instance;
        private static List<BigObject> listOfBigObjects = new List<BigObject>();
 
	    private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }

        //Every time you call Singleton.CreateBigObject() a new “BigObject” instance is created and is added to the static List named listOfBigObjects
        //Since the Singleton lives forever, its listOfBigObjects does as well, as does every BigObject added to the list. 
        //The created chain won’t allow the garbage collector to reclaim the memory off the BigObject instances
        public static BigObject CreateBigObject()
        {
            var bigObject = new BigObject();
            bigObject.AllocateMemory(4096);
            listOfBigObjects.Add(bigObject);
            return bigObject;
        }
    }

    public class BigObject
    {
        public int[] BigArray { get; private set; }

        public void AllocateMemory(int memoryBytes)
        {
            BigArray = new int[memoryBytes];
        }
    }

    #endregion
}
