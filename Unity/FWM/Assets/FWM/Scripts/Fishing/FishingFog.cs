using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class FishingFog : MonoBehaviour
    {
		public Camera cam;
		public Light light;
		
		public float minCameraDepth = 0f;
		public float maxCameraDepth = 50f;
		
		public float shallowDensity = 0.01f;
		public float deepDensity = 0.3f;
		
		public float shallowBrightness = 1.8f;
		public float deepBrightness = 0.3f;
		
		public Gradient colGrad;
		private Color curColour;
		
		private float i = 0f;
		
		private void FixedUpdate()
		{
			if(cam != null)
			{
				i = Mathf.InverseLerp(minCameraDepth, -maxCameraDepth, cam.transform.position.y);

				RenderSettings.fogDensity = Mathf.Lerp(shallowDensity, deepDensity, i);
				
				if(light != null)
				{
					light.intensity = Mathf.Lerp(shallowBrightness, deepBrightness, i);
				}
				
				curColour = colGrad.Evaluate(Mathf.Lerp(0f,1f,i));
				RenderSettings.fogColor = curColour;
			}
		}
		
    }
}
