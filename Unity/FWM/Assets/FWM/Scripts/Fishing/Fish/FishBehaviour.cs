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
		public float curiosityRadius = 1.5f; // how far the fish will get to the hook when interested
		public float tooCloseRadius = 0.5f; // how close the fish will get to the hook when interested
		public float getBoredDistance = 5f; // the distance the hook must be from the attentionPos for the fish to get bored
		private Vector3 attentionPos; // the location from which the fish measures the getbored distance
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
		public bool fishCaught = false; // has the fish been caught
		
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
			
			if(seesHook && initialTug && !attentionGrabbed && hookScript.activeFish == null && interestCountdownVal <= 0)
			{
				hookScript.activeFish = gameObject;
				
				float middleDistance = Vector3.Distance(transform.position, hook.transform.position) - Mathf.Lerp(tooCloseRadius, curiosityRadius, 0.5f); // distance from fish to hook minus the halfway point between the fish's distances of interest
				Vector3 dirFromFishToHook = (hook.transform.position - transform.position).normalized;
				
				attentionPos = (dirFromFishToHook * middleDistance) + transform.position; // set the attention pos as a position in the direction of the hook, within the fish's distance of interest from the hook
				
				attentionGrabbed = true;
			}
			
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
				case 1:
				
					if(Vector3.Distance(hook.transform.position, transform.position) > curiosityRadius) // if far away, move to be closer to hook
					{
						targetPos = hook.transform.position + (Vector3.down * 0.5f);
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
					break;
				case 2:
					
					
					targetPos = hook.transform.position + (Vector3.down * (body.transform.localScale.y * 0.5f + 0.4f) );
					
					
					break;
			}
			
			/* old if then based code, probably remove
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
			*/
		}
		
		private void MoveToTarget()
		{
			rb.velocity = Vector3.Lerp(rb.velocity, ( (targetPos - transform.position).normalized * swimSpeed), Time.deltaTime * accelFactor);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * rotFactor);
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
			
			if( (Vector3.Distance(hook.transform.position, attentionPos) > getBoredDistance && Vector3.Distance(transform.position, hook.transform.position) < curiosityRadius ) || (attentionAmt <= 0) ) //if (hook goes too far from attentionPos && fish is within curiosity radius) OR attention amt hits zero it gets bored
			{
				LoseInterest();
			}
		}
		
		private void CalcAttention()
		{
			if(Vector3.Distance(hook.transform.position, transform.position) < curiosityRadius)
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
		
		private void BiteBehaviour()
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
				Catch();
			}
		}
		
		private void Catch()
		{
			Debug.Log("caught");
			fishCaught = true;
		}
    }
}
