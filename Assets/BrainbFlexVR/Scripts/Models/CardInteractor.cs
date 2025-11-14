using UnityEngine.InputSystem;

namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	public class CardInteractor : MonoBehaviour
	{
		[SerializeField] private InputActionReference interactAction;

		private void OnEnable()
		{
			interactAction.action.performed += OnInteractPerformed;
		}
		
		private void OnDisable()
		{
			interactAction.action.performed -= OnInteractPerformed;
		}
		
		private void OnInteractPerformed(InputAction.CallbackContext context)
		{
			RaycastHit hit;
			Ray ray = new Ray(transform.position, transform.forward);

			if (Physics.Raycast(ray, out hit, 10f))
			{
				if (hit.transform.TryGetComponent<IInteractable>(out var interactable))
				{
					interactable.OnInteract();
				}
			}
		}
	}
}