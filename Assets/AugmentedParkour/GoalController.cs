using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : CheckPointController
{
    [SerializeField] public GameLogic gameLogic;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameLogic)
            {
                gameLogic.OnPlayerGoaled();
            }
        }
    }
}
