using UnityEngine;

public class GemLightController : MonoBehaviour
{
    public Light[] gemLights; // 寶石燈光陣列
    public string gemID;

    private void Start()
    {
        // 檢查是否蒐集過寶石
        if (GemManager.Instance.IsGemCollected(gemID))
        {
            // 更新所有燈光的強度
            foreach (Light light in gemLights)
            {
                light.intensity = 250f;
            }
        }
        else
        {
            // 設置所有燈光的初始強度
            foreach (Light light in gemLights)
            {
                light.intensity = 0f;
            }
        }
    }
}
