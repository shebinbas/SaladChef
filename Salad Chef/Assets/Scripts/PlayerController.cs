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
        VegPicked,
        Dropped,
        SaladPicked,
        Delivered
    }
    public  PlayerState playerState;
    public static event Action<List<GameObject>, int, int> OnDroppedToCustomerPlate;
    public static event Action OnGameOver;
    public List<GameObject> pickedVegs;
    public List<GameObject> finalChoppedCombinationVegs;
    public List<GameObject> boardVegs;
    public VegetableObjects vegetableObjects;
    public GameObject selectedObject;
    public GameObject saladObject;
    public int playerScore = 0;
    public float timeLeft = 20f;

    Rigidbody2D playerRigidbody;
    Vector2 movement;
    bool isTrigger;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MovePlayer();
        CheckPlayerTimeOutToOver();
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
            DropToBoard(boardController.boardId, selectedObject);
        }
        else if (Input.GetKeyDown(KeyCode.E) && isTrigger && selectedObject.tag == "Customer")
        {
            CustomerController customerController = selectedObject.GetComponent<CustomerController>();
            DropToCustomerPlate(customerController.customerId);
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
        if (pickedVegs.Count == 2 && playerState == PlayerState.SaladPicked)
        {
            return;
        }
        InstantiateVegetableFromTable();
        playerState = PlayerState.VegPicked;
    }

    void InstantiateVegetableFromTable()
    {
        GameObject generatePrefab = Instantiate(selectedObject, transform);
        generatePrefab.GetComponent<Animator>().enabled = false;
        generatePrefab.GetComponent<BoxCollider2D>().enabled = false;
        ResetTransform(generatePrefab);
        pickedVegs.Add(generatePrefab);
    }

    void InstantiateVegetableFromBoard(int boardId)
    {
        if (boardVegs != null && PlayerId != boardId)
        {
            return;
        }
        if(playerState == PlayerState.Dropped)
        {
            GameObject generatePrefab = Instantiate(saladObject, transform);
            ResetTransform(generatePrefab);
            foreach (GameObject item in boardVegs)
            {
                finalChoppedCombinationVegs.Add(item);
            }
            boardVegs.Clear();
            playerState = PlayerState.SaladPicked;
        }
    }       
    public void DropToBoard(int boardId, GameObject board)
    {
        if(PlayerId != boardId)
        {
            return;
        }
        GameObject generatePrefab = pickedVegs[0];
        pickedVegs.RemoveAt(0);
        generatePrefab.transform.parent = board.transform;
        ResetTransform(generatePrefab);
        boardVegs.Add(generatePrefab);
        playerState = PlayerState.Dropped;
    }

    void DropToCustomerPlate(int customerId)
    {
        if(playerState == PlayerState.SaladPicked)
        OnDroppedToCustomerPlate(finalChoppedCombinationVegs, PlayerId, customerId);
    }

    void ResetTransform(GameObject temp)
    {
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = Vector3.zero;
    }

    void CheckPlayerTimeOutToOver()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            //OnGameOver();
        }
    }

    void SuccessFullDelivering()
    {
        playerScore += 10;
        finalChoppedCombinationVegs.Clear();
    }
}
