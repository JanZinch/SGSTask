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

        private ImagesLoader _imagesLoader = null;

        private const int MaxImagesCount = 66;

        private readonly int FragmentCount = Mathf.CeilToInt(MaxImagesCount / 10);

        private const float GrowthStep = 0.1f;
        private Vector2 _minNormalizedPosition = new Vector2(0.0f, 1.0f);
        
        private void Awake()
        {
            _imagesLoader = new ImagesLoader(_imageOriginal, _imagesLayout);
            _imagesLoader.Load(8);
        }

        private void OnEnable()
        {
            _scrollRect.onValueChanged.AddListener(OnScrollRectUpdated);
        }

        private void OnScrollRectUpdated(Vector2 normalizedPosition)
        {
            if (_minNormalizedPosition.y - normalizedPosition.y > GrowthStep)
            {
                _imagesLoader.Load(8);
                _minNormalizedPosition.y -= GrowthStep;
            }
        }
        
        private void FixedUpdate()
        {
            Debug.Log("Pos: " + _scrollRect.normalizedPosition);
        }
        
        private void OnDisable()
        {
            _scrollRect.onValueChanged.RemoveListener(OnScrollRectUpdated);
        }
    }
}