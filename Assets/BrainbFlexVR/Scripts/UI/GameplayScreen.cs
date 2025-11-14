using System;
using DataBindingFramework;
using ServiceLocatorFramework;
using TMPro;
using UnityEngine;

namespace BrainFlexVR
{

	public class GameplayScreen : UISystem.Screen
	{
		private IPropertyManager propertyManager;
		private Property<float> maxExperimentDurationProperty;
		private Property<float> experimentTimeProperty;
		
		[SerializeField] private TMP_Text durationText;
		

		private void OnEnable()
		{
			propertyManager = ServiceLocator.Current.Get<IPropertyManager>();
			InitializeProperties();
			BindProperties();
		}

		private void OnDisable()
		{
			DeInitializeProperties();
			UnbindProperties();
		}
        

		public override void Show()
		{
			base.Show();
		}

		public override void Hide()
		{
			base.Hide();
		}
		
		
		private void InitializeProperties()
		{
			experimentTimeProperty = propertyManager.GetOrCreateProperty<float>(PropertyNameConstants.experimentDurationProperty);
		}
		
		private void DeInitializeProperties()
		{
			propertyManager.GetOrCreateProperty<float>(PropertyNameConstants.experimentDurationProperty);
		}
		
		private void BindProperties()
		{
			experimentTimeProperty.Bind(this,OnUpdateTimerUI);
		}

		private void UnbindProperties()
		{
			experimentTimeProperty.Unbind(OnUpdateTimerUI);
		}

		private void OnUpdateTimerUI(float timer)
		{
			int minutes = Mathf.FloorToInt(timer / 60);
			int seconds = Mathf.FloorToInt(timer % 60);
			
			durationText.text = $"{minutes}:{seconds:00}";
		}
		
	}
}