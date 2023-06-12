using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common
{
    public class TexturesCache
    {
        private LinkedList<Tuple<int, Texture2D>> _cachedTextures = new LinkedList<Tuple<int, Texture2D>>();
        
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
    }
}