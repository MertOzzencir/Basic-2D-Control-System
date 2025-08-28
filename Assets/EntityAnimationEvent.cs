using UnityEngine;

public class EntityAnimationEvent : MonoBehaviour
{

    Entity player;

    void Awake() => player = GetComponentInParent<Entity>();

    private void EnableMovementAndJump() => player.AttackStateChanging(true);
    private void DisableMovementAndJump() => player.AttackStateChanging(false);
    private void AttackState() => player.AttackLogic();

}
