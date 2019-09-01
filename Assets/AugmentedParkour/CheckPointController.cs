using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    [SerializeField] public GameLogic gameLogic;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (gameLogic != null)
            {
                gameLogic.countUpCPoint();
            }
        }
    }
}
