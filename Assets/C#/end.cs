using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public string playerTag = "Player"; // 玩家对象的标签
    public Vector3 startPoint  = new Vector3(0, 5, 0); // 初始位置; // 起点位置

    void OnTriggerEnter(Collider other)
    {
        // 检测是否是玩家进入终点
        if (other.CompareTag(playerTag))
        {
            Debug.Log("玩家到达终点！");
            // 将玩家传送回起点
            TeleportToStart(other);
        }
    }

    void TeleportToStart(Collider player)
    {
        // 将玩家传送到起点位置
        if (startPoint != null)
        {
            player.transform.position = startPoint;
            Debug.Log("玩家已被传送回起点！");
        }
        else
        {
            Debug.LogError("起点位置未设置！");
        }
    }
}
