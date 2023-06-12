using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasResizer : MonoBehaviour
    {
        private const float BaseCanvasHeight = 1920f;
        private const float BaseCanvasWidth = 1080f;
        private const float BaseScreenRatio = 16 / 9f;

        public event Action OnCanvasResized;
        
        private void Awake()
        {
            Setup();
        }

        private void Setup()
        {
            GetComponent<CanvasScaler>().referenceResolution = new Vector2(
                BaseCanvasWidth,
                BaseCanvasHeight * Mathf.Max(1f, Screen.height / (float) Screen.width / BaseScreenRatio));
        }

        protected void OnCanvasResizeDone()
        {
            Canvas.ForceUpdateCanvases();
            OnCanvasResized?.Invoke();
        }
    }
}