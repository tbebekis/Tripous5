/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using Tripous.Data;


namespace Tripous.Model
{

    /// <summary>
    /// A list of <see cref="FieldDescriptor"/> items.
    /// </summary>
    public class FieldDescriptors : ModelItems<FieldDescriptor> // NamedItems
    {

        /// <summary>
        /// Normalizes a specified name.
        /// <para>Some inheritors may need to replace or delete invalid characters.</para>
        /// </summary>
        protected override string NormalizeName(string Name)
        {
            TableDescriptor TableDes = Owner as TableDescriptor;
            if (TableDes.ConnectionInfo != null)
            {
                Name = Db.NormalizeFieldName(Name, TableDes.ConnectionInfo);
            }

            return Name;
        }
 

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldDescriptors()
        {
            UseSafeAdd = true;
        }

        /* methods */
        /// <summary>
        /// Displays an edit dialog for this instance. 
        /// <para>Returns true if the user presses the OK button in the dialog</para>
        /// </summary>
        public bool ShowEditDialog()
        {
            FieldDescriptors Instance = this.Clone() as FieldDescriptors;

            if ((bool)ObjectStore.CallDef("FieldDescriptors.Edit.Dialog", false, Instance))
            {
                this.Assign(Instance);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor Add(string Name, SimpleType DataType, int Size, string TitleKey, FieldFlags Flags, string DefaultValue)
        {
            FieldDescriptor Result = base.Add(Name);

            Result.TitleKey = TitleKey;
            Result.DataType = DataType;
            Result.Flags = Flags;
            Result.Size = Size;
            Result.DefaultValue = DefaultValue;
            return Result;
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor Add(string Name, SimpleType DataType, int Size, string TitleKey, FieldFlags Flags)
        {
            return Add(Name, DataType, Size, TitleKey, Flags, Sys.NULL);
        }
        /// <summary>
        /// Adds a field to the list
        /// <para>NOTE: The LookUpTableName could be the name of a table in the database 
        /// or the name of a QueryDescriptor in the same broker.  </para>
        /// </summary>
        public FieldDescriptor AddLookUp(string Name, SimpleType DataType, int Size, string TitleKey, FieldFlags Flags,
                                         string LookUpTableName, string LookUpResultField, string LookUpDisplayFields)
        {
            Flags |= FieldFlags.LookUpField;

            FieldDescriptor Result = Add(Name, DataType, Size, TitleKey, Flags, Sys.NULL);

            Result.LookUpTableName = LookUpTableName;
            Result.LookUpResultField = LookUpResultField;
            Result.LookUpDisplayFields = LookUpDisplayFields;

            return Result;
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor AddInt(string Name, string TitleKey, FieldFlags Flags, string DefaultValue)
        {
            return Add(Name, SimpleType.Integer, 0, TitleKey, Flags, DefaultValue);
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor AddFloat(string Name, string TitleKey, FieldFlags Flags, string DefaultValue)
        {
            return Add(Name, SimpleType.Float, 0, TitleKey, Flags, DefaultValue);
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor AddString(string Name, string TitleKey, FieldFlags Flags, string DefaultValue)
        {
            return Add(Name, SimpleType.String, 0, TitleKey, Flags, DefaultValue);
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor AddDate(string Name, string TitleKey, FieldFlags Flags, string DefaultValue)
        {
            return Add(Name, SimpleType.DateTime, 0, TitleKey, Flags, DefaultValue);
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor AddBool(string Name, string TitleKey, FieldFlags Flags)
        {
            return Add(Name, SimpleType.Boolean, 0, TitleKey, Flags, Sys.NULL);
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public FieldDescriptor AddId(string Name)
        {
            return AddInt(Name, "", FieldFlags.None, Sys.NULL);
        }

        /// <summary>
        /// Finds a field by its Alias.
        /// </summary>
        public FieldDescriptor FindByAlias(string Alias)
        {
            Alias = NormalizeName(Alias); 
            return Descriptor.FindByAlias(Alias, this) as FieldDescriptor;
        }

        /// <summary>
        /// Sets a previously added field under Name, to be a look up field.
        /// <para>LookUpDisplayFields is a semi-colon separated list of field names.</para>
        /// <para>NOTE: The LookUpTableName could be the name of a table in the database 
        /// or the name of a QueryDescriptor in the same broker.  </para>
        /// </summary>
        public void SetAsLookUp(string Name, string LookUpTableName, string LookUpResultField, string LookUpDisplayFields, string LookUpTableAlias)
        {
            FieldDescriptor Des = Find(Name);
            if (Des != null)
            {
                Des.Flags |= FieldFlags.LookUpField;
                Des.LookUpTableName = LookUpTableName;
                Des.LookUpResultField = LookUpResultField;
                Des.LookUpDisplayFields = LookUpDisplayFields;
                Des.LookUpTableAlias = LookUpTableAlias;
            }
        }
        /// <summary>
        /// Sets a previously added field under Name, to be a look up field.
        /// <para>LookUpDisplayFields is a semi-colon separated list of field names.</para>
        /// <para>NOTE: The LookUpTableName could be the name of a table in the database 
        /// or the name of a QueryDescriptor in the same broker.  </para>
        /// </summary>
        public void SetAsLookUp(string Name, string LookUpTableName, string LookUpResultField, string LookUpDisplayFields)
        {
            SetAsLookUp(Name, LookUpTableName, LookUpResultField, LookUpDisplayFields, string.Empty);
        }
    }
}
