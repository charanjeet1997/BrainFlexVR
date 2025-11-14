using UnityEngine.XR.Interaction.Toolkit;

namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;

	public interface IXRHoverable
	{
		void HoverEnter(HoverEnterEventArgs args);
		void HoverExit(HoverExitEventArgs args);
	}
}