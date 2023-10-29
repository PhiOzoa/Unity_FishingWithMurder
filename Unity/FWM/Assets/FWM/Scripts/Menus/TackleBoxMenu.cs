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
		
		public void OnEnable()
		{
			initButton.Select();
		}
		
		public void GoFishing()
		{
			SceneManager.LoadScene("Beach");
		}
		
		public void Inventory()
		{
			
		}
		
		public void MainMenu()
		{
			SceneManager.LoadScene("MainMenu");
		}
    }
}
