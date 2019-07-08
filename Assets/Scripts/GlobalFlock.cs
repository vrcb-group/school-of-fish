using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public GameObject[] fishPrefabs;
    public GameObject fishSchool;

    static int numFish = 100;
    public static int tankSize = 5;
    public static GameObject[] allFish = new GameObject[numFish];
    public static Vector3 goalPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize)
            );
            GameObject fish = Instantiate(fishPrefabs[Random.Range(0, fishPrefabs.Length)], pos, Quaternion.identity);
            fish.transform.parent = fishSchool.transform;
            allFish[i] = fish;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 10000) < 100)
        {
            goalPos = new Vector3(
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize)
            );
        }
    }
}
