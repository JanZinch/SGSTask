using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Common
{
    public class ImagesLoader
    {
        private Image _imageOriginal;
        private LayoutGroup _layout;
        
        private LinkedList<Image> _images = new LinkedList<Image>();

        private const string CollectionURL = "http://data.ikppbb.com/test-task-unity-data/pics/{0:d}.jpg";
        private const int MaxImagesCount = 66;
        private int _currentImagesCount = 0;

        public ImagesLoader(Image imageOriginal, LayoutGroup layout)
        {
            _imageOriginal = imageOriginal;
            _layout = layout;
        }

        public void Update()
        {
            for (int i = 0; i < 66; i++)
            {
                AddImage();
            }
        }

        IEnumerator LoadImage(string uri, Image image)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
            {
                yield return webRequest.SendWebRequest();
                Debug.Log("Request result: " + webRequest.result);

                Texture2D receivedTexture = DownloadHandlerTexture.GetContent(webRequest);
                Rect receivedTextureRect = new Rect(0.0f, 0.0f, receivedTexture.width, receivedTexture.height);
                
                image.sprite = Sprite.Create(receivedTexture, receivedTextureRect, Vector2.zero);
            }

            yield return null;
        }
        
        private void AddImage()
        {
            Image newImage = Object.Instantiate(_imageOriginal, _layout.transform);

            _layout.StartCoroutine(LoadImage(string.Format(CollectionURL, _currentImagesCount + 1), newImage));
            
            _images.AddLast(newImage);

            _currentImagesCount++;
        }



    }
    
}