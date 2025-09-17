using System.Collections.Generic;
using System.Threading.Tasks;

namespace EEA.Services.SaveServices
{
    public interface ISaveService
    {
        /// <summary>
        /// Sets up a save type and loads it. 
        /// </summary>
        /// <typeparam name="T" must be a BaseEntity></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> CreateTypeDataAsync<T>(string key) where T : BaseEntity;
        T CreateTypeData<T>(string key) where T : BaseEntity;

        /// <summary>
        /// Save/update Saved Data Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveData"></param>
        /// <returns></returns>
        Task SetTypeDataAsync<T>(T saveData) where T : BaseEntity;
        void SetTypeData<T>(T saveData) where T : BaseEntity;

        /// <summary>
        /// Gets current loaded data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="saveData"></param>
        /// <returns></returns>
        T GetTypeData<T>() where T : BaseEntity;

        /// <summary>
        /// Load Saved Data Type. It calls automaticlly when Data Type created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> LoadTypeDataAsync<T>() where T : BaseEntity;
        T LoadTypeData<T>() where T : BaseEntity;

        // BASIC FUNCTIONS //

        // ASYNC FUNCTIONS
        public Task<string> LoadDataAsync(string saveKey);
        public Task<float> LoadFloatDataAsync(string saveKey);

        public Task<long> LoadLongDataAsync(string saveKey);

        public Task<double> LoadDoubleDataAsync(string saveKey);

        public Task<int> LoadIntDataAsync(string saveKey);

        public Task<bool> LoadBoolDataAsync(string saveKey);

        public Task SaveDataAsync(string saveKey, string saveData);

        // SYNC FUNCTIONS
        public string LoadData(string saveKey);

        public float LoadFloatData(string saveKey);

        public long LoadLongData(string saveKey);

        public double LoadDoubleData(string saveKey);

        public int LoadIntData(string saveKey);

        public bool LoadBoolData(string saveKey);

        public void SaveData(string saveKey, string saveData);

        public bool CheckKeyExist(string saveKey);
    }
}