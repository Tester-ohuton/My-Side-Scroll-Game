using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HpBarCtrl : MonoBehaviour
{
    public float countdown = 5.0f;
    public GameObject playerObj;
	private PlayerStatus playerStatus;
    bool fadestart = false;

    // MAXHP
    private float maxHP;

	// 現在のHP
	//どこでも使えるように
	public static float curHP;

	// HP表示用UI
	[SerializeField] private GameObject HPUI;

	// HP表示用スライダー
	//private Slider hpSlider;

    private Image image;

    EnemyData enemy;

    void Start()
	{
		// プレイヤーのステータス取得
		playerStatus = playerObj.GetComponent<PlayerStatus>();

		// HP取得
		curHP = maxHP = playerStatus.GetMaxhp();

		//hpSlider = HPUI.transform.Find("Slider").GetComponent<Slider>();
		//hpSlider.value = 1.0f;


        image = HPUI.transform.Find("Image").GetComponent<Image>();
        image.fillAmount = 1.0f;
    }

	void Update()
	{
		// 現在のプレイヤーHPを取得
		curHP = playerStatus.GetCurHp();

        if (curHP <= 0)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                if (!fadestart)
                {
                    fadestart = true;
                    FadeManager.Instance.LoadScene("Game Over", 2.0f);
                }
            }

            //if (!fadestart)
            //{
            //    fadestart = true;
            //    FadeManager.Instance.LoadScene("Game Over", 2.0f);
            //}
            //fadestart = false;
        }

        //----------------------スライダーの場合------------------------------
        //hpSlider.value = playerStatus.GetCurHp() / playerStatus.GetMaxhp();


		//--------------------------画像の場合-------------------------------------------------------
		image.GetComponent<Image>().fillAmount = playerStatus.GetCurHp() / playerStatus.GetMaxhp();
	}
}