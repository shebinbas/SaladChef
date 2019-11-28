using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public VegetableObjects vegetableObjects;
    public List<GameObject> combinationVegContainer;
    public int customerId;
    public int Index = 1;

    void OnEnable()
    {
        PlayerController.OnDroppedToCustomerPlate += CustomerFeedBack;
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateCombinationOfVegetables();
    }

    void GenerateCombinationOfVegetables()
    {
        int randomVegetableNumber = Random.Range(0, vegetableObjects.vegetables.Count - 1);
        int temp = Mathf.RoundToInt(vegetableObjects.vegetables.Count / 2);
        int randomCombinationCount = Random.Range(0, temp);
        for (int i = 0; i < randomCombinationCount; i++)
        {
            GameObject generatedPrefab = Instantiate(vegetableObjects.vegetables[i], transform);
            combinationVegContainer.Add(generatedPrefab);
        }
    }

    void CustomerFeedBack(GameObject gameObject, int playerId, int customerIdFromPlayer)
    {
        if (customerId == customerIdFromPlayer)
        {
            foreach (GameObject item in combinationVegContainer)
            {
                if(gameObject.GetComponent<VegetableController>().vegId == item.GetComponent<VegetableController>().vegId)
                {
                    Index++;
                }
            }
            if (Index == combinationVegContainer.Count)
            {
                Debug.Log("Success");
            }
        }
        else
        {
            return;
        }
    }
    void OnDisable()
    {
        PlayerController.OnDroppedToCustomerPlate -= CustomerFeedBack;
    }
}
