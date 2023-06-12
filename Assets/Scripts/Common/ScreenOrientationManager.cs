using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ScreenOrientationManager : MonoBehaviour
    {
        private void Awake()
        {
            Screen.orientation = SceneUtils.GetScreenOrientation(SceneManager.GetActiveScene().name);
        }
    }
}