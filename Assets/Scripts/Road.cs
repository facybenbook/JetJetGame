using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer
{
    public class Road : MonoBehaviour
    {
        void Start()
        {
            //StartCoroutine( RoadLaser() );

        }


        // IEnumerator RoadLaser()
        // {
        //     yield return 0;
        //
        //     float dist = transform.localScale.z;
        //     Vector3 start = transform.position - (transform.forward * (transform.localScale.z/2f));
        //
        //     start += new Vector3(0, 1f, 0);
        //
        //     RaycastHit[] hits;
        //     hits = Physics.RaycastAll(start, transform.forward, dist);
        //
        //     Debug.DrawLine(start, start + (transform.forward * dist), Color.green);
        //
        //     for (int i = 0; i < hits.Length; i++)
        //     {
        //         RaycastHit hit = hits[i];
        //         CityBuilding build = hit.transform.GetComponentInParent<CityBuilding>();
        //         if(build)
        //         {
        //             GPUInstancerPrefab buildPrefab = build.GetComponent<GPUInstancerPrefab>();
        //             if (buildPrefab)
        //             {
        //                 print("HIT " + start);
        //                 Debug.DrawLine(hit.transform.position, hit.transform.position + new Vector3(0, 1000f, 0), Color.red);
        //                 Destroy(buildPrefab.gameObject);
        //             }
        //         }
        //     }
        //
        // }

        void Update()
        {

        }


    }
}
