using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class FishBehaviour : MonoBehaviour
    {
		public GameObject hook; // fish needs to be able to notice hook
		public Rigidbody rb;
		public Collider col;
		
		private Vector3 startPos; // fish's position at spawn, centre of spheroid area it will wander within
		private Vector3 targetPos; // fish's destination
		
		public float wanderRadius = 5f; // how far from startPos the fish can move
		public float arrivedErrorRadius = 0.3f; // how close the fish needs to be to targetPos to select a new targetPos
		public float heightTruncationFactor = 0.5f; // the factor by which the height of the fish's sphere of wandering is smushed down
		
		public float swimSpeed = 1f; // top speed
		public float accelFactor = 0.5f; // factor by which fish approaches top speed
		
		private bool wandering = true; // is the fish in its state of having not notice the hook
		private bool targetSet = false; // has the fish got a target it wants to wander to
		
		private void Awake()
		{
			startPos = transform.position;
		}
		
		private void FixedUpdate()
		{
			Debug.DrawLine(startPos, targetPos);
			
			if(wandering)
			{
				Wander();
			}
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
		
		private void OnCollisionEnter(Collision col)
		{
			targetSet = false; // choose a new place to swim if you bump into something
		}
		
		private void SetTarget()
		{
			targetPos = (startPos + ( Random.onUnitSphere * Random.Range(0f, wanderRadius) ) ); // choose random location relative to start position and maximum radius
			targetPos.y = ( (startPos.y + targetPos.y) * heightTruncationFactor ); // truncate hight of randomly generated position by the truncation factor
			targetSet = true;
		}
		
		private void MoveToTarget()
		{
			rb.velocity = Vector3.Lerp(rb.velocity, ( (targetPos - transform.position).normalized * swimSpeed ), Time.deltaTime * accelFactor);
			
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity, Vector3.up), Time.deltaTime);
			
			if( (Vector3.Distance(transform.position, targetPos) ) <= arrivedErrorRadius )
			{
				targetSet = false;
			}
		}
    }
}
