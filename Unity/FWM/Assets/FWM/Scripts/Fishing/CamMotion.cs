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
		
		private Quaternion rot;
		private Quaternion targetRot;
		
		private Vector3 lookDir = Vector3.down;
		
		public float translateFactor = 0.5f;
		
		public float shiftangle = 5f;
		
		private void Awake()
		{
			pos = gameObject.transform.position;
			rot = gameObject.transform.rotation;
		}
		
		private void Update()
		{
			pos = gameObject.transform.position;
			rot = gameObject.transform.rotation;
			
			FollowHook();
			
			LookAtHook();
			
			gameObject.transform.position = pos;
			gameObject.transform.rotation = rot;
		}
		
		private void FollowHook()
		{
			targetPos.y = (hook.transform.position.y + camHeight);
			pos.y = Mathf.Lerp(pos.y, targetPos.y, Time.deltaTime * translateFactor);
		}
		
		private void LookAtHook()
		{
			
		}
    }
}
