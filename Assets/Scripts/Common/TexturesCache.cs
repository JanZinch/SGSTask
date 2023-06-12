using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Common
{
    public class TexturesCache
    {
        private LinkedList<Tuple<int, Texture2D>> _cachedTextures = new LinkedList<Tuple<int, Texture2D>>();

        public void Preload(string uri)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(uri);
            
            Debug.Log("LOADED 0");
            
            webRequest.SendWebRequest().completed += operation =>
            {
                Texture2D receivedTexture = DownloadHandlerTexture.GetContent(webRequest);
                Add(receivedTexture);
                webRequest.Dispose();
                
                Debug.Log("LOADED 1");
            };
        }
        
        public int Add(Texture2D texInstance)
        {
            int texIndex = _cachedTextures.Count + 1;
            _cachedTextures.AddLast(new Tuple<int, Texture2D>(texIndex, texInstance));
            return texIndex;
        }

        public Texture2D Get(int texIndex)
        {
            Tuple<int, Texture2D> foundTexture = _cachedTextures.FirstOrDefault(tuple => tuple.Item1 == texIndex);

            if (foundTexture != null)
            {
                return foundTexture.Item2;
            }
            else 
                return null;
        }
    }
}