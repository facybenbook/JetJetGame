using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using GPUInstancer;

public class CityMake : MonoBehaviour
{
    public float gridWidth = 32000f;
    public float gridDepth = 120000f;

    public int seed = 0;

    int total = 0;

    // [Header("20000")]
    // public List<> o20000VeryTall = new List<GameObject>();
    // int i20000VeryTall = 0;

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

    // public GPUInstancerPrefabManager prefabManager;
    // List<GPUInstancerPrefab> gpuInstances = new List<GPUInstancerPrefab>();

    float blockSize = 4000f;
    float borderSize = 20000f;

    Vector3 blockPos;
    Quaternion blockRot;
    GameObject cityGO;
    GameObject roadsGO;
    GameObject samplesGO;

    //

    void Start()
    {

    }

    //

    public void Decompile()
    {
        if(transform.Find("Builds"))
            cityGO = transform.Find("Builds").gameObject;

        DestroyImmediate(cityGO);
    }

    public void Compile()
    {


        //gpuInstances.Clear();
        if(transform.Find("_Build"))
            cityGO = transform.Find("_Build").gameObject;
        if(transform.Find("Roads"))
            roadsGO = transform.Find("Roads").gameObject;
        if(transform.Find("Samples"))
            roadsGO = transform.Find("Samples").gameObject;

        if (cityGO == null)
        {
            Debug.Log("Setup _Build gameobject");
            cityGO = new GameObject("_Build");
            cityGO.transform.position = gameObject.transform.position;
            cityGO.transform.parent = gameObject.transform;
        }

        if (roadsGO == null)
        {
            Debug.Log("Setup Roads gameobject");
            roadsGO = new GameObject("Roads");
            roadsGO.transform.position = gameObject.transform.position;
            roadsGO.transform.parent = gameObject.transform;
        }
        if (samplesGO == null)
        {
            Debug.Log("Setup Samples gameobject");
            samplesGO = new GameObject("Samples");
            samplesGO.transform.position = gameObject.transform.position;
            samplesGO.transform.parent = gameObject.transform;
        }

        // if (prefabManager != null && prefabManager.gameObject.activeSelf && prefabManager.enabled)
        // {
            PerlinMake();
            //BorderMake();

            // GPUInstancerAPI.RegisterPrefabInstanceList(prefabManager, gpuInstances);
            // GPUInstancerAPI.InitializeGPUInstancer(prefabManager);
        // }

        //Debug.Break();
    }



    public void PerlinMake()
    {

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
                {
                    // VeryTall
                    PrintSquare( 4000f, blockPos, "VeryTall" );
                }
                else if( perlin < 4)
                {
                    // Tall
                    PrintSquare( 4000f, blockPos, "Tall" );
                }
                else if( perlin < 5)
                {
                    // Medium
                    PrintSquare( 4000f, blockPos, "Medium" );
                }
                else if( perlin < 6)
                {
                    // Short
                    PrintSquare( 4000f, blockPos, "Short" );
                }
                else if( perlin < 7 )
                {
                    // VeryShort
                    PrintSquare( 4000f, blockPos, "VeryShort" );
                }
                else
                {
                    // Ground
                    PrintSquare( 4000f, blockPos, "Ground" );
                }
            }
        }

        Debug.Log("PerlinMake " + total + " objects");
    }


    void PrintSquare( float size, Vector3 pos, string height )
    {
        if(size < 1000f)
            return;

        GameObject prefab;

        Debug.DrawLine(pos, pos + new Vector3(0, 10f, 0), Color.white);

        if( IsClear(pos, size) )
        {
            if(size >= 4000f)
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
                Debug.Log("Error instantiating building height: "+height + " size: "+size);
            else
            {


                string assetpath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
                GameObject go = AssetDatabase.LoadAssetAtPath( assetpath, typeof(GameObject)) as GameObject;
                GameObject allocatedGO = PrefabUtility.InstantiatePrefab(go) as GameObject;

                allocatedGO.transform.position = pos;
                allocatedGO.transform.rotation = Quaternion.identity;
                allocatedGO.transform.parent = cityGO.transform;
                //gpuInstances.Add( allocatedGO );
                total++;
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


    bool IsClear(Vector3 pos, float size)
    {
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



    void BorderMake()
    {
        for (float d = 0; d < gridDepth; d += blockSize)
        {
            PrintSquare( borderSize, transform.position + (-transform.right * (borderSize/2f)) + (transform.forward * d), "VeryTall");
            PrintSquare( borderSize, transform.position + (transform.right * (gridWidth + (borderSize/2f)) ) + (transform.forward * d), "VeryTall");
        }
    }



    void OnDrawGizmos()
	{
        float SphereSize = 1000f;
        float SphereHeight = 20000f;
        Gizmos.color = Color.green;

        //

        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Gizmos.DrawSphere(startPos, SphereSize);
        Gizmos.DrawSphere(startPos + (transform.forward * gridDepth), SphereSize);
        Gizmos.DrawSphere(startPos + (transform.right * gridWidth), SphereSize);
        Gizmos.DrawSphere(startPos + (transform.right * gridWidth) + (transform.forward * gridDepth), SphereSize);

        Gizmos.DrawLine(startPos, startPos + (transform.forward * gridDepth));
        Gizmos.DrawLine(startPos + (transform.right * gridWidth), startPos + (transform.right * gridWidth) + (transform.forward * gridDepth));

        //

        startPos += (transform.up * SphereHeight);

        Gizmos.DrawSphere(startPos, SphereSize);
        Gizmos.DrawSphere(startPos + (transform.forward * gridDepth), SphereSize);
        Gizmos.DrawSphere(startPos + (transform.right * gridWidth), SphereSize);
        Gizmos.DrawSphere(startPos + (transform.right * gridWidth) + (transform.forward * gridDepth), SphereSize);

        Gizmos.DrawLine(startPos, startPos + (transform.forward * gridDepth));
        Gizmos.DrawLine(startPos + (transform.right * gridWidth), startPos + (transform.right * gridWidth) + (transform.forward * gridDepth));
	}



}
