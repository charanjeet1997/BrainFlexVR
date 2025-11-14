using System.Collections.Generic;
using JetBrains.Annotations;

namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	[CreateAssetMenu(fileName = "DataLogContainer" , menuName = "Data/DataLogContainer")]
	public class DataLogContainer:ScriptableObject
	{
		public List<ExperimentLog> experimentLogs;

		public void CreateNewLog(string id)
		{
			experimentLogs.Add(new ExperimentLog()
			{
				id = id,
				logs = new List<DataLog>()
			});
		}

		public bool HasLogs()
		{
			return experimentLogs.Count > 0;
		}
		[CanBeNull]
		public ExperimentLog GetLatestExperiment()
		{
			if (experimentLogs.Count == 0)
			{
				return null;
			}
			int expCount = experimentLogs.Count;
			return experimentLogs[expCount - 1];
		}
		
		public void AddLog(string id, DataLog log)
		{
			experimentLogs.Find(x => x.id == id).Add(log);
		}
	}

	[Serializable]
	public class Data
	{
		public List<ExperimentLog> logs;
	}

	[Serializable]
	public class ExperimentLog
	{
		public string id;
		public List<DataLog> logs;

		public int GetTotalLogCount()
		{
			return logs.Count;
		}

		public int GetCorrectLogCount()
		{
			int correctLogCount = 0;
			correctLogCount = logs.FindAll(x => x.isCorrect == Constants.Correct).Count;
			return correctLogCount;
		}

		public int GetWrongLogCount()
		{
			int incorrectLogCount = 0;
			incorrectLogCount = logs.FindAll(x => x.isCorrect == Constants.Incorrect).Count;
			return incorrectLogCount;
		}
		public void Add(DataLog log)
		{
			logs.Add(log);
		}
	}
	[Serializable]
	public class DataLog
	{
		public int id;
		public string colorHex;
		public string shape;
		public string number;
		public string isCorrect;
		public float responseTime;

		public DataLog(int id, string colorHex, string shape, string number,string isCorrect, float responseTime)
		{
			this.id = id;
			this.colorHex = colorHex;
			this.shape = shape;
			this.number = number;
			this.responseTime = responseTime;
			this.isCorrect = isCorrect;
		}
	}
}