using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverUI : MonoBehaviour
{
    Quest CurrentOffer;

    public Text AcceptCompleteButtonText;
    public Text TitleText;
    public Text DescriptionText;

    public List<Image> RewardItemIcons = new List<Image>();
    public Text GoldRewardText;

    private void OnEnable()
    {
        if (CurrentOffer == null)
        {
            GetNewQuest();
        }
    }

    void GetNewQuest()
    {
        if (CurrentOffer == null && QuestManager.Instance.CurrentQuest == null)
        {
            CurrentOffer = QuestManager.Instance.Quests.GetQuest();
            CurrentOffer.Reset();
        }

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (QuestManager.Instance.CurrentQuest != null)
        {
            TitleText.text = QuestManager.Instance.CurrentQuest.QuestName;
            DescriptionText.text = QuestManager.Instance.CurrentQuest.QuestText;
            if (QuestManager.Instance.CurrentQuest.IsComplete)
            {
                AcceptCompleteButtonText.text = "Complete Quest!";
            }
            else
            {
                AcceptCompleteButtonText.text = "Abandon Quest";
            }

            UpdateRewards(QuestManager.Instance.CurrentQuest.CompletionReward);
        }
        else
        {
            if (CurrentOffer == null)
            {
                GetNewQuest();
            }

            TitleText.text = CurrentOffer.QuestName;
            DescriptionText.text = CurrentOffer.QuestText;
            AcceptCompleteButtonText.text = "Accept Quest";

            UpdateRewards(CurrentOffer.CompletionReward);
        }
    }

    void UpdateRewards(Reward _reward)
    {
        for (int i = 0; i < RewardItemIcons.Count; i++)
        {
            RewardItemIcons[i].gameObject.SetActive(i < _reward.ItemRewards.Count);
            if (i < _reward.ItemRewards.Count)
            {
                RewardItemIcons[i].sprite = _reward.ItemRewards[i].Image;
            }
        }

        GoldRewardText.text = "G" + _reward.GoldReward.ToString();
    }


    public void AcceptCompleteButtonClicked()
    {
        if (QuestManager.Instance.CurrentQuest != null)
        {
            if (QuestManager.Instance.CurrentQuest.IsComplete)
            {
                //reward player.
            }
            QuestManager.Instance.CurrentQuest = null;
            UIController.Instance.PlayerHUD.UpdateQuestText();
            UpdateDisplay();
        }
        else
        {
            QuestManager.Instance.CurrentQuest = CurrentOffer;
            CurrentOffer = null;
            UIController.Instance.PlayerHUD.UpdateQuestText();
            UpdateDisplay();
        }
    }
}
