using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class HookControl : MonoBehaviour
    {
		//constantly move hook down
		//accept input directions
		//move hook in that direction with momentum damping, apply force perhaps?
		//tug input to jostle hook quickly
		//rotate hook to face the direction its moving with a delay
		
		private HookControlInput input;
		
		public Rigidbody rb;
		public GameObject hookDir;
		
		[Range(0.5f, 5.0f)]
		public float targetVelMag = 3.0f;
		[Range(0.1f, 1.0f)]
		public float accelFactor = 1.0f;
		[Range(0.1f, 5.0f)]
		public float decelFactor = 1.0f;
		
		[Range(0.0f, 90.0f)]
		public float maxRotationAngle =45.0f;
		
		private Vector3 lookDir = Vector3.down;
		
		private Vector3 targetVel = Vector3.zero;
		
		
		private void Awake()
		{
			input = gameObject.AddComponent(typeof(HookControlInput)) as HookControlInput;
		}
		
		private void FixedUpdate()
		{
			//Debug.Log(input.moveInputVec);
			
			targetVel = (new Vector3(input.moveInputVec.x, 0f, input.moveInputVec.y)) * targetVelMag;
			MoveLaterally();
			RotateToDirection();
		}
		
		private void MoveLaterally()
		{
			
			if(targetVel != rb.velocity)
			{
				if(targetVel != Vector3.zero) //if speeding up, use accelleration factor, else use deceleration factor
				{
					rb.velocity = Vector3.Lerp(rb.velocity, targetVel, Time.deltaTime * accelFactor);
				}
				else
				{
					rb.velocity = Vector3.Lerp(rb.velocity, targetVel, Time.deltaTime * decelFactor);
				}
			}
			
		}
		
		private void RotateToDirection()
		{
			Debug.DrawLine(gameObject.transform.position, (gameObject.transform.position + rb.velocity));
			
			
			

			lookDir = new Vector3(rb.velocity.x, -1.0f,rb.velocity.z);


			
			Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
			hookDir.transform.rotation = rotation;
			
		}
    }
}
