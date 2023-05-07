using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class FishingManager : MonoBehaviour
    {
		public GameObject fishingHook;
		public HookControl hookScript;
		
		void OnEnable()
		{
			fishingHook = GameObject.Find("HookController");
			hookScript = fishingHook.GetComponent<HookControl>() as HookControl;
		}
		
		void OnDisable()
		{
			fishingHook = null;
			hookScript = null;
		}
		
    }
}
