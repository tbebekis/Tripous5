﻿/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Tripous
{
    /// <summary>
    /// A helper class for returning typed values from a DataRow in a safe manner
    /// </summary>
    static public class DataRowExtensions
    {


        /* get column value by ColumnName */
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public object AsObject(this DataRow row, string ColumnName, object Default)
        {
            if (Default == null)
                throw new ArgumentNullException("Default", "Default parameter can not be null");

            return AsObject(row, row.Table.Columns.IndexOf(ColumnName), Default);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public int AsInteger(this DataRow row, string ColumnName, int Default)
        {
            return AsInteger(row, row.Table.Columns.IndexOf(ColumnName), Default);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public int AsInteger(this DataRow row, string ColumnName)
        {
            return AsInteger(row, ColumnName, 0);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public string AsString(this DataRow row, string ColumnName, string Default)
        {
            return AsString(row, row.Table.Columns.IndexOf(ColumnName), Default);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public string AsString(this DataRow row, string ColumnName)
        {
            return AsString(row, ColumnName, string.Empty);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public double AsFloat(this DataRow row, string ColumnName, double Default)
        {
            return AsFloat(row, row.Table.Columns.IndexOf(ColumnName), Default);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public double AsFloat(this DataRow row, string ColumnName)
        {
            return AsFloat(row, ColumnName, 0);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public decimal AsDecimal(this DataRow row, string ColumnName, decimal Default)
        {
            return AsDecimal(row, row.Table.Columns.IndexOf(ColumnName), Default);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public decimal AsDecimal(this DataRow row, string ColumnName)
        {
            return AsDecimal(row, ColumnName, 0);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public bool AsBoolean(this DataRow row, string ColumnName, bool Default)
        {
            return AsBoolean(row, row.Table.Columns.IndexOf(ColumnName), Default);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public bool AsBoolean(this DataRow row, string ColumnName)
        {
            return AsBoolean(row, ColumnName, false);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public DateTime AsDateTime(this DataRow row, string ColumnName, DateTime Default)
        {
            return AsDateTime(row, row.Table.Columns.IndexOf(ColumnName), Default);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public DateTime AsDateTime(this DataRow row, string ColumnName)
        {
            return AsDateTime(row, ColumnName, DateTime.MinValue);
        }



        /* get column value by ColumnIndex */
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public object AsObject(this DataRow row, int ColumnIndex, object Default)
        {
            if (Default == null)
                throw new ArgumentNullException("Default", "Default parameter can not be null");

            if ((ColumnIndex >= 0) && (ColumnIndex <= row.Table.Columns.Count - 1))
            {
                if (!row.IsNull(ColumnIndex))
                {
                    if (row.Table.Columns[ColumnIndex].DataType.Equals(Default.GetType()))
                        return row[ColumnIndex];
                }
            }



            return Default;
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public int AsInteger(this DataRow row, int ColumnIndex, int Default)
        {
            if ((ColumnIndex >= 0) && (ColumnIndex <= row.Table.Columns.Count - 1))
            {
                if (!row.IsNull(ColumnIndex))
                {
                    if (row.Table.Columns[ColumnIndex].DataType.Equals(typeof(int)))
                        return (int)row[ColumnIndex];
                    else
                    {
                        try
                        {
                            return Convert.ToInt32(row[ColumnIndex]);
                        }
                        catch
                        {
                        }
                    }
                }

            }



            return Default;
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public int AsInteger(this DataRow row, int ColumnIndex)
        {
            return AsInteger(row, ColumnIndex, 0);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public string AsString(this DataRow row, int ColumnIndex, string Default)
        {
            if ((ColumnIndex >= 0) && (ColumnIndex <= row.Table.Columns.Count - 1))
            {
                if (!row.IsNull(ColumnIndex))
                {
                    if (row.Table.Columns[ColumnIndex].DataType.Equals(typeof(string)))
                        return (string)row[ColumnIndex];
                    else
                        return row[ColumnIndex].ToString();
                }
            }

            return Default;
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public string AsString(this DataRow row, int ColumnIndex)
        {
            return AsString(row, ColumnIndex, string.Empty);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public double AsFloat(this DataRow row, int ColumnIndex, double Default)
        {
            if ((ColumnIndex >= 0) && (ColumnIndex <= row.Table.Columns.Count - 1))
            {
                if (!row.IsNull(ColumnIndex))
                {
                    if (row.Table.Columns[ColumnIndex].DataType.Equals(typeof(double)))
                        return (double)row[ColumnIndex];
                    else
                    {
                        try
                        {
                            return Sys.AsDouble(row[ColumnIndex]);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return Default;
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public double AsFloat(this DataRow row, int ColumnIndex)
        {
            return AsFloat(row, ColumnIndex, 0);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnName of row, if any, else Default.
        /// </summary>
        static public decimal AsDecimal(this DataRow row, int ColumnIndex, decimal Default)
        {
            if ((ColumnIndex >= 0) && (ColumnIndex <= row.Table.Columns.Count - 1))
            {
                if (!row.IsNull(ColumnIndex))
                {
                    if (row.Table.Columns[ColumnIndex].DataType.Equals(typeof(decimal)))
                        return (decimal)row[ColumnIndex];
                    else
                    {
                        try
                        {
                            return Sys.AsDecimal(row[ColumnIndex]);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return Default;
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public decimal AsDecimal(this DataRow row, int ColumnIndex)
        {
            return AsDecimal(row, ColumnIndex, 0);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public bool AsBoolean(this DataRow row, int ColumnIndex, bool Default)
        {
            if ((ColumnIndex >= 0) && (ColumnIndex <= row.Table.Columns.Count - 1))
            {
                if (!row.IsNull(ColumnIndex))
                {
                    if (row.Table.Columns[ColumnIndex].DataType.Equals(typeof(bool)))
                        return (bool)row[ColumnIndex];
                    else
                    {
                        try
                        {
                            return Convert.ToBoolean(row[ColumnIndex]);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return Default;
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public bool AsBoolean(this DataRow row, int ColumnIndex)
        {
            return AsBoolean(row, ColumnIndex, false);
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public DateTime AsDateTime(this DataRow row, int ColumnIndex, DateTime Default)
        {
            if ((ColumnIndex >= 0) && (ColumnIndex <= row.Table.Columns.Count - 1))
            {
                if (!row.IsNull(ColumnIndex))
                {
                    if (row.Table.Columns[ColumnIndex].DataType.Equals(typeof(DateTime)))
                        return (DateTime)row[ColumnIndex];
                    else
                    {
                        try
                        {
                            return Convert.ToDateTime(row[ColumnIndex]);
                        }
                        catch
                        {
                        }
                    }
                }
            }


            return Default;
        }
        /// <summary>
        /// Retusn the value of the field by ColumnIndex of row, if any, else Default.
        /// </summary>
        static public DateTime AsDateTime(this DataRow row, int ColumnIndex)
        {
            return AsDateTime(row, ColumnIndex, DateTime.MinValue);
        }


        /* copy-append */
        /// <summary>
        /// Copies Source to Dest.  
        /// <para>WARNING: Assumes that Source and Dest are identical in schema.</para>
        /// <para>WARNING: Dest columns that do not exist in Source remain untouched.</para>
        /// </summary>
        static public void CopyTo(this DataRow Source, DataRow Dest)
        {
            for (int i = 0; i < Dest.Table.Columns.Count; i++)
                Dest[i] = Source[i];
        }
        /// <summary>
        /// Copies Source to Dest.  
        /// <para>WARNING: Only data from columns with identical names to both DataRows are copied.</para>
        /// <para>WARNING: Dest columns that do not exist in Source remain untouched.</para>
        /// </summary>
        static public void SafeCopyTo(this DataRow Source, DataRow Dest)
        {
            for (int i = 0; i < Dest.Table.Columns.Count; i++)
                if (Source.Table.Columns.Contains(Dest.Table.Columns[i].ColumnName))
                    Dest[Dest.Table.Columns[i].ColumnName] = Source[Dest.Table.Columns[i].ColumnName];
        }
        /// <summary>
        /// Copies, using a safe copy, and appends Source DataRow to DestTable
        /// </summary>
        static public DataRow AppendTo(this DataRow Source, DataTable DestTable)
        {
            DataRow Result = DestTable.NewRow();
            SafeCopyTo(Source, Result);
            DestTable.Rows.Add(Result);
            return Result;
        }


        /* blobs */
        /// <summary>
        /// Loads FielName field of Row, if field is of type byte[] or object using the content of Stream
        /// </summary>
        static public void StreamToBlob(this DataRow Row, string FieldName, Stream Stream)
        {
            if ((Row != null) && Row.Table.Columns.Contains(FieldName))
            {
                Type DataType = Row.Table.Columns[FieldName].DataType;
                if ((DataType == typeof(byte[]) || (DataType == typeof(object))))
                {
                    Row[FieldName] = DBNull.Value;

                    if ((Stream != null) && (Stream.Length > 0))
                    {
                        byte[] Bytes = new byte[Stream.Length];
                        Stream.Position = 0;
                        Stream.Read(Bytes, 0, Convert.ToInt32(Stream.Length));
                        Row[FieldName] = Bytes;
                    }
                }
            }
        }
        /// <summary>
        /// Saves FieldName field of Row, if field is of type byte[] or object, to Stream
        /// </summary>
        static public void BlobToStream(this DataRow Row, string FieldName, Stream Stream)
        {
            if (Stream != null)
            {
                Stream.SetLength(0);

                if ((Row != null) && Row.Table.Columns.Contains(FieldName))
                {
                    Type DataType = Row.Table.Columns[FieldName].DataType;
                    if ((DataType == typeof(byte[]) || (DataType == typeof(object))))
                    {
                        if (!Sys.IsNull(Row[FieldName]))
                        {
                            byte[] Bytes = (byte[])Row[FieldName];
                            Stream.Write(Bytes, 0, Bytes.Length);
                            Stream.Position = 0;
                        }
                    }
                }

            }




        }
        /// <summary>
        /// If the specified FieldName field is of type byte[] or object, then it returns
        /// the field contents as a MemoryStream.
        /// </summary>
        static public MemoryStream BlobToStream(this DataRow Row, string FieldName)
        {
            MemoryStream Result = new MemoryStream();
            BlobToStream(Row, FieldName, Result);
            return Result;
        }
        /// <summary>
        /// Loads FieldName field of Row, if field is of type byte[] or object using the content of Stream
        /// </summary>
        static public void LoadFromStream(this DataRow Row, string BlobFieldName, Stream Stream)
        {
            StreamToBlob(Row, BlobFieldName, Stream);
        }
        /// <summary>
        /// Saves FieldName field of Row, if field is of type byte[] or object, to Stream
        /// </summary>
        static public void SaveToStream(this DataRow Row, string BlobFieldName, Stream Stream)
        {
            using (MemoryStream Source = BlobToStream(Row, BlobFieldName))
            {
                Source.WriteTo(Stream);
            }
        }
        /// <summary>
        /// Writes Value to the BlobFieldName blob field of the specified Row
        /// </summary>
        static public void StringToBlob(this DataRow Row, string BlobFieldName, string Value)
        {
            if (Value == null)
                Value = string.Empty;

            Type DataType = Row.Table.Columns[BlobFieldName].DataType;
            if (DataType == typeof(string))
            {
                Row[BlobFieldName] = Value;
            }
            else // if ((DataType == typeof(byte[]) || (DataType == typeof(object))))
            {
                using (MemoryStream MS = new MemoryStream())
                {
                    byte[] Bytes = Streams.BytesOf(Value);
                    MS.Write(Bytes, 0, Bytes.Length);
                    MS.Position = 0;
                    StreamToBlob(Row, BlobFieldName, MS);
                }
            }
        }
        /// <summary>
        /// Reads and returns a string value stored in the BlobFieldName blob field of the specified Row
        /// </summary>
        static public string BlobToString(this DataRow Row, string BlobFieldName)
        {
            Type DataType = Row.Table.Columns[BlobFieldName].DataType;
            if (DataType == typeof(string))
            {
                return Sys.IsNull(Row[BlobFieldName]) ? string.Empty : Row[BlobFieldName].ToString();
            }
            else // if ((DataType == typeof(byte[]) || (DataType == typeof(object))))
            {
                using (MemoryStream MS = BlobToStream(Row, BlobFieldName))
                {
                    MS.Position = 0;
                    return Streams.StringOf(MS.ToArray());
                }
            }
        }

        /// <summary>
        /// Saves an image into a blob field
        /// </summary>
        static public void ImageToBlob(this DataRow Row, string FieldName, Image Image)
        {
            if (Image != null)
            {
                using (MemoryStream MS = new MemoryStream())
                {
                    Image.Save(MS, Image.RawFormat);
                    StreamToBlob(Row, FieldName, MS);
                }
            }
        }
        /// <summary>
        /// Reads a blob field and returns an image. 
        /// <para>WARNING: Returns null if field is null</para>
        /// </summary>
        static public Image BlobToImage(this DataRow Row, string FieldName)
        {
            MemoryStream MS = BlobToStream(Row, FieldName);
            if (MS.Length > 0)
            {
                MS.Position = 0;
                Image Result = Image.FromStream(MS);
                return Result;
            }
            return null;
        }

        /* miscs */
        /// <summary>
        /// Returns the value of a column specified by name and true on success.
        /// </summary>
        static public bool TryGetValue(this DataRow Row, string FieldName, out object Value)
        {
            Value = null;

            DataColumn Column = Row.Table.FindColumn(FieldName);  
            if (Column != null)
            {
                Value = Row[Column];
                return true;
            }

            return false;
        }
    }
}
