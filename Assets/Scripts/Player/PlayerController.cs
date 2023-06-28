using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {
    public static PlayerController instance;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Animator m_animator;

    private Rigidbody m_rb;
    private bool m_isFacingRight;
    private PlayerDirection m_direction;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
        m_isFacingRight = true;
        m_direction = PlayerDirection.South;
    }

    private void FixedUpdate() {
        Movement();
    }

    public void changePlayerSate(PlayerDirection t_newSate) {
        if (m_direction == t_newSate) {
            return;
        }
        m_direction = t_newSate;
        switch (m_direction) {
            case PlayerDirection.North:
                break;
            case PlayerDirection.South:
                break;
            case PlayerDirection.East:
                break;
            case PlayerDirection.West:
                break;
        }
    }

    public PlayerDirection getPlayerDirection() {
        return m_direction;
    }

    private void Movement() {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");
        m_animator.SetFloat("moveX", xMove);
        m_animator.SetFloat("moveY", yMove);
        m_rb.velocity = new Vector3(xMove * movementSpeed, m_rb.velocity.y, yMove * movementSpeed);
        if ((xMove < 0 && m_isFacingRight) || (xMove > 0 && !m_isFacingRight)) {
            flip();
        }
        if (xMove != 0 || yMove != 0) {
            PlayerManager.instance.changePlayerSate(PlayerState.Moving);
            m_animator.SetFloat("lastMoveX", xMove);
            m_animator.SetFloat("lastMoveY", yMove);
        } else if (xMove == 0) {
            PlayerManager.instance.changePlayerSate(PlayerState.Idle);
        }
        if (xMove == 1) {
            changePlayerSate(PlayerDirection.East);
        } else if (xMove == -1) {
            changePlayerSate(PlayerDirection.West);
        } else if (yMove == 1) {
            changePlayerSate(PlayerDirection.North);
        } else if (yMove == -1) {
            changePlayerSate(PlayerDirection.South);
        }
    }

    private void flip() {
        //transform.Rotate(0, 180, 0);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }
}

public enum PlayerDirection {
    North,
    South,
    East,
    West
}