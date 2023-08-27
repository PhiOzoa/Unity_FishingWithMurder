using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class FishBehaviour : MonoBehaviour
    {
		public GameObject hook; // fish needs to be able to notice hook
		private HookControl hookScript;
		public Rigidbody rb;
		public GameObject body;
		public CapsuleCollider col;
		
		private Vector3 startPos; // fish's position at spawn, centre of spheroid area it will WanderBehaviour within
		private Vector3 targetPos; // fish's destination
		
		[Header("Wandering Constraints")]
		public float wanderRadius = 5f; // how far from startPos the fish can move
		public float arrivedErrorRadius = 0.3f; // how close the fish needs to be to targetPos to select a new targetPos
		public float heightTruncationFactor = 0.5f; // the factor by which the height of the fish's sphere of wandering is smushed down
		
		[Header("Swimming Physics")]
		public float swimSpeed = 1f; // top speed
		public float accelFactor = 0.5f; // factor by which fish approaches top speed
		public float rotFactor = 1.3f; // factor by which fish approaches desired rotation
		
		private bool targetSet = false; // has the fish got a target it wants to WanderBehaviour to
		
		[Header("Hook Detection")]
		public float furthestRadius = 1.5f; // max distance of fish to hook when interested
		public float closestRadius = 0.5f; // min distance of fish to hook when interested
		public float belowHookFactor = 0.5f; // vertical distance of fish from hook when interested
		
		public float getBoredDistance = 5f; // the distance of the hook from attentionPos at which the fish gets bored
		private Vector3 attentionPos; // the location from which the getBoredDistance is measured
		private bool seesHook = false; // hook is withing sight trigger
		private bool attentionGrabbed = false; // hook has successfully grabbed fish's attention
		public int countDownAfterLoseInterest = 600; // time in frames it takes after you lose the fish's interest for it to be possible to regain interest
		private int interestCountdownVal = 0;
		
		[Header("Catch Mechanics")]
		public int initAttention = 25; // how much attention the fish starts with
		public int attentionIncrement = 10; // how much attention the fish gains with a tug
		public int attentionDecrement = 5; // how much attention the fish loses every attention decrement countdown
		public int maxAttention = 100; // how much attention you need to reach to get the fish to bite
		public int attentionAmt = 0;
		private bool attentionFilled = false; // has the fish reached max attention
		public bool fishSnagged = false; // has the fish been caught
		
		public int countdownBetweenDecrement = 60; // time between each instance of fish losing attention
		private int decCountdownVal = 0;
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
		
		private Vector3 FlattenedDir() // flatten the direction from the hook to the fish, in the vertical plane
		{
			Vector3 dirFromHookToFish = (transform.position - hook.transform.position).normalized;
			Vector3 flattenedDir = new Vector3(dirFromHookToFish.x, 0f, dirFromHookToFish.z).normalized; // TODO: determine whether I want to make this a single declaration or leave it as is
			
			if(flattenedDir == Vector3.zero) // somehow (direction should never be nothing)
			{
				flattenedDir = Vector3.forward; // why not
			}
			
			return flattenedDir;
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
			
			GetAttention();
			
			Debug.DrawLine(hook.transform.position, attentionPos);
			
			if(!attentionGrabbed)
			{
				WanderBehaviour();
			}
			else
			{
				if(!attentionFilled)
				{
					targetSet = false;
					
					FollowHookBehaviour();
					BiteCalculation();
				}
				else
				{
					BiteBehaviour();
				}
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
			isTugging = hookScript.tugging;
		}
		
		private void GetAttention()
		{
			if(seesHook && initialTug && !attentionGrabbed && hookScript.activeFish == null && interestCountdownVal <= 0)
			{
				hookScript.activeFish = gameObject;
				
				float distanceOfInterest = Mathf.Lerp(closestRadius, furthestRadius, 0.5f); // midpoint between max distance and min distance from hook
				
				attentionPos = (FlattenedDir() * distanceOfInterest) + hook.transform.position;
				
				attentionGrabbed = true;
			}
		}
		
		private void WanderBehaviour()
		{
			if(interestCountdownVal > 0)
			{
				interestCountdownVal --;
			}
			
			if(!targetSet)
			{
				SetTarget(0);
			}
			
			lookDir = rb.velocity.normalized;
			
			MoveToTarget();
			
			if( (Vector3.Distance(transform.position, targetPos) ) <= arrivedErrorRadius )
			{
				targetSet = false;
			}
		}
		
		private void SetTarget(int status)
		{
			switch(status) // 0 for wander, 1 for interested in hook, 2 for biting hook, switch to an enumerator with names later
			{
				case 0:
				
					targetPos = (startPos + ( Random.onUnitSphere * Random.Range(0f, wanderRadius) ) ); // choose random location relative to start position and maximum radius
					targetPos.y = ( ( (targetPos.y - startPos.y) * heightTruncationFactor) + startPos.y);
					targetSet = true;
					break;
					
				case 1: // go to position at a distance from the hook related to the level of interest
					
					Vector3 furthestPosToHook = hook.transform.position + (FlattenedDir() * furthestRadius);
					Vector3 closestPosToHook = hook.transform.position + (FlattenedDir() * closestRadius);
					
					float distanceInterpolant = Mathf.InverseLerp(0f, (float)maxAttention, (float)attentionAmt);
					
					targetPos = Vector3.Lerp(furthestPosToHook, closestPosToHook, distanceInterpolant) + (Vector3.down * belowHookFactor);
					
					break;
					
				case 2:
					
					targetPos = hook.transform.position + (Vector3.down * (body.transform.localScale.y * 0.5f + 0.2f) );
					
					
					break;
			}
		}
		
		private void MoveToTarget()
		{
			Vector3 targetVel = (targetPos - transform.position).normalized * swimSpeed;
			Vector3 velDif = targetVel - rb.velocity;
			Vector3 movement = velDif * accelFactor;
			
			rb.AddForce(movement);
			
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), rotFactor);
		}
		
		private void FollowHookBehaviour()
		{
			SetTarget(1);
			
			lookDir = (hook.transform.position - transform.position).normalized;
			
			MoveToTarget();
		}
		
		private void BiteCalculation()
		{
			if(!attentionInitialized)
			{
				attentionAmt = initAttention;
				decCountdownVal = countdownBetweenDecrement;
				attentionInitialized = true;
			}
			
			if(attentionAmt >= maxAttention)
			{
				attentionFilled = true;
			}
			else
			{
				CalcAttention();
			}
			
			if( (Vector3.Distance(hook.transform.position, attentionPos) > getBoredDistance && Vector3.Distance(transform.position, hook.transform.position) < furthestRadius ) || (attentionAmt <= 0) ) //if (hook goes too far from attentionPos && fish is within curiosity radius) OR attention amt hits zero it gets bored
			{
				LoseInterest();
			}
		}
		
		private void CalcAttention()
		{
			if(Vector3.Distance(hook.transform.position, transform.position) < furthestRadius)
			{
				if(initialTug && seesHook)
				{
					attentionAmt+= attentionIncrement;
				}
				
				if(decCountdownVal > 0)
				{
					decCountdownVal--;
				}
				else
				{
					attentionAmt-= attentionDecrement;
					
					decCountdownVal = countdownBetweenDecrement;
				}
			}
		}
		
		private void LoseInterest()
		{
			hookScript.activeFish = null; // hook is catching no fish
			
			interestCountdownVal = countDownAfterLoseInterest; // fish wont be interested in new catch attempts
			
			attentionPos = startPos; 
			attentionGrabbed = false; // negate all bools
			attentionInitialized = false;
			attentionFilled = false;
			attentionAmt = 0; // reset attention of fish
		}
		
		private void BiteBehaviour() // TODO: fix this lol
		{
			SetTarget(2);
			bool biting = false;
			if(Vector3.Distance(hook.transform.position, transform.position) < (0.1f + (body.transform.localScale.y / 2f) ) )
			{
				biting = true;
			}
			
			lookDir = (hook.transform.position - transform.position).normalized;
			
			MoveToTarget();
			
			if(Vector3.Distance(hook.transform.position, attentionPos) > getBoredDistance)
			{
				LoseInterest();
			}
			
			if(initialTug && biting)
			{
				Snag();
			}
		}
		
		private void Snag()
		{
			Debug.Log("snagged");
			fishSnagged = true;
		}
	}
}
