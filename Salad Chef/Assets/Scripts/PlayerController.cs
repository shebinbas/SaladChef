using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using System;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public int PlayerId;
    public enum PlayerState
    {
        Idle,
        PickedVeg,
        ChoppedVeg
    }
    public static PlayerState playerState;
    public static event Action<GameObject, int, int> OnDroppedToCustomerPlate;
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
        if (Input.GetKeyDown(KeyCode.Space) && isTrigger && selectedObject.tag == "Veg")
        {
            PickVegetable();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isTrigger && selectedObject.tag == "Board")
        {
            BoardController boardController = selectedObject.GetComponent<BoardController>();
            InstantiateVegetableFromBoard(boardController.boardId);
        }
        else if(Input.GetKeyDown(KeyCode.E) && isTrigger && selectedObject.tag == "Board")
        {
            BoardController boardController = selectedObject.GetComponent<BoardController>();
            DropToBoard(boardController.boardId);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isTrigger && selectedObject.tag == "Customer")
        {
            CustomerController customerController = selectedObject.GetComponent<CustomerController>();
            DropToBoard(customerController.customerId);
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
            InstantiateVegetableFromTable();
    }

    void InstantiateVegetableFromTable()
    {
        GameObject temp = Instantiate(selectedObject, transform);
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = new Vector3(temp.transform.position.x + 2, temp.transform.position.y, 0);
        pickedVegs.Add(temp);
    }

    void InstantiateVegetableFromBoard(int boardId)
    {
        if (choppedVegs != null && PlayerId != boardId)
        {
            return;
        }
        choppedVegs[0].transform.parent = transform;
        choppedVegs.Clear();
    }       
    public void DropToBoard(int boardId)
    {
        if(choppedVegs.Count > 0 && PlayerId != boardId)
        {
            return;
        }
        GameObject generatePrefab = pickedVegs[0];
        pickedVegs.RemoveAt(0);
        generatePrefab.transform.parent = null;
        generatePrefab.transform.localPosition = new Vector3(-2.59f, -4.83f, 0);
        choppedVegs.Add(generatePrefab);
    }

    void DropToCustomerPlate(int customerId)
    {
        OnDroppedToCustomerPlate(pickedVegs[0], PlayerId, customerId);
    }

}
