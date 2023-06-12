using System;
using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UI
{
    public class Gallery : MonoBehaviour
    {
        [SerializeField] private Image _imageOriginal;
        [SerializeField] private LayoutGroup _imagesLayout;
        [SerializeField] private ScrollRect _scrollRect;
        
        private const int MaxImagesCount = 66;
        private const int ImagesInScreenCount = 10;     // 10

        private int _increaseStepsCount;   
        private float _increaseStep;
        private Vector2 _minNormalizedPosition = new Vector2(0.0f, 1.0f);
        
        private ImagesLoader _imagesLoader = null;
        
        private void Awake()
        {
            _increaseStepsCount = Mathf.CeilToInt(MaxImagesCount / (float)ImagesInScreenCount);  //  7
            _increaseStep = 1.0f / _increaseStepsCount;
            
            _imagesLoader = new ImagesLoader(_imageOriginal, _imagesLayout, MaxImagesCount);
            _imagesLoader.Load(ImagesInScreenCount);
        }

        private void OnEnable()
        {
            _scrollRect.onValueChanged.AddListener(OnScrollRectUpdated);
        }

        private void OnScrollRectUpdated(Vector2 normalizedPosition)
        {
        
            //Debug.LogFormat("Pos: {0:f3}", _scrollRect.normalizedPosition.y);
            //Debug.Log("Pos: " + new Vector2(0.0f, 1.345f));
        
            if (_minNormalizedPosition.y - normalizedPosition.y > _increaseStep)
            {
                _imagesLoader.Load(ImagesInScreenCount);
                _minNormalizedPosition.y -= _increaseStep;
            }
        }
        
        /*private void FixedUpdate()
        {
            Debug.Log("Pos: " + _scrollRect.normalizedPosition);
        }*/
        
        private void OnDisable()
        {
            _scrollRect.onValueChanged.RemoveListener(OnScrollRectUpdated);
        }
    }
}