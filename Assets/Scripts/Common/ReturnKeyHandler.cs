using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ReturnKeyHandler : MonoBehaviour
    {
        [SerializeField] private string _previousSceneName;
        [SerializeField] private bool _quitApplication;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_quitApplication)
                {
                    Application.Quit(0);
                }
                else
                {
                    SceneManager.LoadScene(_previousSceneName); 
                }
            }
        }
    }
}