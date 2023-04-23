using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class CamMotion : MonoBehaviour
    {
		public GameObject hook;
		
		public float camHeight = 5.0f;
		private Vector3 pos = Vector3.zero;
		private Vector3 targetPos = Vector3.zero;

		public float translateFactor = 0.5f;
		
		private void Awake()
		{
			pos = gameObject.transform.position;
		}
		
		private void FixedUpdate()
		{
			pos = gameObject.transform.position;
			
			FollowHook();
			
			gameObject.transform.position = pos;
		}
		
		private void FollowHook()
		{
			pos.x = hook.transform.position.x;
			pos.z = hook.transform.position.z;
			
			targetPos.y = (hook.transform.position.y + camHeight);
			pos.y = Mathf.Lerp(pos.y, targetPos.y, Time.deltaTime * translateFactor);
		}
    }
}
