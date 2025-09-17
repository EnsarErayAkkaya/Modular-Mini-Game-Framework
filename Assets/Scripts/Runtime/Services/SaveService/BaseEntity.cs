namespace EEA.Services.SaveServices
{
    public abstract class BaseEntity
    {
        public abstract string Name { get; }
        public abstract string Serialize();
        public abstract T Derialize<T>(string data);
    }
}