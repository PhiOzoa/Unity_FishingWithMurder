using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FWM
{
    public class InstantiateFish : MonoBehaviour
    {
		public FishBehaviour fb;
		public GameObject body;
		public CapsuleCollider sightCol;
		
		public FishScriptableObject fishStats;
		
		public void OnEnable()
		{
			SetupScriptFromConfig();
			SetupLookRadiusFromConfig();
			SetupModelFromConfig();
		}
		
		public void SetupScriptFromConfig()
		{
			fb.wanderRadius = fishStats.wanderRadius;
			fb.arrivedErrorRadius = fishStats.arrivedErrorRadius;
			fb.heightTruncationFactor = fishStats.heightTruncationFactor;
			
			fb.swimSpeed = fishStats.swimSpeed;
			fb.accelFactor = fishStats.accelFactor;
			fb.rotFactor = fishStats.rotFactor;
			
			fb.curiosityRadius = fishStats.curiosityRadius;
			fb.tooCloseRadius = fishStats.tooCloseRadius;
			fb.getBoredDistance = fishStats.getBoredDistance;
			
			fb.initAttention = fishStats.initAttention;
			fb.attentionIncrement = fishStats.attentionIncrement;
			fb.attentionDecrement = fishStats.attentionDecrement;
			fb.maxAttention = fishStats.maxAttention;
		}
		
		public void SetupLookRadiusFromConfig()
		{
			sightCol.radius = fishStats.sightRadius;
			sightCol.height = fishStats.sightDist;
			sightCol.center = new Vector3(0f, 0f, (fishStats.sightDist / 2) );
		}
		
		public void SetupModelFromConfig()
		{
			body.transform.localScale = fishStats.scale;
		}
		
    }
}
