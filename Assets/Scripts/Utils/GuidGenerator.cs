using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Utils
{
	public static class GuidGenerator
	{
		public static int guid()
		{
			int guidInt = Mathf.Abs(System.Guid.NewGuid().GetHashCode());
			return guidInt;
		}
	}
}
