using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public Player player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
    }

    public void DownRightLook()
    {
        player.KeyDownLook(AttackDirection.Right);
    }
    public void UpRightLook()
    {
        player.KeyUpLook();
    }
    public void DownLeftLook()
    {
        player.KeyDownLook(AttackDirection.Left);
    }
    public void UpLeftLook()
    {
        player.KeyUpLook();
    }
    public void DownupLook()
    {
        player.KeyDownLook(AttackDirection.Up);
    }
    public void UpupLook()
    {
        player.KeyUpLook();
    }
    public void DowndownLook()
    {
        player.KeyDownLook(AttackDirection.Down);
    }
    public void UpdownLook()
    {
        player.KeyUpLook();
    }
}
