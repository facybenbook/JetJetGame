#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CityMake : MonoBehaviour
{
    public float gridWidth = 32000f;
    public float gridHeight = 32000f;
    public float gridDepth = 120000f;


    // Perlin seed
    public int seed = 0;

    int totalMade = 0;


    [Header("1000")]
    public List<GameObject> o1000VeryTall = new List<GameObject>();
    public List<GameObject> o1000Tall = new List<GameObject>();
    public List<GameObject> o1000Medium = new List<GameObject>();
    public List<GameObject> o1000Short = new List<GameObject>();
    public List<GameObject> o1000VeryShort = new List<GameObject>();
    public List<GameObject> o1000Ground = new List<GameObject>();

    int i1000VeryTall = 0;
    int i1000Tall = 0;
    int i1000Medium = 0;
    int i1000Short = 0;
    int i1000VeryShort = 0;
    int i1000Ground = 0;

    [Header("2000")]
    public List<GameObject> o2000VeryTall = new List<GameObject>();
    public List<GameObject> o2000Tall = new List<GameObject>();
    public List<GameObject> o2000Medium = new List<GameObject>();
    public List<GameObject> o2000Short = new List<GameObject>();
    public List<GameObject> o2000VeryShort = new List<GameObject>();
    public List<GameObject> o2000Ground = new List<GameObject>();

    int i2000VeryTall = 0;
    int i2000Tall = 0;
    int i2000Medium = 0;
    int i2000Short = 0;
    int i2000VeryShort = 0;
    int i2000Ground = 0;

    [Header("4000")]
    public List<GameObject> o4000VeryTall = new List<GameObject>();
    public List<GameObject> o4000Tall = new List<GameObject>();
    public List<GameObject> o4000Medium = new List<GameObject>();
    public List<GameObject> o4000Short = new List<GameObject>();
    public List<GameObject> o4000VeryShort = new List<GameObject>();
    public List<GameObject> o4000Ground = new List<GameObject>();

    int i4000VeryTall = 0;
    int i4000Tall = 0;
    int i4000Medium = 0;
    int i4000Short = 0;
    int i4000VeryShort = 0;
    int i4000Ground = 0;


    [Header("16000")]
    public List<GameObject> o16000All = new List<GameObject>();
    int i16000All = 0;


    Vector3 blockPos;
    Quaternion blockRot;
    GameObject cityGO;
    GameObject roadsGO;
    GameObject prefabGO;

    bool NeedsDetect = false;
    bool NeedsShowPlacement = false;
    bool NeedsPlacement = false;

    //

    void Start()
    {
        // In play mode hide prefabs
        if(transform.Find("Prefab"))
            Destroy( transform.Find("Prefab").gameObject );
    }

    //

    void MakeRoadsObj()
    {
        if(transform.Find("Roads"))
            roadsGO = transform.Find("Roads").gameObject;

        if (roadsGO == null)
        {
            roadsGO = new GameObject("Roads");
            roadsGO.transform.position = gameObject.transform.position;
            roadsGO.transform.parent = gameObject.transform;
        }

    }

    void MakeBuildObj()
    {
        if(transform.Find("_Build"))
            cityGO = transform.Find("_Build").gameObject;

        if (cityGO == null)
        {
            cityGO = new GameObject("_Build");
            cityGO.transform.position = gameObject.transform.position;
            cityGO.transform.parent = gameObject.transform;
        }
    }

    public void DetectPrefabs()
    {
        NeedsDetect = true;

    }

    public void Compile()
    {
        NeedsPlacement = true;

    }

    public bool IsCompiled()
    {
        return transform.Find("_Build") && (cityGO.transform.childCount != 0);
    }

    public void ShowPlacements()
    {
        NeedsShowPlacement = true;

    }

    public void HidePlacements()
    {
        NeedsShowPlacement = false;

    }

    public bool IsShowPlacements()
    {
        return NeedsShowPlacement;
    }



    public void DoPlacement()
    {


        //PerlinMake();
        BorderMake();

        NeedsPlacement = false;
    }

    public void Decompile()
    {
        if(transform.Find("_Build"))
            DestroyImmediate( transform.Find("_Build").gameObject );
    }

    void PerlinMake()
    {
        // Size of starting block
        float blockSize = 4000f;

        float perlin = 0;

        Vector3 origin = transform.position + ( transform.right * (blockSize/2f) );
        Vector3 offs = new Vector3(0, 0, 0);
        Vector3 offs2 = new Vector3(0, 0, 0);

        for (float d = 0; d < gridDepth; d += blockSize)
        {
            for (float w = 0; w < gridWidth; w += blockSize)
            {
                blockPos = origin + (transform.right * w) + (transform.forward * d);
                blockRot = Quaternion.identity;
                Debug.DrawLine(blockPos, blockPos + new Vector3(0, 1000f, 0), Color.white);

                float pD = (d/blockSize) + 0.5f;
                float pW = (w/blockSize) + 0.5f;

                perlin = Mathf.PerlinNoise(seed + pD, seed + pW);
                perlin *= 10f;
                if( perlin < 3 )
                    // VeryTall
                    PrintSquare( 4000f, blockPos, "VeryTall" );
                else if( perlin < 4)
                    // Tall
                    PrintSquare( 4000f, blockPos, "Tall" );
                else if( perlin < 5)
                    // Medium
                    PrintSquare( 4000f, blockPos, "Medium" );
                else if( perlin < 6)
                    // Short
                    PrintSquare( 4000f, blockPos, "Short" );
                else if( perlin < 7 )
                    // VeryShort
                    PrintSquare( 4000f, blockPos, "VeryShort" );
                else
                    // Ground
                    PrintSquare( 4000f, blockPos, "Ground" );
            }
        }

        if(NeedsPlacement)
            Debug.Log("CityMake made " + totalMade + " objects");
    }


    void PrintSquare( float size, Vector3 pos, string height )
    {
        if(size < 1000f)
            return;

        GameObject prefab;

        if( IsClear(pos, size) )
        {

            Debug.DrawLine(pos, pos + new Vector3(0, 10f, 0), Color.white);

            if(!NeedsPlacement)
                return;

            if(size >= 16000f)
            {
                // 16000 block
                // if(height == "VeryTall")
                // {
                    prefab = o16000All[i16000All%=o16000All.Count];
                    i16000All++;
                // }
            }
            else if(size >= 4000f)
            {
                // 4000 block
                if(height == "VeryTall")
                {
                    prefab = o4000VeryTall[i4000VeryTall%=o4000VeryTall.Count];
                    i4000VeryTall++;
                }
                else if(height == "Tall")
                {
                    prefab = o4000Tall[i4000Tall%=o4000Tall.Count];
                    i4000Tall++;
                }
                else if(height == "Medium")
                {
                    prefab = o4000Medium[i4000Medium%=o4000Medium.Count];
                    i4000Medium++;
                }
                else if(height == "Short")
                {
                    prefab = o4000Short[i4000Short%=o4000Short.Count];
                    i4000Short++;
                }
                else if(height == "VeryShort")
                {
                    prefab = o4000VeryShort[i4000VeryShort%=o4000VeryShort.Count];
                    i4000VeryShort++;
                }
                else
                {
                    // Ground
                    prefab = o4000Ground[i4000Ground%=o4000Ground.Count];
                    i4000Ground++;
                }
            }
            else if(size >= 2000f)
            {
                // 2000 block
                if(height == "VeryTall")
                {
                    prefab = o2000VeryTall[i2000VeryTall%=o2000VeryTall.Count];
                    i2000VeryTall++;
                }
                else if(height == "Tall")
                {
                    prefab = o2000Tall[i2000Tall%=o2000Tall.Count];
                    i2000Tall++;
                }
                else if(height == "Medium")
                {
                    prefab = o2000Medium[i2000Medium%=o2000Medium.Count];
                    i2000Medium++;
                }
                else if(height == "Short")
                {
                    prefab = o2000Short[i2000Short%=o2000Short.Count];
                    i2000Short++;
                }
                else if(height == "VeryShort")
                {
                    prefab = o2000VeryShort[i2000VeryShort%=o2000VeryShort.Count];
                    i2000VeryShort++;
                }
                else
                {
                    // Ground
                    prefab = o2000Ground[i2000Ground%=o2000Ground.Count];
                    i2000Ground++;
                }
            }
            else
            {
                // 1000 block
                if(height == "VeryTall")
                {
                    prefab = o1000VeryTall[i1000VeryTall%=o1000VeryTall.Count];
                    i1000VeryTall++;
                }
                else if(height == "Tall")
                {
                    prefab = o1000Tall[i1000Tall%=o1000Tall.Count];
                    i1000Tall++;
                }
                else if(height == "Medium")
                {
                    prefab = o1000Medium[i1000Medium%=o1000Medium.Count];
                    i1000Medium++;
                }
                else if(height == "Short")
                {
                    prefab = o1000Short[i1000Short%=o1000Short.Count];
                    i1000Short++;
                }
                else if(height == "VeryShort")
                {
                    prefab = o1000VeryShort[i1000VeryShort%=o1000VeryShort.Count];
                    i1000VeryShort++;
                }
                else
                {
                    // Ground
                    prefab = o1000Ground[i1000Ground%=o1000Ground.Count];
                    i1000Ground++;
                }
            }

            if( prefab == null )
            {
                Debug.Log("Error instantiating building height: "+height + " size: "+size);
            }
            else
            {
                // Instantiate the prefab
                string assetpath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
                GameObject go = AssetDatabase.LoadAssetAtPath( assetpath, typeof(GameObject)) as GameObject;
                GameObject allocatedGO = PrefabUtility.InstantiatePrefab(go) as GameObject;

                allocatedGO.transform.position = pos;
                allocatedGO.transform.rotation = Quaternion.identity;
                allocatedGO.transform.parent = cityGO.transform;
                totalMade++;
            }

            return;
        }
        else
        {
            float block = size/2f;
            float quadrant = size/4f;

            PrintSquare( block, pos + ( transform.right * quadrant) + ( transform.forward * quadrant), height );
            PrintSquare( block, pos + ( transform.right * quadrant) + (-transform.forward * quadrant), height );
            PrintSquare( block, pos + (-transform.right * quadrant) + (-transform.forward * quadrant), height );
            PrintSquare( block, pos + (-transform.right * quadrant) + ( transform.forward * quadrant), height );
        }
    }

    void BorderMake()
    {
        float borderBuildingSize = 16000f;
        for (float d = 0; d < gridDepth; d += borderBuildingSize)
        {
            PrintSquare( borderBuildingSize, transform.position + (-transform.right * (borderBuildingSize/2f)) + (transform.forward * d), "VeryTall");
            PrintSquare( borderBuildingSize, transform.position + (transform.right * (gridWidth + (borderBuildingSize/2f)) ) + (transform.forward * d), "VeryTall");
        }
    }

    bool IsClear(Vector3 pos, float size)
    {

        if(size >= 16000f)
            return true;

        // Check this position for Space
        int layerMask = 1 << 8; // RoadNetwork
        Vector3 raydir = transform.forward;
        RaycastHit rayhit;
        Vector3 rayCorner = new Vector3(0, 0, 0);
        float quadrant = size/2f;
        float hypotenuse = Mathf.Sqrt( (quadrant * quadrant) + (quadrant * quadrant) );
        bool clear = true;

        // 4 rays around perimiter
        for(int r=0; r<4; r++)
        {
            if(r==0) {        rayCorner = pos + ( transform.right * quadrant) + ( transform.forward * quadrant); raydir = -transform.forward;   Debug.DrawLine(rayCorner, rayCorner + (raydir * size), Color.green); }
            else if(r==1) {   rayCorner = pos + ( transform.right * quadrant) + (-transform.forward * quadrant); raydir = -transform.right;     Debug.DrawLine(rayCorner, rayCorner + (raydir * size), Color.green); }
            else if(r==2) {   rayCorner = pos + (-transform.right * quadrant) + (-transform.forward * quadrant); raydir = transform.forward;    Debug.DrawLine(rayCorner, rayCorner + (raydir * size), Color.green); }
            else if(r==3) {   rayCorner = pos + (-transform.right * quadrant) + ( transform.forward * quadrant); raydir = transform.right;      Debug.DrawLine(rayCorner, rayCorner + (raydir * size), Color.green);  }

            if( Physics.Raycast(rayCorner, raydir, out rayhit, quadrant, layerMask))
            {
                Debug.DrawLine(rayCorner, rayhit.point, Color.red);
                clear = false;
            }
        }

        // 4 rays from center to corners
        for(int r=0; r<4; r++)
        {
            if(r==0) {   rayCorner = pos; raydir = (transform.forward + transform.right).normalized;                Debug.DrawLine(rayCorner, rayCorner + (raydir * hypotenuse), Color.green);  }
            else if(r==1) {   rayCorner = pos; raydir = (-transform.forward + transform.right).normalized;          Debug.DrawLine(rayCorner, rayCorner + (raydir * hypotenuse), Color.green);  }
            else if(r==2) {   rayCorner = pos; raydir = (-transform.forward + -transform.right).normalized;         Debug.DrawLine(rayCorner, rayCorner + (raydir * hypotenuse), Color.green);  }
            else if(r==3) {   rayCorner = pos; raydir = (transform.forward + -transform.right).normalized;          Debug.DrawLine(rayCorner, rayCorner + (raydir * hypotenuse), Color.green);  }

            if( Physics.Raycast(rayCorner, raydir, out rayhit, hypotenuse, layerMask))
            {
                Debug.DrawLine(rayCorner, rayhit.point, Color.red);
                clear = false;
            }
        }


        if(clear)
        {
            //Debug.DrawLine(pos, pos + new Vector3(0, 12500f, 0), Color.white);
        }
        else
        {
            Debug.DrawLine(pos, pos + new Vector3(0, 12500f, 0), Color.red);
        }

        return clear;
    }

    //////////
    //  Sampling Prefabs in the area
    //  Of the type "500_ 1000_ 2000_ 4000_"
    //  And attaching to this objects building arrays
    //  Based on their height and width
    /////////

    int IsTaller(GameObject a, GameObject b)
    {
        return (a.transform.GetComponent<Renderer>().bounds.size.y).CompareTo( b.transform.GetComponent<Renderer>().bounds.size.y );
    }

    void RefreshBuildArrays(List<GameObject> hit1000, List<GameObject> hit2000, List<GameObject> hit4000, List<GameObject> hit16000)
    {
        // Sort based on height, shortest first
        hit1000.Sort(delegate(GameObject a, GameObject b) { return IsTaller(a, b); });
        hit2000.Sort(delegate(GameObject a, GameObject b) { return IsTaller(a, b); });
        hit4000.Sort(delegate(GameObject a, GameObject b) { return IsTaller(a, b); });
        hit16000.Sort(delegate(GameObject a, GameObject b) { return IsTaller(a, b); });

        // Chunk the lists
        int numTallArrays = 6;
        int chunksize = 0;
        int j;

        j=0;
        chunksize = (int)Mathf.Floor(hit1000.Count / (float)numTallArrays);
        o1000Ground = hit1000.GetRange(j, chunksize); j += chunksize;
        o1000VeryShort = hit1000.GetRange(j, chunksize); j += chunksize;
        o1000Short = hit1000.GetRange(j, chunksize); j += chunksize;
        o1000Medium = hit1000.GetRange(j, chunksize); j += chunksize;
        o1000Tall = hit1000.GetRange(j, chunksize); j += chunksize;
        o1000VeryTall = hit1000.GetRange(j, hit1000.Count - j);

        j=0;
        chunksize = (int)Mathf.Floor(hit2000.Count / (float)numTallArrays);
        o2000Ground = hit2000.GetRange(j, chunksize); j += chunksize;
        o2000VeryShort = hit2000.GetRange(j, chunksize); j += chunksize;
        o2000Short = hit2000.GetRange(j, chunksize); j += chunksize;
        o2000Medium = hit2000.GetRange(j, chunksize); j += chunksize;
        o2000Tall = hit2000.GetRange(j, chunksize); j += chunksize;
        o2000VeryTall = hit2000.GetRange(j, hit2000.Count - j);

        j=0;
        chunksize = (int)Mathf.Floor(hit4000.Count / (float)numTallArrays);
        o4000Ground = hit4000.GetRange(j, chunksize); j += chunksize;
        o4000VeryShort = hit4000.GetRange(j, chunksize); j += chunksize;
        o4000Short = hit4000.GetRange(j, chunksize); j += chunksize;
        o4000Medium = hit4000.GetRange(j, chunksize); j += chunksize;
        o4000Tall = hit4000.GetRange(j, chunksize); j += chunksize;
        o4000VeryTall = hit4000.GetRange(j, hit4000.Count - j);

        j=0;
        o16000All = hit16000.GetRange(j, hit16000.Count - j);

        // Reverse so tallest first
        hit16000.Reverse();
        hit4000.Reverse();
        hit2000.Reverse();
        hit1000.Reverse();

        // Position the objects
        float spaceBelowGround = 5000f;
        int iter;
        Vector3 objpos = transform.position +
                        (-transform.up * (gridHeight + spaceBelowGround)) +
                        (transform.right * gridWidth) +
                        (transform.forward * 3000f);

        iter = 0;
        objpos += (-transform.right * 2000f);
        Vector3 upAdjust = new Vector3(0, 100f, 0);
        foreach(GameObject go in hit16000)
        {
            go.transform.position = objpos + upAdjust + (iter++ * (transform.forward * 16000f));
            go.transform.parent = prefabGO.transform;
            go.transform.localRotation = Quaternion.Euler(180f, 0, 0);
        }

        iter = 0;
        objpos += (-transform.right * 8000f);
        foreach(GameObject go in hit4000)
        {
            go.transform.position = objpos + (iter++ * (transform.forward * 4000f));
            go.transform.parent = prefabGO.transform;
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        iter = 0;
        objpos += (-transform.right * 8000f);
        foreach(GameObject go in hit2000)
        {
            go.transform.position = objpos + (iter++ * (transform.forward * 3000f));
            go.transform.parent = prefabGO.transform;
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        iter = 0;
        objpos += (-transform.right * 8000f);
        foreach(GameObject go in hit1000)
        {
            go.transform.position = objpos + (iter++ * (transform.forward * 2000f));
            go.transform.parent = prefabGO.transform;
            go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }


    }

    void PrefabBoxCast(Matrix4x4 rotMatrix)
    {
        if(transform.Find("Prefab"))
            prefabGO = transform.Find("Prefab").gameObject;

        if (prefabGO == null)
        {
            prefabGO = new GameObject("Prefab");
            prefabGO.transform.position = gameObject.transform.position;
            prefabGO.transform.parent = gameObject.transform;
        }

        // Cube for sampling and attaching prefabs
        // in the area of this box cast
        float spaceBelowGround = 5000f;
        float bottomPlatformHeight = 200f;
        float percentDepth = 0.8f;
        float percentWidth = 1.2f;
        float opacity = 0.01f;

        if(SceneView.currentDrawingSceneView.camera.transform.position.y < 0)
            opacity = 0.1f;

        float boxDepth = gridDepth * percentDepth;
        float boxWidth = gridWidth * percentWidth;
        Vector3 boxCenter = transform.position +
                            (transform.right * (boxWidth/2f)) +
                            (-transform.up * (gridHeight + spaceBelowGround)) +
                            (transform.forward * (boxDepth/2f) );

        Gizmos.color = new Color(0, 0, 1f, opacity);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero + (-transform.up * (bottomPlatformHeight/2f)), new Vector3(boxWidth, bottomPlatformHeight, boxDepth));
        Gizmos.color = new Color(0, 0, 1f, opacity);
        Gizmos.DrawWireCube(Vector3.zero + (transform.up * gridHeight/2f), new Vector3(boxWidth, gridHeight, boxDepth));
        Gizmos.matrix = rotMatrix;

        // Perform the Box Cast within Cube
        float boxStepSize = 1f;
        Vector3 boxStart = transform.position +
                            (transform.right * (boxWidth/2f)) +
                            (-transform.up * ((gridHeight/2f) + spaceBelowGround));

        RaycastHit[] hits = Physics.BoxCastAll( boxStart, new Vector3(boxWidth/2f, gridHeight/2f, boxStepSize), transform.forward, transform.rotation, boxDepth );
        if(hits.Length > 0)
        {
            List<GameObject> hit1000 = new List<GameObject>();
            List<GameObject> hit2000 = new List<GameObject>();
            List<GameObject> hit4000 = new List<GameObject>();
            List<GameObject> hit16000 = new List<GameObject>();

            foreach(RaycastHit hit in hits)
            {
                string[] prefix = hit.transform.name.Split('_');
                //Debug.DrawRay(boxStart, transform.forward * boxDepth, new Color(0, 1f, 1f, 0.4f) );
                Debug.DrawLine(hit.transform.position, hit.transform.position + ( Vector3.up * 15000f), new Color(1f, 0, 0, 0.01f));

                if(NeedsDetect)
                {
                    bool IsPrefab = true;

                    foreach(Transform child in prefabGO.transform)
                        child.transform.parent = null;

                    if( PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(hit.transform.gameObject) == "" )
                        IsPrefab = false;
                    else if(prefix[0] == "1000")
                        hit1000.Add(hit.transform.gameObject);
                    else if(prefix[0] == "2000")
                        hit2000.Add(hit.transform.gameObject);
                    else if(prefix[0] == "4000")
                        hit4000.Add(hit.transform.gameObject);
                    else if(prefix[0] == "16000")
                        hit16000.Add(hit.transform.gameObject);
                    else
                        IsPrefab = false;

                    if(!IsPrefab)
                    {
                        Debug.Log(hit.transform.name + " is not a prefab, or not named 1000_ 2000_ 4000_ 16000_");
                        hit.transform.position = new Vector3(hit.transform.position.x, -gridHeight * 2f, hit.transform.position.z);
                        hit.transform.parent = null;
                    }
                }
            }

            if(NeedsDetect)
                RefreshBuildArrays(hit1000, hit2000, hit4000, hit16000);
        }
        NeedsDetect = false;

    }


    void StartBox(Matrix4x4 rotMatrix)
    {
        // Cube for showing start of run
        float cubeDepth = 200f;

        Vector3 boxCenter = transform.position +
                            (transform.right * (gridWidth/2f)) +
                            (transform.up * (gridHeight/2f)) +
                            (transform.forward * (cubeDepth/2f) );

        Gizmos.color = new Color(0, 1f, 0, 0.6f);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridWidth, gridHeight, cubeDepth));
        Gizmos.matrix = rotMatrix;

        // draw start pos
        Gizmos.color = new Color(0, 1f, 0, 0.8f);
        Gizmos.DrawSphere(transform.position, 2500f);
    }


    void MainrunBox(Matrix4x4 rotMatrix)
    {
        // Cube for showing the run proper
        Vector3 boxCenter = transform.position +
                            (transform.right * (gridWidth/2f)) +
                            (transform.up * (gridHeight/2f)) +
                            (transform.forward * (gridDepth/2f) );

        Gizmos.color = new Color(0, 0, 1f, 0.2f);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridWidth, gridHeight, gridDepth));
        Gizmos.matrix = rotMatrix;
    }


    void EndBox(Matrix4x4 rotMatrix)
    {
        // Cube for showing end of run
        float cubeDepth = 200f;

        Vector3 boxCenter = transform.position +
                            (transform.forward * gridDepth) +
                            (transform.right * (gridWidth/2f)) +
                            (transform.up * (gridHeight/2f)) +
                            (transform.forward * (cubeDepth/2f) );

        Gizmos.color = new Color(1f, 0, 0, 0.01f);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridWidth, gridHeight, cubeDepth));
        Gizmos.matrix = rotMatrix;
    }


    void OnDrawGizmos()
	{
        if (Application.isPlaying)
            return;

        Matrix4x4 rotMatrix = Gizmos.matrix;
        Color gizColor = Gizmos.color;

        if(roadsGO == null)
            MakeRoadsObj();

        if(cityGO == null)
            MakeBuildObj();


        // Boxes
        StartBox(rotMatrix);
        MainrunBox(rotMatrix);
        EndBox(rotMatrix);

        PrefabBoxCast(rotMatrix);

        if(NeedsPlacement || NeedsShowPlacement)
            DoPlacement();

        Gizmos.color = gizColor;
    }


}
#endif
