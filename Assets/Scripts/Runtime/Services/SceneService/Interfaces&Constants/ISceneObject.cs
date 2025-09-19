using log4net.Util;
using System.Threading.Tasks;

namespace EEA.Services.SceneService
{
    public interface ISceneObject
    {
        Transform transform { get; }
        Task Initialize();
        Task Clear();
    }
}