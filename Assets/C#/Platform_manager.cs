using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject[] Platforms;
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        if (Platforms == null || Platforms.Length == 0)
        {
            Platforms = GameObject.FindGameObjectsWithTag("Platform");
        }

        if (Platforms == null || Platforms.Length == 0)
        {
            Debug.LogError("沒有找到任何平台物件！請確保場景中有物件並且標記為 'Platform'");
        }
        else
        {
            foreach (GameObject platform in Platforms)
            {
                initialPositions[platform] = platform.transform.position;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < Platforms.Length; i++)
        {
            if (!Platforms[i].activeSelf) // 檢查物件是否被關閉
            {
                ResetPlatform(Platforms[i]); // 只重置這個平台
                MoveToEnd(i); // 移動到 Hierarchy 最後面
                break; // 只處理一個，避免一次變動過多
            }
        }
    }

    public void Restart() // 重置所有平台
    {
        foreach (GameObject platform in Platforms)
        {
            if (initialPositions.ContainsKey(platform))
            {
                platform.transform.position = initialPositions[platform]; // 回到初始位置
            }
            platform.SetActive(true); // 重新啟用

            FallingPlatformController fallingScript = platform.GetComponent<FallingPlatformController>();
            if (fallingScript != null)
            {
                fallingScript.ResetFalling();
            }
        }
        Debug.Log("所有平台已重置！");
    }

    private void ResetPlatform(GameObject platform) // 只重置單一關閉的平台
    {
        if (initialPositions.ContainsKey(platform))
        {
            platform.transform.position = initialPositions[platform]; // 回到初始位置
        }
        platform.SetActive(true); // 重新啟用

        FallingPlatformController fallingScript = platform.GetComponent<FallingPlatformController>();
        if (fallingScript != null)
        {
            fallingScript.ResetFalling();
        }

        Debug.Log($"{platform.name} 已重置並啟用！");
    }

    private void MoveToEnd(int index)
    {
        if (index < 0 || index >= Platforms.Length) return;

        GameObject movedPlatform = Platforms[index];

        // **移動到 Hierarchy 最後面**
        movedPlatform.transform.SetSiblingIndex(movedPlatform.transform.parent.childCount - 1);

        // **同步更新陣列順序**
        List<GameObject> platformList = new List<GameObject>(Platforms);
        platformList.RemoveAt(index);
        platformList.Add(movedPlatform);
        Platforms = platformList.ToArray();
    }
}
