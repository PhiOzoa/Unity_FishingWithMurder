using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace FWM
{
    public class DynamicCam : MonoBehaviour
    {
		[SerializeField]
		private CinemachineVirtualCamera vcam0; // gofishing
		[SerializeField]
		private CinemachineVirtualCamera vcam1; // inventory
		[SerializeField]
		private CinemachineVirtualCamera vcam2; // mainmenu

		public void SwitchPriority(int index)
		{
			
			switch(index)
			{
				case 0:
					
					vcam0.Priority = 1;
					vcam1.Priority = 0;
					vcam2.Priority = 0;
					
					break;
					
				case 1:
					
					vcam0.Priority = 0;
					vcam1.Priority = 1;
					vcam2.Priority = 0;
					
					break;
				case 2:
					
					vcam0.Priority = 0;
					vcam1.Priority = 0;
					vcam2.Priority = 1;
					
					break;
			}
			
		}
		
    }
}
