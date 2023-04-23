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

		public float latTranslateFactor = 1.0f;
		public float vertTranslateFactor = 0.5f;
		
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
			targetPos.x = hook.transform.position.x;
			targetPos.z = hook.transform.position.z;
			targetPos.y = (hook.transform.position.y + camHeight);
			
			pos = Vector3.Lerp(pos, targetPos, Time.deltaTime * latTranslateFactor);
			pos.y = Mathf.Lerp(pos.y, targetPos.y, Time.deltaTime * vertTranslateFactor);
		}
    }
}
