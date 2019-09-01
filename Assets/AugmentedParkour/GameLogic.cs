using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public enum State
    {
        NotInitialized = 1,
        Initialized,
        Started,
        Ended
    }

    [SerializeField] public GameObject TimeText;
    [SerializeField] public GameObject ScoreText;

    private Timer timer;
    private int checkPointCount = 0;    //CheckPointCount

    private State currentState = State.NotInitialized;

    public State CurrentState {
        get {
            return currentState;
        }
        set {
            currentState = value;
            if (currentState == State.Started)
            {
                GameStart();
            }
        }
    }

    void Update()
    {
        if (TimeText)
        {
            Text time = TimeText.GetComponent<Text>();
            time.text = "Time: " + timer.CurrentTime.ToString();
        }
        if (ScoreText)
        {
            Text score = ScoreText.GetComponent<Text>();
            score.text = "Score: " + checkPointCount;
        }
    }

    public void GameStart()
    {
        timer = this.gameObject.AddComponent<Timer>();
        timer.CurrentTime = 60.0f;
        timer.countdownEnabled = true;
    }

    public void IncrementState()
    {
        switch (currentState)
        {
            case State.NotInitialized:
                CurrentState = State.Initialized;
                break;
            case State.Initialized:
                CurrentState = State.Started;
                break;
            case State.Started:
                CurrentState = State.Ended;
                break;
            case State.Ended:
                Debug.LogWarning("State has already reached to Ended");
                break;
        }
    }

    public void countUpCPoint()
    {
        checkPointCount++;
        Debug.Log(checkPointCount);
    }

    public void OnPlayerGoaled()
    {
        Debug.Log("goal stub");
    }
}
