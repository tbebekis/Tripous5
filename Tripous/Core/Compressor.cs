/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Text;
using System.IO;

namespace Tripous
{

    /// <summary>
    /// Compresses and decompresses data using the GZipStream.
    /// <para>NOTE: Using GZipStream to compress a file and save it as *.gz file
    /// makes the result file usable by WinZip, 7-zip, and the gzip tool.
    /// Also the GZip format embedds CRC information in order to ensure that the data is correct. </para>
    /// </summary>
    static public class Compressor
    {
        /// <summary>
        /// Compresses Buffer
        /// </summary>
        public static byte[] Compress(byte[] Buffer)
        {
            if ((Buffer != null) && (Buffer.Length > 0))
            {
                using (MemoryStream Result = new MemoryStream())
                {
                    using (GZipStream ZS = new GZipStream(Result, CompressionMode.Compress, true))
                    {
                        ZS.Write(Buffer, 0, Buffer.Length);
                    }
                    
                    return Result.ToArray();
                }

            }

            return new byte[0];
        }
        /// <summary>
        /// Decompresses Buffer
        /// </summary>
        static public byte[] Decompress(byte[] Buffer)
        {
            if ((Buffer != null) && (Buffer.Length > 0))
            {
                using (MemoryStream Result = new MemoryStream())
                {
                    using (MemoryStream Source = new MemoryStream(Buffer, false))
                    {
                        Source.Position = 0;
                        byte[] WorkBuffer = new byte[1024 * 4];
                        int BytesRead;

                        using (GZipStream ZS = new GZipStream(Source, CompressionMode.Decompress))
                        {
                            while (true)
                            {
                                BytesRead = ZS.Read(WorkBuffer, 0, WorkBuffer.Length);
                                if (BytesRead <= 0)
                                    break;
                                Result.Write(WorkBuffer, 0, BytesRead);
                            }
                        }
                    }

                    return Result.ToArray();
                }
            }

            return new byte[0];
        }

        /// <summary>
        /// Compresses Stream in-place
        /// </summary>
        static public void Compress(MemoryStream Stream)
        {
            if (Stream != null)
            {
                Stream.Position = 0;
                byte[] Buffer = Stream.ToArray();
                Stream.SetLength(0);

                Buffer = Compress(Buffer);
                if (Buffer.Length > 0)
                {
                    Stream.Write(Buffer, 0, Buffer.Length);
                }

                Stream.Position = 0;
            }
        }
        /// <summary>
        /// Decompresses Stream in-place
        /// </summary>
        static public void Decompress(MemoryStream Stream)
        {
            if (Stream != null)
            {
                Stream.Position = 0;
                byte[] Buffer = Stream.ToArray();
                Stream.SetLength(0);

                Buffer = Decompress(Buffer);
                if (Buffer.Length > 0)
                {
                    Stream.Write(Buffer, 0, Buffer.Length);
                }

                Stream.Position = 0;
            }
        }

        /// <summary>
        /// Compresses Text. Uses Encoding to get the bytes of Text.
        /// </summary>
        static public byte[] CompressText(string Text, Encoding Encoding)
        {
            return Compress(Encoding.GetBytes(Text));
        }
        /// <summary>
        /// Decompresses Buffer into a string. Uses Encoding to get the characters of the decompressed Buffer.
        /// </summary>
        static public string DecompressText(byte[] Buffer, Encoding Encoding)
        {
            return new string(Encoding.GetChars(Decompress(Buffer)));
        }
    }
}
