using UnityEngine;

public class Billbording : MonoBehaviour {
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}