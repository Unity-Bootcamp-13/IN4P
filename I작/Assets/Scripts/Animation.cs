using UnityEngine;
using UnityEngine.InputSystem;

public class Animation : StateMachineBehaviour
{
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �ִϸ��̼� ���¿� �������� ��
        player = animator.GetComponent<Player>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �ִϸ��̼��� ����ǰ� �ִ� ���� �� ������ ȣ���

      
    }
}