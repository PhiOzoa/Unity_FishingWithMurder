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
		public FishBehaviour fishScript = null;
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
		public float fastFallFactor = 2.0f;
		
		private void Awake()
		{

		}
		
		private void FixedUpdate()
		{
			v = rb.velocity;
			
			if(activeFish != null)
			{
				if(fishScript == null)
				{
					fishScript = activeFish.GetComponent<FishBehaviour>() as FishBehaviour;
				}
			}
			else
			{
				fishScript = null;
			}
			
			RaycastHit hit;
			
			if(!tugging && tugHeld) // this makes the hook rise if you are still holding the tug button after the tug action is complete
			{
				raiseInput = true;
			}
			else
			{
				raiseInput = false;
			}
			
			if(!raiseInput)
			{
				if(Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
				{
					if(hit.transform.tag == "Env")
					{
						targetVel = ( new Vector3(inputDir.x * lateralMag, 0f, inputDir.y*lateralMag) );
					}
					else
					{
						targetVel = ( new Vector3(inputDir.x * lateralMag, -sinkMag, inputDir.y * lateralMag) );
					}
				}
				else
				{
					targetVel = ( new Vector3(inputDir.x * lateralMag, -sinkMag, inputDir.y * lateralMag) );
				}
				
			}
			else
			{
				targetVel = ( new Vector3(0f, raiseMag, 0f) );
			}
			
			
			MoveLaterally();
			
			if( (tugging) && (tugCountdown == 0) && (v.y < 0f) )
			{
				//tugging = false;
				v.y = tugForce;
				tugCountdown = tugTime;
			}
			else
			{	
				if(tugCountdown != 0)
				{
					tugCountdown--;
					
					if(tugCountdown == 0)
					{
						tugging = false;
					}
				}
				
				Vert();
			}
			
			//RotateToDirection();
			
			
			
			//rb.velocity = v;
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
			if(context.performed && tugCountdown == 0)
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
		
		private void MoveLaterally()
		{
			/*
			if( (v.x != targetVel.x) || (v.z != targetVel.z) ) //if already at target, don't change velocity
			{
				if( (targetVel.x != 0f) || (targetVel.z != 0f) ) //if target is not zero use accel, if target is at zero use decel
				{
					//v.x = Mathf.Lerp(v.x, targetVel.x, Time.deltaTime * accelFactorLat);//fix this whole shit
					//v.z = Mathf.Lerp(v.z, targetVel.z, Time.deltaTime * accelFactorLat);
					
					v.x 
				}
				else
				{
					v.x = Mathf.Lerp(v.x, targetVel.x, Time.deltaTime * decelFactorLat);
					v.z = Mathf.Lerp(v.z, targetVel.z, Time.deltaTime * decelFactorLat);
				}
			}*/
			
			//what i want to do: make velocity v = targetvel at a rate of accelfactor
			
			
			Vector2 targetSpeed = inputDir * lateralMag;
			
			Vector2 speedDif = targetSpeed - new Vector2(rb.velocity.x, rb.velocity.z);
			
			float accelRate = (targetSpeed.magnitude > 0.01f) ? accelFactorLat : decelFactorLat;
			
			Vector2 movement = speedDif * accelRate;
			
			rb.AddForce(new Vector3(movement.x, 0f, movement.y) );
			Debug.Log(rb.velocity.magnitude);
			
		}
		
		private void Vert()
		{
			if( v.y != targetVel.y )
			{
				if(!raiseInput)
				{
					if(!fastFall)
					{
						v.y = Mathf.Lerp(v.y, targetVel.y, Time.deltaTime*sinkFactor);
					}
					else
					{
						v.y = Mathf.Lerp(v.y, targetVel.y * fastFallFactor, Time.deltaTime * (sinkFactor * fastFallFactor));
					}
				}
				else
				{
					v.y = Mathf.Lerp(v.y, targetVel.y, Time.deltaTime*raiseFactor);
				}
				
			}
		}
		
		private void RotateToDirection()
		{
			Debug.DrawLine(gameObject.transform.position, (gameObject.transform.position + rb.velocity));
			
			
			
			if(!raiseInput)
			{
				lookDir = Vector3.Lerp(lookDir, new Vector3(v.x, v.y, v.z), Time.deltaTime * sinkRotFactor);
			}
			else
			{
				lookDir = Vector3.Lerp(lookDir, transform.position + Vector3.down, Time.deltaTime * raiseRotFactor);
			}

			
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
