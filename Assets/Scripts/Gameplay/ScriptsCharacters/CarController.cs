using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, forwardInput;

    [SerializeField] private float turnSpeed = 100f; // Velocidad de giro
    [SerializeField] private float speed = 10f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Configuración recomendada para evitar atravesar objetos
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate() // Usar FixedUpdate para físicas
    {
        if (!GameManager.Instance.isGameOver)
        {
            horizontalInput = Input.GetAxis("Horizontal"); // Movimiento lateral
            forwardInput = Input.GetAxis("Vertical"); // Movimiento adelante/atrás

            // Movimiento corregido con MovePosition
            Vector3 moveDirection = transform.forward * forwardInput * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);

            // Rotación corregida sin Quaternion
            float nuevaRotacionY = transform.eulerAngles.y + (horizontalInput * turnSpeed * Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3(0, nuevaRotacionY, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Has chocado con: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Meteor"))
        {
            Debug.Log("Has chocado con un meteorito");
            GameManager.Instance.DecreaseLives();
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Has llegado a la zona segura ");
            GameManager.Instance.onGameOver=false;
        }


    }
}