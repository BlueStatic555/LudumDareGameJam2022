using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crate : MonoBehaviour
{
    public GameObject ammo;
    public GameObject hPick;
    public GameObject ass;
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
                Instantiate(ass);
                spawned = true;
                for (int i = 0; i < Random.Range(3,5); i++)
                {
                    float xplu = Random.Range(-2,2);
                    float yplu = Random.Range(-2, 2);
                    int decision = (int)Random.Range(0, 2);

                    GameObject spawnObj = null;
                    if(decision==0)
                    {
                        spawnObj = ammo;

                    }
                    else
                    {
                        spawnObj = hPick;
                    }
                    Instantiate(spawnObj,
                        transform.position + new Vector3(xplu, yplu,0.0f),
                        Quaternion.identity); 
                }
                Destroy(this.gameObject);
            }
        }
    }
}
