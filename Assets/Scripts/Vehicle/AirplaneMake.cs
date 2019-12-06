using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneMake : MonoBehaviour
{

    public Fighter FighterType;
    public int Number;

    public Rigidbody Target;

    void Start()
    {
        int i = 0;
        while(i < Number)
        {
            Fighter newFi = GameObject.Instantiate(FighterType).GetComponent<Fighter>();
            newFi.transform.position = transform.position + (Vector3.up * 5000f);
            newFi.transform.rotation = Quaternion.identity;
            newFi.SetTarget(Target);
            newFi.IdleDirection = transform.forward;
            i++;
        }

    }

    void Update()
    {

    }

    //
}
