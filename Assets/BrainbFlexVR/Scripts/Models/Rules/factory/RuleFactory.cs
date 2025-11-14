using System.Collections.Generic;

namespace BrainFlexVR
{
	using UnityEngine;
	using System;
	using System.Collections;


	public static class RuleFactory
	{
		public static Dictionary<string, IRule> Rules = new()
		{
			{ "COLOR", new ColorRule() },
			{ "SHAPE", new ShapeRule() },
			{ "NUMBER", new NumberRule() }
		};
	}
}