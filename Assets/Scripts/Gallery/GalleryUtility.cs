using UnityEngine;

namespace Gallery
{
    public static class GalleryUtility
    {
        public struct SceneNames
        {
            public const string Menu = "Menu";
            public const string Load = "Load";
            public const string Gallery = "Gallery";
            public const string View = "View";
        }
        
        public const int MaxImagesCount = 66;
        public const int StartImagesCount = 10;
        public const string RequestURL = "http://data.ikppbb.com/test-task-unity-data/pics/{0:d}.jpg";
        
        public static ScreenOrientation GetScreenOrientation(string sceneName)
        {
            switch (sceneName)
            {
                case SceneNames.Menu: 
                case SceneNames.Load:
                case SceneNames.Gallery:
                    return ScreenOrientation.Portrait;
                
                case SceneNames.View:
                default:
                    return ScreenOrientation.AutoRotation;
            }
        }

    }
}