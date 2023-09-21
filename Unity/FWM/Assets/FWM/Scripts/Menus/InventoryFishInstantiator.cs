using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FWM
{
    public class InventoryFishInstantiator : MonoBehaviour
    {
		public TMP_Text nameLabel;
		public TMP_Text lengthLabel;
		
		public string fishName = "defaultName";
		public float fishLength = 0f;
		
		private void OnEnable()
		{
			nameLabel.SetText(fishName);
			lengthLabel.SetText(fishLength.ToString("#.0 cm"));
		}
		
		
		
    }
}
