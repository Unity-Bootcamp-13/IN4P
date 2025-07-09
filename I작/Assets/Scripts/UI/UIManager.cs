using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] heartImage;
    public Sprite FullHeartSprite;
    public Sprite HalfHeartSprite;
    public Sprite BlacnkHeartSprite;

    public Sprite PassiveItemImage;

    private void OnEnable()
    {
        Player.OnHpChanged += UPdateHearts;
    }

    private void OnDisable()
    {
        Player.OnHpChanged -= UPdateHearts;
    }

    public void UPdateHearts(int hp)
    {
        for (int i = 0; i< heartImage.Length; i++)
        {
            int heartHp = hp - (i * 2);
            if (heartHp >= 2)
            {
                heartImage[i].sprite = FullHeartSprite;
            }
            else if (heartHp == 1)
            {
                heartImage[i].sprite = HalfHeartSprite;
            }
            else
            {
                heartImage[i].sprite = BlacnkHeartSprite;
            }
        }
    }

    public void UpdateBombCount()
    {

    }

    public void UpdateKeyCount()
    {

    }
}
