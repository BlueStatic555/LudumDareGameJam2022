using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crate : MonoBehaviour
{
    public GameObject ammo;
    bool spawned;           // whether or not ammo has been spawned already
    // Start is called before the first frame update
    void Start()
    {
        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bullet")
        {
            if (!spawned)
            {
                // Play crate breaking sound

                spawned = true;
                Instantiate(ammo, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }
}
