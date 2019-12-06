using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanCull : MonoBehaviour
{

    private Rigidbody Target;
    public float CullDistance = 35000f;
    float TargetDistance = Mathf.Infinity;

    GameObject gobj;
    BoxCollider[] colliders;
    MeshRenderer[] meshes;
    Rigidbody rigid;

    bool Live = false;

    void Awake()
    {
        gobj = gameObject;
        rigid = GetComponent<Rigidbody>();
        meshes = GetComponentsInChildren<MeshRenderer>();
        colliders =  GetComponentsInChildren<BoxCollider>();
        Off();
    }

    void Start()
    {
        if(Game.I.CamTrk != null && Game.I.CamTrk.Airwing != null )
            Target = Game.I.CamTrk.Airwing.GetComponent<Rigidbody>();

        if(Target == null)
            print(gameObject.name + " cannot find Airwing.GetComponent<Rigidbody>() for CanCull");

        float ObserveTime = 3f;
        InvokeRepeating("Observe", Random.Range(0, ObserveTime), ObserveTime);
    }

    void Update()
    {

    }

    //

    void Off()
    {
        //gobj.SetActive(false);

        if(colliders != null)
            foreach(BoxCollider b in colliders)  b.enabled = false;
        if(meshes != null)
            foreach(MeshRenderer m in meshes)  m.enabled = false;
        if(rigid != null)
            rigid.isKinematic = true;

        Live = false;
    }

    void On()
    {
        //gobj.SetActive(true);

        if(colliders != null)
            foreach(BoxCollider b in colliders)  b.enabled = true;
        if(meshes != null)
            foreach(MeshRenderer m in meshes)  m.enabled = true;
        if(rigid != null)
            rigid.isKinematic = false;

        Live = true;
    }

    bool InCullRange() {
        return TargetDistance < CullDistance;
    }

    void Observe() {

        //print("Observe Can Cull");

        if(Target == null)
            return;

        TargetDistance = Vector3.Distance(transform.position, Target.transform.position);

        // print( gameObject.name + " " + Vector3.Dot(  Game.I.CamTrk.GetRailForward(),  Target.transform.position - transform.position  ) );

        if ( Vector3.Dot(  Game.I.CamTrk.GetRailForward(),  Target.transform.position - transform.position  ) > 0 )
        {   //print ("Destory " + gameObject.name);
            Destroy(gameObject);
            return;
            // Behind frame
            // Cull
        }

        if( !Live && InCullRange())
            On();
            // In range
            // Activate
    }

}
