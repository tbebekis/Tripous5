/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Model
{
    /// <summary>
    /// A list of <see cref="JoinTableDescriptor"/> items.
    /// </summary>
    public class JoinTableDescriptors : ModelItems<JoinTableDescriptor>  
    {
        /// <summary>
        /// Extends the base.Find().
        /// <para>Searches the collection for an item with NameOrAlias.</para>
        /// Returns an item if found, null else.
        /// </summary>
        public override JoinTableDescriptor Find(string NameOrAlias)
        {
            JoinTableDescriptor Result = base.Find(NameOrAlias);

            if (Result == null)
                Result = Descriptor.FindByAlias(NameOrAlias, this) as JoinTableDescriptor;

            return Result;
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public JoinTableDescriptors()
        {
            UseSafeAdd = false;
            UniqueNames = false;
        }

        /// <summary>
        /// Displays an edit dialog for this instance. 
        /// <para>Returns true if the user presses the OK button in the dialog</para>
        /// </summary>
        public bool ShowEditDialog(TableDescriptorBase MasterTable)
        {
            JoinTableDescriptors Instance = this.Clone() as JoinTableDescriptors;

            if ((bool)ObjectStore.CallDef("JoinTableDescriptors.Edit.Dialog", false, Instance, MasterTable))
            {
                this.Assign(Instance);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Adds a join table to the list.
        /// </summary>
        public JoinTableDescriptor Add(string Name, string Alias, string MasterKeyField, string ZoomCommand)
        {
            JoinTableDescriptor Result = base.Add(Name);
            Result.Alias = Alias;
            Result.MasterKeyField = MasterKeyField;
            Result.ZoomCommand = ZoomCommand;
            return Result;
        }
        /// <summary>
        /// Adds a join table to the list.
        /// </summary>
        public JoinTableDescriptor Add(string Name, string Alias, string MasterKeyField)
        {
            return Add(Name, Alias, MasterKeyField, string.Empty);
        }
        /// <summary>
        /// Adds a join table to the list.
        /// </summary>
        public JoinTableDescriptor Add(string Name, string MasterKeyField)
        {
            return Add(Name, Name, MasterKeyField);
        }


        /// <summary>
        /// Searces the whole joined tree for a table by a Name or Alias and returns
        /// a JoinTableDescriptor, if any, else null.
        /// </summary>
        public JoinTableDescriptor FindAny(string NameOrAlias)
        {
            JoinTableDescriptor Result = base.Find(NameOrAlias);
            if (Result == null)
            {
                foreach (JoinTableDescriptor JoinTable in this)
                {
                    Result = JoinTable.JoinTables.FindAny(NameOrAlias);
                    if (Result != null)
                        return Result;
                }
            }

            return Result;
        }
        /// <summary>
        /// Finds a join table descriptor by MasterKeyField, if any, else null.
        /// </summary>
        public JoinTableDescriptor FindByMasterKeyField(string MasterKeyField)
        {
            foreach (JoinTableDescriptor JoinTableDes in this)
                if (Sys.IsSameText(MasterKeyField, JoinTableDes.MasterKeyField))
                    return JoinTableDes;
            return null;
        }
        /// <summary>
        /// Searces the whole joined tree for a join table descriptor by MasterKeyField
        /// and returns that table, if any, else null.
        /// </summary>
        public JoinTableDescriptor FindAnyByMasterKeyField(string MasterKeyField)
        {
            JoinTableDescriptor Result = FindByMasterKeyField(MasterKeyField);
            if (Result == null)
            {
                foreach (JoinTableDescriptor JoinTable in this)
                {
                    Result = JoinTable.JoinTables.FindAnyByMasterKeyField(MasterKeyField);
                    if (Result != null)
                        return Result;
                }
            }

            return Result;
        }
    }
}
