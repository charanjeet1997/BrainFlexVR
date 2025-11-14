using System;
using DataBindingFramework;
using ServiceLocatorFramework;
using UISystem;
using UnityEngine;

namespace BrainFlexVR
{

	public class StartScreen : UISystem.Screen
	{
		private DataBindingFramework.IObserver<bool> startExperimentObserver;
		private IObserverManager observerManager;

		private void OnEnable()
		{
			observerManager = ServiceLocator.Current.Get<IObserverManager>();
			startExperimentObserver = observerManager.GetOrCreateObserver<bool>(ObserverNameConstants.StartExperimentObserver);
		}

		private void OnDisable()
		{
			observerManager.RemoveObserver(ObserverNameConstants.StartExperimentObserver);
		}

		public override void Show()
		{
			base.Show();
		}

		public override void Hide()
		{
			base.Hide();
		}

		[ContextMenu("Start Game")]
		public void StartGame()
		{
			startExperimentObserver.Notify(true);
			ViewController.Instance.ChangeScreen(ScreenName.GameplayScreen);
		}
	}
}