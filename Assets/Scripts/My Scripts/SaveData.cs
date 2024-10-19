using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu]
public class SaveData : ScriptableObject
{
    public int HP;          // HP
    public int SPD;         // スピード
    public int ATK;         // 攻撃力
    public int DEF;         // 防御力
    public int LUK;         // 運
    public int jumpSpeed;   //ジャンプスピード
    public float gravity;   //重力

}

