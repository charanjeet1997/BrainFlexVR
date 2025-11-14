namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	public class AudioFeedback:MonoBehaviour,IFeedbackProvider
	{
		[SerializeField] private AudioClip correctClip;
		[SerializeField] private AudioClip incorrectClip;

		[SerializeField] private AudioSource audioSource;
		

		public void PlayCorrect() => audioSource.PlayOneShot(correctClip);
		public void PlayIncorrect() => audioSource.PlayOneShot(incorrectClip);
		public void PlayEnd()
		{
			Debug.Log("End");
		}
	}
}