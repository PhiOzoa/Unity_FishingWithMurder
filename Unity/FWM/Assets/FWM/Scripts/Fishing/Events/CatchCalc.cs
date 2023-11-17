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
		private UIFishingController UIScript;
		public List<FishInfo> snaggedFishList;
		public List<GameObject> snaggedFishObjects;
		public float scaleFactor = 10f;
		
		public GameObject gm = null;
		
		private void Awake()
		{
			snaggedFishObjects = new List<GameObject>();
			UIObject = GameObject.Find("UIController");
			UIScript = UIObject.GetComponent<UIFishingController>();
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
			
			//DeleteFish();
			
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
					snaggedFishObjects.Add(curTrans.gameObject);
					
					FishInfo info = new FishInfo();
					info.fishName = curTrans.name;
					info.fishLength = curTrans.GetChild(0).localScale.y * scaleFactor;
					info.isNew = true;
					
					snaggedFishList.Add(info);
				}
			}
		}
		
		private void DeleteFish()
		{
			foreach (Transform child in hook.transform)
			{
				if(child.tag == "Fish")
				{
					GameObject.Destroy(child.gameObject);
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
				//UIObject.SendMessage("GetFishObjects", snaggedFishObjects);
				Debug.Log(snaggedFishObjects.Count);
				UIScript.caughtFish = snaggedFishObjects;
				//snaggedFishObjects.Clear();
				UIObject.SendMessage("ActivateCatchMenu", true);
			}
			else
			{
				UIObject.SendMessage("ActivateCatchMenu", false);
			}
		}
    }
}
