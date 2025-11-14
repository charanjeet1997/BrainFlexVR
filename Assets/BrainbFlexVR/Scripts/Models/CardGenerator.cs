using System;

namespace BrainFlexVR
{
	using System.Collections.Generic;
	using UnityEngine;

	public class CardGenerator:MonoBehaviour
	{
		[SerializeField] private Card cardPrefab;
		[SerializeField] private TargetCard targetCardPrefab;
		[SerializeField] private Transform spawnPoint,targetSpawnPoint;
		[SerializeField] private Color[] colorOptions;
		[SerializeField] private Texture2D[] shapeOptions;
		[SerializeField] private Card[] deck;
		[SerializeField] private TargetCard targetCard;
		[SerializeField] private int maxCardInDeck = 4;
		[SerializeField] private float cardSpacing = 0.2f;
		private int seed = 0;
		
		public TargetCard TargetCard => targetCard;
		

		public void SpawnCards()
		{
			List<Color> colorIndices = new List<Color>(colorOptions) ;
			List<Texture2D> shapeIndices = new List<Texture2D>(shapeOptions);
			List<int> numberIndices = new List<int>() { 1, 2, 3, 4 };
			Random.InitState(seed);
			if(deck.Length != maxCardInDeck)
				deck = new Card[maxCardInDeck];
			DestroyCards();
			for (int i = 0; i < maxCardInDeck; i++)
			{
				seed = Guid.NewGuid().GetHashCode();
				Random.InitState(seed);
				Vector3 cardPosition = spawnPoint.position + new Vector3(i * cardSpacing, 0, 0) - new Vector3(((maxCardInDeck - 1) * cardSpacing) / 2, 0, 0);
				Card card = Instantiate(cardPrefab, cardPosition, Quaternion.identity,spawnPoint);
				Random.InitState(seed);
				int targetColorIndex = Random.Range(0, colorIndices.Count);
				int targetShapeIndex = Random.Range(0, shapeIndices.Count);
				int targetNumberIndex = Random.Range(0, numberIndices.Count);
				
				int number = numberIndices[targetNumberIndex];
				Color color = colorIndices[targetColorIndex];
				Texture2D shape = shapeIndices[targetShapeIndex];
				card.Initialize(color, shape, number);
				card.Initialize(color,shape,number);
				deck[i] = card;
				colorIndices.Remove(colorIndices[targetColorIndex]);
				shapeIndices.Remove(shapeIndices[targetShapeIndex]);
				numberIndices.Remove(numberIndices[targetNumberIndex]);
			}	
			
		}
		
		public void SpawnTargetCard()
		{
			if(targetCard!=null)
				Destroy(targetCard.gameObject);
			seed = Guid.NewGuid().GetHashCode();
			Random.InitState(seed);
			Color color = colorOptions[Random.Range(0, colorOptions.Length)];
			Texture2D shape = shapeOptions[Random.Range(0, shapeOptions.Length)];
			int number = deck[Random.Range(0, deck.Length)].Number;
			targetCard = Instantiate(targetCardPrefab, targetSpawnPoint.position, Quaternion.identity,targetSpawnPoint);
			targetCard.Initialize(color, shape, number);
		}

		public void DestroyCards()
		{
			for (int i = 0; i < deck.Length; i++)
			{
				if (deck[i] != null)
					Destroy(deck[i].gameObject);
			}
		}

		public void DestroyTargetCard()
		{
			if (targetCard != null)
			{
				Destroy(targetCard);
			}

			int childCount = targetSpawnPoint.childCount;

			for (int i = 0; i < childCount; i++)
			{
				if (targetSpawnPoint.GetChild(i) != null)
				{
					Destroy(targetSpawnPoint.GetChild(i).gameObject);
				}
			}
		}
		
	}
}