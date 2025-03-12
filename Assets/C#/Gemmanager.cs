using UnityEngine;
using System.Collections.Generic;

public class GemManager : MonoBehaviour
{
    private static GemManager _instance;
    public static GemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var existingInstance = FindObjectOfType<GemManager>();
                if (existingInstance != null)
                {
                    _instance = existingInstance;
                }
                else
                {
                    GameObject obj = new GameObject(nameof(GemManager));
                    _instance = obj.AddComponent<GemManager>();
                }
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    // 使用 HashSet 代替 Dictionary
    private HashSet<string> collectedGems = new HashSet<string>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 设置宝石为已收集
    public void SetGemCollected(string gemID) => collectedGems.Add(gemID);

    // 检查宝石是否已收集
    public bool IsGemCollected(string gemID) => collectedGems.Contains(gemID);
}
