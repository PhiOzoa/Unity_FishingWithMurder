using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class CatchCalc : MonoBehaviour
    {
		
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
			
			
			ControlHook();
			
			GetFish();
		}
		
		private void ControlHook()
		{
			
		}
		
		private void GetFish()
		{
			
		}
		
    }
}
