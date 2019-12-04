using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private int FixedSteps = 30; // how many physics timesteps to take
    private int DoneSteps = 0;
    private bool tweening = false;
    private Quaternion targetRotation;
    private float incrementAngle;

    //

    void Start()
    {
        targetRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        if(!tweening)  return;

        //print("STAGETWEEN " + DoneSteps + "/" + FixedSteps);

        if( DoneSteps >= FixedSteps)  Destroy(gameObject);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + incrementAngle, transform.rotation.eulerAngles.z);
        Game.I.CamTrk.SetRailDir(transform);
        DoneSteps++;
    }

    //

    void OnTriggerEnter(Collider other)
    {
        if( !tweening && other.tag == "AirwingBubble")
        {
            //Game.I.CamTrk.SetRailDir(transform);

            transform.rotation = Game.I.CamTrk.GetRailDir();
            incrementAngle = Quaternion.Angle(transform.rotation, targetRotation);
            incrementAngle *= -1f;
            incrementAngle /= FixedSteps;

            tweening = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Vector3.one);
        Gizmos.DrawWireCube (Vector3.zero, transform.localScale);
        Gizmos.matrix = oldGizmosMatrix;
    }

}
