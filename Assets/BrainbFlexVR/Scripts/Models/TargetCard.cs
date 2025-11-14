using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

namespace BrainFlexVR
{
	using UnityEngine;

	public class TargetCard : MonoBehaviour
	{
		[field: SerializeField] public Color Color { get; private set; }
		[field: SerializeField] public Texture2D Shape { get; private set; }
		[field: SerializeField] public int Number { get; private set; }
		
		[SerializeField] private Renderer colorRenderer;
		[SerializeField] private Renderer shapeRenderer;
		[SerializeField] private TMP_Text numberText;
		

		public void Initialize(Color color, Texture2D shape, int number)
		{
			Color = color;
			Shape = shape;
			Number = number;
			
			colorRenderer.material.color = Color;
			shapeRenderer.material.mainTexture = Shape;
			numberText.text = Number.ToString();
		}

		
	}
}