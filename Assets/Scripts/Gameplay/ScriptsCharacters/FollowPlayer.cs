using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform carTransform;

    [SerializeField] private Vector3 offset;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        carTransform = GameObject.FindWithTag("Player").transform;


        offset = new Vector3(0f, 3.25f, -7.84f); // ** Se cambia si se cambia la posición de la cámara en el editor de Unity
    
    }


    void LateUpdate()
    {

        Vector3 desiredPosition = carTransform.position + carTransform.TransformDirection(offset);

        transform.position = desiredPosition;

        transform.rotation = carTransform.rotation;

        
    }
}



