using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace TC.GPConquest.Utility
{
    /* Thanks to g-klein 
      https://github.com/g-klein/ForgeAuthoritativeMovementDemo
     */

    public static class ByteArrayUtils
    {
        public static byte[] ObjectToByteArray(Object obj)
        {
            byte[] res = null;
            if (!ReferenceEquals(obj, null))
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    res = ms.ToArray();
                }
            }
            return res;
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            object res = null;
            if (!ReferenceEquals(arrBytes, null) && arrBytes.Length>0)
            {
                using (var memStream = new MemoryStream())
                {
                    var binForm = new BinaryFormatter();
                    memStream.Write(arrBytes, 0, arrBytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    var obj = binForm.Deserialize(memStream);
                    res = obj;
                }
            }
            return res;
        }
    }
}