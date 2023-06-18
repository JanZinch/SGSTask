using Gallery;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ScreenOrientationManager : MonoBehaviour
    {
        private void Awake()
        {
            Screen.orientation = GalleryUtility.GetScreenOrientation(SceneManager.GetActiveScene().name);
        }
    }
}