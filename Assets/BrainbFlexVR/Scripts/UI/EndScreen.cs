using System.IO;
using DataBindingFramework;
using ServiceLocatorFramework;
using TMPro;
using UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlexVR
{
    public class EndScreen : UISystem.Screen
    {
        [Header("Data")]
        [SerializeField] private DataLogContainer container;

        [Header("UI Text")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text totalTrials;
        [SerializeField] private TMP_Text correctTrials;
        [SerializeField] private TMP_Text incorrectTrials;

        [SerializeField] private TMP_Text path;

        public override void Show()
        {
            base.Show();
            if (!container.HasLogs()) return;

            ExperimentLog log = container.GetLatestExperiment();

            int totalCount     = log.GetTotalLogCount();
            int correctCount   = log.GetCorrectLogCount();
            int incorrectCount = log.GetWrongLogCount();
            path.text =  $"Data Note: {Path.Combine(Application.persistentDataPath, "WCST_Data.json")}";
            totalTrials.text     = totalCount.ToString();
            correctTrials.text   = correctCount.ToString();
            incorrectTrials.text = incorrectCount.ToString();
            
        }

        public void CloseButtonClick()
        {
            ViewController.Instance.ChangeScreen(ScreenName.StartScreen);
        }
    }
}
