using System;
using Common;
using Gallery;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LargeImageView : ImageView
    {
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            if (ImageDataTransmitter.Instance != null)
            {
                Sprite = ImageDataTransmitter.Instance.Receive();
            }
        }

        protected override void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseClick);
        }

        private void OnCloseClick()
        {
            SceneManager.LoadScene(GalleryUtility.SceneNames.Gallery);
        }

        protected override void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseClick);
        }
        
    }
}