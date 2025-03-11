using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, forwardInput;

    [SerializeField] private float turnSpeed = 60f; // Velocidad de giro
    [SerializeField] private float speed = 30f;

    private void Start()
    {

    }

    void Update() {

        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");


        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);

        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
        
    }
}
