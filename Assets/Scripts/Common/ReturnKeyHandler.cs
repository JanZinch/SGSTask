using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ReturnKeyHandler : MonoBehaviour
    {
        [SerializeField] private string _previousSceneName;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(_previousSceneName);
            }
        }
    }
}