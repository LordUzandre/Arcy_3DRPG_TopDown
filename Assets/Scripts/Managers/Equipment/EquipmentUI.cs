using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Inventory;

namespace Arcy.UI
{
	public class EquipmentUI : MonoBehaviour
	{
		[Header("Left Panel")]
		[SerializeField] private GameObject gmeObject;

		[Header("Right Window")]

		[SerializeField] private MenuBtn _equipmentBtn;
		[SerializeField] private MenuBtn _skillsBtn;

		[SerializeField] private Transform _rightHandParent;
		[SerializeField] private Transform _leftHandParent;
		[SerializeField] private Transform _helmetParent;
		[SerializeField] private Transform _outfitParent;
		[SerializeField] private Transform _accessoryParent;

		[Header("Slots (sets automatically)")]
		[SerializeField] private List<EquipmentSlot> _rightHandSlots;
		[SerializeField] private List<EquipmentSlot> _leftHandSlots;
		[SerializeField] private List<EquipmentSlot> _helmetSlots;
		[SerializeField] private List<EquipmentSlot> _outfitSlots;
		[SerializeField] private List<EquipmentSlot> _accessorySlots;

		private void OnValidate()
		{
			if (_rightHandParent != null && _rightHandSlots.Count == 0)
				MakeList(_rightHandParent, _rightHandSlots);

			if (_leftHandParent != null && _leftHandSlots.Count == 0)
				MakeList(_leftHandParent, _leftHandSlots);

			if (_helmetParent != null && _helmetSlots.Count == 0)
				MakeList(_helmetParent, _helmetSlots);

			if (_outfitParent != null && _outfitSlots.Count == 0)
				MakeList(_outfitParent, _outfitSlots);

			if (_accessoryParent != null && _accessorySlots.Count == 0)
				MakeList(_accessoryParent, _accessorySlots);
		}

		private void MakeList(Transform parent, List<EquipmentSlot> currentList)
		{
			currentList.Clear();

			foreach (Transform childObject in parent)
			{
				childObject.TryGetComponent<EquipmentSlot>(out EquipmentSlot hit);

				if (hit != null)
					currentList.Add(hit);
			}
		}

	}
}
