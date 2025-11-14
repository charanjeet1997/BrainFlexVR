namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	public interface IDataLogger
	{
		void CreateNewLog(string id);
		void LogTrial(string experimentID,int trialIndex, Card card, TargetCard target, string rule, bool isCorrect, float responseTime);
	}
}