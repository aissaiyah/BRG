using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour
{
    public float speed = 15f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
