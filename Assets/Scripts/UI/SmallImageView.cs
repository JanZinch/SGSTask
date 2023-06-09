﻿using Gallery;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SmallImageView : ImageView
    {
        [SerializeField] private Button _button;
        [SerializeField] private ImageDataTransmitter _dataTransmitterOriginal;
        
        protected override void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (IsSpriteSpecified)
            {
                Instantiate(_dataTransmitterOriginal).Send(Sprite);
                SceneManager.LoadScene(GalleryUtility.SceneNames.View);
            }
        }

        protected override void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}