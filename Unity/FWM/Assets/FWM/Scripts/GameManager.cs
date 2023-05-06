using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class GameManager : MonoBehaviour
    {
		
		//Scene scene 
        void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}
    }
}
