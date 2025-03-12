using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class mashroommanager : MonoBehaviour
{
    public GameObject[] mashrooms;

    // Start is called before the first frame update
    void Start()
    {
        if (mashrooms == null || mashrooms.Length == 0)
        {
            mashrooms = GameObject.FindGameObjectsWithTag("Mushroom"); // 找到所有帶有 "Mushroom" 標籤的物件
        }

        if (mashrooms == null || mashrooms.Length == 0)
        {
            Debug.LogError("沒有找到任何香菇物件！請確保場景中有物件並且標記為 'Mushroom'");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void restart()
{
    foreach (GameObject mashroom in mashrooms)
    {
        mashroom.SetActive(true);
    }
}
}
