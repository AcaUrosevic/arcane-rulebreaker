using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask groundLayer;

    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookPoint = hit.point;
            lookPoint.y = transform.position.y;

            Vector3 direction = (lookPoint - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
            }
        }
    }
}
