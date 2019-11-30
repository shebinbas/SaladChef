using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action<PlayerController, int> OnDroppedToCustomerPlate;
    public static event Action<PlayerController> OnTimeOver;
    public static event Action<int, int, float> OnSendPlayerDataToUIManager;
    public  List<GameObject> pickedVegs;
    public List<GameObject> boardVegs;
    public List<GameObject> finalChoppedCombinationVegs;
    public ConstantsObject constants;
    public KeyCode PickKey;
    public KeyCode DropKey;
    public int PlayerId;
    public VegetableObjects vegetableObjects;
    public float speed;
    public int playerScore = 0;
    public enum PlayerState
    {
        VegPicked,
        Dropped,
        SaladPicked,
        Delivered
    }
    public PlayerState playerState;

    //Private Variables
    private bool gameOver;
    private bool playerMove;
    public GameObject selectedObject;
    private GameObject saladObject;
    private float timeLeft;
    private Rigidbody2D playerRigidbody;
    private Vector2 movement;
    private bool isTrigger;

    void Start()
    {
        timeLeft = constants.PlayerTime;
        playerMove = true;
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(playerMove)
        MovePlayer();

        if(!gameOver)
        CheckPlayerTimeOutToOver();

        //Vegetable Picking
        if (Input.GetKeyDown(PickKey) && isTrigger && selectedObject.tag == "Veg")
        {
            PickVegetableFromTable();
        }

        //Picking Final Salad From Cjopping Board
        else if (Input.GetKeyDown(PickKey) && isTrigger && selectedObject.tag == "Board" && playerMove)
        {
            BoardController boardController = selectedObject.GetComponent<BoardController>();
            InstantiateVegetableFromBoard(boardController.boardId);
        }
        
        // Drop To Board
        else if(Input.GetKeyDown(DropKey) && isTrigger && selectedObject.tag == "Board" && playerMove)
        {
            BoardController boardController = selectedObject.GetComponent<BoardController>();
            DropToBoard(boardController.boardId, selectedObject);
        }

        //Drop or Devlivering Salad To Customer
        else if (Input.GetKeyDown(DropKey) && isTrigger && selectedObject.tag == "Customer")
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

    //For Player Movements
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
    
    //Picking Vegetable From Both Side and Add To Player PickegVeg List 
    void PickVegetableFromTable()
    {
        if (pickedVegs.Count == constants.MaximumPickingVegetabels && playerState == PlayerState.SaladPicked)
        {
            return;
        }
        playerState = PlayerState.VegPicked;
        GameObject generatePrefab = Instantiate(selectedObject, transform);
        pickedVegs.Add(generatePrefab);
        generatePrefab.transform.localPosition = new Vector3(0, pickedVegs.Count);
        generatePrefab.transform.localScale = Vector3.one;
        generatePrefab.GetComponent<Animator>().enabled = false;
        generatePrefab.GetComponent<BoxCollider2D>().enabled = false;
    }

    /// <summary>
    /// Picking Vegetable From Corresponding Chopping Board  and Add To finalChoppedCombinationVegs List
    /// </summary>
    /// <param name="boardId">Corresponding Board This Player Triggered</param>
    void InstantiateVegetableFromBoard(int boardId)
    {
        if (boardVegs != null && PlayerId != boardId)
        {
            return;
        }
        if(playerState == PlayerState.Dropped)
        {
            saladObject = Instantiate(constants.SaladObject, transform);
            ResetTransform(saladObject);
            foreach (GameObject item in boardVegs)
            {
                finalChoppedCombinationVegs.Add(item);
                item.SetActive(false);
            }
            boardVegs.Clear();
            playerState = PlayerState.SaladPicked;
        }
    }       

    /// <summary>
    /// Function Is Used For Dropping Player Selected Vegetables To Board
    /// </summary>
    /// <param name="boardId">Triggered Board ID</param>
    /// <param name="board">Corresponding Board GameObject This Player Triggered</param>
    public void DropToBoard(int boardId, GameObject board)
    {
        if(PlayerId != boardId || pickedVegs.Count <= 0)
        {
            return;
        }
        Debug.Log("Board");
        GameObject generatePrefab = pickedVegs[constants.StartIndex];
        pickedVegs.RemoveAt(constants.StartIndex);
        generatePrefab.transform.parent = board.transform;
        ResetTransform(generatePrefab);
        boardVegs.Add(generatePrefab);
        playerState = PlayerState.Dropped;
        board.GetComponent<Animator>().SetTrigger("Cutting");
        StartCoroutine("WaitForChopping");
    }

    //Wait Function For Chopping Vegetables
    IEnumerator WaitForChopping()
    {
        playerMove = false;
        yield return new WaitForSeconds(3f);
        playerMove = true;
    }

    /// <summary>
    /// Drop To Customer Plate Function.Call OnDroppedToCustomerPlate Event Which SubScribe On Selected Customer and Pass Parameter This PlayerScript and CustomerId
    /// </summary>
    /// <param name="customerId"> Player Triggered CustomrId</param>
    void DropToCustomerPlate(int customerId)
    {
        if(playerState == PlayerState.SaladPicked)
        {
            OnDroppedToCustomerPlate(this, customerId);
            Destroy(saladObject);
        }
    }

    void ResetTransform(GameObject temp)
    {
        temp.transform.localScale = Vector3.one;
        temp.transform.localPosition = Vector3.zero;
    }

    //Checking Player Time Is Over and Call An Event OnTineOver
    void CheckPlayerTimeOutToOver()
    {
        timeLeft -= Time.deltaTime;
        OnSendPlayerDataToUIManager(PlayerId, playerScore, timeLeft);
        if (timeLeft <= 0 )
        {
            if(OnTimeOver != null)
            {
                OnTimeOver(this);
                gameOver = true;
                playerMove = false;
                Debug.Log("Game Over");
            }

        }
    }

    //Call When Player Delivered SuccessFull Delivery To Customer
    public void SuccessFullDelivering(int point)
    {
        playerScore += point;
        ClearChoppedVegetables();
    }

    //Call When Player Delivered Failed Delivery To Customer
    public void FailedDelivering(int point)
    {
        playerScore -= point;
        ClearChoppedVegetables();
    }

    public void CustmerPenality()
    {
        playerScore -= constants.DefaultPenality;
    }
    void ClearChoppedVegetables()
    {
        foreach (GameObject item in finalChoppedCombinationVegs)
        {
            Destroy(item);
        }
        finalChoppedCombinationVegs.Clear();
        playerState = PlayerState.Delivered;
    }
}
