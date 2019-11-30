using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    public GameEvent OnCustomerTimeOver;
    public ConstantsObject constants;
    public VegetableObjects vegetableObjects;
    public int customerId;
    public Vector3 spawnPoint;
    public Slider timeSilder;
    public GameObject customerPlate;
    public GameObject customerMoodObject;
    public List<GameObject> combinationVegContainer;

    //Private Variables
    private int positiveCombinationIndex = 0;
    private int negativeCombinationIndex = 0;
    private PlayerController lastPlayerDelivered;
    private bool gameOver;
    private bool stopTimer;
    private float timeLeft;
    private float timeMax;

    void OnEnable()
    {
        PlayerController.OnDroppedToCustomerPlate += CustomerFeedBack;
        GameController.OnGameOver += ClearCustomer;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("GenerateCombinationOfVegetables");
    }

    void Update()
    {
        if(!stopTimer)
        {
            timeSilder.value = CalculateSliderValue();
            if (timeLeft <= 0 && !gameOver)
            {
                timeLeft = 0;
                ResetTimer();
            }
            else if (timeLeft > 0 && !gameOver)
            {
                timeLeft -= Time.deltaTime;
            }
        }

    }

    //Return Customer Slider Value
    float CalculateSliderValue()
    {
        return (timeLeft / timeMax);
    }

    public void ResetTimer()
    {
        CustomerMoodSmiley(false);
        if(lastPlayerDelivered == null)
        OnCustomerTimeOver.InvokeEvent();
        Invoke("ResetCustomer", 3f);
        stopTimer = true;
    }

    public void StopTimer()
    {
        stopTimer = true;
    }

    //This Function is Generating Random Wanted Vegetables For Customers
    IEnumerator GenerateCombinationOfVegetables()
    {
        Vector3 spawnTemp = spawnPoint;
        int randomCombinationCount = Random.Range(1, vegetableObjects.vegetables.Count /2);
        timeMax = constants.CustomerTime * randomCombinationCount;
        timeLeft = timeMax;
        stopTimer = false;
        for (int i = 0; i < randomCombinationCount; i++)
        {
            int randomVegetableNumber = Random.Range(0, vegetableObjects.vegetables.Count - 1);
            GameObject generatedPrefab = Instantiate(vegetableObjects.vegetables[randomVegetableNumber], transform);
            generatedPrefab.transform.localPosition = spawnTemp;
            generatedPrefab.transform.localScale = new Vector3(.2f, .2f,0f);
            spawnTemp.y -= 0.7f;
            combinationVegContainer.Add(generatedPrefab);
            yield return null;
        }
    }

    /// <summary>
    /// Function is Used When Player Delivered The Salad That This Function Wil Check The Salad Is Suitable For Corresponding Triggered Customer
    /// </summary>
    /// <param name="playerController">PlayerController That Delivered The Salad</param>
    /// <param name="customerIdFromPlayer">CustomerId That Player Delivered</param>
    void CustomerFeedBack(PlayerController playerController, int customerIdFromPlayer)
    {
        if (customerId == customerIdFromPlayer)
        {
            InstantiateSaladOnPlate();
            if (playerController.finalChoppedCombinationVegs.Count!= combinationVegContainer.Count)
            {
                Debug.Log("False Salad");
                lastPlayerDelivered = playerController;
                CustomerMoodSmiley(false);
                timeLeft -= timeLeft / 2;
            }
            else
            {
                for (int i = 0; i < combinationVegContainer.Count; i++)
                {
                    for (int j = 0; j < combinationVegContainer.Count; j++)
                    {
                        if (playerController.finalChoppedCombinationVegs[i].GetComponent<VegetableController>().vegId == combinationVegContainer[j].GetComponent<VegetableController>().vegId)
                        {
                            positiveCombinationIndex++;
                            if (positiveCombinationIndex == combinationVegContainer.Count)
                            {
                                Debug.Log("Success");
                                playerController.SuccessFullDelivering(constants.DefaultScore * combinationVegContainer.Count);
                                CustomerMoodSmiley(true);
                                Invoke("ResetCustomer", 3f);
                                return;
                            }
                        }
                        else
                        {
                            negativeCombinationIndex++;
                            if(negativeCombinationIndex == combinationVegContainer.Count)
                            {
                                Debug.Log("Failed Delivery");
                                lastPlayerDelivered = playerController;
                                CustomerMoodSmiley(false);
                                timeLeft -= timeLeft / 2;
                                return;
                            }
                        }
                    }
                    negativeCombinationIndex = 0;
                }
            }            
        }
        else
        {
            return;
        }
    }

    //Customer Mood Smiley Corresponding To The Order
    public void CustomerMoodSmiley(bool mood)
    {
        if(mood)
        {
            GameObject generateObject = Instantiate(constants.HappyObject, customerMoodObject.transform);
            Destroy(generateObject, 3f);
        }
        else
        {
            GameObject generateObject = Instantiate(constants.AngryObject, customerMoodObject.transform);
            Destroy(generateObject, 3f);
        }
    }

    //Sample Salad Customer Got
    public void InstantiateSaladOnPlate()
    {
        GameObject generateObject = Instantiate(constants.SaladObject, customerPlate.transform);
        generateObject.transform.localScale = new Vector3(5, 5, 5);
        Destroy(generateObject, 3f);
    }
    public void ResetCustomer()
    { 
        foreach (GameObject item in combinationVegContainer)
        {
            Destroy(item);
        }
        combinationVegContainer.Clear();
        StartCoroutine("GenerateCombinationOfVegetables");
        if (lastPlayerDelivered != null)
        {
            lastPlayerDelivered.FailedDelivering(constants.DefaultPenality * 2);
            lastPlayerDelivered = null;
        }
        else
        {
            return;
        }
    }

    void ClearCustomer()
    {
        gameOver = true;
        foreach (GameObject item in combinationVegContainer)
        {
            Destroy(item);
        }
        combinationVegContainer.Clear();
        StopAllCoroutines();
    }
    void OnDisable()
    {
        PlayerController.OnDroppedToCustomerPlate -= CustomerFeedBack;
        GameController.OnGameOver -= ClearCustomer;
    }
}
