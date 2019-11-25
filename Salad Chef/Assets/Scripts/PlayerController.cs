using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    Rigidbody2D playerRigidbody;
    Vector2 movement;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MovePlayer();
    }

    //Movement of both players
    void MovePlayer()
    {
        if (gameObject.tag == "Player")
        {
            movement.x = Input.GetAxisRaw("Player1Horizontal");
            movement.y = Input.GetAxisRaw("Player1Vertical");
        }
        else
        {
            movement.x = Input.GetAxisRaw("Player2Horizontal");
            movement.y = Input.GetAxisRaw("Player2Vertical");
        }
    }
    void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + movement * speed * Time.fixedDeltaTime);
    }
}
