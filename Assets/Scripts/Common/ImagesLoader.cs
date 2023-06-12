using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private static LinkedList<Tuple<int, Texture2D>> _cachedTextures = new LinkedList<Tuple<int, Texture2D>>();
        
        private LinkedList<ImageView> _images = new LinkedList<ImageView>();
        
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

        private IEnumerator LoadImageFromServer(string uri, ImageView image)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri))
            {
                yield return webRequest.SendWebRequest();
                
                Texture2D receivedTexture = DownloadHandlerTexture.GetContent(webRequest);
                _cachedTextures.AddLast(new Tuple<int, Texture2D>(_cachedTextures.Count + 1, receivedTexture));
                
                SetTextureToImageView(receivedTexture, image);
            }

            yield return null;
        }
        
        private bool TryLoadImageFromCache(int id, ImageView image)
        {
            Tuple<int, Texture2D> foundTexture = _cachedTextures.FirstOrDefault(tuple => tuple.Item1 == id);

            if (foundTexture != null)
            {
                SetTextureToImageView(foundTexture.Item2, image);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private static void SetTextureToImageView(Texture2D texture, ImageView image)
        {
            Rect textureRect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            image.Sprite = Sprite.Create(texture, textureRect, Vector2.zero);
        }

        private void AddImage()
        {
            ImageView newImage = Object.Instantiate(_imageOriginal, _imagesParentLayout.transform);
            
            if (!TryLoadImageFromCache(_images.Count + 1, newImage))
            {
                _imagesParentLayout.StartCoroutine(LoadImageFromServer(string.Format(RequestURL, _images.Count + 1), newImage));
            }
            
            _images.AddLast(newImage);
        }
    }
    
}