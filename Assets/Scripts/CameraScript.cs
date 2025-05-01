using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset;

    // cameraAnchor - ����� �������� ������
    [SerializeField]
    private Transform cameraAnchor;

    private InputAction lookAction;
    private float angelY = 0f;
    private float sensitivityX = 70f;

    private float _angelX = 0f;
    private float angelX
    {
        get { return _angelX; }
        set
        { 
            if (value > 35 && value < 45)
            {
                _angelX = value;
            }
        }
    }
    private float sensitivityY = 10f;


    void Start()
    {
        offset = this.transform.position - cameraAnchor.position;
        lookAction = InputSystem.actions.FindAction("Look");

        angelX = this.transform.rotation.eulerAngles.x;
        angelY = this.transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        // ��������
        // Vector2 lookValue = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // Before Unity 6
        // lookValue � Input.GetAxis ��������� ������ � ��������� �������, � �� ��� �������. ���� ������ ���� �� ���������, �� ����� ������ = 0. ����� ����� ������ ���� �������� ���������� ����������� ��� ������� (�������������)

        Vector2 lookValue = Time.deltaTime * lookAction.ReadValue<Vector2>();
        angelY += lookValue.x * sensitivityX;
        angelX -= lookValue.y * sensitivityY;
        this.transform.eulerAngles = new Vector3(angelX, angelY, 0);

        //--------------------------------------------------------------------------------

        Debug.Log($"angelX: {angelX}");

        // ����������
        // this.transform.position = cameraAnchor.position + offset; // ��� �������� ������
        this.transform.position = cameraAnchor.position +
            Quaternion.Euler(0f, angelY, 0f) * offset; // � ��������� ������� offset
    }
}
