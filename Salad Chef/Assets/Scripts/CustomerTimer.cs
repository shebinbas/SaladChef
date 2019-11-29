using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerTimer : MonoBehaviour
{
    
    private float timeLeft;
    private float timeMax = 15f;
    public Slider slider;
    public GameEvent OnOverCustomerTime;

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
            Reset();
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

    public void Reset()
    {
        timeMax = 15f;
        timeLeft = timeMax;
    }
}
