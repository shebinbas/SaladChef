using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableController : MonoBehaviour
{
    private Animator vegAnimator;
    void Start()
    {
        vegAnimator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            vegAnimator.SetTrigger("HighLight");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            vegAnimator.SetTrigger("Idle");
        }
    }

}
