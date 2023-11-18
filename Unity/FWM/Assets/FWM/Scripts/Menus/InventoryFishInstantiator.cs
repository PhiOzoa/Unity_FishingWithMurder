using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FWM
{
    public class InventoryFishInstantiator : MonoBehaviour
    {
		public TMP_Text nameLabel;
		public TMP_Text lengthLabel;
		
		public Image spriteImage;
		public List<Sprite> sprites;
		
		public string fishName = "defaultName";
		public float fishLength = 0f;
		
		private void OnEnable()
		{
			nameLabel.SetText(fishName);
			lengthLabel.SetText(fishLength.ToString("#.0 cm"));
			
			switch(fishName)
			{
				case "Slippery Minnow":
				
					spriteImage.sprite = sprites[0];
					
					break;
				
				case "Cutterback":
				
					spriteImage.sprite = sprites[1];
					
					break;
				
				case "Douglas":
				
					spriteImage.sprite = sprites[2];
					
					break;
				
				case "Funny Fluffer":
				
					spriteImage.sprite = sprites[3];
					
					break;
				
				case "Blue Crease":
				
					spriteImage.sprite = sprites[4];
					
					break;
				
				case "Slimefin":
				
					spriteImage.sprite = sprites[5];
					
					break;
				
				case "Splinterfish":
				
					spriteImage.sprite = sprites[6];
					
					break;
				
				case "Dorieteaus":
				
					spriteImage.sprite = sprites[7];
					
					break;
				
				default:
				
					Debug.Log("Error");
					
					break;
			}
			
			spriteImage.preserveAspect = true;
		}
		
		
		
    }
}
