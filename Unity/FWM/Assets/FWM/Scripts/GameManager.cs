using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class GameManager : MonoBehaviour
    {
		public FishingManager fm = null;
		public OverworldManager om = null;
		private bool leaveInputReceived = false;
		
		
		
		private Vector3 lastPlayerPos = new Vector3(4f, -1f, -15f);
		private bool movedToPos = true;
		
		private bool sceneLoading = false;
		
		private Scene curScene;
		
        void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}
		
		void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		
		void FixedUpdate()
		{
			
			
			if(curScene.name == "Lake" || curScene.name == "River" || curScene.name == "Beach" && curScene.name != "Overworld")
			{
				if(fm.hookScript.leaveInput && !leaveInputReceived)
				{
					leaveInputReceived = true;
					loadOverworldScene();
				}
				
				if(fm.hookScript.activeFish != null && fm.hookScript.fishScript != null)
				{
					if(fm.hookScript.fishScript.fishCaught)
					{
						loadOverworldScene();
					}
				}
			}
			
			if(curScene.name == "Overworld")
			{
				if(!movedToPos)
				{
					if(om.ovwPlayer != null)
					{
						om.ovwPlayer.transform.position = lastPlayerPos;
						movedToPos = true;
					}
				}
			}
		}
		
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			curScene = scene;

			if(scene.name == "Overworld" && om.enabled == false)
			{
				om.enabled = true;
				fm.enabled = false;
			}
			
			if(scene.name == "Lake" || scene.name == "River" || scene.name == "Beach" && fm.enabled == false)
			{
				fm.enabled = true;
				om.enabled = false;
			}
			
			
			sceneLoading = false;
		}
		
		public void loadFishingScene()
		{
			if(!sceneLoading && curScene.name == "Overworld" && om.playerScript.fishingTrigger != null)
			{
				sceneLoading = true;
				
				lastPlayerPos = om.ovwPlayer.transform.position;
				movedToPos = false;
				leaveInputReceived = false;
				

				
				switch(om.playerScript.fishingTrigger.name)
				{
					case "LakeSpot":
						SceneManager.LoadScene("Lake");
						break;
					case "BeachSpot":
						SceneManager.LoadScene("Beach");
						break;
					case "RiverSpot":
						SceneManager.LoadScene("River");
						break;
					default:
						sceneLoading = false;
						break;
				}
			}
		}
		
		public void loadOverworldScene()
		{
			if(!sceneLoading && (curScene.name == "Lake" || curScene.name == "Beach" || curScene.name == "River") )
			{
				sceneLoading = true;
				
				SceneManager.LoadScene("Overworld");
			}
		}
		
		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
    }
}
