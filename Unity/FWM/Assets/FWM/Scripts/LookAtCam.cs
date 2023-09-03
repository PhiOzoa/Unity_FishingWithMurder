using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class LookAtCam : MonoBehaviour
    {
		public Camera cam;
		
		void Update()
		{
			transform.rotation = Quaternion.LookRotation( (cam.transform.position - transform.position).normalized, Vector3.up);
		}
		
    }
}
