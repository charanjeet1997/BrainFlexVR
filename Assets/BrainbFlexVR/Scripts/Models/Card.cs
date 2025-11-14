using ServiceLocatorFramework;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

namespace BrainFlexVR
{
	using UnityEngine;

	public class Card : MonoBehaviour,IInteractable,IXRHoverable
	{
		[field: SerializeField] public Color Color { get; private set; }
		[field: SerializeField] public Texture2D Shape { get; private set; }
		[field: SerializeField] public int Number { get; private set; }
		
		[SerializeField] private Renderer colorRenderer;
		[SerializeField] private Renderer shapeRenderer;
		[SerializeField] private Renderer selectionRenderer;
		[SerializeField] private TMP_Text numberText;
		
		[Header("Selection Colors")]
		[SerializeField] private Color selectedColor;
		[SerializeField] private Color deselectedColor;
		[SerializeField] private Color hoverColor;

		public void Initialize(Color color, Texture2D shape, int number)
		{
			Color = color;
			Shape = shape;
			Number = number;
			
			colorRenderer.material.color = Color;
			shapeRenderer.material.mainTexture = Shape;
			numberText.text = Number.ToString();
		}

		public void OnInteract()
		{
			selectionRenderer.material.color = selectedColor;
			ServiceLocator.Current.Get<ExperimentManager>().Evaluate(this);
			Debug.Log("Card Interacted");
		}
		
		public void HoverEnter(HoverEnterEventArgs args)
		{
			selectionRenderer.material.color = hoverColor;
			Debug.Log($"Hover Entered on Card {transform.name}");
		}
		public void HoverExit(HoverExitEventArgs args)
		{
			selectionRenderer.material.color = deselectedColor;
			Debug.Log("Hover Exited on Card");
		}
	}
}