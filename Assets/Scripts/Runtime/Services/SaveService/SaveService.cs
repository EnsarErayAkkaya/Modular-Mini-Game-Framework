using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Services.SaveServices
{
    public class SaveService : BaseService, ISaveService
    {
        private ISaveHandler saveHandler;

        private Dictionary<Type, string> typesAndKeys = new();
        private List<BaseEntity> datas = new List<BaseEntity>();

        public SaveService(ISaveHandler _saveHandler)
        {
            saveHandler = _saveHandler;
        }

        public async Task<T> CreateTypeDataAsync<T>(string key) where T : BaseEntity
        {
            if (typesAndKeys.ContainsKey(typeof(T)))
            {
                return GetTypeData<T>();
            }

            typesAndKeys.Add(typeof(T), key);

            datas.Add((BaseEntity)Activator.CreateInstance(typeof(T)));

            var data = await LoadTypeDataAsync<T>();

            if (ServicesContainer.Instance.Settings.debugLog)
                Debug.Log("[SaveService] AddNewEntity, Entity " + typesAndKeys[typeof(T)] + ", data: " + datas.Last().Serialize());

            return data;
        }

        public T CreateTypeData<T>(string key) where T : BaseEntity
        {
            if (typesAndKeys.ContainsKey(typeof(T)))
            {
                return GetTypeData<T>();
            }

            typesAndKeys.Add(typeof(T), key);

            datas.Add((BaseEntity)Activator.CreateInstance(typeof(T)));

            var data = LoadTypeData<T>();

            if (ServicesContainer.Instance.Settings.debugLog)
                Debug.Log("[SaveService] AddNewEntity, Entity " + typesAndKeys[typeof(T)] + ", data: " + datas.Last().Serialize());

            return data;
        }

        public async Task SetTypeDataAsync<T>(T entityToSave) where T : BaseEntity
        {
            var entity = datas.FirstOrDefault(s => s.Name == entityToSave.Name);

            if (entity != null) 
            {
                entity = entityToSave;

                if (typesAndKeys.ContainsKey(typeof(T)))
                {
                    if (ServicesContainer.Instance.Settings.debugLog)
                        Debug.Log("[SaveService] SetData, Entity " + typesAndKeys[typeof(T)] + ", data: " + entity.Serialize());

                    await saveHandler.SaveDataAsync(typesAndKeys[typeof(T)], entity.Serialize());
                }
                else
                {
                    Debug.LogError("[SaveService] SetData Failed! Type " + typeof(T).Name + " doesn't exist in assigned types.");
                }
            }
            else
            {
                Debug.LogError("[SaveService] SetData Failed! Type " + typeof(T).Name + " doesn't exist in save datas.");
            }
        }

        public void SetTypeData<T>(T entityToSave) where T : BaseEntity
        {
            var entity = datas.FirstOrDefault(s => s.Name == entityToSave.Name);

            if (entity != null)
            {
                entity = entityToSave;

                if (typesAndKeys.ContainsKey(typeof(T)))
                {
                    if (ServicesContainer.Instance.Settings.debugLog)
                        Debug.Log("[SaveService] SetData, Entity " + typesAndKeys[typeof(T)] + ", data: " + entity.Serialize());

                    saveHandler.SaveData(typesAndKeys[typeof(T)], entity.Serialize());
                }
                else
                {
                    Debug.LogError("[SaveService] SetData Failed! Type " + typeof(T).Name + " doesn't exist in assigned types.");
                }
            }
            else
            {
                Debug.LogError("[SaveService] SetData Failed! Type " + typeof(T).Name + " doesn't exist in save datas.");
            }
        }

        public T GetTypeData<T>() where T : BaseEntity
        {
            var entity = datas.FirstOrDefault(s => s is T);

            if (entity != null)
            {
                if (ServicesContainer.Instance.Settings.debugLog)
                    Debug.Log("[SaveService] GetData, Entity " + typesAndKeys[typeof(T)] + ", data: " + entity.Serialize());

                return (T)entity;
            }
            else
            {
                Debug.LogError("[SaveService] GetData Failed! Type " + typeof(T).Name + " doesn't exist in save datas.");
            }

            return null;
        }

        public async Task<T> LoadTypeDataAsync<T>() where T : BaseEntity
        {
            if (typesAndKeys.ContainsKey(typeof(T)))
            {
                string fileData = await saveHandler.LoadDataAsync(typesAndKeys[typeof(T)]);

                int entityIndex = datas.FindIndex(s => s is T);
                T readEntity = (T)Activator.CreateInstance(typeof(T));
                readEntity = readEntity.Derialize<T>(fileData);

                if (entityIndex != -1 && readEntity != null)
                {
                    if (ServicesContainer.Instance.Settings.debugLog)
                        Debug.Log("[SaveService] LoadData Success. Entity " + typesAndKeys[typeof(T)] + ", read data: " + readEntity.Serialize());
                    datas[entityIndex] = readEntity;
                    return (T)datas[entityIndex];
                }
                else
                {
                    if (entityIndex == -1)
                    {
                        Debug.LogError("[SaveService] LoadData Failed! Type " + typeof(T).Name + " doesn't exist in save datas.");
                    }
                    else if (readEntity == null)
                    {
                        Debug.LogWarning("[SaveService] LoadData Failed! Type " + typeof(T).Name + " save file doesn't exist.");
                    }
                }

            }
            else
            {
                Debug.LogError("[SaveService] LoadData Failed! Type " + typeof(T).Name + " doesn't exist in assigned types.");
            }

            return null;
        }

        public T LoadTypeData<T>() where T : BaseEntity
        {
            if (typesAndKeys.ContainsKey(typeof(T)))
            {
                string fileData = saveHandler.LoadData(typesAndKeys[typeof(T)]);

                int entityIndex = datas.FindIndex(s => s is T);
                T readEntity = (T)Activator.CreateInstance(typeof(T));
                readEntity = readEntity.Derialize<T>(fileData);

                if (entityIndex != -1 && readEntity != null)
                {
                    if (ServicesContainer.Instance.Settings.debugLog)
                        Debug.Log("[SaveService] LoadData Success. Entity " + typesAndKeys[typeof(T)] + ", read data: " + readEntity.Serialize());
                    datas[entityIndex] = readEntity;
                    return (T)datas[entityIndex];
                }
                else
                {
                    if (entityIndex == -1)
                    {
                        Debug.LogError("[SaveService] LoadData Failed! Type " + typeof(T).Name + " doesn't exist in save datas.");
                    }
                    else if (readEntity == null)
                    {
                        Debug.LogWarning("[SaveService] LoadData Failed! Type " + typeof(T).Name + " save file doesn't exist.");
                    }
                }
            }
            else
            {
                Debug.LogError("[SaveService] LoadData Failed! Type " + typeof(T).Name + " doesn't exist in assigned types.");
            }

            return null;
        }

        // ASYNC FUNCTIONS
        public async Task<string> LoadDataAsync(string saveKey)
        {
            return await saveHandler.LoadDataAsync(saveKey);
        }

        public async Task<float> LoadFloatDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent float parsing
            if (float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }

            return 0f;
        }

        public async Task<long> LoadLongDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent long parsing
            if (long.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value))
            {
                return value;
            }

            return 0L;
        }

        public async Task<double> LoadDoubleDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent double parsing
            if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            return 0d;
        }

        public async Task<int> LoadIntDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // Using InvariantCulture for consistent integer parsing
            if (int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }

            return 0;
        }

        public async Task<bool> LoadBoolDataAsync(string saveKey)
        {
            string data = await LoadDataAsync(saveKey);

            // No need for InvariantCulture here, as bool parsing isn't affected by culture.
            if (bool.TryParse(data, out bool value))
            {
                return value;
            }

            return false;
        }

        public async Task SaveDataAsync(string saveKey, string saveData)
        {
            await saveHandler.SaveDataAsync(saveKey, saveData);
        }

        // SYNC FUNCTIONS
        public string LoadData(string saveKey)
        {
            return saveHandler.LoadData(saveKey);
        }

        public float LoadFloatData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent float parsing
            if (float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
            {
                return value;
            }

            return 0f;
        }

        public long LoadLongData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent long parsing
            if (long.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value))
            {
                return value;
            }

            return 0L;
        }

        public double LoadDoubleData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent double parsing
            if (double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            return 0d;
        }

        public int LoadIntData(string saveKey)
        {
            string data = LoadData(saveKey);

            // Using InvariantCulture for consistent integer parsing
            if (int.TryParse(data, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                return value;
            }

            return 0;
        }

        public bool LoadBoolData(string saveKey)
        {
            string data = LoadData(saveKey);

            if (bool.TryParse(data, out bool value))
            {
                return value;
            }

            return false;
        }

        public void SaveData(string saveKey, string saveData)
        {
            saveHandler.SaveData(saveKey, saveData);
        }

        public bool CheckKeyExist(string saveKey)
        {
            return saveHandler.CheckKeyExist(saveKey);
        }
    }
}
