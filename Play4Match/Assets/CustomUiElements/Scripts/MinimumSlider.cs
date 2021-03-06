﻿using UnityEngine;
using UnityEngine.UI;

namespace CustomUiElements
{
    public class MinimumSlider : Slider
    {
        public MaximumSlider MaxSlider;
        public Text Indicator;
        public string NumberFormat;

        protected override void Set(float input, bool sendCallback)
        {
            if (MaxSlider == null)
            {
                MaxSlider = transform.parent.Find("MaxSlider").GetComponent<MaximumSlider>();
            }

            float newValue = input;
            if (wholeNumbers)
            {
                newValue = Mathf.Round(newValue);
            }
            if (newValue >= MaxSlider.RealValue && MaxSlider.RealValue != MaxSlider.minValue)
            {
				// invalid
				return;
            }
			if (Indicator != null)
            {
				if(newValue == 0)
				{
					newValue = 18;
				}
				Indicator.text = newValue.ToString(NumberFormat);
            }
            base.Set(input, sendCallback);
        }

        public void Refresh(float input)
        {
            Set(input, false);
        }
    }
}