using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoSingleton<QuestManager>
{
    public QuestCollection Quests;
    public Quest CurrentQuest;

    public bool IsComplete()
    {
        for (int i = 0; i < CurrentQuest.Objectives.Count; i++)
        {
            if (!CurrentQuest.Objectives[i].IsComplete)
            {
                return false;
            }
        }
        return true;
    }


    public void AddQuestKill(Monster _killed)
    {
        if (CurrentQuest != null && !CurrentQuest.IsComplete)
        {
            for(int i = 0; i < CurrentQuest.Objectives.Count; i++)
            {
                if (CurrentQuest.Objectives[i].TargetKillCount > 0 
                    && CurrentQuest.Objectives[i].TargetType == _killed.MonsterType)
                {
                    CurrentQuest.Objectives[i].CompleteObjective();
                }
            }
        }
    }


    public void AddExploreRoom(Room _room)
    {
        if (CurrentQuest != null && !CurrentQuest.IsComplete)
        {
            for (int i = 0; i < CurrentQuest.Objectives.Count; i++)
            {
                if (CurrentQuest.Objectives[i].TargetExploreRoomsCount > 0
                    && !CurrentQuest.Objectives[i].ExploredRooms.Contains(_room))
                {
                    if (!CurrentQuest.Objectives[i].IsComplete)
                    {
                        CurrentQuest.Objectives[i].ExploredRooms.Add(_room);
                        CurrentQuest.Objectives[i].CompleteObjective();
                    }
                }
            }
        }
    }
}
