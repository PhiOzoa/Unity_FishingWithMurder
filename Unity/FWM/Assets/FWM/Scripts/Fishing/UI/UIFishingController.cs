using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class UIFishingController : MonoBehaviour
    {
		public RectTransform depthDialTransform;
		
		private float dialZero = 119f;
		private float dialHundred = -117f;
		
		public float maxMeasuredDepth = 150f;
		
		public GameObject pauseMenu;
		public Animator snagUI;
		public Animator catchUI;
		
		public GameObject hook;

		public float depthFactor = 5f;
		
		private bool isPaused = false;
		
		private void Start()
		{
			pauseMenu.SetActive(false);
		}
		
		private void Update()
		{
			AlterDepthGauge();
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
		
		private void AlterDepthGauge()
		{
			float interpolant = Mathf.InverseLerp(0f, maxMeasuredDepth, -(hook.transform.position.y * depthFactor) );
			
			float depthInGauge = Mathf.Lerp(dialZero, dialHundred, interpolant);
			
			depthDialTransform.rotation = Quaternion.Euler(0f,0f, depthInGauge);
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
		
		
		public void SnagAnim()
		{
			snagUI.SetTrigger("Play");
		}
		
		public void ActivateCatchMenu(bool caught)
		{
			catchUI.gameObject.SetActive(true);
			
			if(caught)
			{
				CatchAnim();
			}
		}
		
		private void CatchAnim()
		{
			catchUI.SetTrigger("Play");
			catchUI.SetTrigger("BucketPlay");
			
		}
		
		public void ReturnToFishing()
		{
			Debug.Log("return");
		}
		
		public void ReturnToMenu()
		{
			Unpause();
			SceneManager.LoadScene("TackleBoxMenu");
		}
    }
}
