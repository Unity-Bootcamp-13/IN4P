using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayer : MonoBehaviour
{    
    public CharacterData characterData;
    public GameObject testImage;

    public int keyCount;
    public int bombCount;
    public int hp;
    public float atk;
    public float atkSpeed;
    public float speed;
    public float atkRange;
    public float projectileSpeed;
    public int currentHp;

    private int h;
    private int v;
    private int isMove;
   
    private void Awake()
    {
        hp = characterData.PlayerHp;
        atk = characterData.Atk;
        atkSpeed = characterData.AtkSpeed;
        speed = characterData.Speed;
        atkRange = characterData.AtkRange;
        projectileSpeed = characterData.ProjectileSpeed;
        currentHp = hp;       
    }


    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, y, 0).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    
    public void ApplyEffect(Item item)
    {
        Debug.Log("applyEffect");
        testImage.GetComponent<SpriteRenderer>().sprite = item.itemIcon;
        List<EffectModel> effectModels = item.EffectModels;

        for (int i = 0; i < effectModels.Count; i++)
        {
            switch (effectModels[i].target)
            {
                case EffectTarget.BombCount:
                    bombCount += (int)effectModels[i].value;
                    break;
                case EffectTarget.KeyCount:
                    keyCount *= (int)effectModels[i].value;
                    break;
                case EffectTarget.Atk:
                    atk += effectModels[i].value;
                    break;
                case EffectTarget.AtkRate:
                    atk *= effectModels[i].value;
                    break;
                case EffectTarget.AtkSpeed:
                    atkSpeed += effectModels[i].value;
                    break;
                case EffectTarget.AtkSpeedRate:
                    atkSpeed *= effectModels[i].value;
                    break;
                case EffectTarget.AtkRange:
                    atkRange += effectModels[i].value;
                    break;
                case EffectTarget.AtkRangeRate:
                    atkRange *= effectModels[i].value;
                    break;
                case EffectTarget.Speed:
                    speed += effectModels[i].value;
                    break;
                case EffectTarget.ProjectileSpeed:
                    projectileSpeed += effectModels[i].value;
                    break;
                case EffectTarget.MaxHp:
                    hp += (int)effectModels[i].value;
                    break;
                case EffectTarget.CurrentHp:
                    currentHp += (int)effectModels[i].value;
                    break;
                default:
                    break;
            }
        }
    }
}
