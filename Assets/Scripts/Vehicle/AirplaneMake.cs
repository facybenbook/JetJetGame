using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneMake : MonoBehaviour
{

    public Fighter FighterType;
    public int Number;

    public Rigidbody Target;
    float TargetDistance = Mathf.Infinity;
    float CullDistance = 35000f;

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

    void Observe() {

        if(Target == null)
            return;

        TargetDistance = Vector3.Distance(transform.position, Target.transform.position);


        if( InCullRange())
            print("AIRPLANEMAKE");

    }

    bool InCullRange() {
        return TargetDistance < CullDistance;
    }
}
