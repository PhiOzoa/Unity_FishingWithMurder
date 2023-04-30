using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FWM
{
    public class UIFishingController : MonoBehaviour
    {
		public TMP_Text depthDisplay;
		public Image fishAttention;
		public Canvas fishUI;
		public GameObject hook;
		private HookControl hookScript;
		public float depthFactor = 2f;
		private float depthRounded;
		
		private void Awake()
		{
			hookScript = hook.GetComponent<HookControl>();
		}
		
		private void Update()
		{
			depthRounded = -( (UnityEngine.Mathf.Round((hook.transform.position.y * depthFactor) * 10f)) / 10f );
			depthDisplay.text = string.Format("Depth: {0:F1}", depthRounded);//"Depth: " + depthRounded;
			
			if(hookScript.activeFish != null)
			{
				if(fishUI.enabled == false)
				{
					fishUI.enabled = true;
				}
				fishUI.transform.position = hookScript.activeFish.transform.position;
			}
			else
			{
				fishUI.enabled = false;
			}
		}			
		
    }
}
