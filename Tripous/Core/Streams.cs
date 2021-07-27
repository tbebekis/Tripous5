/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
 

namespace Tripous
{

    /// <summary>
    /// Stream related utilities and extensions
    /// </summary>
    static public class Streams
    {
        /* copy-move content from one stream to another. NOTE: CopyTo() is added to Stream class in .Net 4 */
        /// <summary>
        /// Copies all bytes from the Source to the current Dest position.
        /// Does not reset the position of the Source or Dest after the copy operation is complete.
        /// </summary>
        public static void CopyAllTo(this Stream Source, Stream Dest, int BufferSize = 1024 * 1024)
        { 
            Source.Position = 0;
            Source.CopyTo(Dest, BufferSize); 
        }
        /// <summary>
        /// Clears Dest content and copies all Source content to it.
        /// </summary>
        public static void Assign(this Stream Source, Stream Dest)
        {
            Dest.SetLength(0);
            Dest.Position = 0;
            CopyAllTo(Source, Dest);
        }

        /// <summary>
        /// Writes the stream contents to a byte array, regardless of the Stream Position
        /// </summary>
        static public byte[] ToArray(Stream Stream)
        {
            if (Stream is MemoryStream)
                return (Stream as MemoryStream).ToArray();

            using (MemoryStream MS = new MemoryStream())
            {
                CopyAllTo(Stream, MS);
                return MS.ToArray();
            }
        }
        /// <summary>
        /// Returns true if the A and B arrays match exactly
        /// </summary>
        static public bool BuffersAreEqual(byte[] A, byte[] B)
        {
            if ((A != null) && (B != null) && (A.Length == B.Length))
            {
                for (int i = 0; i < A.Length; i++)
                    if (A[i] != B[i])
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves a stream into a file using Stream.CopyTo() to copy data from the source stream into a FileStream.
        /// <para>NOTE: The Stream.CopyTo() method uses a temporary buffer of a specified size. </para>
        /// </summary>
        static public void SaveToFile(this Stream Instance, string FilePath, int BufferSize = 1024 * 1024)
        {
            string Folder = Path.GetDirectoryName(FilePath);
            Directory.CreateDirectory(Folder);
            
            using (var FS = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                Instance.CopyTo(FS, BufferSize);
            }
        }
        /// <summary>
        /// Reads the contents of the file into a byte array.
        /// <para>NOTE: The Stream.CopyTo() method uses a temporary buffer of a specified size. </para>
        /// </summary>
        static public byte[] LoadFromFile(string FilePath, int BufferSize = 1024 * 1024)
        {
            if (!File.Exists(FilePath))
                return new byte[0];

            using (var FS = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                byte[] Buffer = new byte[FS.Length];

                // Initializes a new non-resizable instance of the MemoryStream,
                // that is the MemoryStream is initialized with an array which doesn't need to be resized.
                // NOTE: The  MemoryStream.ToArray() which creates a copy of the internal buffer won't be called omitting the copy
                using (MemoryStream MS = new MemoryStream(Buffer))
                {
                    FS.CopyTo(MS, BufferSize);
                    return Buffer;
                }
            }
        }

        /* text streams */
        /// <summary>
        /// Returns the Encoding if a Preamble exists in a text buffer, if any, else null.
        /// </summary>
        static public Encoding GetEncoding(byte[] Buffer)
        {
            var encodings = Encoding.GetEncodings()
                            .Select(e => e.GetEncoding())
                            .Select(e => new { Encoding = e, Preamble = e.GetPreamble() })
                            .Where(e => e.Preamble.Any())
                            .ToArray();

            var maxPrembleLength = encodings.Max(e => e.Preamble.Length);

            return encodings
                .Where(enc => enc.Preamble.SequenceEqual(Buffer.Take(enc.Preamble.Length)))
                .Select(enc => enc.Encoding)
                .FirstOrDefault();
        }
        /// <summary>
        /// Returns the Encoding if a Preamble exists in a text buffer, if any, else Encoding.Default.
        /// </summary>
        static public Encoding FindEncoding(byte[] Buffer)
        {
            return GetEncoding(Buffer) ?? Encoding.Default;
        }

        /// <summary>
        /// Adds the preable of the Encoding in front of the Buffer
        /// </summary>
        static public byte[] AddPreambleTo(byte[] Buffer, Encoding Encoding)
        {
            byte[] Preamble = Encoding.GetPreamble();

            // CAUTION: Not all encodings have a preamble
            if (Preamble != null && Preamble.Length > 0)
            {
                byte[] Result = new byte[Preamble.Length + Buffer.Length];
                Array.Copy(Preamble, 0, Result, 0, Preamble.Length);
                Array.Copy(Buffer, 0, Result, Preamble.Length, Buffer.Length);

                return Result;
            }

            return Buffer;

        }
        /// <summary>
        /// Removes any preamble in front of the Buffer
        /// </summary>
        static public byte[] RemovePreambleFrom(byte[] Buffer)
        {
            Encoding Encoding = GetEncoding(Buffer);
            if (Encoding == null)
                return Buffer;

            byte[] Result = new byte[Buffer.Length - Encoding.GetPreamble().Length];

            Array.Copy(Buffer, Encoding.GetPreamble().Length, Result, 0, Result.Length);

            return Result;
        }

        /// <summary>
        /// Encodes Text into a byte array. Text must be in SourceEncoding. 
        /// <para>If SourceEncoding is null then Encoding.Unicode is assumed.</para>
        /// <para>If DestEncoding is not null then the result byte array is converted to that Encoding. </para>
        /// <para>If PutPreamble is true then a preable is put in front of the result array</para>
        /// </summary>
        static public byte[] BytesOf(string Text, Encoding SourceEncoding = null, Encoding DestEncoding = null, bool PutPreamble = false)
        {
            if (SourceEncoding == null)
                SourceEncoding = Encoding.Unicode;

            byte[] Buffer = SourceEncoding.GetBytes(Text);

            if (DestEncoding != null && SourceEncoding.CodePage != DestEncoding.CodePage)
                Buffer = Encoding.Convert(SourceEncoding, DestEncoding, Buffer);

            if (PutPreamble)
                Buffer = AddPreambleTo(Buffer, DestEncoding == null ? SourceEncoding : DestEncoding);

            return Buffer;
        }
        /// <summary>
        /// Decodes Buffer into a string. Buffer must be in SourceEncoding.
        /// <para>If SourceEncoding is null then Encoding.Unicode is assumed.</para>
        /// <para>If DestEncoding is not null then Buffer is first converted to that Encoding</para>
        /// </summary>
        static public string StringOf(byte[] Buffer, Encoding SourceEncoding = null, Encoding DestEncoding = null)
        {
            if (Buffer == null || Buffer.Length == 0)
                return string.Empty;

            if (SourceEncoding == null)
                SourceEncoding = GetEncoding(Buffer);

            // CAUTION: Not all encodings have a preamble
            if (SourceEncoding == null)
                SourceEncoding = Encoding.Default;

            Buffer = RemovePreambleFrom(Buffer);

            if (DestEncoding != null)
            {
                Buffer = Encoding.Convert(SourceEncoding, DestEncoding, Buffer);
                return DestEncoding.GetString(Buffer, 0, Buffer.Length);
            }
            else
            {
                return SourceEncoding.GetString(Buffer, 0, Buffer.Length);
            }
        }

        /// <summary>
        /// Gets the content of the Stream as a string.
        /// </summary>
        static public string GetAsString(this Stream Stream)
        {
            if ((Stream != null) && (Stream.Length > 0))
            {
                byte[] Bytes = ToArray(Stream);
                return StringOf(Bytes);
            }

            return string.Empty;

        }
        /// <summary>
        /// Writes the specified string Value as the content of the Stream.
        /// </summary>
        static public void SetAsString(this Stream Stream, string Value)
        {
            if (Stream != null)
            {
                Stream.SetLength(0);

                if (!string.IsNullOrWhiteSpace(Value))
                {
                    byte[] Bytes = BytesOf(Value);
                    Stream.Write(Bytes, 0, Bytes.Length);
                }
            }
        }


    }
}
