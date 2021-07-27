/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using Tripous.Data;

namespace Tripous.Model
{

    /// <summary>
    /// A list of <see cref="JoinFieldDescriptor"/> items.
    /// </summary>
    public class JoinFieldDescriptors : ModelItems<JoinFieldDescriptor>  
    {
        /// <summary>
        /// Normalizes a specified name.
        /// <para>Some inheritors may need to replace or delete invalid characters.</para>
        /// </summary>
        protected override string NormalizeName(string Name)
        {
            JoinTableDescriptor TableDes = Owner as JoinTableDescriptor;
            if (TableDes.ConnectionInfo != null)
            {
                Name = Db.NormalizeFieldName(Name, TableDes.ConnectionInfo);
            }

            return Name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public JoinFieldDescriptors()
        {
            UseSafeAdd = true;
        }

 

        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public JoinFieldDescriptor Add(string Name, SimpleType DataType, int Size, string TitleKey, FieldFlags Flags)
        {
            JoinFieldDescriptor Result = base.Add(Name);
            Result.Alias = string.Empty; // force the field to call the virtual GetAlias()
            Result.TitleKey = TitleKey;
            Result.DataType = DataType;
            Result.Flags = Flags;
            Result.Size = Size;

            return Result;
        }
        /// <summary>
        /// Adds a field to the list
        /// </summary>
        public JoinFieldDescriptor FindByAlias(string Alias)
        {
            Alias = NormalizeName(Alias);  
            return Descriptor.FindByAlias(Alias, this) as JoinFieldDescriptor;
        }
    }
}
