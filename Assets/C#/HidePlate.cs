using UnityEngine;

public class PlatformVisibility : MonoBehaviour
{
    private Renderer platformRenderer; // 存儲 Renderer 組件

    void Start()
    {
        // 取得平台的 Renderer 組件
        platformRenderer = GetComponent<Renderer>();

        // 初始隱藏平台
        if (platformRenderer != null)
        {
            platformRenderer.enabled = false; // 關閉外觀顯示
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 檢查碰到的物體是否是玩家（透過標籤判斷）
        if (collision.gameObject.CompareTag("Player"))
        {
            ShowPlatform(); // 顯示平台外觀
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 觸發版本：檢查玩家是否進入平台範圍
        if (other.CompareTag("Player"))
        {
            ShowPlatform(); // 顯示平台外觀
        }
    }

    void ShowPlatform()
    {
        // 顯示平台外觀

        
            platformRenderer.enabled = true;
            Debug.Log("平台外觀已恢復！");
        
    }
}
