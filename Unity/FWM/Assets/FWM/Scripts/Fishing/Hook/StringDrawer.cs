using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class StringDrawer : MonoBehaviour
    {
		public LineRenderer fishingLine;
		
		private Vector3 lineTopPos = Vector3.up;
		
		private void Awake()
		{
			SetPositions();
			DrawLine();
		}
		
		private void Update()
		{
			SetPositions();
			DrawLine();
		}
		
		private void SetPositions()
		{
			lineTopPos = Vector3.Lerp(lineTopPos, transform.position + Vector3.up * 5, 0.02f);
		}
		
		private void DrawLine()
		{
			fishingLine.SetPosition(0, transform.position);
			fishingLine.SetPosition(1, lineTopPos);
		}
    }
}
