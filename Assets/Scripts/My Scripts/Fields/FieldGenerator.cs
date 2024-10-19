using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public int gridWidth = 101; // �O���b�h�̕�
    public int gridHeight = 7; // �O���b�h�̍���
    public GameObject cubePrefab; // �L���[�u��Prefab
    public float cubeXSpacing = 1.0f; // �L���[�u��X�̃X�y�[�X
    public float cubeYSpacing = 1.0f; // �L���[�u��Y�̃X�y�[�X
    public GameObject pillarPrefab; // ����Prefab
    public GameObject leftWallPrefab; // ���[�̕�Prefab
    public GameObject rightWallPrefab; // ���[�̕�Prefab

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

        // �O���b�h�𐶐�
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

            // �c�̐ڑ��������_���ɐ���
            if (y < gridHeight - 2 && Probability(100))
            {
                Vector3 verticalPosition = new Vector3(Random.Range(5,95), y * cubeYSpacing + cubeYSpacing / 2, 0);
                Instantiate(pillarPrefab, verticalPosition, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// �m������
    /// </summary>
    /// <param name="fPercent">�m�� (0~100)</param>
    /// <returns>���I���� [true]���I</returns>
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
