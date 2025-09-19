using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Services
{
    public class GameLoader : MonoBehaviour
    {
        void Start()
        {
            ServicesContainer.Instance.Initialize();

            ServicesContainer.SceneService.LoadScene(SceneServices.SceneKeys.InitialScene);

            ServicesContainer.SceneService.LoadScene(SceneServices.SceneKeys.MenuScene);
        }
    }
}