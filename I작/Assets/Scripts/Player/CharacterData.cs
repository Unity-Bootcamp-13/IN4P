using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private int characterId;
    [SerializeField] private string characterName;
    [SerializeField] private Sprite headSprite;
    [SerializeField] private Sprite bodySprite;
    [SerializeField] private float hp;
    [SerializeField] private float atk;
    [SerializeField] private float atkSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float atkRange;
    [SerializeField] private float projectileSpeed;

    public int CharacterId => characterId;
    public string CharacterName => characterName;
    public Sprite HeadSprite => headSprite;
    public Sprite BodySprite => bodySprite;
    public float Hp => hp;
    public float Atk => atk;
    public float AtkSpeed => atkSpeed;
    public float Speed => speed;
    public float AtkRange => atkRange;
    public float ProjectileSpeed => projectileSpeed;
}
