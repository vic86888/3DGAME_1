using UnityEngine;

public class Gem2 : MonoBehaviour
{
    public string gemID; // 每个宝石的唯一 ID

    public float rotationSpeed = 50f; // 旋转速度

    public float floatAmplitude = 0.5f; // 浮动幅度
    public float floatFrequency = 1f; // 浮动频率
    public int alphaOnCollision = 140; // 碰撞后的透明度（0-255）
    public string targetTag = "Player"; // 目标对象的标签

    private Vector3 startPos; // 初始位置
    private Collider gemCollider; // Collider 组件
    private Renderer gemRenderer; // Renderer 组件

    void Start()
    {
        // 记录初始位置
        startPos = transform.position;

        // 获取 Collider 组件
        gemCollider = GetComponent<MeshCollider>();

        // 获取 Renderer 组件
        gemRenderer = GetComponent<Renderer>();

        // 自动生成唯一 ID（如果为空）
        if (string.IsNullOrEmpty(gemID))
        {
            gemID = "Gem_" + gameObject.name + "_" + Random.Range(1000, 9999);
        }

        // 如果宝石已被收集，隐藏
        if (GemManager.Instance.IsGemCollected(gemID))
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        RotateGem();
        FloatGem();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectGem();

        }
    }

    // 旋转宝石
    private void RotateGem()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    // 上下浮动宝石
    private void FloatGem()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 确保碰撞对象是目标
        if (other.CompareTag(targetTag))
        {
            CollectGem();
        }
    }

    // 收集宝石
    public void CollectGem()
    {
        GemManager.Instance.SetGemCollected(gemID);
        ChangeMaterialAlpha(alphaOnCollision);
        DisableCollider();
    }

    // 修改材质透明度
    private void ChangeMaterialAlpha(float newAlpha)
    {
        if (gemRenderer != null)
        {
            Color newColor = gemRenderer.material.color;
            newColor.a = Mathf.Clamp01(newAlpha / 255f);
            gemRenderer.material.color = newColor;
        }
    }

    // 禁用 Collider
    private void DisableCollider()
    {
        if (gemCollider != null)
        {
            gemCollider.enabled = false;
        }
    }
}
