namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	public interface IFeedbackProvider 
	{
		void PlayCorrect();
		void PlayIncorrect();
		void PlayEnd();
	}
}