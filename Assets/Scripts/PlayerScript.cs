using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;

    private static PlayerScript prevInstance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Switching between scenes, moving objects between scenes
        if (prevInstance != null ) // check if there is another object with this class
        {
            // if so, we need to decice which object will remain: this or prevInstance

            // a) remain this object and destroy prevInstance
            //this.rb.linearVelocity = prevInstance.rb.linearVelocity;
            //this.rb.angularVelocity = prevInstance.rb.angularVelocity;
            //GameObject.Destroy(prevInstance.gameObject);

            // b) remain prevInstance object and destroy this
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            prevInstance = this; // save reference on this object like static field
        }

        prevInstance.transform.position = Vector3.zero;
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        // Before Unity 6
        //Vector2 moveValue = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Unity 6
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // При таком управлении учитывается оси X и Y в игровом мире, а не от камеры 
        //rb.AddForce(moveValue.x, 0f, moveValue.y);
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;                          // убираем вертикальную составляющую
        if (camForward == Vector3.zero)             // если вектор был полностью вертикальным
        {
            camForward = Camera.main.transform.up;  // тогда up заменяет вектор
        } else {                                    // или
            camForward.Normalize();                 // удлиняем вектор до единичной длины
        }

        Vector3 force = 
            camForward * moveValue.y +      // сигнал Y - вдоль исправленного вектора forward
            camRight * moveValue.x;         // сигнал X - вдоль вектора right
        rb.AddForce(force * 700f * Time.deltaTime);
    }
}
