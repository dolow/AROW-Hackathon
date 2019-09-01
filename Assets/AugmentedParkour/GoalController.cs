using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : CheckPointController
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameLogic)
            {
                gameLogic.OnPlayerGoaled();
            }
            SceneManager.LoadScene("GoalScene");
        }
    }
}


