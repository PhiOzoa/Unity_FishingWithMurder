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
		private OverworldManager om = null;
		
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
			
			if(scene.name == "Overworld")
			{
				
			}
			
			sceneLoading = false;
		}
		
		public void loadFishingScene()
		{
			if(!sceneLoading)
			{
				SceneManager.LoadScene("Lake");
				sceneLoading = true;
			}
		}
		
		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
    }
}
