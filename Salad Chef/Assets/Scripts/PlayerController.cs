using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public enum PlayerState
    {
        Idle,
        PickedVeg,
        ChoppedVeg
    }
    public static PlayerState playerState;
    public List<GameObject> pickedVegs;
    public List<GameObject> choppedVegs;
    public VegetableObjects vegetableObjects;
    public GameObject selectedObject;

    Rigidbody2D playerRigidbody;
    Vector2 movement;
    bool isTrigger;

    void Start()
    {
        playerState = PlayerState.Idle;
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MovePlayer();
        if (Input.GetKeyDown(KeyCode.Space) && isTrigger)
        {
            PickVegetable();
        }
    }

    void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + movement * speed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isTrigger = true;
        selectedObject = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isTrigger = false;
        selectedObject = null;
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

    void PickVegetable()
    {
        if (pickedVegs.Count == 2)
        {
            return;
        }
        if (playerState == PlayerState.Idle)
            InstantiateVegetable();
    }

    void InstantiateVegetable()
    {
        GameObject temp = Instantiate(selectedObject, transform);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = new Vector3(temp.transform.position.x + 2, temp.transform.position.y, 0);
        pickedVegs.Add(temp);
    }

}
