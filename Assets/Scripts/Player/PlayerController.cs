using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    private Rigidbody m_rb;

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");
        m_rb.velocity = new Vector3(xMove * movementSpeed, m_rb.velocity.y, yMove * movementSpeed);
    }
}
