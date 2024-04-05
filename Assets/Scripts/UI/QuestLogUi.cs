using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arcy.Quests;
using UnityEngine.UI;
using TMPro;
using Arcy.Management;
using UnityEngine.EventSystems;

namespace Arcy.UI
{
	public class QuestLogUi : MonoBehaviour
	{
		[Header("Components")]
		[SerializeField] private GameObject _contentParent;
		[SerializeField] private QuestLogScrollingList _scrollingList;
		[SerializeField] private TMP_Text _questDisplayName;
		[SerializeField] private TMP_Text _questStatusText;
		[SerializeField] private TMP_Text _goldRewardsText;
		[SerializeField] private TMP_Text _experienceRewardText;
		[SerializeField] private TMP_Text _levelRequirementText;
		[SerializeField] private TMP_Text _questRequirementText;
		[SerializeField] private Button _firstSelectedBtn;

		private void OnEnable()
		{
			GameEventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
		}

		private void OnDisable()
		{
			GameEventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
		}

		#region FadeUI. TODO - replace with own pause Menu system

		private void QuestLogTogglePressed()
		{
			if (_contentParent.activeInHierarchy)
			{
				HideUI();
			}
			else
			{
				ShowUI();
			}
		}

		private void ShowUI()
		{
			_contentParent.SetActive(true);
			if (_firstSelectedBtn != null)
			{
				_firstSelectedBtn.Select();
			}
		}

		private void HideUI()
		{
			_contentParent.SetActive(false);
			EventSystem.current.SetSelectedGameObject(null);
		}

		#endregion

		#region Quest Log Buttons

		private void QuestStateChange(Quest quest)
		{
			// add the button to the scrolling list if not already added
			QuestLogBtn questLogBtn = _scrollingList.CreateBtnIfNotExists(quest, () => { SetQuestLogInfo(quest); });

			//Initialize the first selected button if not already so that it's always the top button
			if (_firstSelectedBtn == null)
			{
				_firstSelectedBtn = questLogBtn.button;
			}

			// set the button color based on quest state
			questLogBtn.SetState(quest.currentStatusEnum);
		}

		#endregion

		#region Big Ui window

		private void SetQuestLogInfo(Quest quest)
		{
			// quest Name
			_questDisplayName.text = quest.infoSO.displayName;

			// status
			_questStatusText.text = quest.GetFullStatusText();

			// requirements
			// _levelRequirementText.text = "Level " + quest.info.levelRequirement;
			_questRequirementText.text = "";

			foreach (QuestInfoSO prerequisiteQuestInfo in quest.infoSO.questPrerequisites)
			{
				_questRequirementText.text += prerequisiteQuestInfo.displayName + "\n";
			}

			// rewards
			// _goldRewardsText.text = quest.info.goldReward + " Gold";
			// _experienceRewardText.text = quest.info.experienceReward + " XP";
		}

		#endregion
	}
}
