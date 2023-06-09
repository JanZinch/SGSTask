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

        private void Start()
        {
            new ImagesLoader(_imageOriginal, _imagesLayout).Update();
        }
        
    }
}