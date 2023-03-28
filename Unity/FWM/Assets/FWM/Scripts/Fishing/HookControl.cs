using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fishing
{
    public class HookControl : MonoBehaviour
    {
		//constantly move hook down
		//accept input directions
		//move hook in that direction with momentum damping, apply force perhaps?
		//tug input to jostle hook quickly
		//rotate hook to face the direction its moving with a delay
		
		//private HookControlInput input;
		
		private Vector2 inputDir = Vector2.zero;
		
		public Rigidbody rb;
		Vector3 v = Vector3.down;
		public GameObject hookDir;
		
		[Range(0.5f, 5.0f)]
		public float lateralMag = 3.0f;
		[Range(0.5f, 3.0f)]
		public float sinkMag = 3.0f;
		[Range(0.1f, 1.0f)]
		public float accelFactorLat = 1.0f;
		[Range(0.1f, 5.0f)]
		public float decelFactorLat = 1.0f;
		
		public float sinkFactor = 1.0f;
		
		[Range(0.0f, 90.0f)]
		public float maxRotationAngle =45.0f;
		
		private Vector3 lookDir = Vector3.down;
		
		private Vector3 targetVel = Vector3.zero;
		
		
		private void Awake()
		{

		}
		
		private void FixedUpdate()
		{
			v = rb.velocity;
			
			targetVel = ( new Vector3(inputDir.x * lateralMag, -sinkMag, inputDir.y * lateralMag) );
			
			MoveLaterally();
			Sink();
			RotateToDirection();
			
			
			
			rb.velocity = v;
		}
		
		public void ReadMoveInput(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				inputDir = context.ReadValue<Vector2>();
			}
			else
			{
				if(context.canceled)
				{
					inputDir = Vector2.zero;
				}
			}
		}
		
		public void TugInput(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				Tug();
			}
		}
		
		public void RaiseInput()
		{
			
		}
		
		private void MoveLaterally()
		{
			if( (v.x != targetVel.x) || (v.z != targetVel.z) ) //if already at target, don't change velocity
			{
				if( (targetVel.x != 0f) || (targetVel.z != 0f) ) //if target is not zero use accel, if target is at zero use decel
				{
					v.x = Mathf.Lerp(v.x, targetVel.x, Time.deltaTime * accelFactorLat);
					v.z = Mathf.Lerp(v.z, targetVel.z, Time.deltaTime * accelFactorLat);
				}
				else
				{
					v.x = Mathf.Lerp(v.x, targetVel.x, Time.deltaTime * decelFactorLat);
					v.z = Mathf.Lerp(v.z, targetVel.z, Time.deltaTime * decelFactorLat);
				}
			}
		}
		
		private void Sink()
		{
			if( v.y != targetVel.y )
			{
				v.y = Mathf.Lerp(v.y, targetVel.y, Time.deltaTime*sinkFactor);
			}
		}
		
		private void RotateToDirection()
		{
			Debug.DrawLine(gameObject.transform.position, (gameObject.transform.position + rb.velocity));
			
			
			

			lookDir = new Vector3(v.x, v.y, v.z);


			
			Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
			hookDir.transform.rotation = rotation;
			
		}
		
		private void Tug()
		{
			Debug.Log("Tug!");
		}
		
		private void Raise()
		{
			
		}
		
    }
}
