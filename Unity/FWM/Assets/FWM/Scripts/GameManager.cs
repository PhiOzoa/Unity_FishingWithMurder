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
		
		private Scene curScene;
		
		public List<FishInfo> inventoryFish = new List<FishInfo>();
		
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
		}
		
		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
		
		void AddToInventory()
		{
			
		}
    }
}
