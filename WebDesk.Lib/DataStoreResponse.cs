namespace WebLib
{

    /// <summary>
    /// Data store response
    /// </summary>
    public class DataStoreResponse
    {
        public void AddError(string ErrorText)
        {
            if (Errors == null)
                Errors = new List<string>();

            Errors.Add(ErrorText);
        }
        public void ClearErrors()
        {
            if (Errors != null)
                Errors.Clear();
        }

        [JsonIgnore]
        public bool Succeeded => Errors == null || Errors.Count == 0;
        [JsonIgnore]
        public string Error => Errors != null && Errors.Count > 0 ? Errors[0] : string.Empty;
        public List<string> Errors { get; set; }
    }


    /// <summary>
    /// Data service response for a single item
    /// </summary>
    public class ItemResponse<T> : DataStoreResponse
    {
        /// <summary>
        /// The item
        /// </summary>
        public T Item { get; set; }
    }


    /// <summary>
    /// Data service response for lists.
    /// </summary>
    public class ListResponse<T> : DataStoreResponse
    {
        /// <summary>
        /// The number of total items when this is a paged response. Used only with paged responses.
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// The list of items
        /// </summary>
        public List<T> List { get; set; } = new List<T>();
    }

}