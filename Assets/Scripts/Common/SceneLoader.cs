using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Common
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string _loadedSceneName;
        [SerializeField] private Slider _loadingSlider;

        private const float DefaultLoadingDuration = 2.0f;

        private List<AsyncOperation> _asyncOperations = null;
        private List<UnityWebRequest> _webRequests = null;

        private static bool _resourcesLoaded = false;
        
        private void Start()
        {
            if (!_resourcesLoaded)
            {
                StartCoroutine(LoadResources(() =>
                {
                    SceneManager.LoadScene(_loadedSceneName);
                }));
            }
            else
            {
                _loadingSlider.DOValue(1.0f, DefaultLoadingDuration).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    SceneManager.LoadScene(_loadedSceneName);
                
                }).SetLink(gameObject);
            }
        }

        private IEnumerator LoadResources(UnityAction onComplete)
        {
            RunOperations(ProjectUtils.StartImagesCount);
            
            while (!AllOperationsAreDone())
            {
                _loadingSlider.value = GetUpdatedProgress();
                yield return null;
            }

            SaveTexturesToCache();
            _resourcesLoaded = true;
            onComplete?.Invoke();
        }
        
        private void RunOperations(int operationsCount)
        {
            _asyncOperations = new List<AsyncOperation>(operationsCount);
            _webRequests = new List<UnityWebRequest>(_asyncOperations.Count);
            
            for (int i = 0; i < operationsCount; i++)
            {
                string uri = string.Format(ProjectUtils.RequestURL, i + 1);

                UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri);
                _asyncOperations.Add(webRequest.SendWebRequest());
                _webRequests.Add(webRequest);
            }
            
        }

        private bool AllOperationsAreDone()
        {
            bool result = true;

            foreach (var asyncOperation in _asyncOperations)
            {
                result = result && asyncOperation.isDone;
            }

            return result;
        }

        private float GetUpdatedProgress()
        {
            float progress = 0.0f;
            
            foreach (var asyncOperation in _asyncOperations)
            {
                progress += asyncOperation.progress / _asyncOperations.Count;
            }

            return progress;
        }

        private void SaveTexturesToCache()
        {
            for (int i = 0; i < _webRequests.Count; i++)
            {
                Texture2D receivedTexture = DownloadHandlerTexture.GetContent(_webRequests[i]);
                TexturesCache.Instance.Add(i + 1, receivedTexture);
                _webRequests[i].Dispose();
            }
        }

        
        
    }
}