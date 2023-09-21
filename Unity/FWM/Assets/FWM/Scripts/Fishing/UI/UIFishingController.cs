using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FWM
{
    public class UIFishingController : MonoBehaviour
    {
		public TMP_Text depthDisplay;
		
		public GameObject pauseMenu;
		
		public GameObject hook;

		public float depthFactor = 2f;
		private float depthRounded;
		
		private bool isPaused = false;
		
		private void Start()
		{
			pauseMenu.SetActive(false);
		}
		
		private void Update()
		{
			depthRounded = -( (UnityEngine.Mathf.Round((hook.transform.position.y * depthFactor) * 10f)) / 10f );
			depthDisplay.text = string.Format("Depth: {0:F1}", depthRounded);//"Depth: " + depthRounded;
		}
		
		public void PauseInput()
		{
			if(!isPaused)
			{
				Pause();
			}
			else
			{
				Unpause();
			}
		}
		
		private void Pause()
		{
			pauseMenu.SetActive(true);
			Time.timeScale = 0f;
			isPaused = true;
		}
		
		private void Unpause()
		{
			pauseMenu.SetActive(false);
			Time.timeScale = 1f;
			isPaused = false;
		}
		
    }
}
