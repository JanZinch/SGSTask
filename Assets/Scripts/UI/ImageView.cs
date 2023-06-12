using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class ImageView : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public Sprite Sprite {
            
            get => _image.sprite;
            set => _image.sprite = value;
        }

        protected abstract void OnEnable();
        protected abstract void OnDisable();
    }
}