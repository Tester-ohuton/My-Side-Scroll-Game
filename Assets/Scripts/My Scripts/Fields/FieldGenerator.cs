using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public int gridWidth = 101; // グリッドの幅
    public int gridHeight = 7; // グリッドの高さ
    public GameObject cubePrefab; // キューブのPrefab
    public float cubeXSpacing = 1.0f; // キューブ間Xのスペース
    public float cubeYSpacing = 1.0f; // キューブ間Yのスペース
    public GameObject pillarPrefab; // 柱のPrefab
    public GameObject leftWallPrefab; // 両端の壁Prefab
    public GameObject rightWallPrefab; // 両端の壁Prefab

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int y = 0; y < gridHeight * gridHeight; y += gridHeight)
        {
            for (int x = 0; x < 1; x++)
            {
                Vector3 position = new Vector3(x, y, 0);
                Instantiate(leftWallPrefab, position, Quaternion.identity);
            }

            for (int x = gridWidth; x < gridWidth + 1; x++)
            {
                Vector3 position = new Vector3(x, y, 0);
                Instantiate(rightWallPrefab, position, Quaternion.identity);
            }
        }

        // グリッドを生成
        for (int y = 0; y < 1; y++)
        {
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 position = new Vector3(x * cubeXSpacing, y * cubeYSpacing, 0);
                Instantiate(cubePrefab, position, Quaternion.identity);
            }
        }

        for (int y = 1; y < gridHeight; y++)
        {
            for (int x = 1; x < gridWidth; x++)
            {
                Vector3 position = new Vector3(x * cubeXSpacing, y * cubeYSpacing, 0);
                Instantiate(cubePrefab, position, Quaternion.identity);
            }

            // 縦の接続をランダムに生成
            if (y < gridHeight - 2 && Probability(100))
            {
                Vector3 verticalPosition = new Vector3(Random.Range(5,95), y * cubeYSpacing + cubeYSpacing / 2, 0);
                Instantiate(pillarPrefab, verticalPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// 確率判定
    /// </summary>
    /// <param name="fPercent">確率 (0~100)</param>
    /// <returns>当選結果 [true]当選</returns>
    public static bool Probability(float fPercent)
    {
        float fProbabilityRate = UnityEngine.Random.value * 100.0f;

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
