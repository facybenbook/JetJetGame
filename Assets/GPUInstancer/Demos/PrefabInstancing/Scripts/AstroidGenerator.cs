using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer
{
    public class AstroidGenerator : MonoBehaviour
    {
        public float gridWidth = 1000;
        public float gridDepth = 1000;
        public float blockSize = 10;

        private int step;

        public List<GPUInstancerPrefab> asteroidObjects = new List<GPUInstancerPrefab>();
        public GPUInstancerPrefabManager prefabManager;

        private List<GPUInstancerPrefab> asteroidInstances = new List<GPUInstancerPrefab>();
        private int instantiatedCount;
        private Vector3 allocatedPos;
        private Quaternion allocatedRot;
        private GPUInstancerPrefab allocatedGO;
        private GameObject goParent;


        public Rigidbody Target = null;
        float TargetDistance = Mathf.Infinity;
        float CullDistance = 35000f;



        private void RunGenerator()
        {
            instantiatedCount = 0;


            goParent = new GameObject("Asteroids");
            goParent.transform.position = gameObject.transform.position;
            goParent.transform.parent = gameObject.transform;

            asteroidInstances.Clear();

            // find the object with the largest footprint
            //


            // foreach(GPUInstancerPrefab go in asteroidObjects)
            //     print(go.GetComponent<Renderer>().bounds.size.x);



            for (float d = 0; d < gridDepth; d += blockSize)
            {
                for (float w = 0; w < gridWidth; w += blockSize)
                {

                    allocatedPos = new Vector3(transform.position.x + w, 0, transform.position.z + d);
                    allocatedRot = Quaternion.identity;

                    float pD = (d/blockSize) + 0.5f;
                    float pW = (w/blockSize) + 0.5f;

                    float n = Mathf.PerlinNoise(pD, pW);
                    n *= 10f;
                    if( n<2)
                        allocatedGO = Instantiate(asteroidObjects[0], allocatedPos, allocatedRot);
                    else if( n < 4)
                        allocatedGO = Instantiate(asteroidObjects[1], allocatedPos, allocatedRot);
                    else if( n < 6)
                        allocatedGO = Instantiate(asteroidObjects[2], allocatedPos, allocatedRot);
                    else if( n < 8)
                        allocatedGO = Instantiate(asteroidObjects[3], allocatedPos, allocatedRot);
                    else if( n < 10)
                        allocatedGO = Instantiate(asteroidObjects[4], allocatedPos, allocatedRot);

                    allocatedGO.transform.parent = goParent.transform;

                    instantiatedCount++;

                    asteroidInstances.Add( allocatedGO );





                }
            }


            //
            //     float dist = 500f;
            //     RaycastHit hit;
            //     Vector3 start = (transform.position + new Vector3(0, 10f, 0)) + (transform.forward * (blockSize * h));
            //
            //     if( Physics.Raycast(start, transform.forward, out hit, step)) { // raycast from above to detect terrain collisions
            //
            //         print(hit.collider.tag);
            //         // if( hit.collider.tag == "Terrain") {
            //             //currentTerrainY = hit.point.y;
            //         // }
            //     }
            //     else {
            //         //currentTerrainY = 0;
            //     }
            //
            //     Debug.DrawLine(start, start + (transform.forward * (step * h)), Color.yellow);
            //
            //     Debug.Break();
            //
            //
            //     asteroidInstances.Add(InstantiateOnRay(transform.position, transform.forward, h * step));
            //
            // }


            Debug.Log("Instantiated " + instantiatedCount + " objects.");
        }

        private void Start()
        {

             RunGenerator();

            if (prefabManager != null && prefabManager.gameObject.activeSelf && prefabManager.enabled)
            {
                GPUInstancerAPI.RegisterPrefabInstanceList(prefabManager, asteroidInstances);
                GPUInstancerAPI.InitializeGPUInstancer(prefabManager);
            }

            //InvokeRepeating("Observe", 0, 0.5f);

        }

        void Observe() {

            // if(Target == null)
            //     return;
            //
            // TargetDistance = Vector3.Distance(transform.position, Target.transform.position);
            //
            //
            // if ( Vector3.Dot(  Game.I.CamTrk.GetRailForward(),  Target.transform.position - transform.position  ) > 0 )
            // {
            //     // Behind frame
            //     // Cull
            //
            //     GPUInstancerAPI.UnregisterPrefabInstanceList(prefabManager, asteroidInstances);
            //     GPUInstancerAPI.InitializeGPUInstancer(prefabManager);
            //
            //     Destroy(gameObject);
            // }
            //
            //
            // print("d " + TargetDistance + " " + Vector3.Dot(  Game.I.CamTrk.GetRailForward(),  Target.transform.position - transform.position  ) );

        }

        private GPUInstancerPrefab InstantiateOnRay(Vector3 start, Vector3 dir, float dist)
        {
            allocatedPos = start + (dir * dist);
            allocatedRot = Quaternion.identity; //Quaternion.FromToRotation(Vector3.forward, start - allocatedPos);

            // cast a ray from start


            allocatedGO = Instantiate(asteroidObjects[Random.Range(0, asteroidObjects.Count)], allocatedPos, allocatedRot);

            allocatedGO.transform.parent = goParent.transform;

            // allocatedLocalEulerRot.x = Random.Range(-180f, 180f);
            // allocatedLocalEulerRot.y = Random.Range(-180f, 180f);
            // allocatedLocalEulerRot.z = Random.Range(-180f, 180f);
            // allocatedGO.transform.localRotation = Quaternion.Euler(allocatedLocalEulerRot);

            // allocatedLocalScaleFactor = Random.Range(0.3f, 1.2f);
            // allocatedLocalScale.x = allocatedLocalScaleFactor;
            // allocatedLocalScale.y = allocatedLocalScaleFactor;
            // allocatedLocalScale.z = allocatedLocalScaleFactor;
            // allocatedGO.transform.localScale = allocatedLocalScale;

            instantiatedCount++;

            return allocatedGO;
        }
    }
}
