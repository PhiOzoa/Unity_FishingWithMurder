using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FWM
{
    public class PlayerController : MonoBehaviour
    {
		public Rigidbody rb;
		private Vector3 inputDir = Vector3.zero;
		private Vector3 targetVel = Vector3.zero;
		private Vector3 v = Vector3.zero;
		public float speed = 2f;
		
        void Start()
        {
        
        }
		
        void FixedUpdate()
        {
			v = rb.velocity;
			
			Move();
			
			
			
			rb.velocity = v;
        }
		
		public void ReadMoveInput(InputAction.CallbackContext context)
		{
			if(context.performed)
			{
				inputDir.x = context.ReadValue<Vector2>().x;
				inputDir.z = context.ReadValue<Vector2>().y;
			}
			else
			{
				if(context.canceled)
				{
					inputDir = Vector3.zero;
				}
			}
		}
		
		private void Move()
		{
			if(inputDir != Vector3.zero)
			{
				targetVel = new Vector3(inputDir.x, v.y, inputDir.z) * speed;
			}
			else
			{
				targetVel = new Vector3(0f, v.y, 0f);
			}
			v.x = Mathf.Lerp(v.x, targetVel.x, Time.deltaTime * 5f);
			v.z = Mathf.Lerp(v.z, targetVel.z, Time.deltaTime * 5f);
		}
    }
}
