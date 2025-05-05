using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset;
    private float minOffset = 2f;
    private float maxOffset = 5f;

    // cameraAnchor - точка привязки камеры
    [SerializeField] private Transform cameraAnchor;

    private InputAction lookAction;
    private bool isFpv;
    private float minAngleX = 10f;
    private float maxAngleX = 60f;
    private float minAngleXFpv = -20f;
    private float maxAngleXFpv = 20f;
    private float angleX0;
    private float angleY0;
    private float angelY = 0f;
    private float sensitivityX = 70f;
    private float _angelX = 0f;
    private float angelX
    {
        get { return _angelX; }
        set
        {
            if (!isFpv && (value > minAngleX && value < maxAngleX))
            {
                //Debug.Log($"AngleX in RPG: {this.transform.eulerAngles.x}");
                _angelX = value;
            }
            else if (isFpv && (value > minAngleXFpv && value < maxAngleXFpv))
            {
                //Debug.Log($"AngleX in FPV: {this.transform.eulerAngles.x}");
                _angelX = value;
            }
        }
    }
    private float sensitivityY = 10f;

    public static bool isFixed = false;
    public static Transform fixedCameraPosition = null!;

    void Start()
    {
        offset = this.transform.position - cameraAnchor.position;
        lookAction = InputSystem.actions.FindAction("Look");

        angleX0 = angelX = this.transform.rotation.eulerAngles.x;
        angleY0 = angelY = this.transform.rotation.eulerAngles.y;

        isFpv = offset.magnitude < minOffset;
    }

    void Update()
    {
        if (isFixed)
        {
            this.transform.position = fixedCameraPosition.position;
            this.transform.rotation = fixedCameraPosition.rotation;
        }
        else
        {
            // Вращение
            // Vector2 lookValue = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // Before Unity 6
            // lookValue и Input.GetAxis возращают данные о ИЗМЕНЕНИИ курсора, а не его позицию. Если курсор мыши не двигается, то тогда сигнал = 0. Чтобы знать полный угол поворота необходимо накапливать все сигналы (интегрировать)

            Vector2 lookValue = Time.deltaTime * lookAction.ReadValue<Vector2>();
            angelY += lookValue.x * sensitivityX;
            angelX -= lookValue.y * sensitivityY;
            this.transform.eulerAngles = new Vector3(angelX, angelY, 0);

            //--------------------------------------------------------------------------------

            // Следование
            // this.transform.position = cameraAnchor.position + offset; // без поворота камеры
            this.transform.position = cameraAnchor.position +
                Quaternion.Euler(angelX - angleX0, angelY - angleY0, 0f) * offset; // с поворотом вектора offset

            //--------------------------------------------------------------------------------

            // Приближение/Отдаление
            Vector2 zoom = Input.mouseScrollDelta;
            if (zoom.y > 0 && !isFpv)
            {
                offset *= 0.9f;

                if (offset.magnitude < minOffset)
                {
                    offset *= 0.01f;
                    isFpv = true;
                    angelX = (maxAngleXFpv + minAngleXFpv) / 2;
                }
            }
            else if (zoom.y < 0)
            {
                if (isFpv)
                {
                    offset *= minOffset / offset.magnitude;
                    isFpv = false;
                    angelX = (maxAngleX + minAngleX) / 2;
                }
                if (offset.magnitude < maxOffset)
                {
                    offset *= 1.1f;
                }
            }
        } 
    }
}
