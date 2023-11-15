using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
	public class FishInfo
	{
		public string fishName;
		public float fishLength;
		public bool isNew = false;
	}
	
	public class PointInfo
	{
		public Transform pointTrans;
		public bool occupied;
	}
	
}
