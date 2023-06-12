using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class ImageView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public bool IsSpriteSpecified { get; private set; } = false;

        public Sprite Sprite {
            
            get => _image.sprite;
            
            set
            {
                _image.sprite = value;
                IsSpriteSpecified = true;
            }
        }

        protected abstract void OnEnable();
        protected abstract void OnDisable();
    }
}