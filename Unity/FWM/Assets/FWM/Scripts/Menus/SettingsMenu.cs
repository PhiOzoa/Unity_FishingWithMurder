using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FWM
{
    public class SettingsMenu : MonoBehaviour
    {
		public Button initButton;
		
		public void OnEnable()
		{
			initButton.Select();
		}
    }
}
