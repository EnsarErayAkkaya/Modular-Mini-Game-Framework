namespace EEA.Services.Windows
{
    public struct OnWindowClosed
    {
        public string WindowId;
    }

    public struct OnWindowOpened
    {
        public Window Window;
    }
}