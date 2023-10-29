using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FWM
{
    public class SendSelectionStatus : MonoBehaviour, ISelectHandler
    {
		private GameObject parentMenu;
		
		private void Awake()
		{
			parentMenu = transform.parent.gameObject;
		}
		
		public void OnSelect(BaseEventData eventData)
		{
			parentMenu.SendMessage("SwitchCameraOnHighlight", this.gameObject.name);
		}
		
    }
}
