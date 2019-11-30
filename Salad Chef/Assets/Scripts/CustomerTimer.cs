using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerTimer : MonoBehaviour
{
    
    private float timeLeft;
    private float timeMax = 3f;
    public Slider slider;
    public GameEvent OnOverCustomerTime;

    void OnEnable()
    {
        GameController.OnGameOver += StopTimer;
    }
    void Start()
    {
        timeLeft = timeMax;
    }
    void Update()
    {
        slider.value = CalculateSliderValue();
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            OnOverCustomerTime.InvokeEvent();
            ResetTimer();
        }
        else if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
    }

    float CalculateSliderValue()
    {
        return (timeLeft / timeMax);
    }

    public void ResetTimer()
    {
        timeMax = 15f;
        timeLeft = timeMax;
    }

    void StopTimer()
    {
        Debug.Log("Stop Time");
    }

    void OnDisable()
    {
        GameController.OnGameOver -= StopTimer;
    }
}
