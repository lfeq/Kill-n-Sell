using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField]
    private Transform leftAttackPosition, rightAttackPosition,
                      downAttackPosition, upAttackPosition;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            attack();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftAttackPosition.position, attackRange);
        Gizmos.DrawWireSphere(rightAttackPosition.position, attackRange);
        Gizmos.DrawWireSphere(downAttackPosition.position, attackRange);
        Gizmos.DrawWireSphere(upAttackPosition.position, attackRange);
    }

    private void attack() {
        PlayerManager.instance.changePlayerSate(PlayerState.Attacking);
        switch (PlayerController.instance.getPlayerDirection()) {
            case PlayerDirection.North:
                damage(upAttackPosition);
                break;
            case PlayerDirection.South:
                damage(downAttackPosition);
                break;
            case PlayerDirection.East:
                damage(rightAttackPosition);
                break;
            case PlayerDirection.West:
                damage(leftAttackPosition);
                break;
        }
    }

    // Check for collisions
    private void damage(Transform attackPosition) {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPosition.position, attackRange, enemyLayer);
        foreach (Collider enemy in hitEnemies) {
            // Do damage
        }
    }
}