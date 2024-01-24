using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Range(0, 1000)]
    [SerializeField] private float sensX;
    [Range(0, 1000)]
    [SerializeField] private float sensY;
    [Space]
    [SerializeField] private Transform orientation;

    private float xRotation, yRotation;
    [Space]
    [SerializeField] private bool stealCursor = true;
    [Space]
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform cameraHolder;

    private void Start()
    {
        if (stealCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;//UNITY
        xRotation -= mouseY;//PIJANE

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0 , yRotation, 0);

        cameraHolder.position = cameraPosition.position;
    }
}