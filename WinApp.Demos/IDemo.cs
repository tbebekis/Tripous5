namespace WinApp.Demos
{
    public interface IDemo
    {
        void ShowUi(TabPage Page);
        bool Singleton { get; }
        string Title { get; }
        string Description { get; }
    }
}
