using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public string team;
    public float speed;
    public Transform mesh;
    public GameObject trail;

    float range;
    RaycastHit hit;
    Vector3 hitpositon;

    int layerMask = 1 << 0; // Default layer

    //

    void Start()
    {
        StartCoroutine( EnLargen());
    }

    void Update () {

        range = speed * Time.deltaTime;
        DoStep();

        if (Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask))
            DoHit();

	}

    //

    void DoStep()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * range, Color.yellow);
        transform.position += transform.forward * range;
    }

    void DoHit()
    {
        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        hitpositon = transform.position + transform.forward * hit.distance;
        transform.position = hitpositon;

        // Opposing Aircar aircraft
        if ( hit.collider.GetComponentInParent<CanHealth>() != null && hit.collider.GetComponentInParent<CanHealth>().team != team)
        {
            hit.collider.GetComponentInParent<CanHealth>().TakeDamage(1);
            Spark(hitpositon); //print(hit.collider + " received Opposition Fire from: " + team);

        }
        // Friendly Aircar aircraft
        else if ( hit.collider.GetComponentInParent<CanHealth>() != null && hit.collider.GetComponentInParent<CanHealth>().team == team)
        {
            //print(hit.collider + " received Friendly Fire from: " + team);
        }
        // Other projectile
        else if ( hit.collider.GetComponentInParent<Projectile>() != null)
        {
            //print(hit.collider + " received Friendly Fire from: " + team);
        }
        else if ( hit.collider.GetComponentInParent<Airwing>() != null && team != "Airwing")
        {
            print(hit.collider + " received ENEMY FIRE Fire from: " + team);

        }
        else if ( hit.collider.tag == "StageTrack")
        {
            // should be outside the default layer
            print("StageTrackLayer Projectile Collision!! Error.");
        }
        else if ( hit.collider.tag == "RoadNetwork")
        {
            // should be outside the default layer
            print("RoadNetwork Projectile Collision!! Error.");
        }
        // Environment
        else {
            Spark(hitpositon); //print(hit.collider + " received Fire from: " + team);
            //print(hit.collider + " received from: " + team);
        }

    }

    void Spark(Vector3 pos)
    {
        GameObject explosionObject = (GameObject)Instantiate(Resources.Load("Explosion/0ExplosionSmall"), pos, transform.rotation);
        GameObject.Destroy(gameObject);
        GameObject.Destroy(explosionObject, 1f);
    }

    IEnumerator EnLargen()
    {
        yield return 0;
        mesh.localScale = new Vector3(2f, 200f, 2f);

        yield return 0;
        mesh.localScale = new Vector3(5f, 200f, 5f);

        yield return new WaitForSeconds(0.2f);
        mesh.localScale = new Vector3(10f, 500f, 10f);
        trail.SetActive(true);
    }

}
