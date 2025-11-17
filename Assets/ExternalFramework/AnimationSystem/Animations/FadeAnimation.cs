using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimationSystem
{
	public class FadeAnimation : BaseAnimation<Renderer>
	{
		#region PUBLIC_VARS

		public Color fromColor;
		public Color toColor;

		#endregion

		#region PRIVATE_VARS

		#endregion

		#region UNITY_CALLBACKS

		#endregion

		#region PUBLIC_METHODS
		public override void OnAnimationStart()
		{
			base.OnAnimationStart();
			animatableComponent.material.color = fromColor;
		}

		public override void OnAnimationRunning(float percentage)
		{
			base.OnAnimationRunning(percentage);
			animatableComponent.material.color = Color.Lerp(fromColor, toColor, percentage);
		}

		public override void OnAnimationEnd()
		{
			base.OnAnimationEnd();
			animatableComponent.material.color = toColor;
		}
		#endregion

		#region PRIVATE_METHODS

		#endregion
	}
}
