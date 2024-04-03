using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcy.Quests
{
	public enum QuestStateEnum
	{
		REQUIREMENTS_NOT_MET,
		CAN_START,
		IN_PROGRESS,
		CAN_FINISH,
		FINISHED
	}
}
