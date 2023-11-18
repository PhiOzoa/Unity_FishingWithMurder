using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

namespace FWM
{
    public class TrailerCam : MonoBehaviour
    {
        [SerializeField]
		private CinemachineVirtualCamera vcam0; // normal
		[SerializeField]
		private CinemachineVirtualCamera vcam1; // cinematics
		
		public GameObject WellFocus;
		public GameObject ArchFocus;
		public GameObject PanFocus;
		
		int camIndex = 0;
		int focusIndex = 0;
		
		public void SwitchCamAction(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				SwitchCam();
			}
		}
		
		public void SwitchFocusAction(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				SwitchFocus();
			}
		}
		
		public void SwitchCam()
		{
			camIndex++;
			if(camIndex == 2)
			{
				camIndex = 0;
			}
			
			switch(camIndex)
			{
				case 0:
					
					vcam0.Priority = 1;
					vcam1.Priority = 0;
					
					break;
					
				case 1:
					
					vcam0.Priority = 0;
					vcam1.Priority = 1;
					
					break;
			}
			
		}
		
		public void SwitchFocus()
		{
			Debug.Log(focusIndex);
			focusIndex++;
			if(focusIndex == 3)
			{
				focusIndex = 0;
			}
			//Debug.Log(focusIndex);
			
			switch(focusIndex)
			{
				case 0:
					
					vcam1.Follow = WellFocus.transform;
					vcam1.LookAt = WellFocus.transform;
					
					break;
					
				case 1:
				
					vcam1.Follow = ArchFocus.transform;
					vcam1.LookAt = ArchFocus.transform;
					
					break;
					
				case 2:
				
					vcam1.Follow = PanFocus.transform;
					vcam1.LookAt = PanFocus.transform;
					
					break;
			}
		}
    }
}
