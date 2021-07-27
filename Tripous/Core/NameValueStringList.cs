/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Tripous
{

    /// <summary>
    /// Represents a string list where each element has the format Name=Value.
    /// <para>NOTE: Names are case insensitive</para>
    /// <para>NOTE: It could be used as a simple case insensitive string list too.</para>
    /// </summary>
    public class NameValueStringList : IEnumerable, IAssignable
    {


        #region NamesList nested class
        /// <summary>
        /// Represents the list of Names of the container class
        /// </summary>
        public class NamesList
        {
            private NameValueStringList owner;

            /// <summary>
            /// Constructor
            /// </summary>
            public NamesList(NameValueStringList owner)
            {
                this.owner = owner;
            }

            /// <summary>
            /// Indexer
            /// </summary>
            public string this[int Index]
            {
                get
                {
                    string Name;
                    string Value;
                    owner.Split(owner.list[Index], out Name, out Value);
                    return Name;
                }
            }
        }
        #endregion NamesList nested class

        #region ValuesList nested class
        /// <summary>
        /// Represents the list of Values of the container class
        /// </summary>
        public class ValuesList
        {
            private NameValueStringList owner;
            /// <summary>
            /// Constructor
            /// </summary>
            public ValuesList(NameValueStringList owner)
            {
                this.owner = owner;
            }
            /// <summary>
            /// Indexer
            /// </summary>
            public string this[string Name]
            {
                get
                {
                    int Index = owner.IndexOfName(Name);
                    if (Index != -1)
                    {
                        string sName;
                        string sValue;
                        owner.Split(owner.list[Index], out sName, out sValue);
                        return sValue;
                    }

                    return string.Empty;
                }
                set
                {
                    string sName;
                    string sValue;

                    int Index = owner.IndexOfName(Name);
                    if (Index != -1)
                    {
                        owner.Split(owner.list[Index], out sName, out sValue);
                        owner.list[Index] = owner.Concat(Name, value);
                    }
                    else
                    {
                        owner.list.Add(owner.Concat(Name, value));
                    }
                }
            }
        }
        #endregion ValuesList nested class


        /* private */
        private List<string> list = new List<string>();

        /* private */
        private void Split(string Item, out string Name, out string Value)
        {
            Name = string.Empty;
            Value = string.Empty;

            if (!string.IsNullOrWhiteSpace(Item))
            {
                string[] Parts = Item.Split('=');
                if ((Parts != null) && (Parts.Length > 0))
                {
                    Name = Parts[0].Trim();
                    if (Parts.Length > 1)
                        Value = Parts[1].Trim();
                }
            }
        }
        private string Concat(string Name, string Value)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                if (!string.IsNullOrWhiteSpace(Name))
                    return string.Format("{0}={1}", Name, Value);
                else
                    return Value;
            }

            return !string.IsNullOrWhiteSpace(Name) ? Name : string.Empty;
        }
        private void LocalizeValues()
        {

            string Name;
            string Value;
            for (int i = 0; i < list.Count; i++)
            {
                Split(list[i], out Name, out Value);

                if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Value))
                {
                    Value = Res.GS(Value, Value);
                    list[i] = Concat(Name, Value);
                }

            }
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public NameValueStringList()
        {
            this.Names = new NamesList(this);
            this.Values = new ValuesList(this);
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public NameValueStringList(string Text, bool UseValuesAsResourceKeys = false)
            : this()
        {
            this.Text = Text;
            if (UseValuesAsResourceKeys)
                LocalizeValues();
        }
        /// <summary>
        /// Constructor
        /// <para>Each string in Lines may (or may be not) have the format Key=Value.</para>
        /// </summary>
        public NameValueStringList(string[] Lines, bool UseValuesAsResourceKeys = false)
            : this()
        {
            Assign(Lines);
            if (UseValuesAsResourceKeys)
                LocalizeValues();
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public NameValueStringList(IDictionary<string, string> Dic, bool UseValuesAsResourceKeys = false)
            : this()
        {
            Assign(Dic);
            if (UseValuesAsResourceKeys)
                LocalizeValues();
        }
        /// <summary>
        /// Constructor
        /// <para>Table must have two string columns. The first column is considered the Key
        /// where the second the Value.</para>
        /// </summary>
        public NameValueStringList(DataTable Table, bool UseValuesAsResourceKeys = false)
            : this()
        {
            Assign(Table);
            if (UseValuesAsResourceKeys)
                LocalizeValues();
        }


        /* static */
        /// <summary>
        /// Creates a NameValueStringList instance passing Text and returns the list content as a dictionary
        /// </summary>
        static public IDictionary<string, string> ToDictionary(string Text)
        {
            return new NameValueStringList(Text).ToDictionary();
        }


        /* public */
        /// <summary>
        /// Clears the list
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }
        /// <summary>
        /// Adds a string to the list.
        /// <para>The string may or may not have the format Name=Value </para>
        /// </summary>
        public void Add(string Item)
        {
            Insert(list.Count, Item);
        }
        /// <summary>
        /// Inserts a string at Index.
        /// <para>The string may or may not have the format Name=Value </para>
        /// </summary>
        public void Insert(int Index, string Item)
        {
            if (string.IsNullOrWhiteSpace(Item))
                throw new ArgumentException("Item can not be null, empty, or white space");

            string Name;
            string Value;

            Split(Item, out Name, out Value);

            Insert(Index, Name, Value);
        }
        /// <summary>
        /// Adds a string to the list.
        /// <para>The string is given as a Name and Value pair.</para>
        /// </summary>
        public void Add(string Name, string Value)
        {
            Insert(list.Count, Name, Value);
        }
        /// <summary>
        /// Inserts a string at Index.
        /// <para>The string is given as a Name and Value pair.</para>
        /// </summary>
        public void Insert(int Index, string Name, string Value)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name can not be null, empty, or white space");

            if (!string.IsNullOrWhiteSpace(Value) && ContainsName(Name))
                throw new ArgumentException(string.Format("Name already exists in list: {0}", Name));

            list.Insert(Index, Concat(Name, Value));
        }
        /// <summary>
        /// Returns the index of Item in list, if there, else -1.
        /// <para>NOTE: Case insensitive</para>
        /// </summary>
        public int IndexOf(string Item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (string.Compare(list[i], Item, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return i;
            }

            return -1;
        }
        /// <summary>
        /// Returns true if Item exists in list.
        /// <para>NOTE: Case insensitive</para>
        /// </summary>
        public bool Contains(string Item)
        {
            return IndexOf(Item) != -1;
        }
        /// <summary>
        /// Returns the index of Name in list.
        /// <para>NOTE: Case insensitive</para>
        /// </summary>
        public int IndexOfName(string Name)
        {
            string sName;
            string sValue;

            for (int i = 0; i < list.Count; i++)
            {
                Split(list[i], out sName, out sValue);
                if (string.Compare(sName, Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return i;
            }

            return -1;
        }
        /// <summary>
        /// Returns true if Name exists in list.
        /// <para>NOTE: Case insensitive</para>
        /// </summary>
        public bool ContainsName(string Name)
        {
            return IndexOfName(Name) != -1;
        }
        /// <summary>
        /// Removes Item from list.
        /// <para>NOTE: Case insensitive</para>
        /// </summary>
        public void Remove(string Item)
        {
            RemoveAt(IndexOf(Item));
        }
        /// <summary>
        /// Removes string at Index.
        /// </summary>
        public void RemoveAt(int Index)
        {
            if ((Index >= 0) && (Index <= list.Count - 1))
                list.RemoveAt(Index);
        }


        /// <summary>
        /// Moves item at CurIndex to NewIndex
        /// </summary>
        public void Move(int CurIndex, int NewIndex)
        {
            if (CurIndex != NewIndex)
            {
                string TempString = list[CurIndex];
                list.RemoveAt(CurIndex);
                list.Insert(NewIndex, TempString);
            }
        }
        /// <summary>
        /// Exchanges items at Index1 and Index2
        /// </summary>
        public void Exchange(int Index1, int Index2)
        {
            if ((Index1 < 0) || (Index1 >= list.Count))
                throw (new ApplicationException("List index out of bounds"));
            if ((Index2 < 0) || (Index2 >= list.Count))
                throw (new ApplicationException("List index out of bounds"));

            string Temp = list[Index1];
            list[Index1] = list[Index2];
            list[Index2] = Temp;
        }

        /// <summary>
        /// Creates and returns a list with the same content as this instance.
        /// </summary>
        public object Clone()
        {
            NameValueStringList Result = new NameValueStringList();
            Result.list.AddRange(list.ToArray());
            return Result;
        }
        /// <summary>
        /// Returns the list content as an array of strings
        /// </summary>
        public string[] ToArray()
        {
            return list.ToArray();
        }
        /// <summary>
        /// Returns the list content as a generic list of strings
        /// </summary>
        public List<string> ToList()
        {
            return new List<string>(ToArray());
        }
        /// <summary>
        /// Returns the list content as a dictionary
        /// </summary>
        public IDictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();

            string sName;
            string sValue;

            for (int i = 0; i < list.Count; i++)
            {
                Split(list[i], out sName, out sValue);
                Dictionary[sName] = sValue;
            }
            return Dictionary;
        }
        /// <summary>
        /// Returns the list content as a DataTable.
        /// <para>The result Table contains two columns, Name and Value.</para>
        /// </summary>
        public DataTable ToTable()
        {
            DataTable Table = new DataTable("NameValue");
            Table.Columns.Add("Name", typeof(string)).AllowDBNull = false;
            Table.Columns.Add("Value", typeof(string));

            string sName;
            string sValue;

            for (int i = 0; i < list.Count; i++)
            {
                Split(list[i], out sName, out sValue);
                Table.Rows.Add(sName, sValue);
            }

            Table.AcceptChanges();

            return Table;
        }



        /// <summary>
        /// Assigns Source to this instance.
        /// <para>Source could be StringNameValueList, IList&lt;string&gt;, string[], IDictionary&lt;string, string&gt;, DataTable or just a plain string.</para>
        /// </summary>
        public void Assign(object Source)
        {
            if (Source is NameValueStringList)
            {
                Clear();
                list.AddRange((Source as NameValueStringList).list.ToArray());
            }
            else if (Source is IList<string>)
            {
                Clear();
                foreach (string Item in (Source as IList<string>))
                    this.Add(Item);
            }
            else if (Source is string[])
            {
                Clear();
                foreach (string Item in (Source as string[]))
                    this.Add(Item);
            }
            else if (Source is IDictionary<string, string>)
            {
                Clear();
                foreach (var Item in (Source as IDictionary<string, string>))
                    this.Add(Item.Key, Item.Value);
            }
            else if (Source is DataTable)
            {
                Clear();
                foreach (DataRow Row in (Source as DataTable).Rows)
                    this.Add(Row.AsString(0), Row.AsString(1));
            }
            else if (Source is string)
            {
                this.Text = Source as string;
            }
        }
        /// <summary>
        /// Returns an enumerator for the list.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return list[i];
            }
        }
        /// <summary>
        /// Merges a source dictionary to this instance.
        /// </summary>
        public void Merge(IDictionary<string, string> Source)
        {
            foreach (var Entry in Source)
                this.Values[Entry.Key] = Entry.Value;
        }

        /* properties */
        /// <summary>
        /// The number of items in the list
        /// </summary>
        public int Count { get { return list.Count; } }
        /// <summary>
        /// Gets or sets a list item.
        /// <para>This indexer property handles the item as a whole i.e key=value</para>
        /// </summary>
        public string this[int Index]
        {
            get { return list[Index]; }
            set
            {
                string sName;
                string sValue;
                Split(value, out sName, out sValue);

                int index = IndexOfName(sName);
                if (index != Index)
                    throw new ArgumentException(string.Format("Name already exists in list: {0}", sName));

                list[Index] = value;
            }
        }
        /// <summary>
        /// Gets or sets a list item by Name (where Name is the key in a key=value pair)
        /// </summary>
        public string this[string Name]
        {
            get { return Values[Name]; }
            set { Values[Name] = value; }
        }
        /// <summary>
        /// Gets or sets the list content as a whole
        /// </summary>
        public string Text
        {
            get
            {
                StringBuilder SB = new StringBuilder();
                foreach (string Item in list)
                    SB.AppendLine(Item);
                return SB.ToString();
            }
            set
            {
                Clear();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    string[] Lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string Item in Lines)
                    {
                        if (!string.IsNullOrWhiteSpace(Item))
                            Add(Item);
                    }
                }
            }

        }
        /// <summary>
        /// Gets the whole content of the List as a single line of text,
        /// where each line ends with a comma (,)
        /// </summary>
        public string CommaText { get { return list.CommaText(); } }
        /// <summary>
        /// Gives access to names
        /// </summary>
        public NamesList Names { get; private set; }
        /// <summary>
        /// Gives access to values
        /// </summary>
        public ValuesList Values { get; private set; }
    }
}
