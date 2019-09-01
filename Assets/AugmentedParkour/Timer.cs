using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // TODO: コールバック処理をするGameObject を渡すことも検討する
    public delegate void OnTimeout();

    private double currentTime = 0.0f;
    public bool countdownEnabled = false;
    private OnTimeout onTimeout = null;

    public double CurrentTime
    {
        get
        {
            return currentTime;
        }
        set
        {
            currentTime = value;
        }
    }

    public void SetDelegate(OnTimeout callback)
    {
        onTimeout = callback;
    }

    // Update is called once per frame
    void Update()
    {
        if (!countdownEnabled || currentTime <= 0.0f)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
            if (onTimeout != null) {
                onTimeout();
            }
        }
    }
}
