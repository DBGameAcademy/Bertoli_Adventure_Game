using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWindowUI : MonoBehaviour
{
    public Text HealthText;
    public Image HealthBar;

    public Text QuestText;

    public void UpdateHUD(Player _player)
    {
        HealthText.text = _player.CurrentHealth.ToString() + " / " + _player.MaxHealth.ToString();
        float normalisedHealth = (float)_player.CurrentHealth / (float)_player.MaxHealth;
        HealthBar.fillAmount = normalisedHealth;

        UpdateQuestText();
    }

    public void UpdateQuestText()
    {
        string questText = "No Quest!";

        if (QuestManager.Instance.CurrentQuest)
        {
            questText = QuestManager.Instance.CurrentQuest.QuestName;
            questText += "\n";
            questText += QuestManager.Instance.CurrentQuest.QuestText;
            questText += "\n";
            questText += QuestManager.Instance.CurrentQuest.GetObjectiveText();
        }


        QuestText.text = questText;
    }
}
