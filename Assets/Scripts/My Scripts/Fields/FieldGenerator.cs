using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public int gridWidth = 100; // グリッドの幅
    public GameObject cubePrefab; // キューブのPrefab
    public float cubeSpacing = 1.0f; // キューブ間Xのスペース
    public GameObject leftWallPrefab; // 両端の壁Prefab
    public GameObject rightWallPrefab; // 両端の壁Prefab

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < 1; x++)
        {
            Vector3 position = new Vector3(x, 3, 0);
            Instantiate(leftWallPrefab, position, Quaternion.identity);
        }

        for (int x = gridWidth; x < gridWidth + 1; x++)
        {
            Vector3 position = new Vector3(x, 3, 0);
            Instantiate(rightWallPrefab, position, Quaternion.identity);
        }

        // グリッドを生成
        for (int y = 0; y < 1; y++)
        {
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 position = new Vector3(x * cubeSpacing, y * cubeSpacing, 0);
                Instantiate(cubePrefab, position, Quaternion.identity);
            }
        }
    }
}
