using UnityEngine;

public class EnemyAI : MonoBehaviour {
    private EnemyState m_currentState;

    private void Start() {
        m_currentState = EnemyState.Patrolling;
    }

    public void ChangeState(EnemyState t_newSate) {
        if (m_currentState == t_newSate) {
            return;
        }

        // Change to the new state
        m_currentState = t_newSate;
        switch (m_currentState) {
            case EnemyState.Idle:
                // Implement idle behavior
                break;

            case EnemyState.Patrolling:
                // Implement patrolling behavior
                break;

            case EnemyState.Chasing:
                // Implement chasing behavior
                break;

            case EnemyState.Attacking:
                // Implement attacking behavior
                break;

            default:
                break;
        }
    }

    public EnemyState GetEnemyState() {
        return m_currentState;
    }
}

public enum EnemyState {
    Idle,
    Patrolling,
    Chasing,
    Attacking
}