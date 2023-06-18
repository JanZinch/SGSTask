using Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Gallery : MonoBehaviour
    {
        [SerializeField] private ImageView _imageOriginal;
        [SerializeField] private LayoutGroup _imagesLayout;
        [SerializeField] private ScrollRect _scrollRect;
        
        private const int IncreaseImagesCount = 2; 
        
        private int _increaseStepsCount;   
        private float _increaseStep;
        private Vector2 _minNormalizedPosition = new Vector2(0.0f, 1.0f);
        
        private ImagesLoader _imagesLoader = null;
        
        private void Awake()
        {
            _increaseStepsCount = Mathf.RoundToInt(ProjectUtils.MaxImagesCount / (float)IncreaseImagesCount);
            _increaseStep = 1.0f / _increaseStepsCount;
            
            _imagesLoader = new ImagesLoader(_imageOriginal, _imagesLayout, ProjectUtils.MaxImagesCount, TexturesCache.Instance);
            _imagesLoader.LoadIfPossible(ProjectUtils.StartImagesCount);
        }

        private void OnEnable()
        {
            _scrollRect.onValueChanged.AddListener(OnScrollRectUpdated);
        }

        private void OnScrollRectUpdated(Vector2 normalizedPosition)
        {
            if (_minNormalizedPosition.y - normalizedPosition.y > _increaseStep)
            {
                _imagesLoader.LoadIfPossible(IncreaseImagesCount);
                _minNormalizedPosition.y -= _increaseStep;
            }
        }
        
        private void OnDisable()
        {
            _scrollRect.onValueChanged.RemoveListener(OnScrollRectUpdated);
        }
    }
}