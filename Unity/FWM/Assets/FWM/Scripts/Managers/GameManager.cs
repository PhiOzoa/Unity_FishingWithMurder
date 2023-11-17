using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class GameManager : MonoBehaviour
    {
		
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
		}
		
		void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
		
    }
}
