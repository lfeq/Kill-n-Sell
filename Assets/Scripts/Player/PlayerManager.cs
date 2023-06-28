using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    [SerializeField] private Animator m_animator;

    private PlayerState m_playerState;
    private Rigidbody m_rb;

    private void Awake() {
        if (FindObjectOfType<PlayerManager>() != null &&
            FindObjectOfType<PlayerManager>().gameObject != gameObject) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    private void Start() {
        m_playerState = PlayerState.None;
        m_rb = GetComponent<Rigidbody>();
    }

    public void changePlayerSate(PlayerState t_newSate) {
        if (m_playerState == t_newSate) {
            return;
        }
        resetAnimatorParameters();
        m_playerState = t_newSate;
        switch (m_playerState) {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                m_animator.SetBool("isIdeling", true);
                break;
            case PlayerState.Moving:
                m_animator.SetBool("isMoving", true);
                break;
            case PlayerState.Attacking:
                m_rb.velocity = new Vector2(0, 0);
                m_animator.SetBool("isAttacking", true);
                break;
            case PlayerState.Dead:
                m_rb.velocity = new Vector2(0, 0);
                m_animator.SetBool("isDying", true);
                break;
        }
    }

    private void resetAnimatorParameters() {
        foreach (AnimatorControllerParameter parameter in m_animator.parameters) {
            if (parameter.type == AnimatorControllerParameterType.Bool) {
                m_animator.SetBool(parameter.name, false);
            }
        }
    }
}

public enum PlayerState {
    None,
    Idle,
    Moving,
    Dead,
    Attacking
}