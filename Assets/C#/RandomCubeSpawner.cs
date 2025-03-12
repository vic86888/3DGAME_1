using UnityEngine;

public class RandomCubeSpawner : MonoBehaviour
{
    public GameObject smallCubePrefab; // 小方塊的預製體
    public int numberOfCubes = 10; // 生成的小方塊數量
    public Vector3 cubeSize = new Vector3(5f, 5f, 5f); // 大方塊範圍

    void Start()
    {
        SpawnCubes();
    }

    void SpawnCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            // 在大方塊範圍內隨機選擇一個位置
            Vector3 randomPosition = new Vector3(
                Random.Range(-cubeSize.x / 2, cubeSize.x / 2),
                Random.Range(-cubeSize.y / 2, cubeSize.y / 2),
                Random.Range(-cubeSize.z / 2, cubeSize.z / 2)
            );

            // 在當前物件（大方塊）內生成小方塊
            GameObject smallCube = Instantiate(smallCubePrefab, transform.position + randomPosition, Quaternion.identity);
            smallCube.transform.parent = transform; // 設定為大方塊的子物件
        }
    }
}
