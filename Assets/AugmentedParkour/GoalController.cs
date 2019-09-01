using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : CheckPointController
{
    [SerializeField] GameLogic gameLogic;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameLogic.OnPlayerGoaled();
        }
    }
}
