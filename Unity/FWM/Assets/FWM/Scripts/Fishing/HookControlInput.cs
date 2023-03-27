using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Fishing
{
	public class HookControlInput : MonoBehaviour
	{
		private FishingControls controlInput = null;
		public Vector2 moveInputVec = Vector2.zero;
		
		private void Awake()
		{
			controlInput = new FishingControls();
		}
		
		private void OnEnable()
		{
			controlInput.Enable();
			controlInput.Hook.Movement.performed += OnMovementPerformed;
			controlInput.Hook.Movement.canceled += OnMovementCancelled;
		}
		
		private void OnDisable()
		{
			controlInput.Disable();
			controlInput.Hook.Movement.performed -= OnMovementPerformed;
			controlInput.Hook.Movement.canceled -= OnMovementCancelled;
		}
		
		private void OnMovementPerformed(InputAction.CallbackContext value)
		{
			moveInputVec = value.ReadValue<Vector2>();
		}
		
		private void OnMovementCancelled(InputAction.CallbackContext value)
		{
			moveInputVec = Vector2.zero;
		}
	}
}