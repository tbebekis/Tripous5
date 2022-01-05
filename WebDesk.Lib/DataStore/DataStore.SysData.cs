using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Tripous;
using Tripous.Data;

namespace WebLib
{
    static public partial class DataStore
    {
        static void SysDataSaveItem_Table(SysDataItem Item)
        {
            string JsonText = Item.Data1;
            DataTableDef NewTableDef = Json.Deserialize<DataTableDef>(JsonText);
            DataTableDef OldTableDef = null;

            object Id = SysData.SelectId(Item.DataType, Item.DataName);
            if (!Sys.IsNull(Id))
            {
                SysDataItem OldItem = SysData.SelectItemById(Id);
                JsonText = OldItem.Data1;
                OldTableDef = Json.Deserialize<DataTableDef>(JsonText);
            }
        }

        /// <summary>
        /// Selects a specified DataType from SYS_DATA and returns the DataTable.
        /// </summary>
        static public DataTable SysDataSelectList(string DataType, bool NoBlobs)
        {
            DataTable Table = SysData.Select(DataType, NoBlobs);
            return Table;
        }
        /// <summary>
        /// Selects a <see cref="SysDataItem"/> by a specified Id and returns the item.
        /// </summary>
        static public SysDataItem SysDataSelectItemById(string Id)
        {
            SysDataItem Item = SysData.SelectItemById(Id);
            return Item;
        }
        /// <summary>
        /// Saves a <see cref="SysDataItem"/> to the SYS_DATA table.
        /// </summary>
        static public void SysDataSaveItem(SysDataItem Item)
        { 
            if (Item.DataType == "Table")
            {
                SysDataSaveItem_Table(Item);
            }
        }
    }
}
