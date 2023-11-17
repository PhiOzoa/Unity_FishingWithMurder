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
		public RectTransform canvasTransform;
		public RectTransform bucketTransform;
		
		public RectTransform depthDialTransform;
		
		private float dialZero = 119f;
		private float dialHundred = -117f;
		
		public float maxMeasuredDepth = 150f;
		
		public GameObject pauseMenu;
		public Animator snagUI;
		public Animator catchUI;
		
		public Button keepFishingButton;
		
		public GameObject hook;

		public float depthFactor = 5f;
		
		private bool isPaused = false;
		
		public List<GameObject> caughtFish;
		
		public GameObject fishIconPrefab;
		
		private bool spawningFishIcons = false;
		//private GameObject currentFishIcon;
		private float nextSpawnTime = 0.0f;
		private float period = 1f;
		private int fishCountToSpawn = 0;
		private bool fishCountSet = false;
		
		private void Start()
		{
			pauseMenu.SetActive(false);
		}
		
		private void Update()
		{
			AlterDepthGauge();
			
			if(spawningFishIcons)
			{
				if(!fishCountSet)
				{
					fishCountToSpawn = caughtFish.Count - 1;
					fishCountSet = true;
				}
				
				if( Time.time > nextSpawnTime )
				{
					nextSpawnTime = Time.time + period;
					
					if(fishCountToSpawn >= 0)
					{
						InstantiateFishIcons(fishCountToSpawn);
						fishCountToSpawn--;
					}
					else
					{
						spawningFishIcons = false;
						fishCountSet = false;
						keepFishingButton.interactable = true;
					}
				}
			}
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
		
		//PAUSING
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
		
		//SNAGGING
		public void SnagAnim()
		{
			snagUI.SetTrigger("Play");
		}
		
		//CATCHING
		public void ActivateCatchMenu(bool caught)
		{
			catchUI.gameObject.SetActive(true);
			
			if(caught)
			{
				CatchAnim();
			}
			else
			{
				keepFishingButton.interactable = true;
			}
		}
		/*
		public void GetFishObjects(List<GameObject> fishesList)
		{
			caughtFish = fishesList;
			Debug.Log(caughtFish[0].name);
		}*/
		
		private void CatchAnim()
		{
			catchUI.SetTrigger("Play");
			catchUI.SetTrigger("BucketPlay");

			spawningFishIcons = true;
		}
		
		//BACKTOFISHING
		public void ReturnToFishing()
		{
			caughtFish.Clear();
			
			keepFishingButton.interactable = false;
			catchUI.Play("Bucket.BucketGone");
			catchUI.gameObject.SetActive(false);
			
			hook.SendMessage("ReturnActions");
		}
		
		//BACKTOMENU
		public void ReturnToMenu()
		{
			Unpause();
			SceneManager.LoadScene("TackleBoxMenu");
		}
		
		public void InstantiateFishIcons(int i)
		{
			// turn gameobject position to a canvas space position
			Vector2 fishViewport = Camera.main.WorldToViewportPoint(caughtFish[i].transform.GetChild(0).transform.position);
			Vector2 fishScreen = new Vector2(
			( (fishViewport.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f) ),
			( (fishViewport.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f) ) );
			
			// create icon prefab and pass it values for beginning, end, name string, and the animator for the bucket
			GameObject currentFishIcon;
			currentFishIcon = Instantiate(fishIconPrefab, canvasTransform);
			currentFishIcon.GetComponent<MoveToTarget>().SetParams(fishScreen, new Vector2(-645f,0f) , caughtFish[i].name, catchUI);
			
			// destroy the fish object once the icon is created
			Destroy(caughtFish[i], 1f);
			
			//Vector2 anchoredPos;
			//RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.transform.GetChild(0),)
		}
    }
}
