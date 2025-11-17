using System;
using UnityEngine;
using System.Collections.Generic;
using DataBindingFramework;
using ServiceLocatorFramework;
using UISystem;

namespace BrainFlexVR
{
    public class ExperimentManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private CardGenerator cardGenerator;
        private IFeedbackProvider feedbackProvider;
        private IDataLogger dataLogger;

        [SerializeField] private int maxTrials = 10;
        [SerializeField] private float maxExperimentDuration = 300f;
        private int totalQuestions;

        // ORIGINAL rule sequence (unchanged)
        private List<string> ruleSequence = new() { "COLOR", "SHAPE", "NUMBER", "COLOR" };

        // NEW — working shuffled list
        [SerializeField]private List<string> shuffledRules = new();

        // Random generator for shuffle
        private System.Random rng = new System.Random();

        private int ruleIndex = 0;
        private IRule currentRule;

        private int trialIndex = 0;
        private float trialStartTime;
        private float experimentStartTime;

        [SerializeField] private bool experimentRunning = false;
        private float experiementTime;

        private DataBindingFramework.IObserver<bool> startExperimentObserver;
        private IObserverManager observerManager;

        private IPropertyManager propertyManager;
        private Property<float> experimentTimeProperty;
        
        bool paused = false;

        private string experimentID;

        private void OnEnable()
        {
            ServiceLocatorFramework.ServiceLocator.Current.Register(this);
            observerManager = ServiceLocator.Current.Get<IObserverManager>();
            propertyManager = ServiceLocator.Current.Get<IPropertyManager>();
            InitializeObservers();
        }

        private void Start()
        {
            feedbackProvider = GetComponent<IFeedbackProvider>();
            dataLogger = GetComponent<IDataLogger>();
        }

        private void OnDisable()
        {
            DeInitializeObservers();
            ServiceLocatorFramework.ServiceLocator.Current.Unregister<ExperimentManager>();
        }

        private void OnDestroy()
        {
            if (dataLogger is ISavable savable)
            {
                savable.Save();
            }
        }

        private void Update()
        {
            if (experimentRunning && !paused)
            {
                experiementTime -= Time.deltaTime;
                experimentTimeProperty.Value = experiementTime;

                if (experiementTime <= 0)
                {
                    EndExperiment();
                }
            }
        }

        // -------------------------------------------------------------------
        // 🔄 NEW — Rule Shuffle Logic (Minimal Change)
        // -------------------------------------------------------------------

        private void ReshuffleRules()
        {
            shuffledRules.Clear();
            for (int i = 0; i < ruleSequence.Count; i++)
            {
                if (!shuffledRules.Contains(ruleSequence[i]))
                {
                    shuffledRules.Add(ruleSequence[i]);
                }
            }
            
            int n = shuffledRules.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (shuffledRules[k], shuffledRules[n]) = (shuffledRules[n], shuffledRules[k]);
            }

            Debug.Log("Rules reshuffled!");
        }

        // -------------------------------------------------------------------

        public void StartExperiment(bool canStartTrail)
        {
            experimentRunning = canStartTrail;
            totalQuestions = 0;
            trialIndex = 0;
            experiementTime = maxExperimentDuration;

            if (canStartTrail)
            {
                experimentID = Guid.NewGuid().ToString();
                dataLogger.CreateNewLog(experimentID);

                // NEW — Shuffle rule order
                ReshuffleRules();
                ruleIndex = 0;
                
                currentRule = RuleFactory.Rules[shuffledRules[ruleIndex]];

                experimentStartTime = Time.time;
                StartNextTrial();
                return;
            }

            EndExperiment();
        }

        public void StartNextTrial()
        {
            if (!experimentRunning) return;

            trialStartTime = Time.time;
            cardGenerator.SpawnCards();
            totalQuestions++;

            CoroutineExtension.ExecuteAfterFrame(this, cardGenerator.SpawnTargetCard);
        }

        public void Evaluate(Card response, out bool isTrue)
        {

            if (!experimentRunning)
            {
                isTrue = false;
                return;
            }

            paused = true;
            trialIndex++;
            bool isCorrect = currentRule.Evaluate(response, cardGenerator.TargetCard);
            float responseTime = Time.time - trialStartTime;

            if (isCorrect)
            {
                isTrue = true;
                feedbackProvider.PlayCorrect();
            }
            else
            {
                isTrue = false;
                feedbackProvider.PlayIncorrect();
            }

            dataLogger.LogTrial(
                experimentID,
                trialIndex,
                response,
                cardGenerator.TargetCard,
                currentRule.Name,
                isCorrect,
                responseTime
            );
            cardGenerator.DisableOtherCards(response);
            

            // -------------------------------------------------------------
            // 🔄 Rule change logic (MINIMAL PATCH)
            // -------------------------------------------------------------
            if (trialIndex >= maxTrials)
            {
                trialIndex = 0;

                ruleIndex++;

                // If we finished all shuffled rules → reshuffle & restart
                if (ruleIndex >= shuffledRules.Count)
                {
                    ReshuffleRules();
                    ruleIndex = 0;
                }

                currentRule = RuleFactory.Rules[shuffledRules[ruleIndex]];

                Debug.Log($"Rule changed → {currentRule.Name}");
            }

            CoroutineExtension.Execute(this, Constants.changeAnswerAfterSec + 0.1f,cardGenerator.DestroyTargetCard,StartNextTrial,
                () =>
                {
                    paused = false;
                });
        }

        private void InitializeObservers()
        {
            startExperimentObserver = observerManager.GetOrCreateObserver<bool>(ObserverNameConstants.StartExperimentObserver);
            experimentTimeProperty = propertyManager.GetOrCreateProperty<float>(PropertyNameConstants.experimentDurationProperty);

            startExperimentObserver.Bind(this, StartExperiment);
        }

        private void DeInitializeObservers()
        {
            startExperimentObserver.Unbind(StartExperiment);
            propertyManager.RemoveProperty<float>(PropertyNameConstants.experimentDurationProperty);
            observerManager.RemoveObserver(ObserverNameConstants.StartExperimentObserver);
        }

        private void EndExperiment()
        {
            experimentRunning = false;
            feedbackProvider.PlayEnd();

            if (dataLogger is ISavable savable)
            {
                savable.Save();
            }

            UISystem.ViewController.Instance.ChangeScreen(ScreenName.GameOverScreen);

            cardGenerator.DestroyCards();
            cardGenerator.DestroyTargetCard();

            Debug.Log("Experiment complete — 5 minutes have passed.");
        }
    }
}
