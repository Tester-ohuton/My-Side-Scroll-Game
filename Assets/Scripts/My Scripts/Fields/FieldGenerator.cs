using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public int gridWidth = 100; // �O���b�h�̕�
    public GameObject cubePrefab; // �L���[�u��Prefab
    public float cubeSpacing = 1.0f; // �L���[�u��X�̃X�y�[�X
    public GameObject leftWallPrefab; // ���[�̕�Prefab
    public GameObject rightWallPrefab; // ���[�̕�Prefab

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

        // �O���b�h�𐶐�
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
