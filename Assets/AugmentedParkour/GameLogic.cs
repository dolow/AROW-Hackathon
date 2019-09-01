using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public enum State
    {
        NotInitialized = 1,
        Initialized,
        Started,
        Ended
    }

    private State currentState = State.NotInitialized;

    public State CurrentState {
        get {
            return currentState;
        }
        set {
            currentState = value;
        }
    }

    public void IncrementState()
    {
        switch (currentState)
        {
            case State.NotInitialized:
                currentState = State.Initialized;
                break;
            case State.Initialized:
                currentState = State.Started;
                break;
            case State.Started:
                currentState = State.Ended;
                break;
            case State.Ended:
                Debug.LogWarning("State has already reached to Ended");
                break;
        }
    }
}
