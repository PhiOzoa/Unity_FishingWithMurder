using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class MoveToTarget : MonoBehaviour
    {
		private Vector3 startPos;
		private Vector3 endPos;
		private string fishType;
		private bool ready = false;
		private bool fishSet = false;
		private Animator anim;
		
		private void Awake()
		{
			anim = GetComponent<Animator>();
		}
		
		public void SetParams(Vector3 start, Vector3 end, string givenName)
		{
			startPos = start;
			endPos = end;
			fishType = givenName;
			transform.position = startPos;
			ready = true;
		}
		
		private void Update()
		{
			if(ready)
			{
				if(!fishSet)
				{
					SetFish();
				}
				
				transform.position = Vector3.Lerp(transform.position, endPos, 0.2f);
				
				if(Vector3.Distance(transform.position, endPos) < 0.2f)
				{
					anim.SetTrigger("Away");
				}
			}
		}
		
		private void SetFish()
		{
			switch(fishType)
			{
				case "Slippery Minnow":
				
					anim.Play("Base Layer.OrangeIcon");
					
					break;
				
				case "Cutterback":
				
					anim.Play("Base Layer.StripeyIcon");
					
					break;
				
				case "Douglas":
				
					anim.Play("Base Layer.DouglasIcon");
					
					break;
				
				case "Funny Fluffer":
				
					anim.Play("Base Layer.ClownIcon");
					
					break;
				
				case "Blue Crease":
				
					anim.Play("Base Layer.BlueboyIcon");
					
					break;
				
				case "Slimefin":
				
					anim.Play("Base Layer.PinkyIcon");
					
					break;
				
				case "Splinterfish":
				
					anim.Play("Base Layer.DryIcon");
					
					break;
				
				case "Dorieteaus":
				
					anim.Play("Base Layer.OrangeIcon");
					
					break;
				
				default:
				
					Debug.Log("Error");
					
					break;
			}
			
			anim.SetTrigger("Appear");
			
			fishSet = true;
		}
		
    }
}
