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
		public float curiosityRadius = 1.5f;
		public float tooCloseRadius = 0.5f;
		public float getBoredDistance = 5f;
		private bool seesHook = false;
		private bool attentionGrabbed = false;
		
		private Vector3 lookDir = Vector3.forward;
		
		private void Awake()
		{
			startPos = transform.position;
			
			hook = GameObject.Find("HookController");
			hookScript = hook.GetComponent<HookControl>();
		}
		
		private void FixedUpdate()
		{
			Debug.DrawLine(transform.position, transform.position + (transform.forward * 3f) );
			Debug.DrawLine(startPos, targetPos);
			
			if(seesHook && hookScript.tugging && !attentionGrabbed)
			{
				attentionGrabbed = true;
				Debug.Log("got em");
			}
			
			if(!attentionGrabbed)
			{
				Wander();
			}
			else
			{
				targetSet = false;
				
				
				InteractWithHook();
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
		
		private void Wander()
		{
			
			if(!targetSet)
			{
				SetTarget();
			}
			else
			{
				MoveToTarget();
			}
		}
		
		private void SetTarget()
		{
			targetPos = (startPos + ( Random.onUnitSphere * Random.Range(0f, wanderRadius) ) ); // choose random location relative to start position and maximum radius
			
			targetPos.y = ( ( (targetPos.y - startPos.y) * heightTruncationFactor) + startPos.y);
			
			targetSet = true;
		}
		
		private void MoveToTarget()
		{
			rb.velocity = Vector3.Lerp(rb.velocity, ( (targetPos - transform.position).normalized * swimSpeed ), Time.deltaTime * accelFactor);
			
			lookDir = rb.velocity.normalized;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * rotFactor);
			
			if( (Vector3.Distance(transform.position, targetPos) ) <= arrivedErrorRadius )
			{
				targetSet = false;
			}
		}
		
		private void InteractWithHook()
		{
			if(Vector3.Distance(hook.transform.position, transform.position) > curiosityRadius) // if far away, move to be closer to hook
			{
				targetPos = hook.transform.position;
			}
			else // if close enough, dont move
			{
				if(Vector3.Distance(hook.transform.position, transform.position) < tooCloseRadius)
				{
					targetPos = ( transform.position + ( (transform.position - hook.transform.position).normalized * curiosityRadius) );
				}
				else
				{
					targetPos = transform.position;
				}
			}
			
			
			rb.velocity = Vector3.Lerp(rb.velocity, ( (targetPos - transform.position).normalized * swimSpeed ), Time.deltaTime * accelFactor);
			
			
			lookDir = (hook.transform.position - transform.position).normalized;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir, Vector3.up), Time.deltaTime * rotFactor);
			
			if(Vector3.Distance(hook.transform.position, startPos) > getBoredDistance) //if hook goes too far from its patrol zone it gets bored
			{
				attentionGrabbed = false;
			}
		}
    }
}
