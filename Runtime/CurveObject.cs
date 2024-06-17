using UnityEngine;
using Sirenix.OdinInspector;

namespace Moonloop.Core
{
	[CreateAssetMenu(menuName = "Moonloop/Curve Object")]
	public class CurveObject : ScriptableObject
	{
		[HideLabel, InlineProperty]
		public AnimationCurve curve;

		[Tooltip("Multiplies the Y axis value when getting the value for a certain X value"), Space]
		public float multiplier = 1;

		[MultiLineProperty(), PropertySpace, HideLabel, Title("Description")]
		public string description;

		public float ValueFor(float xAxisInput) => curve.Evaluate(xAxisInput) * multiplier;
	}
}