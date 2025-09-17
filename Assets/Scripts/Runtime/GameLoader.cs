using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Services
{
    public class GameLoader : MonoBehaviour
    {
        async void Start()
        {
            ServicesContainer.Instance.Initialize();

            ServicesContainer.SceneService.LoadScene(SceneServices.SceneKeys.InitialScene);

            ServicesContainer.SceneService.LoadScene(SceneServices.SceneKeys.GameScene);
        }
    }
}