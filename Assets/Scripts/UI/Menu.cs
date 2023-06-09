using Gallery;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button _toGalleryButton;
        
        private void OnEnable()
        {
            _toGalleryButton.onClick.AddListener(OnGalleryClick);
        }

        private void OnGalleryClick()
        {
            SceneManager.LoadScene(GalleryUtility.SceneNames.Load);
        }

        private void OnDisable()
        {
            _toGalleryButton.onClick.RemoveListener(OnGalleryClick);
        }
    }
}
