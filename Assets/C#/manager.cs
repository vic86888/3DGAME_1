using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering; // 必須引用這個命名空間
public class SkyboxLoader : MonoBehaviour
{
    public Material skyboxMaterial; // 要使用的天空盒材質

    void OnEnable()
    {
        // 訂閱場景加載事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 取消訂閱場景加載事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 設置天空盒
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;

            // 更新環境光照
            RenderSettings.ambientMode = AmbientMode.Skybox; // 使用天空盒提供的光源
            DynamicGI.UpdateEnvironment(); // 更新全局光照數據
        }
    }
}
