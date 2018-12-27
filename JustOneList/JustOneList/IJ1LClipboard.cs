namespace JustOneList.Droid
{
    public interface IJ1LClipboard
    {
        void Copy(string text);
        string Paste();
    }
}