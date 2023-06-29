using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class EnemyPatrol : MonoBehaviour {
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f;

    private EnemyAI enemyAI;
    private int currentWaypointIndex = 0;

    private void Start() {
        enemyAI = GetComponent<EnemyAI>();
    }

    private void Update() {
        if (enemyAI.GetEnemyState() != EnemyState.Patrolling) {
            return;
        }
        if (waypoints.Length == 0) {
            throw new UnityException("No waypoints defined for patrol.");
        }
        Transform currentWaypoint = waypoints[currentWaypointIndex];
        Vector3 moveDirection = (currentWaypoint.position - transform.position).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint.position);
        if (distanceToWaypoint < 0.1f) {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}