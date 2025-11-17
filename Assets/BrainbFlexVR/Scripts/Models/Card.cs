using System.Collections;
using AnimationSystem;
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
		[SerializeField] private Color deselectedColor;
		[SerializeField] private Color hoverColor;

		[SerializeField] private Color correctColor;
		[SerializeField] private Color incorrectColor;
		[SerializeField] private float colorChangeTime = 0.15f;
		[SerializeField] bool canInteract = false;

		[SerializeField] private FadeAnimation selectionFadeAnimation;

		public void Initialize(Color color, Texture2D shape, int number)
		{
			Color = color;
			Shape = shape;
			Number = number;
			canInteract = true;
			colorRenderer.material.color = Color;
			shapeRenderer.material.mainTexture = Shape;
			numberText.text = Number.ToString();
		}

		public void OnInteract()
		{
			if (canInteract)
			{
				ServiceLocator.Current.Get<ExperimentManager>().Evaluate(this, out bool isTrue);
				canInteract = false;
				// StartCoroutine(UpdateCardColor(isTrue ? correctColor : incorrectColor));
				selectionFadeAnimation.fromColor = isTrue ? correctColor : incorrectColor;
				selectionFadeAnimation.StartAnimate();
				CoroutineExtension.Execute(this, () => { StartCoroutine(UpdateCardColor(deselectedColor)); }, Constants.changeAnswerAfterSec);
				Debug.Log($"Card Interacted {Number}");
			}
		}

		public void DisableCard()
		{
			Debug.Log($"Card Disabled {Number}");
			StartCoroutine(UpdateCardColor(deselectedColor));
			canInteract = false;
		}
		
		
		public void HoverEnter(HoverEnterEventArgs args)
		{
			if (canInteract)
			{
				StartCoroutine(UpdateCardColor(hoverColor));
				Debug.Log($"Hover Entered on Card {transform.name}");
			}
		}
		public void HoverExit(HoverExitEventArgs args)
		{
			if (canInteract)
			{
				StartCoroutine(UpdateCardColor(deselectedColor));
				Debug.Log("Hover Exited on Card");
			}
		}

		IEnumerator UpdateCardColor(Color nextColor)
		{
			float currentTime = 0;
			Color currentColor = colorRenderer.material.color;
			while (currentTime < colorChangeTime)
			{
				currentTime+=Time.deltaTime;
				selectionRenderer.material.color = Color.Lerp(currentColor, nextColor, currentTime / colorChangeTime);
				yield return null;
			}
		}
	}
}