using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 單例模式確保只有一個 GameManager
    public Vector3 respawnPoint;  // 記錄存檔點
    public int playerScore = 0;   // 其他需要存儲的數據

    private void Awake()
    {
        // 確保 GameManager 只存在一個實例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 不摧毀物件
        }
        else
        {
            Destroy(gameObject); // 如果已有 GameManager，則摧毀新創建的
        }
    }

    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }
}
