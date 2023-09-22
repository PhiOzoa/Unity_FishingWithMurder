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
		
		public InventoryManager im = null;
		
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
			
		}
		
		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			curScene = scene;
			/*
			if(curScene.name == "Beach")
			{
				GameObject.Find("CatchManager").GetComponent<CatchCalc>().im = im;
			}*/
			
			if(curScene.name == "TackleBoxMenu")
			{
				im.PopulateInventoryUI(GameObject.Find("Content"));
			}
		
		}
		
		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
		
    }
}
