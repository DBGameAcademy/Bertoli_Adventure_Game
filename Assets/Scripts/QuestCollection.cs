using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Collection", menuName = "Create New Quest Collection")]
public class QuestCollection : ScriptableObject
{
    public List<Quest> Quests = new List<Quest>();

    public Quest GetQuest()
    {
        return Quests[Random.Range(0, Quests.Count)];
    }
}