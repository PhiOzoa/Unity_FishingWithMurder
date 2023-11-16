using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class MoveToTarget : MonoBehaviour
    {
		private RectTransform m_RectTransform;
		
		private Vector3 startPos;
		private Vector3 endPos;
		private string fishType;
		private bool ready = false;
		private bool fishSet = false;
		private Animator anim;
		
		private void Awake()
		{
			m_RectTransform = GetComponent<RectTransform>();
			anim = GetComponent<Animator>();
		}
		
		public void SetParams(Vector2 start, Vector2 end, string givenName)
		{
			startPos = start;
			endPos = end;
			fishType = givenName;
			m_RectTransform.anchoredPosition = startPos;
			ready = true;
		}
		
		private void FixedUpdate()
		{
			if(ready)
			{
				if(!fishSet)
				{
					SetFish();
				}
				
				if(anim.GetCurrentAnimatorStateInfo(1).IsName("Idle"))
				{
					m_RectTransform.anchoredPosition = Vector3.MoveTowards(m_RectTransform.anchoredPosition, endPos, 10f);
					
					
					if(Vector3.Distance(m_RectTransform.anchoredPosition, endPos) < 1f)
					{
						anim.SetTrigger("Away");

						Destroy(gameObject, 0.4f);
						
					}
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
