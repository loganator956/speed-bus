using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpeedBus.GUI
{
    public class ChanceBar : MonoBehaviour
    {
        public float BarSpeed = 1f;
        public float BadRangeSize = 10f; // in total, divided in half on either end of the bar
        public float GoodRangeSize = 5f;

        public UnityEvent<double> OnBarSubmission = new UnityEvent<double>();

        public RectTransform[] BadRegions;
        public RectTransform GoodRegion, SlideRegion, Slider;

        public AnimationCurve TMoveCurve;

        private bool _direction = true;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void UpdateBar()
        {
            maxWidth = SlideRegion.rect.width;
            foreach(var region in BadRegions)
            {
                /*region.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal)*/
                region.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth * ((BadRangeSize / 2f) / 100f));
            }
            GoodRegion.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth * (GoodRangeSize / 100f));
        }

        double currentT = 0f;
        float maxWidth = 50f;

        private void Update()
        {
            if (_direction)
            {
                currentT += Time.deltaTime * BarSpeed;
                if (currentT > 1f) { _direction = false; };
            }
            else
            {
                currentT -= Time.deltaTime * BarSpeed;
                if (currentT < 0f) { _direction = true; };
            }
            Slider.anchoredPosition = new Vector2(Mathf.Lerp(-maxWidth / 2f, maxWidth / 2f, TMoveCurve.Evaluate((float)currentT)), 0);
        }

        public void ButtonPressed()
        {
            OnBarSubmission.Invoke(currentT);
        }
    }
}