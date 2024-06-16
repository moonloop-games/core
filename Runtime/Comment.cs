using UnityEngine;
using Sirenix.OdinInspector;

namespace Moonloop.Core 
{
	[HideMonoScript]
	public class Comment : MonoBehaviour
	{
		[MultiLineProperty(5), HideLabel]
		public string comments;
	}
}