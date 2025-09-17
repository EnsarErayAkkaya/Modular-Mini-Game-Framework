using System;
using System.Globalization;
using System.Threading.Tasks;

namespace EEA.Services.SaveServices
{
    public class PrimitiveSaveHelper
    {
        private readonly ISaveHandler saveHandler;

        public PrimitiveSaveHelper(ISaveHandler saveHandler)
        {
            this.saveHandler = saveHandler;
        }

        public async Task<int> LoadIntAsync(string key)
        {
            string data = await saveHandler.LoadDataAsync(key);
            return int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) ? value : 0;
        }

        public async Task<float> LoadFloatAsync(string key)
        {
            string data = await saveHandler.LoadDataAsync(key);
            return float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out float value) ? value : 0f;
        }

        public async Task<bool> LoadBoolAsync(string key)
        {
            string data = await saveHandler.LoadDataAsync(key);
            return bool.TryParse(data, out bool value) && value;
        }

        public void Save(string key, string data) => saveHandler.SaveData(key, data);

        public async Task SaveAsync(string key, string data) => await saveHandler.SaveDataAsync(key, data);
    }
}
