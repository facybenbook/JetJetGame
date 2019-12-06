using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

    void Death() { // called from attached CanHealth script

        GameObject explosionObject = (GameObject)Instantiate(Resources.Load("Explosion/1Explosion"), transform.position, transform.rotation);
        GameObject.Destroy(explosionObject, 6f);

        GameObject.Destroy(gameObject);
    }

}
