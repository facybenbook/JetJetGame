using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commuter : Aircar
{
    public Port port;

    GUIStyle guistyle = new GUIStyle();
    public bool DrawGUI = false;

    Vector3 targetRoadPos;
    Vector3 targetRoadEnd;
    float SearchRange = 50000f;

    //

    void Start()
    {
        InvokeRepeating("Check", 0, 0.5f);
        SetStateIdle(transform.position);
    }

    void Update()
    {
        base.UpdateTick();
    }

    //

    void Check()
    {


        bool hasTarget = false;

        if (CarState == S.Idle) { // when the basestate is set to idle, it means the plane has successfully taken off


            RaycastHit hit;
            int layerMask = 1 << 8; // RoadNetwork
            Quaternion rotator = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            float closest = Mathf.Infinity;
            int rays = 0;
            int totalrays = 72;
            float step = 360f / totalrays;

            while(rays < totalrays)
            {

                rotator *= Quaternion.Euler(0f, step, 0f);

                Vector3 rayForward = rotator * Vector3.forward;

                //Debug.DrawLine( transform.position, transform.position + (rayForward * SearchRange), Color.red);

                if (Physics.Raycast( transform.position, rayForward, out hit, SearchRange, layerMask))
                {
                    hasTarget = true;

                    //Debug.DrawLine( transform.position, hit.point, Color.green);

                    if(hit.distance < closest)
                    {
                        closest = hit.distance;
                        targetRoadPos = hit.point;
                        targetRoadEnd =  hit.transform.position + (hit.transform.forward * (hit.transform.localScale.z/2f) );
                    }

                }

                rays++;
            }


            if( hasTarget)
            {
                targetRoadPos = new Vector3(targetRoadPos.x, 100f, targetRoadPos.z);
                targetRoadEnd = new Vector3(targetRoadEnd.x, 100f, targetRoadEnd.z);

                //Debug.DrawLine( targetRoadPos, targetRoadPos + (Vector3.up * 300f), Color.yellow);

                targetPos = targetRoadPos;
                CarState = S.MovingToRoad;
            }
            else
            {
                print("Commuter.cs failed to find target");

            }


            // PORT LANDING
            // if (port != null)
            //     port = PortAuthority.I.RandomNewPort(port.transform.position); // find another port than this one
            // else
            //     port = PortAuthority.I.ClosestPort(transform.position);
            //
            // StartLanding(port); // and land there
        }

        if (CarState == S.FoundTargetRoad) {

            targetPos = targetRoadEnd;
            CarState = S.MovingToRoadEnd;

        }

        if (CarState == S.FoundTargetRoadEnd) {

            targetPos = targetRoadEnd;
            CarState = S.Idle;
            //print("Idle");

        }



    }

    void OnGUI()
    {
        if(!DrawGUI) return;

        guistyle.normal.textColor = Color.red;
        guistyle.alignment = TextAnchor.MiddleCenter;

        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        int yplus = 8;  int ypos = yplus;

        if( TargetDistance < 2000)
            guistyle.fontSize = 14;
        else if( TargetDistance < 3000)
            guistyle.fontSize = 12;
        else if( TargetDistance < 4000)
            guistyle.fontSize = 10;
        else if( TargetDistance < 50000)
            guistyle.fontSize = 8;
        else if( TargetDistance < 60000)
            guistyle.fontSize = 7;
        else
            guistyle.fontSize = 5;

        GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y - 30, 80, 25), CarState.ToString(), guistyle ); // + " " + idlePosition


        if(currentPort != null)
        {
            if(landingStep != 0)
            {
                GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "LandingStep " + landingStep, guistyle ); ypos+=yplus; // + landingSpot.spot.ToString()
            }

            if(landingSpot != null && landingSpot.spot != null)
            {
                GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "Has LandingSpot", guistyle ); ypos+=yplus; // + currentPort.name // + landingSpot.spot.ToString()
            }
            if(gettingDistanceFromTarget)
            {
                GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "Getting Distance", guistyle );  ypos+=yplus; // + currentPort.name // + gettingDistanceTimer.ToString("f1")
            }

            // if(currentPort != null)
            // {
            //     GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "Haz CurrentPort ", guistyle );  ypos+=yplus; // + currentPort.transform.position.ToString()
            // }
        }
    }

}
