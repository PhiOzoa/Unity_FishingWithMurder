using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FWM
{
    public class PauseMenu : MonoBehaviour
    {
		public Button initButton;
		
		public void OnEnable()
		{
			initButton.Select();
		}
		
		public void ReturnToSurface()
		{
			
		}
		
    }
}