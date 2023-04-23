using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class FishBehaviour : MonoBehaviour
    {
		public GameObject hook;
		public Rigidbody rb;
		
		private Vector3 startPos;
		private Vector3 targetPos;
		public float wanderRadius = 5f;
		public float arrivedErrorRadius = 0.05f;
		
		public float swimSpeed = 1f;
		public float accelFactor = 0.2f;
		
		private bool wandering = true;
		private bool targetSet = false;
		
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
		
		private void SetTarget()
		{
			targetPos = (startPos + ( Random.onUnitSphere * Random.Range(0f, wanderRadius) ) );
			Debug.Log(startPos.y + targetPos.y);
			targetPos.y = (startPos.y + targetPos.y) * 0.5f;
			targetSet = true;
		}
		
		private void MoveToTarget()
		{
			rb.velocity = Vector3.Lerp(rb.velocity, ( (targetPos - transform.position).normalized * swimSpeed ), Time.deltaTime * accelFactor);

			if( (Vector3.Distance(transform.position, targetPos) ) <= arrivedErrorRadius )
			{
				targetSet = false;
			}
		}
    }
}
