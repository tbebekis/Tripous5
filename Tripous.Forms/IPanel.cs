namespace Tripous.Forms
{
    public interface IPanel
    {
        void Close();

        string Id { get; set; }
        object Info { get; set; }
        bool CloseableByUser { get; set; }
    }
}
