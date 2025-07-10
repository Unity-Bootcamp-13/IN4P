using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private int characterId;
    [SerializeField] private string characterName;
    [SerializeField] private Sprite headSprite;
    [SerializeField] private Sprite bodySprite;
    [SerializeField] private int playerHp;
    [SerializeField] private float atk;
    [SerializeField] private float atkSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float atkRange;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int bombCount;
    [SerializeField] private int keyCount;

    public int CharacterId => characterId;
    public string CharacterName => characterName;
    public Sprite HeadSprite => headSprite;
    public Sprite BodySprite => bodySprite;
    public int PlayerHp => playerHp;
    public float Atk => atk;
    public float AtkSpeed => atkSpeed;
    public float Speed => speed;
    public float AtkRange => atkRange;
    public float ProjectileSpeed => projectileSpeed;
    public int BombCount => bombCount;
    public int KeyCount => keyCount;

}
