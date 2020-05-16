using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeOfVision : MonoBehaviour
{

    [SerializeField] LayerMask layerMask;
    Mesh coneOfView;
    float fieldOfView;
    float viewDistance;

    // Start is called before the first frame update
    void Start()
    {
        coneOfView = new Mesh();
        GetComponent<MeshFilter>().mesh = coneOfView;
        fieldOfView = 90;
        viewDistance = 80f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float angle = 0f;
        int rayCount = 50;
        float angleBetweenRays = fieldOfView / rayCount;

        Vector3[] verticies = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[verticies.Length];
        int[] triangles = new int[rayCount * 3];
        Vector3 origin = Vector3.zero;
        verticies[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            
            Vector3 vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null) {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else {
                vertex = raycastHit2D.point;
            }


            verticies[vertexIndex] = vertex;
            vertexIndex++;
            if (i > 0) { 
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex - 2;

                triangleIndex += 3;
            }

            coneOfView.vertices = verticies;
            coneOfView.uv = uv;
            coneOfView.triangles = triangles;
            angle = angle + angleBetweenRays;
        }
    }

    Vector3 GetVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
