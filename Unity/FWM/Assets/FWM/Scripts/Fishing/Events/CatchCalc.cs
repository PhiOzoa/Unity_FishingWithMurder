using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class CatchCalc : MonoBehaviour
    {
		public GameObject hook;
		public List<FishInfo> snaggedFishList;
		public float scaleFactor = 10f;
		
		private void OnTriggerEnter(Collider col)
		{
			if(col.gameObject.tag == "Hook")
			{
				Debug.Log("hello");
				Catch();
			}
		}
		
		private void Catch()
		{
			
			
			AnimateHook();
			
			GetFish();
		}
		
		private void AnimateHook()
		{
			
		}
		
		private void GetFish()
		{
			snaggedFishList = new List<FishInfo>();
			
			for(int i = 0; i < hook.transform.childCount; i++)
			{
				Transform curTrans = hook.transform.GetChild(i);
				
				if(curTrans.gameObject.tag == "Fish")
				{
					FishInfo info = new FishInfo();
					info.fishName = curTrans.name;
					info.fishLength = curTrans.GetChild(0).localScale.y * scaleFactor;
					
					snaggedFishList.Add(info);
				}
			}
			
		}
		
    }
}
