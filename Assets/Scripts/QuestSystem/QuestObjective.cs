using UnityEngine;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class QuestObjective
{
    // kills quests
    public int TargetKillCount;
    public MonsterPrototype.eMonsterID TargetType;

    // explore quests
    public int TargetExploreRoomsCount;
    [HideInInspector]
    public List<Room> ExploredRooms = new List<Room>();

    bool isComplete;
    public bool IsComplete { get { return isComplete; } }

    int ObjectiveCount;

    public bool CompleteObjective()
    {
        ObjectiveCount++;

        //kill quest
        if (TargetKillCount > 0 && ObjectiveCount >= TargetKillCount)
        {
            isComplete = true;
        }

        //explore
        if (TargetExploreRoomsCount > 0 && ObjectiveCount >= TargetExploreRoomsCount)
        {
            isComplete = true;
        }

        UIController.Instance.PlayerHUD.UpdateQuestText();
        
        return false;
    }

    public void Reset()
    {
        ObjectiveCount = 0;
        ExploredRooms.Clear();
        isComplete = false;
    }

    public string GetObjectiveText()
    {
        StringBuilder sb = new StringBuilder();

        if (TargetKillCount > 0)
        {
            sb.Append("Kill " + TargetType.ToString() + " : ");
            if (IsComplete)
                sb.AppendLine("Complete!");
            else
                sb.AppendLine(ObjectiveCount + " / " + TargetKillCount);
        }

        if (TargetExploreRoomsCount > 0)
        {
            sb.Append("Explore Rooms : ");
            if (IsComplete)
                sb.AppendLine("Complete!");
            else
                sb.AppendLine(ExploredRooms.Count + " / " + TargetExploreRoomsCount);
        }

        return sb.ToString();
    }
}