using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class EyeLook : MonoBehaviour
    {
		public FishBehaviour parentScript;
		
		void Update()
		{
			
			if(parentScript.fishSnagged)
			{
				transform.localRotation = Quaternion.Euler(-90,0,0);
			}
			else
			{
				if(parentScript.seesHook && !parentScript.attentionGrabbed)
				{
					transform.rotation = Quaternion.LookRotation( (parentScript.hook.transform.position - transform.position).normalized, Vector3.up);
				}
				else
				{
					transform.rotation = Quaternion.LookRotation(parentScript.lookDir, Vector3.up);
				}
			}
		}
		
    }
}
