using UnityEngine;
using System.Collections;

public class FallingPlatformController : MonoBehaviour
{
    public float startDelay = 2f;    // 第一個平台開始掉落的延遲時間
    public float fallInterval = 1f;  // 每個平台之間的掉落間隔
    public float fallSpeed = 3f;     // 掉落速度
    public float destroyHeight = -5f; // 消失的高度

    private bool shouldFall = false; // 控制何時開始掉落
    private Coroutine fallCoroutine;

    void Start()
    {
        // 啟動掉落的協程，讓這個平台按照順序開始掉落
        fallCoroutine = StartCoroutine(StartFallingWithDelay());
    }

    IEnumerator StartFallingWithDelay()
    {
        // 取得這個物件在 Hierarchy 裡的順序 (Index)，用來計算開始掉落的時間
        int index = transform.GetSiblingIndex();
        float delayTime = startDelay + index * fallInterval;
        
        yield return new WaitForSeconds(delayTime); // 等待指定時間

        shouldFall = true; // 開始掉落
    }

    void Update()
    {
        if (shouldFall)
        {
            // 讓平台每幀向下移動
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // 如果平台低於設定的高度，就刪除它
            if (transform.position.y <= destroyHeight)
            {
                gameObject.SetActive(false);
                shouldFall = false;
            }
        }
    }

    // **新增方法**: 重置應該在 Restart() 時執行
    public void ResetFalling()
    {
        shouldFall = false; // 停止掉落
        if (fallCoroutine != null)
        {
            StopCoroutine(fallCoroutine); // 停止舊的 Coroutine，確保不會有錯誤
        }
        fallCoroutine = StartCoroutine(StartFallingWithDelay()); // 重新開始計時
    }
}
