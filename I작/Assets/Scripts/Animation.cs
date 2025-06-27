using UnityEngine;
using UnityEngine.InputSystem;

public class Animation : StateMachineBehaviour
{
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션 상태에 진입했을 때
        player = animator.GetComponent<Player>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 애니메이션이 재생되고 있는 동안 매 프레임 호출됨

      
    }
}