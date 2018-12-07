using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DCPServiceRepository.Common
{
    public class HashGenerator
    {

        public HashGenerator()
        {
            
        }
        public static string GenerateHashKey(string rawData)
        {

            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        //public static String GenerateKey(Object sourceObject)
        //{
        //    String hashString;

        //    //Catch unuseful parameter values
        //    if (sourceObject == null)
        //    {
        //        throw new ArgumentNullException("Null as parameter is not allowed");
        //    }
        //    else
        //    {
        //        //We determine if the passed object is really serializable.
        //        try
        //        {
        //            //Now we begin to do the real work.
        //            hashString = ComputeHash(ObjectToByteArray(sourceObject));
        //            return hashString;
        //        }
        //        catch (AmbiguousMatchException ame)
        //        {
        //            return string.Empty;
        //        }
        //    }
        //}

        //private static string ComputeHash(byte[] objectAsBytes)
        //{
        //    MD5 md5 = new MD5CryptoServiceProvider();
        //    try
        //    {
        //        byte[] result = md5.ComputeHash(objectAsBytes);

        //        // Build the final string by converting each byte
        //        // into hex and appending it to a StringBuilder
        //        StringBuilder sb = new StringBuilder();
        //        for (int i = 0; i < result.Length; i++)
        //        {
        //            sb.Append(result[i].ToString("X2"));
        //        }

        //        // And return it
        //        return sb.ToString();
        //    }
        //    catch (ArgumentNullException ane)
        //    {
        //        //If something occurred during serialization, 
        //        //this method is called with a null argument. 
        //        //Console.WriteLine("Hash has not been generated.");
        //        return null;
        //    }
        //}

        //private static readonly Object locker = new Object();

        //private static byte[] ObjectToByteArray(Object objectToSerialize)
        //{
        //    MemoryStream fs = new MemoryStream();
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    try
        //    {
        //        //Here's the core functionality! One Line!
        //        //To be thread-safe we lock the object
        //        lock (locker)
        //        {
        //            formatter.Serialize(fs, objectToSerialize);
        //        }
        //        return fs.ToArray();
        //    }
        //    catch (SerializationException se)
        //    {
        //        //Console.WriteLine("Error occurred during serialization. Message: " + se.Message);

        //        return null;
        //    }
        //    finally
        //    {
        //        fs.Close();
        //    }
        //}

       
    }
}
