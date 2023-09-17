using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class MainMenu : MonoBehaviour
    {
		public Button initButton;
		
		public void OnEnable()
		{
			initButton.Select();
		}
		
		public void PlayGame()
		{
			SceneManager.LoadScene("TackleBoxMenu");
		}
		
		public void QuitGame()
		{
			Application.Quit();
		}
		
    }
}
