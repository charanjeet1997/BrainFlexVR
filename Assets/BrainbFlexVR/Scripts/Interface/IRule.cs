namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	public interface IRule 
	{
    	bool Evaluate(Card responseCard, TargetCard targetCard);
        string Name { get; }
	}
}