using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[Serializable]

public class PlayerStat : MonoBehaviour
{
    public float hp = 3.0f;             // 체력
    public float atk = 3.5f;            // 공격력
    public float atkspeed = 10.0f;      // 공격 속도
    public float speed = 1.0f;          // 이동 속도
    public float intersection = 1.0f;   // 사거리
    public float ps = 7.5f;             // 투사체 속도
    
}
