using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Management
{
	public class InteractionEvents
	{
		public System.Action<Vector3> onMoveInteractionIconHere; // interactionIcon
		public void moveInteractionIconHere(Vector3 pos)
		{
			onMoveInteractionIconHere?.Invoke(pos);
		}

		public System.Action onNoObjectInFocus; //used by interactionIcon, also used in PlayerManager
		public void noObjectInFocus()
		{
			onNoObjectInFocus?.Invoke();
		}

	}
}
