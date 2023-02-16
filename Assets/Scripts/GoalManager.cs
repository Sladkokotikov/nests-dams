using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalPlace[] goalWaiters;
    
    public void SavePlayerGoals(IEnumerable<Goal> goals)
    {
        var i = 0;
        foreach (var goal in goals)
        {
            goalWaiters[i].Init(goal);
            i++;
        }
    }
}
