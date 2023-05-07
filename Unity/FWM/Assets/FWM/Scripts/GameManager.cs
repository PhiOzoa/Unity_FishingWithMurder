using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class GameManager : MonoBehaviour
    {
		private GameObject ovrWrldPlayer = null;
		
		private FishingManager fm = null;
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
			
			if(scene.name == "Overworld" && om == null)
			{
				om = gameObject.AddComponent<OverworldManager>() as OverworldManager;
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
