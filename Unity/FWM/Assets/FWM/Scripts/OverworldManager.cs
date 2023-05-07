using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class OverworldManager : MonoBehaviour
    {
		public GameObject ovwPlayer;
		public PlayerController playerScript;
		
		void OnEnable()
		{
			ovwPlayer = GameObject.Find("Player");
			playerScript = ovwPlayer.GetComponent<PlayerController>() as PlayerController;
		}
		
		void OnDisable()
		{
			ovwPlayer = null;
			playerScript = null;
		}
	}
}
