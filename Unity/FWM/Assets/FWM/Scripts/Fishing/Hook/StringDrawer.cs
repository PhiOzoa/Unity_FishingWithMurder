using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class StringDrawer : MonoBehaviour
    {
		public LineRenderer fishingLine;
		public GameObject cam;
		
		private Vector3 hookDir;
		
		private Vector3 lineTopPos = Vector3.forward;
		
		private void Awake()
		{
			
			SetPositions();
			lineTopPos = hookDir;
			DrawLine();
		}
		
		private void FixedUpdate()
		{
			SetPositions();
			DrawLine();
		}
		
		private void SetPositions()
		{
			hookDir = new Vector3(transform.position.x, 0f, transform.position.z).normalized;
			lineTopPos = Vector3.Slerp(lineTopPos - cam.transform.position , hookDir, Time.deltaTime) + cam.transform.position;
		}
		
		private void DrawLine()
		{
			fishingLine.SetPosition(0, transform.position);
			fishingLine.SetPosition(1, lineTopPos);
		}
    }
}
