using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class HookRotator : MonoBehaviour
    {
		public Rigidbody rb;
		
		public float maxAngle = 45.0f;
		
		private void FixedUpdate()
		{
			RotateToDirection();
			
		}
		
		private void RotateToDirection()
		{
			
		}
    }
}
