using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FWM
{
    public class HookControl : MonoBehaviour
    {
		public Rigidbody rb;
		public GameObject hookDir;
		public GameObject activeFish = null;

		public bool leaveInput = false;
		
		private Vector2 inputDir = Vector2.zero;
		private bool raiseInput = false;
		
		private Vector3 v = Vector3.down;
		
		[Range(0.5f, 5.0f)]
		public float lateralMag = 3.0f;
		[Range(0.5f, 3.0f)]
		public float sinkMag = 3.0f;
		[Range(0.5f, 3.0f)]
		public float raiseMag = 3.0f;
		[Range(0.1f, 1.0f)]
		public float accelFactorLat = 1.0f;
		[Range(0.1f, 5.0f)]
		public float decelFactorLat = 1.0f;
		[Range(0.05f,2.0f)]
		public float sinkFactor = 1.0f;
		[Range(0.05f,2.0f)]
		public float raiseFactor = 1.0f;
		
		public float sinkRotFactor = 2.0f;
		public float raiseRotFactor = 0.5f;
		
		public float minDepth = 0f;
		public float maxDepth = -30f;
		
		public bool tugging = false;
		public float tugForce = 0.7f;
		public int tugTime = 120;
		private int tugCountdown = 0;
		
		private Vector3 lookDir = Vector3.down;
		private Vector3 targetVel = Vector3.zero;
		
		
		private bool tugHeld = false;
		private bool fastFall = false;
		public float fastFallBoost = 2.0f;
		
		
		private void FixedUpdate()
		{
			raiseInput = TestRaising(); // raise input == tug is still held after tug effect complete
			
			SetTarget(); // determine target velocity based on whether hook is raising or sinking
			
			Lat(); // accelerate to lateral target velocity
			
			Vert(); // accelerate to vertical target velocity
			
			Rotate(); // rotate the hook to the direction it's moving, (needs improvement)
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
			if(context.performed && tugCountdown == 0) //tugging only activates on the keypress, not on hold
			{
				tugging = true;
			}
			
			if(context.performed)
			{
				tugHeld = true;
			}
			
			if(context.canceled)
			{
				tugHeld = false;
			}
		}
		
		public void FastFallInput(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				fastFall = true;
			}
			
			if(context.canceled)
			{
				fastFall = false;
			}
		}
		
		public void LeaveInput(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				leaveInput = true;
			}
		}
		
		private bool TestRaising()
		{
			if(!tugging && tugHeld)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		private void SetTarget()
		{
			if(!raiseInput) // change target velocity depending on if raising or not
			{
				targetVel = new Vector3(inputDir.x * lateralMag, -sinkMag, inputDir.y * lateralMag);
			}
			else
			{
				targetVel = new Vector3(inputDir.x * lateralMag, raiseMag, inputDir.y * lateralMag);
			}
		}
		
		private void Lat()
		{
			Vector2 targetLat = new Vector2(targetVel.x, targetVel.z);
			Vector2 velDif = targetLat - new Vector2(rb.velocity.x, rb.velocity.z);
			float accelRate = (targetLat.magnitude > 0.01f) ? accelFactorLat : decelFactorLat;
			Vector2 movement = velDif * accelRate;
			
			rb.AddForce(new Vector3(movement.x, 0f, movement.y) );
			
		}
		
		private void Vert()
		{			
			bool canTug = (tugging) && (tugCountdown == 0) && (rb.velocity.y <= 0f);
			
			if(canTug) // if tug is pressed, a previous tug is not still in progress, and the hook is not rising
			{
				rb.velocity = new Vector3(rb.velocity.x, tugForce, rb.velocity.z);
				tugCountdown = tugTime;
			}
			else
			{
				if(tugCountdown != 0) // decrement tug countdown until it reaches zero, at which point the player is no longer tugging
				{
					tugCountdown--;
					
					if(tugCountdown == 0)
					{
						tugging = false;
					}
				}
				
				float targetVert = (fastFall && !raiseInput) ? targetVel.y - fastFallBoost : targetVel.y; // if fastfall pressed and not raising, add fastfall boost to target speed
				
				float velDif = targetVert - rb.velocity.y;
				
				float accelRate = (raiseInput) ? raiseFactor : sinkFactor;
				
				float movement  = velDif * accelRate;
				
				rb.AddForce(new Vector3(0f, movement, 0f) );
			}
		}
		
		private void Rotate()
		{
			lookDir = (rb.velocity.magnitude > 0f) ? rb.velocity.normalized : Vector3.down;
			
			Quaternion rotation = Quaternion.LookRotation(lookDir, Vector3.up);
			hookDir.transform.rotation = rotation;
		}
    }
}
