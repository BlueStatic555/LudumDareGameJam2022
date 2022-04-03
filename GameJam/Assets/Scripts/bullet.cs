using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    float lifeTime;
    public float damage;
    public Rigidbody2D body;
    int moveSpeed = 20;
    void Start()
    {
        lifeTime = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        body.AddRelativeForce(new Vector3(moveSpeed, 0, 0));
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0 )
        {
            Destroy(transform.parent.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "R A T":
                Destroy(this.gameObject);
                break;
        }
    }
}
