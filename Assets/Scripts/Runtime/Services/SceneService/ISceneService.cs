using System;
using System.Collections.Generic;
using UnityEngine;
namespace EEA.Services.SceneServices
{
    public interface ISceneService
    {
        public Dictionary<string, GameObject> LoadedScenes { get; }

        GameObject LoadScene(string sceneType);
        void RemoveScene(string scene);

        void Clear();
    }
}