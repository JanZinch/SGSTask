using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Common
{
    public class ImagesLoader
    {
        private readonly ImageView _imageOriginal;
        private readonly LayoutGroup _imagesParentLayout;
        private readonly int _maxImagesCount;
        
        private const string RequestURL = "http://data.ikppbb.com/test-task-unity-data/pics/{0:d}.jpg";
        
        private LinkedList<ImageView> _images = new LinkedList<ImageView>();

        private static TexturesCache _texturesCache = new TexturesCache();
        
        public ImagesLoader(ImageView imageOriginal, LayoutGroup imagesParentLayout, int maxImagesCount)
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

        private IEnumerator LoadImageFromServer(int imageIndex, ImageView image)
        {
            string uri = string.Format(RequestURL, imageIndex);
            
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
            {
                yield return webRequest.SendWebRequest();
                
                Texture2D receivedTexture = DownloadHandlerTexture.GetContent(webRequest);
                _texturesCache.Add(imageIndex, receivedTexture);
                
                image.Sprite = CreateSpriteFromTexture(receivedTexture);
            }
            
            yield return null;
        }
        
        private bool TryGetImageFromCache(int imageIndex, ImageView image)
        {
            Texture2D foundTexture = _texturesCache.Get(imageIndex);

            if (foundTexture != null)
            {
                image.Sprite = CreateSpriteFromTexture(foundTexture);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private static Sprite CreateSpriteFromTexture(Texture2D texture)
        {
            Rect textureRect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            return Sprite.Create(texture, textureRect, Vector2.zero);
        }

        private void AddImage()
        {
            ImageView newImage = Object.Instantiate(_imageOriginal, _imagesParentLayout.transform);
            
            if (!TryGetImageFromCache(_images.Count + 1, newImage))
            {
                _imagesParentLayout.StartCoroutine(LoadImageFromServer(_images.Count + 1, newImage));
            }
            
            _images.AddLast(newImage);
        }
    }
    
}