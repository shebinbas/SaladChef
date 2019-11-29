using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public VegetableObjects vegetableObjects;
    public List<GameObject> combinationVegContainer;
    public int customerId;
    public int Index = 0;
    public Vector3 spawnPoint;

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

        int randomCombinationCount = Random.Range(1, vegetableObjects.vegetables.Count - 1);
        for (int i = 0; i < randomCombinationCount; i++)
        {
            int randomVegetableNumber = Random.Range(0, vegetableObjects.vegetables.Count - 1);
            GameObject generatedPrefab = Instantiate(vegetableObjects.vegetables[randomVegetableNumber], transform);
            generatedPrefab.transform.localPosition = spawnPoint;
            generatedPrefab.transform.localScale = new Vector3(.2f, .2f,0f);
            spawnPoint.y -= 0.7f;
            combinationVegContainer.Add(generatedPrefab);
        }
    }

    void CustomerFeedBack(List<GameObject> finalChoppedVegs, int playerId, int customerIdFromPlayer)
    {
        if (customerId == customerIdFromPlayer)
        {
            if(finalChoppedVegs.Count!= combinationVegContainer.Count)
            {
                Debug.Log("False Salad");
            }
            else
            {
                for (int i = 0; i < combinationVegContainer.Count; i++)
                {
                    for (int j = 0; j < combinationVegContainer.Count; j++)
                    {
                        if (finalChoppedVegs[i].GetComponent<VegetableController>().vegId == combinationVegContainer[j].GetComponent<VegetableController>().vegId)
                        {
                            Index++;
                            if (Index == combinationVegContainer.Count)
                            {
                                Debug.Log("Success");
                                //Invoke Reset 3s
                            }
                        }
                    }
                }
            }            
        }
        else
        {
            return;
        }
    }

    public void Reset()
    {
        foreach (GameObject item in combinationVegContainer)
        {
            Destroy(item);
        }
        combinationVegContainer.Clear();
        GenerateCombinationOfVegetables();
    }
    void OnDisable()
    {
        PlayerController.OnDroppedToCustomerPlate -= CustomerFeedBack;
    }
}
