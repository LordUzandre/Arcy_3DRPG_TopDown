using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Arcy.Quests
{
	public class QuestUiInGame : MonoBehaviour
	{
		/// <summary>
		/// This class is attached to the ui-object and listens for any changes from the questmanager.
		/// </summary>

		[Header("Quest Manager")]
		[SerializeField] private QuestManager _questManager;

		[Header("Text-elements")]
		[SerializeField] private TMP_Text _questTMP;
		[SerializeField] private TMP_Text _questUpdatedTMP;
		[SerializeField] private TMP_Text _pressJToLearnMoreTMP;

		[Header("Quest objectives(?)")]
		[SerializeField] private QuestObjectiveBase _previousQuestStep;
		[SerializeField] private QuestObjectiveBase _currentQuestStep;

		[Header("Parent GameObject")]
		[SerializeField] private GameObject _parentObject;

		private void Start()
		{
			_questManager.QuestUpdated();
		}

		private void OnEnable()
		{
			QuestManager.questObjectiveCompleted += NewQuestAdded;
			QuestManager.questObjectiveCompleted += QuestUpdated;
			QuestManager.questFinished += QuestFinished;
		}

		private void OnDisable()
		{
			QuestManager.questObjectiveCompleted -= NewQuestAdded;
			QuestManager.questObjectiveCompleted -= QuestUpdated;
			QuestManager.questFinished -= QuestFinished;
		}

		private void UiRollIn()
		{
			// Activate the gameobject and roll in ui
		}

		private void UiRollOut()
		{
			StartCoroutine(myRoutine());

			IEnumerator myRoutine()
			{
				yield return new WaitForSeconds(2f);
				// Roll out Ui
				yield return null;
				//deactivate the gameobject
			}
		}

		private void NewQuestAdded(QuestObject quest)
		{
			_questUpdatedTMP.text = "New Quest";
			_questTMP.text = quest.name;
		}

		private void QuestUpdated(QuestObject quest)
		{
			_questUpdatedTMP.text = "Quest Updated";
			_questTMP.text = quest.name;
		}

		private void QuestFinished(QuestObject quest)
		{
			_questUpdatedTMP.text = "Quest Finished";
			_questTMP.text = quest.name;
		}
	}
}
