using UnityEngine;
using System.Collections;

public class PlaverMove : MonoBehaviour {
    public float walkSpeed;
    public float jumpImpulse;

    private Rigidbody2D body;
    private Vector2 movement;

    private float horInput,verInput;

    void Start()
    {
        this.body = this.GetComponent<Rigidbody2D>();
        this.movement = new Vector2();
    }

    void Update()
    {
        this.horInput = Input.GetAxis("Horizontal");
        this.verInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        this.movement = this.body.velocity;

        this.movement.x = horInput * walkSpeed;
        this.movement.y = verInput * walkSpeed;

        this.body.velocity = this.movement;
    }

}
