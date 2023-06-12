using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Common
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string _loadedSceneName;
        [SerializeField] private Slider _loadingSlider;

        private const float LoadingDuration = 2.5f;
        
        private void Start()
        {
            _loadingSlider.DOValue(1.0f, LoadingDuration).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                SceneManager.LoadScene(_loadedSceneName);
                
            }).SetLink(gameObject);

            /*if (_loadedSceneName == SceneUtils.Gallery)
            {
                ImagesLoader.PreloadToCache(8);
            }*/
        }
    }
}