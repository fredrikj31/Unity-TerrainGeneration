using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float jump;
    Rigidbody2D rb;
 
    void Start () {
        rb = GetComponent <Rigidbody2D> ();
    }
 
    void FixedUpdate () {
        float x = Input.GetAxis ("Horizontal");
        if (Input.GetAxis ("Jump") > 0) {
            rb.AddForce (Vector2.up * jump, ForceMode2D.Impulse);
        }
        rb.velocity = new Vector3 (x * speed, rb.velocity.y, 0);
 
    }
}
