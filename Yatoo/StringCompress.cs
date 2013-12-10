using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yatoo
{
    public class StringCompress
    {
        public static string Decompress(byte[] bys)
        {
            return Decompress(Convert.ToBase64String(bys));
        }
        public static string Decompress(string strSource)
        {
            return Decompress(strSource, (3 * 1024 * 1024 + 256));//字符串不会超过3M
        }
        public static string Decompress(string strSource, int length)
        {
            byte[] buffer = Convert.FromBase64String(strSource);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;
            System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress);
            stream.Flush();

            int nSize = length;
            byte[] decompressBuffer = new byte[nSize];
            int nSizeIncept = stream.Read(decompressBuffer, 0, nSize);
            stream.Close();

            return System.Text.Encoding.Unicode.GetString(decompressBuffer, 0, nSizeIncept);//转换为普通的字符串
        }


        public static string Compress(string strSource)
        {
            if (strSource == null)
                throw new System.ArgumentException("字符串为空！");

            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            byte[] buffer = encoding.GetBytes(strSource);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Compress, true);
            stream.Write(buffer, 0, buffer.Length);
            stream.Close();

            buffer = ms.ToArray();
            ms.Close();

            //return buffer;
            return Convert.ToBase64String(buffer); //将压缩后的byte[]转换为Base64String
        }

    }
}
