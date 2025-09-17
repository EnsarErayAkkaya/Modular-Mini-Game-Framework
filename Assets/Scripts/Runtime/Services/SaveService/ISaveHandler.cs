using EEA.Services.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EEA.Services.SaveServices
{
    public interface ISaveHandler
    {
        // ASYNC
        Task SaveDataAsync(string saveKey, string saveData);
        Task<string> LoadDataAsync(string fileName);

        // SYNC
        public string LoadData(string saveKey);
        public void SaveData(string saveKey, string saveData);

        public bool CheckKeyExist(string saveKey);
    }
}