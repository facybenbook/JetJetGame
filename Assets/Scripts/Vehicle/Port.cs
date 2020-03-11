using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {

    public GUIStyle GUIStyle;

    public string name;

    public Transform[] landingSpots;

    [System.Serializable]
    public class ParkingSpot {
        public Aircar landedAircar;
        public Transform spot;
    }
    public List<ParkingSpot> parkingSpots = new List<ParkingSpot>();

    //

    void Start () {

	}

    void Update () {

	}

    //

    public bool HasFreeSpots()
    {
        foreach (ParkingSpot s in parkingSpots)
            if (s.landedAircar == null)
                return true;

        return false;
    }

    public ParkingSpot GetFreeSpot(Aircar p) {
        foreach (ParkingSpot s in parkingSpots) {
            if (s.landedAircar == null) {
                s.landedAircar = p;
                return s;
            }
        }
        return null;
    }

    public void ClearParkingSpot(Aircar landedAircar) {
        foreach (ParkingSpot s in parkingSpots) {
            if(s.landedAircar == landedAircar) {
                s.landedAircar = null;
            }
        }
    }

    public Vector3 LandingPosition() {
        return landingSpots[0].position;
    }

    //

    void OnGUI()
    {
        return;

        Vector2 pos;

        foreach (ParkingSpot s in parkingSpots) {
            pos = Camera.main.WorldToScreenPoint(s.spot.position);

            if(s.landedAircar == null)
                GUI.Label(new Rect(pos.x - 15, Screen.height - pos.y + 15, 60, 25), "0", GUIStyle );
            else
                GUI.Label(new Rect(pos.x - 15, Screen.height - pos.y + 15, 60, 25), s.landedAircar.CarState.ToString(), GUIStyle );
        }

        pos = Camera.main.WorldToScreenPoint(transform.position);
        int yplus = 8;  int ypos = yplus;

        //GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "Total Spots " + parkingSpots.Count, GUIStyle );  ypos+=yplus;
        GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), name, GUIStyle );  ypos+=yplus;


    }


}
