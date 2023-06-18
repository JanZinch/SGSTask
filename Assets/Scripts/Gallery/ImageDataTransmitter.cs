using UnityEngine;

namespace Gallery
{
    public class ImageDataTransmitter : MonoBehaviour
    {
        public static ImageDataTransmitter Instance { get; private set; } = null;

        private Sprite _sprite = null;
        
        public void Send(Sprite sprite)
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            
            _sprite = sprite;
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Sprite Receive()
        {
            Instance = null;
            Destroy(gameObject);
            
            return _sprite;
        }
    }
}