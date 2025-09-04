namespace Tripous.Forms
{
    static public class BindingSourceExtensions
    {
        /// <summary>
        /// Returns the current <see cref="DataRow"/> of a BindingSource
        /// </summary>
        static public DataRow CurrentDataRow(this BindingSource BS)
        {
            if (BS.Position >= 0)
            {
                DataRowView DRV = BS.Current as DataRowView;
                if (DRV != null)
                    return DRV.Row;
            }

            return null;
        }
        /// <summary>
        /// Move row up by one
        /// </summary>
        /// <param name="sender"></param>
        public static void MoveRowUp(this BindingSource sender)
        {
            if (!string.IsNullOrWhiteSpace(sender.Sort))
            {
                sender.Sort = "";
            }

            var newIndex = Convert.ToInt32((sender.Position == 0) ? 0 : sender.Position - 1);

            var dt = (DataTable)sender.DataSource;
            DataRow rowToMove = ((DataRowView)sender.Current).Row;
            var newRow = dt.NewRow();

            newRow.ItemArray = rowToMove.ItemArray;
            dt.Rows.RemoveAt(sender.Position);
            dt.Rows.InsertAt(newRow, newIndex);

            dt.AcceptChanges();

            sender.Position = newIndex;

        }
        /// <summary>
        /// Move row down by one
        /// </summary>
        /// <param name="sender"></param>
        public static void MoveRowDown(this BindingSource sender)
        {
            if (!string.IsNullOrWhiteSpace(sender.Sort))
            {
                sender.Sort = "";
            }

            var upperLimit = sender.Count - 1;
            var newIndex = Convert.ToInt32((sender.Position + 1 >= upperLimit) ? upperLimit : sender.Position + 1);

            var dt = (DataTable)sender.DataSource;
            DataRow rowToMove = ((DataRowView)sender.Current).Row;
            var newRow = dt.NewRow();

            newRow.ItemArray = rowToMove.ItemArray;
            dt.Rows.RemoveAt(sender.Position);
            dt.Rows.InsertAt(newRow, newIndex);

            dt.AcceptChanges();

            sender.Position = newIndex;

        }

    }
}
