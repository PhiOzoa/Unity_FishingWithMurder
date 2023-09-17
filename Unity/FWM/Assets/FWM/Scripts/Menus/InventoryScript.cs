using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FWM
{
    public class InventoryScript : MonoBehaviour
    {
		public GameObject contents;
		public Button backButton;
		private Button initButton;
		
		public ScrollRect scrollRect;
		public RectTransform contentPanel;
		
		public void OnEnable()
		{
			if(contents.transform.childCount > 0)
			{
				initButton = contents.transform.GetChild(0).GetComponent<Button>();
			}
			else
			{
				initButton = backButton;
			}
			
			initButton.Select();
		}
    }
}
