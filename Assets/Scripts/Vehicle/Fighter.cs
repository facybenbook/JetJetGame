using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Airplane
{

    GUIStyle guistyle = new GUIStyle();
    public bool DrawGUI = false;

    private Rigidbody Target = null;
    float TargetDistance = Mathf.Infinity;
    float CullDistance = 35000f;

    public Vector3 IdleDirection;

    public float AttackDistance;
    public float attackTime = 0.1f;
    private float attackTimer;

    public GameObject Projectile;



    private M Mode;
    private enum M {
        Observing,
        Attacking
    }

    //

    void Start()
    {
        InvokeRepeating("Observe", 0, 0.5f);
    }

    public override void Update()
    {

        base.UpdateTick();

        if( Target != null && Mode == M.Attacking)
        {
            base.AimPos = Target.transform.position;

            if( attackTimer <= 0 && InFOV() && InFireRange() )
                Fire();
            else
                attackTimer -= Time.deltaTime;
        }
    }

    //

    public void SetTarget(Rigidbody trg) {
        //print("SetTarget");

        Target = trg;
        GetComponent<Cull>().Target = trg;
    }


    void Observe() {
        //print("Observe Fighter");

        if(Target == null)
            return;

        TargetDistance = Vector3.Distance(transform.position, Target.transform.position);

        //print("d " + TargetDistance + " " + Vector3.Dot(  Game.I.CamTrk.GetRailForward(),  Target.transform.position - transform.position  ) );

        if( Mode != M.Observing)
            return;

        if( InFireRange())
            Attack();
        else
            Idle();

    }

    void Idle()
    {
        // print("Idle");

        Mode = M.Observing;
        base.AimPos = transform.position + (IdleDirection.normalized * 1000f);
    }

    void Attack()
    {
        // print("Attack");

        Mode = M.Attacking;
        base.AimPos = Target.transform.position +  (Target.velocity);
    }

    bool InFireRange() {
        return TargetDistance < AttackDistance;
    }

    bool InCullRange() {
        return TargetDistance < CullDistance;
    }

    bool InFOV() {
        return Vector3.Angle(transform.forward, Target.transform.position - transform.position) < 45f;
    }

    void Fire()
    {
        // ** LASER **
        Projectile projectile = GameObject.Instantiate(Projectile).GetComponent<Projectile>();
        projectile.team = "Airplane";
        projectile.transform.position = transform.position;
        projectile.transform.LookAt( Target.transform.position + (Target.velocity) );
        GameObject.Destroy(projectile.gameObject, 6f);

        attackTimer = attackTime;
    }


    //

    void OnGUI()
    {
        if(!DrawGUI) return;

        guistyle.normal.textColor = Color.blue;
        guistyle.alignment = TextAnchor.MiddleCenter;

        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        int yplus = 20;  int ypos = yplus;

        // if( TargetDistance < 4000)
        //     guistyle.fontSize = 24;
        // else if( TargetDistance < 5000)
        //     guistyle.fontSize = 12;
        // else if( TargetDistance < 8000)
        //     guistyle.fontSize = 8;
        // else if( TargetDistance < 100000)
        //     guistyle.fontSize = 6;
        // else if( TargetDistance < 200000)
        //     guistyle.fontSize = 4;
        // else
        //     guistyle.fontSize = 2;

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

        GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y - ypos, 80, 25), Mode.ToString(), guistyle ); ypos+=yplus;

    }


}
