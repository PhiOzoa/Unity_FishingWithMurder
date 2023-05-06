using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class OverworldCam : MonoBehaviour
    {
		public GameObject player;
		public Vector3 translateOffset = new Vector3(0,9,-9);
		private Vector3 targetPos;
		public float rotateOffset = 45f;
		
		private void FixedUpdate()
		{
			targetPos = player.transform.position + translateOffset;
			transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3f);
		}
    }
}
