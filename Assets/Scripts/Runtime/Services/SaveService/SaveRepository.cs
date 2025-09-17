using System.Threading.Tasks;

namespace EEA.Services.SaveServices
{
    public class SaveRepository<T> where T : ISaveable, new()
    {
        private readonly ISaveHandler saveHandler;
        private readonly string saveKey;
        private T cachedEntity;

        public SaveRepository(ISaveHandler saveHandler, string saveKey)
        {
            this.saveHandler = saveHandler;
            this.saveKey = saveKey;
            cachedEntity = new T();
        }

        public T Get() => cachedEntity;

        public async Task<T> LoadAsync()
        {
            string data = await saveHandler.LoadDataAsync(saveKey);
            if (string.IsNullOrEmpty(data))
                return cachedEntity;

            cachedEntity = new T().Deserialize<T>(data);
            return cachedEntity;
        }

        public T Load()
        {
            string data = saveHandler.LoadData(saveKey);
            if (string.IsNullOrEmpty(data))
                return cachedEntity;

            cachedEntity = new T().Deserialize<T>(data);
            return cachedEntity;
        }

        public async Task SaveAsync(T entity)
        {
            cachedEntity = entity;
            await saveHandler.SaveDataAsync(saveKey, entity.Serialize());
        }

        public void Save(T entity)
        {
            cachedEntity = entity;
            saveHandler.SaveData(saveKey, entity.Serialize());
        }
    }
}