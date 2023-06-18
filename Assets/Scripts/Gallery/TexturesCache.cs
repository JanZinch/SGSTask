using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gallery
{
    public class TexturesCache : MonoBehaviour
    {
        private LinkedList<Tuple<int, Texture2D>> _cachedTextures = new LinkedList<Tuple<int, Texture2D>>();

        public static TexturesCache Instance { get; private set; } = null;

        public void Add(int texIndex, Texture2D texInstance)
        {
            _cachedTextures.AddLast(new Tuple<int, Texture2D>(texIndex, texInstance));
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

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}