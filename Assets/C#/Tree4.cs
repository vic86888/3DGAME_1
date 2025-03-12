using System.Runtime.InteropServices;
using UnityEngine;

public class Tree4 : MonoBehaviour
{
    public float rotationSpeed = 0f; // 旋轉速度
    public float floatAmplitude = 4.5f; // 浮動幅度
    public float floatFrequency = 1f; // 浮動頻率
    public int A = 140;
    private Vector3 startPos;
    public GameObject targetObject; // 要碰撞的目標物件
    private MeshCollider meshCollider;


    void Start()
    {
        // 記錄初始位置
        startPos = transform.position;
        // 獲取 Mesh Collider
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        // 持續旋轉
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // 上下浮動
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 觸碰時觸發
            ChangeMaterialAlpha(A);

            // 檢查 Mesh Collider 是否存在
            if (meshCollider != null)
            {
                Debug.Log("碰撞發生，禁用 Mesh Collider！");
                meshCollider.enabled = false; // 關閉 Mesh Collider
            }
        }
    }

    private void ChangeMaterialAlpha(float newAlpha)
    {
        // 獲取 Renderer 組件
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // 修改材質的顏色
            Color newColor = renderer.material.color;
            newColor.a = newAlpha / 255f; // 將 Alpha 值轉換為 0-1 的範圍
            renderer.material.color = newColor;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // 嘗試獲取碰撞對象的 Renderer 組件
        Renderer renderer = other.GetComponent<Renderer>();

        // 檢查是否找到了 Renderer
        if (renderer != null && renderer.material != null)
        {
            // 獲取材質的主顏色
            Color color = renderer.material.color;

            // 修改 Alpha 值為 90/255 （因為 Alpha 值通常是 0-1 範圍）
            color.a = 90f / 255f;

            // 將修改後的顏色設定回材質
            renderer.material.color = color;
        }
    }
}