using System.IO;
using UnityEngine;
using System;

namespace BrainFlexVR
{
    public class DataLogger : MonoBehaviour, IDataLogger, ISavable, ILoadable
    {
        private string filePath;
        private string jsonPath;

        [SerializeField] private DataLogContainer container;

        private void Start()
        {
            Load();

            filePath = Path.Combine(Application.persistentDataPath, "WCST_Data.csv");
            jsonPath = Path.Combine(Application.persistentDataPath, "WCST_Data.json");

            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "Trial,Card,Target,Rule,Result,ResponseTime\n");
        }

        public void CreateNewLog(string id)
        {
            container.CreateNewLog(id);
        }

        public void LogTrial(string experimentID,int trialIndex, Card card, TargetCard target, string rule, bool isCorrect, float responseTime)
        {
            string correctness = (isCorrect ? Constants.Correct : Constants.Incorrect);
            // ✅ Console log for debugging
            Debug.Log($"[Trial Log] Index: {trialIndex}, " +
                      $"Card: {card.Color.HexFromColor()} | {card.Shape.name} | {card.Number}, " +
                      $"Target: {target.name}, Rule: {rule}, " +
                      $"Result: {correctness}, " +
                      $"Response Time: {responseTime:F2}s");

            // ✅ Create and add log entry
            DataLog log = new DataLog(
                trialIndex,
                card.Color.HexFromColor(),
                card.Shape.name,
                card.Number.ToString(),
                correctness,
                responseTime
            );

            container.AddLog(experimentID,log);
        }

        // ✅ Save JSON
        public void Save()
        {
            try
            {
                Data data = new Data();
                data.logs = container.experimentLogs;
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(jsonPath, json);
                Debug.Log($"✅ Data saved to JSON: {jsonPath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"❌ Error saving JSON data: {e.Message}");
            }
        }

        // ✅ Load JSON
        public void Load()
        {
            jsonPath = Path.Combine(Application.persistentDataPath, "WCST_Data.json");

            if (File.Exists(jsonPath))
            {
                try
                {
                    string json = File.ReadAllText(jsonPath);
                    Data data = JsonUtility.FromJson<Data>(json);
                    container.experimentLogs = data.logs;
                    Debug.Log($"✅ Data loaded from JSON: {jsonPath}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"❌ Error loading JSON data: {e.Message}");
                }
            }
            else
            {
                Debug.Log("ℹ️ No previous JSON log found. Creating new container.");
            }
        }
    }
}
