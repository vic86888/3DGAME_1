using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // 公開變數，可從 Unity 編輯器調整
    public GameObject[] platformPrefabs; // 存放多種平台預製體，隨機選擇
    public int sideLength = 5; // 六邊形邊長
    public float platformSize = 20f; // 平台大小
    public float gapSize = 20f; // 間隔
    public float moveInterval = 1f; // 每秒更新一次
    public Transform markerParent; // 標記點父物體
    public float shakeDuration = 1f; // 震動持續時間
    public float dropDuration = 2f; // 掉落持續時間
    public float shakeIntensity = 0.4f; // 震動強度

    private Queue<GameObject> platformQueue = new Queue<GameObject>(); // 追蹤目前的活動平台
    private List<Vector3> hexagonPath = new List<Vector3>(); // 存儲六邊形的移動路徑
    private int currentIndex = 0; // 當前運動方向索引

    void Start()
    {
        if (platformPrefabs.Length == 0)
        {
            Debug.LogError("請指定平台預製體！");
            return;
        }

        GenerateHexagonPath();
        StartCoroutine(MovePlatforms());
    }

    // 生成六邊形路徑標記點
    void GenerateHexagonPath()
    {
        float step = platformSize + gapSize;
        Vector3 startPos = transform.position;
        Vector3 currentPos = startPos;

        Vector3[] directions = {
            new Vector3(1, 0, 0),
            new Vector3(0.5f, 0, Mathf.Sqrt(3) / 2),
            new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2),
            new Vector3(-1, 0, 0),
            new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2),
            new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 2)
        };

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < sideLength; j++)
            {
                hexagonPath.Add(currentPos);
                currentPos += directions[i] * step;
            }
        }
    }

    // 控制平台移動
    IEnumerator MovePlatforms()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnPlatform(hexagonPath[i]);
        }
        currentIndex = 5;

        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            if (platformQueue.Count > 0)
            {
                GameObject oldPlatform = platformQueue.Dequeue();
                StartCoroutine(ShakeAndDrop(oldPlatform));
            }

            SpawnPlatform(hexagonPath[currentIndex]);

            currentIndex = (currentIndex + 1) % hexagonPath.Count;
        }
    }

    // 生成隨機平台
    void SpawnPlatform(Vector3 position)
    {
        int randomIndex = Random.Range(0, platformPrefabs.Length);
        GameObject newPlatform = Instantiate(platformPrefabs[randomIndex], position, Quaternion.identity);
        platformQueue.Enqueue(newPlatform);
    }

    // 震動 + 掉落 + 銷毀
    IEnumerator ShakeAndDrop(GameObject platform)
    {
        Vector3 originalPos = platform.transform.position;

        // 震動
        float timer = 0;
        while (timer < shakeDuration)
        {
            platform.transform.position = originalPos + (Vector3.right * Random.Range(-shakeIntensity, shakeIntensity)) +
                                            (Vector3.forward * Random.Range(-shakeIntensity, shakeIntensity));
            timer += Time.deltaTime;
            yield return null;
        }

        platform.transform.position = originalPos; // 確保回到原位置

        // 掉落
        timer = 0;
        while (timer < dropDuration)
        {
            platform.transform.position += Vector3.down * (platformSize / dropDuration) * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(platform);
    }

    // 重置平台生成
    public void Restart()
    {
        // 清除現有平台
        while (platformQueue.Count > 0)
        {
            GameObject oldPlatform = platformQueue.Dequeue();
            if (oldPlatform != null) Destroy(oldPlatform);
        }

        // 重新產生六邊形路徑
        hexagonPath.Clear();
        GenerateHexagonPath();

        // 重新開始平台移動
        StopAllCoroutines();
        currentIndex = 0;

        // 初始產生 5 個平台
        for (int i = 0; i < 5; i++)
        {
            SpawnPlatform(hexagonPath[i]);
        }
        currentIndex = 5;

        StartCoroutine(MovePlatforms());
    }

}