using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class StringDrawer : MonoBehaviour
    {
		public LineRenderer fishingLine;
		public GameObject cam;
		
		private Vector3 hookDir;
		
		private Vector3 lineTopPos = Vector3.forward;
		
		private void FixedUpdate()
		{
			hookDir = new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z).normalized;
			
			lineTopPos = Vector3.Slerp(lineTopPos - cam.transform.position , hookDir, Time.deltaTime) + cam.transform.position;
			
			fishingLine.SetPosition(0, gameObject.transform.position);
			fishingLine.SetPosition(1, lineTopPos);
		}
    }
}
