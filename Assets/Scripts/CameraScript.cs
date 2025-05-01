using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset;

    // cameraAnchor - точка привязки камеры
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
        // Вращение
        // Vector2 lookValue = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // Before Unity 6
        // lookValue и Input.GetAxis возращают данные о ИЗМЕНЕНИИ курсора, а не его позицию. Если курсор мыши не двигается, то тогда сигнал = 0. Чтобы знать полный угол поворота необходимо накапливать все сигналы (интегрировать)

        Vector2 lookValue = Time.deltaTime * lookAction.ReadValue<Vector2>();
        angelY += lookValue.x * sensitivityX;
        angelX -= lookValue.y * sensitivityY;
        this.transform.eulerAngles = new Vector3(angelX, angelY, 0);

        //--------------------------------------------------------------------------------

        Debug.Log($"angelX: {angelX}");

        // Следование
        // this.transform.position = cameraAnchor.position + offset; // без поворота камеры
        this.transform.position = cameraAnchor.position +
            Quaternion.Euler(0f, angelY, 0f) * offset; // с поворотом вектора offset
    }
}
