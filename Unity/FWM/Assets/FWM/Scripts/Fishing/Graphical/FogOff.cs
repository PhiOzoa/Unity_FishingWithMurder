using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class FogOff : MonoBehaviour
    {
		public Transform waterHeight;
		private bool fogOn = true;
		
		private void Update()
		{
			if(transform.position.y > waterHeight.position.y && fogOn == true)
			{
				RenderSettings.fog = false;
				fogOn = false;
			}
			
			if(transform.position.y <= waterHeight.position.y && fogOn == false)
			{
				RenderSettings.fog = true;
				fogOn = true;
			}
		}
		
		
    }
}
