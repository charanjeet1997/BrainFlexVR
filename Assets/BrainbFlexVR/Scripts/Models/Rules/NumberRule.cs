namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	public class NumberRule : IRule
	{
		public string Name => "Number";
		public bool Evaluate(Card responseCard, TargetCard targetCard)
			=> responseCard.Number == targetCard.Number;
	}
}