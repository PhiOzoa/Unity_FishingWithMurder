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
		private float randScale;
		
		public FishScriptableObject fishStats;
		
		public void OnEnable()
		{
			SetupScriptFromConfig();
			SetupLookRadiusFromConfig();
			SetupModelFromConfig();
		}
		
		public void SetupScriptFromConfig()
		{
			fb.body = body;
			
			gameObject.name = fishStats.fishName;
			
			fb.wanderRadius = fishStats.wanderRadius;
			fb.arrivedErrorRadius = fishStats.arrivedErrorRadius;
			fb.heightTruncationFactor = fishStats.heightTruncationFactor;
			
			fb.swimSpeed = fishStats.swimSpeed;
			fb.accelFactor = fishStats.accelFactor;
			fb.rotFactor = fishStats.rotFactor;
			
			fb.furthestRadius = fishStats.furthestRadius;
			fb.closestRadius = fishStats.closestRadius;
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
			randScale = Random.Range(fishStats.scaleRange.x, fishStats.scaleRange.y);
			
			body.transform.localScale = new Vector3(randScale, randScale, randScale);
			body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y, body.transform.localPosition.z + ( (1f - randScale)/2f) );
		}
		
    }
}
