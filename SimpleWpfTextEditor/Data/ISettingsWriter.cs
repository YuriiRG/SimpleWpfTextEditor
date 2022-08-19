namespace SimpleWpfTextEditor.Data
{
    public interface ISettingsWriter
    {
        void Save(AppSettings data);
        AppSettings Read();
        void Reset();
    }
}