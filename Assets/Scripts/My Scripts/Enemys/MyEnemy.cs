using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    const int Num = (int)EnemyData.EnemyType.MAX_ENEMY;

    // æ“¾ƒAƒCƒeƒ€Ši”[—p”z—ñ
    int[] Storage = new int[Num];

    bool fadestart = false;

    void Start()
    {
        // ”z—ñ‚Ì—v‘f”•ª‰Šú‰»
        for (int i = 0; i < Storage.Length; ++i)
        {
            // ‘S‚Ä‚O
            Storage[i] = 0;
        }
    }


    void Update()
    {
        // hp‚ª0‚Å‚È‚¢‚Æ‚«‚Éí‚Éfalse
        StaticEnemy.IsUpdate = false;

        /*
        // ‰¼
        if (Storage[0] == 2)
        {
            if (!fadestart)
            {
                fadestart = true;
                FadeManager.Instance.LoadScene("Result", 2.0f);
                Debug.Log("a");
                //fadestart = false;
            }
        }
        */
    }

    // “GƒLƒƒƒ‰Ši”[
    // @param...type Ši”[‚·‚é“GƒLƒƒƒ‰‚Ìí—Ş
    public void AddEnemy(EnemyData.EnemyType type)
    {
        // ˆø”‚Å“n‚³‚ê‚½“GƒLƒƒƒ‰‚Ì”‚ğ‘‚â‚·
        Storage[(int)type]++;
        Debug.Log($"Storage: {Storage[(int)type]}");
    }

    // ƒXƒgƒŒ[ƒW‚Ì’†g‚ğæ“¾
    public int[] GetStorage()
    {
        return Storage;
    }
}
