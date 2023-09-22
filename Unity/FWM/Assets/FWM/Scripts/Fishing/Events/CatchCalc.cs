using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FWM
{
    public class CatchCalc : MonoBehaviour
    {
		public GameObject hook;
		private GameObject UIObject;
		public List<FishInfo> snaggedFishList;
		public float scaleFactor = 10f;
		
		public GameObject gm = null;
		
		private void Awake()
		{
			UIObject = GameObject.Find("UIController");
			gm = GameObject.Find("GameManager");
		}
		
		private void OnTriggerEnter(Collider col)
		{
			if(col.gameObject.tag == "Hook")
			{
				Catch();
			}
		}
		
		private void Catch()
		{
			
			
			AnimateHook();
			
			GetFish();
			
			if(gm != null)
			{
				AddNewFishToManager();
			}
			
			ActivateButtons();
		}
		
		private void AnimateHook()
		{
			hook.SendMessage("CatchActions");
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
		
		private void AddNewFishToManager()
		{
			gm.SendMessage("AddToInventory", snaggedFishList);
		}
		
		private void ActivateButtons()
		{
			if(snaggedFishList.Count > 0)
			{
				UIObject.SendMessage("ActivateCatchMenu", true);
			}
			else
			{
				UIObject.SendMessage("ActivateCatchMenu", false);
			}
		}
    }
}
