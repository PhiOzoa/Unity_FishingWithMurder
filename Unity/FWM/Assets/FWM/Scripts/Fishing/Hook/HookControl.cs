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
		private Transform camTrans;
		
		private bool isControlled = true;
		private bool returning = false;

		public bool leaveInput = false;
		
		private Vector2 inputDir = Vector2.zero;
		private bool raiseInput = false;
		
		private Vector3 v = Vector3.down;
		
		[Range(0.5f, 10.0f)]
		public float lateralMag = 3.0f;
		[Range(0.5f, 3.0f)]
		public float sinkMag = 3.0f;
		[Range(0.5f, 10.0f)]
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
		
		public ParticleSystem bubbleBurst;
		public ParticleSystem ambientBubble;
		
		public List<PointInfo> pointsList;
		
		private void Awake()
		{
			camTrans = Camera.main.transform;
			
			pointsList = new List<PointInfo>();
			
			for(int i = 0; i < transform.childCount; i++)
			{
				Transform curTrans = transform.GetChild(i);
				
				if(curTrans.name == "Point")
				{
					PointInfo info = new PointInfo();
					info.pointTrans = curTrans;
					info.occupied = false;
					
					pointsList.Add(info);
				}
			}
		}
		
		private void FixedUpdate()
		{
			if(isControlled)
			{
				raiseInput = TestRaising(); // raise input == tug is still held after tug effect complete
				
				SetTarget(); // determine target velocity based on whether hook is raising or sinking
				
				Lat(); // accelerate to lateral target velocity
				
				Vert(); // accelerate to vertical target velocity
				
				Rotate(); // rotate the hook to the direction it's moving, (needs improvement)
				
				ControlBubbles();
			}
			else
			{
				ambientBubble.Stop();
				
				if(!returning)
				{
					rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.02f);
				}
				else
				{
					rb.velocity = new Vector3(0f, -sinkMag * fastFallBoost, 0f);
					if(transform.position.y <= -0.5f)
					{
						ambientBubble.Play();
						isControlled = true;
						returning = false;
					}
				}
			}
		}
		
		public void ReadMoveInput(InputAction.CallbackContext context)
		{
			
			if(context.performed)
			{
				Vector2 camFor = new Vector2(camTrans.forward.x, camTrans.forward.z).normalized;
				Vector2 camRight = new Vector2(camTrans.right.x, camTrans.right.z).normalized;

				inputDir = (context.ReadValue<Vector2>().x * camRight) + (context.ReadValue<Vector2>().y * camFor);
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
			if(context.performed && tugCountdown == 0 && isControlled) //tugging only activates on the keypress, not on hold
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
			if(/*!tugging &&*/ tugHeld)
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
				
				RaycastHit hit;
				int layerMask = 1 << 6;
				
				if(Physics.Raycast(transform.position, Vector3.down, out hit, 3f, layerMask) )
				{
					if(hit.transform.CompareTag("Env") && rb.velocity.y <= 0f)
					{
						targetVel.y = 0f;
					}
				}
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
				bubbleBurst.Play();
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
		
		private void Rotate() // bad
		{
			lookDir = (inputDir.magnitude > 0f && rb.velocity.y <= 0f) ? new Vector3(inputDir.x, 0f, inputDir.y) : Vector3.down;
			
			Quaternion rotation = (Quaternion.LookRotation(lookDir, Vector3.up));
			//hookDir.transform.rotation = Quaternion.Lerp(hookDir.transform.rotation, rotation, 0.05f);
			
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.05f);
		}
    
		public void CatchActions()
		{
			isControlled = false;
			rb.velocity = Vector3.up * raiseMag;
		}
		
		public void ReturnActions()
		{
			for(int i = 0; i < pointsList.Count; i++)
			{
				pointsList[i].occupied = false;
			}

			returning = true;
		}
	
		private void ControlBubbles()
		{
			float interpolant = Mathf.InverseLerp(0f, 5f, rb.velocity.magnitude);
			
			var emissionController = ambientBubble.emission;
			var main = ambientBubble.main;
			emissionController.rateOverTime = Mathf.Lerp(0f, 20f, interpolant);
			main.startSpeed = Mathf.Lerp(0.2f, 1f, interpolant);
		}
	}

}
