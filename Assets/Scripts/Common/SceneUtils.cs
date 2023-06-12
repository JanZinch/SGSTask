using UnityEngine;

namespace Common
{
    public static class SceneUtils
    {
        public const string Menu = "Menu";
        public const string Load = "Load";
        public const string Gallery = "Gallery";
        public const string View = "View";
        
        public static ScreenOrientation GetScreenOrientation(string sceneName)
        {
            switch (sceneName)
            {
                case Menu: 
                case Load:
                case Gallery:
                    return ScreenOrientation.Portrait;
                
                case View:
                default:
                    return ScreenOrientation.AutoRotation;
            }
        }

    }
}