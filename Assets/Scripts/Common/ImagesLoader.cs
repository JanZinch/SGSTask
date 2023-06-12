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
        private readonly Image _imageOriginal;
        private readonly LayoutGroup _imagesParentLayout;
        private readonly int _maxImagesCount;
        
        private const string RequestURL = "http://data.ikppbb.com/test-task-unity-data/pics/{0:d}.jpg";
        
        private LinkedList<Image> _images = new LinkedList<Image>();

        public ImagesLoader(Image imageOriginal, LayoutGroup imagesParentLayout, int maxImagesCount)
        {
            _imageOriginal = imageOriginal;
            _imagesParentLayout = imagesParentLayout;
            _maxImagesCount = maxImagesCount;
        }

        public void LoadIfPossible(int imagesCount)
        {
            int unloadedImagesCount = _maxImagesCount - _images.Count;
            
            for (int i = 0; i < Mathf.Clamp(imagesCount, 0, unloadedImagesCount) ; i++)
            {
                AddImage();
            }
        }

        IEnumerator LoadImage(string uri, Image image)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
            {
                yield return webRequest.SendWebRequest();
                
                Texture2D receivedTexture = DownloadHandlerTexture.GetContent(webRequest);
                Rect receivedTextureRect = new Rect(0.0f, 0.0f, receivedTexture.width, receivedTexture.height);
                
                image.sprite = Sprite.Create(receivedTexture, receivedTextureRect, Vector2.zero);
            }

            yield return null;
        }
        
        private void AddImage()
        {
            Image newImage = Object.Instantiate(_imageOriginal, _imagesParentLayout.transform);
            _imagesParentLayout.StartCoroutine(LoadImage(string.Format(RequestURL, _images.Count + 1), newImage));
            _images.AddLast(newImage);
        }
    }
    
}