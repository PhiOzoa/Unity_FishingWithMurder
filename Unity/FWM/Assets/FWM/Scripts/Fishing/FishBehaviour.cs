using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class FishBehaviour : MonoBehaviour
    {
		public GameObject hook; // fish needs to be able to notice hook
		public HookControl hookScript;
		public Rigidbody rb;
		public Collider col;
		
		private Vector3 startPos; // fish's position at spawn, centre of spheroid area it will wander within
		private Vector3 targetPos; // fish's destination
		
		[Header("Wandering Constraints")]
		public float wanderRadius = 5f; // how far from startPos the fish can move
		public float arrivedErrorRadius = 0.3f; // how close the fish needs to be to targetPos to select a new targetPos
		public float heightTruncationFactor = 0.5f; // the factor by which the height of the fish's sphere of wandering is smushed down
		
		[Header("Swimming Physics")]
		public float swimSpeed = 1f; // top speed
		public float accelFactor = 0.5f; // factor by which fish approaches top speed
		public float rotFactor = 1.3f; // factor by which fish approaches desired rotation
		
		private bool targetSet = false; // has the fish got a target it wants to wander to
		
		[Header("Hook Detection")]
		public float curiosityRadius = 1.5f; // how far the fish will get to the hook when interested
		public float tooCloseRadius = 0.5f; // how close the fish will get to the hook when interested
		public float getBoredDistance = 5f; // the distance the hook must be from the attentionPos for the fish to get bored
		private Vector3 attentionPos; // where the fish is when its attention is grabbed
		private bool seesHook = false;
		private bool attentionGrabbed = false;
		
		[Header("Catch Mechanics")]
		public int initAttention = 25;
		public int attentionIncrement = 10;
		public int attentionDecrement = 5;
		public int maxAttention = 100;
		public int attentionAmt = 0;
		
		public int countdownBetweenDecrement = 60;
		private int countdownVal = 0;
		private bool desirePerformed = false;
		private bool attentionInitialized = false;
		
		private Vector3 lookDir = Vector3.forward;
		
		private bool initialTug = false;
		
		private bool _isTugging;
		public bool isTugging
		{
			get
			{
				return _isTugging;
			}
			set
			{
				if (_isTugging == false && value == true)
				{
					initialTug = true;
				}
				else
				{
					initialTug = false;
				}
				
				
				_isTugging = value;
			}
		}
		
		
		private void Awake()
		{
			startPos = transform.position;
			attentionPos = startPos;
			
			hook = GameObject.Find("HookController");
			hookScript = hook.GetComponent<HookControl>();
		}
		

		
		private void FixedUpdate()
		{
			DetectInitialTug();
			
			if(seesHook && initialTug && !attentionGrabbed && hookScript.activeFish == null)
			{
				hookScript.activeFish = gameObject;
				attentionPos = transform.position;
				attentionGrabbed = true;
			}
			
			if(!attentionGrabbed)
			{
				Wander();
			}
			else
			{
				targetSet = false;
				
				FollowHook();
				CatchCalculation();
			}
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.gameObject == hook)
			{
				seesHook = true;
			}
			
		}
		
		private void OnTriggerExit(Collider other)
		{
			if(other.gameObject == hook)
			{
				seesHook = false;
			}
		}
		
		private void OnCollisionEnter(Collision col)
		{
			targetSet = false; // choose a new place to swim if you bump into something
		}
		
		private void DetectInitialTug()
		{
			if(hookScript.tugging)
			{
				isTugging = true;
			}
			else
			{
				isTugging = false;
			}
		}
		
		private void Wander()
		{
			if(!targetSet)
			{
				SetTarget();
			}
			
			lookDir = rb.velocity.normalized;
			
			MoveToTarget();
			
			if( (Vector3.Distance(transform.position, targetPos) ) <= arrivedErrorRadius )
			{
				targetSet = false;
			}
		}
		
		private void SetTarget()
		{
			if(!attentionGrabbed) // if wandering
			{
				targetPos = (startPos + ( Random.onUnitSphere * Random.Range(0f, wanderRadius) ) ); // choose random location relative to start position and maximum radius
				
				targetPos.y = ( ( (targetPos.y - startPos.y) * heightTruncationFactor) + startPos.y);
				
				targetSet = true;
			}
			else // if attention is grabbed
			{
				if(Vector3.Distance(hook.transform.position, transform.position) > curiosityRadius) // if far away, move to be closer to hook
				{
					targetPos = hook.transform.position;
				}
				else // if close enough, dont move
				{
					if(Vector3.Distance(hook.transform.position, transform.position) < tooCloseRadius)
					{
						targetPos = (transform.position + ( (transform.position - hook.transform.position).normalized * curiosityRadius) );
					}
					else
					{
						targetPos = transform.position;
					}
				}
			}
		}
		
		private void FollowHook()
		{
			SetTarget();
			
			lookDir = (hook.transform.position - transform.position).normalized;
			
			MoveToTarget();
		}
		
		private void MoveToTarget()
		{
			rb.velocity = Vector3.Lerp(rb.velocity, ( (targetPos - transform.position).normalized * swimSpeed), Time.deltaTime * accelFactor);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * rotFactor);
		}
		
		private void CatchCalculation()
		{
			if(!attentionInitialized)
			{
				attentionAmt = initAttention;
				countdownVal = countdownBetweenDecrement;
				attentionInitialized = true;
			}
			
			TestForDesires();
			
			if(attentionAmt >= maxAttention)
			{
				Catch();
			}
			
			if(Vector3.Distance(hook.transform.position, attentionPos) > getBoredDistance) //if hook goes too far from attentionPos it gets bored
			{
				hookScript.activeFish = null;
				attentionGrabbed = false;
				attentionPos = startPos;
				attentionInitialized = false;
				attentionAmt = 0;
			}
		}
		
		private void TestForDesires()
		{
			if(initialTug && !desirePerformed)
			{
				//desirePerformed = true;
				attentionAmt+= attentionIncrement;
			}
			
			countdownVal--;
			
			if(countdownVal <= 0)
			{
				if(desirePerformed)
				{
					desirePerformed = false;
				}
				
				attentionAmt-= attentionDecrement;
				
				countdownVal = countdownBetweenDecrement;
			}
			
			//Debug.Log(attentionAmt);
		}
		
		private void Catch()
		{
			Debug.Log("caught");
		}
    }
}
