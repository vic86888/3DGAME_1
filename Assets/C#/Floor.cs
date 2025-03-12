using System.Collections;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public string GemID; // 寶石 ID
    public Vector3 initialposition;
    private bool isMoving = false;
    public float moveSpeed = 2f;
    private Vector3 targetPosition; // 當前目標位置
    private Vector3 positionY30;
    private Vector3 positionY50;
    public int min_height = 30;
    public int max_height = 50;

    // Start is called before the first frame update
    void Start()
    {
        initialposition = transform.position; // 初始化位置

        if (string.IsNullOrEmpty(GemID))
        {
            Debug.LogWarning("GemID is not set for this platform.");
            return;
        }

        // 設置 y+30 和 y+50 位置
        positionY30 = new Vector3(initialposition.x, initialposition.y + min_height, initialposition.z);
        positionY50 = new Vector3(initialposition.x, initialposition.y + max_height, initialposition.z);

        // 檢查是否蒐集過寶石，初始化到 y+30
        if (GemManager.Instance.IsGemCollected(GemID))
        {
            transform.position = positionY30;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isMoving)
        {
            StartCoroutine(MovePlatformBetweenY30AndY50());
        }
    }

    private IEnumerator MovePlatformBetweenY30AndY50()
    {
        isMoving = true;

        // 初始目標設置為 y+50
        targetPosition = positionY50;

        while (true) // 持續在 y+30 和 y+50 之間來回
        {
            // 移動到目標位置
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 暫停一段時間
            yield return new WaitForSeconds(0.5f);

            // 切換目標位置
            targetPosition = (targetPosition == positionY50) ? positionY30 : positionY50;
        }
    }
}
