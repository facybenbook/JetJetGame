using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortAuthority : MonoBehaviour {

    public static PortAuthority I;

    private List<Port> ports = new List<Port>();
    private List<Commuter> airliners = new List<Commuter>();

    //

    private void Awake() {
        I = this;
    }

    void Start () {
        RefreshCommuter();
        RefreshPorts();
    }

    //

    public void RefreshCommuter()
    {
        airliners.Clear();
        foreach( object go in GameObject.FindObjectsOfType(typeof(Commuter)) )
            airliners.Add((Commuter)go);
    }

    public void RefreshPorts()
    {
        ports.Clear();
        foreach( object go in GameObject.FindObjectsOfType(typeof(Port)) )
            ports.Add((Port)go);
    }

    public Port ClosestPort(Vector3 pos)
    {
        Port currentClosest = null;
        float currentDistance = -1;
        foreach (Port a in ports)
        {
            float dist = Vector3.Distance(a.transform.position, pos);
            if( currentDistance == -1 || dist < currentDistance)
            {
                currentClosest = a;
                currentDistance = dist;
            }
        }
        return currentClosest;
    }

    public Port RandomNewPort(Vector3 currentPortPos)
    {
        List<Port> newPorts = new List<Port>();

        foreach(Port p in ports)
            if( p.transform.position != currentPortPos && p.HasFreeSpots())
                newPorts.Add(p);

        if(newPorts.Count > 0)
            return newPorts[UnityEngine.Random.Range(0, newPorts.Count - 1)];
        else
            return null;
    }

}
