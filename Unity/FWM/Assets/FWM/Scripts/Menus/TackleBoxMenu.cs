using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class TackleBoxMenu : MonoBehaviour
    {
		public Button initButton;
		public DynamicCam dynamicCam;
		
		public void OnEnable()
		{
			initButton.Select();
			
		}
		
		public void SelectLast(Button lastButton)
		{
			lastButton.Select();
		}
		
		public void GoFishing()
		{
			SceneManager.LoadScene("Beach");
		}
		
		public void SwitchCameraOnHighlight(string highlightedButtonName)
		{
			
			switch (highlightedButtonName)
			{
				case "GoFishingButton":
					
					dynamicCam.SwitchPriority(0);
					
					break;
				
				case "InventoryButton":
					
					dynamicCam.SwitchPriority(1);
					
					break;
					
				case "MainMenuButton":
				
					dynamicCam.SwitchPriority(2);
					
					break;
			}
			
		}
		
		public void MainMenu()
		{
			SceneManager.LoadScene("MainMenu");
		}
    }
}
