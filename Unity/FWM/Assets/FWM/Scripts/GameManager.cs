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
			if(!sceneLoading && curScene.name == "Overworld")
			{
				SceneManager.LoadScene("River");
				sceneLoading = true;
			}
		}
		
		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
    }
}
