using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class StayRotated : MonoBehaviour
    {
		private Quaternion rot;
		
		void Awake()
		{
			rot = transform.rotation;
		}
		
		void Update()
        {
			transform.rotation = rot;
        }
    }
}
