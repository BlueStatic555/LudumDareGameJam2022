using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rat_script : MonoBehaviour
{
    //delete rat of contact
    bool played;
    public GameObject deathSource;
    // Start is called before the first frame update
    void Start()
    {
        played = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "bullet":
                if (!played)
                {
                    Instantiate(deathSource, transform.position, Quaternion.identity);
                    Destroy(this.gameObject);
                }
                break;
        }
    }
}
