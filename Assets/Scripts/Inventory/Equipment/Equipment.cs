using System;
using System.Collections;
using System.Collections.Generic;
using Arcy.Battle;
using UnityEngine;

namespace Arcy.Inventory
{
	public class Equipment : InventoryItemBase
	{
		[SerializeField] BattleCharacterBase wielder;
		[SerializeField] GameObject whereToPutItem;
	}
}
