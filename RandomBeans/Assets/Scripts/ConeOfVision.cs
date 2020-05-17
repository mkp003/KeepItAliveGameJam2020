using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeOfVision : MonoBehaviour
{

    [SerializeField] LayerMask layerMask;
    Mesh coneOfView;
    float fieldOfView;
    float viewDistance;
    float startingAngle;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        coneOfView = new Mesh();
        GetComponent<MeshFilter>().mesh = coneOfView;
        fieldOfView = 60;
        viewDistance = 25f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float angle = startingAngle;
        int rayCount = 300;
        float angleBetweenRays = fieldOfView / rayCount;

        Vector3[] verticies = new Vector3[rayCount + 2];
        Vector2[] uv = new Vector2[verticies.Length];
        int[] triangles = new int[rayCount * 3];
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
                Vector3 toInsideObject = GetVectorFromAngle(angle) * 0.1f;
                vertex = raycastHit2D.point + new Vector2(toInsideObject.x, toInsideObject.y);
            }


            verticies[vertexIndex] = vertex;
            vertexIndex++;
            if (i > 0) { 
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex - 2;

                triangleIndex += 3;
            }

            coneOfView.bounds = new Bounds(origin, Vector3.one * 1000f);
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

    public void SetOrigin(Vector3 newOrigin) {
        origin = newOrigin;
    }

    public void SetAimingAngle(Vector3 direction) {
        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        startingAngle = angle - (fieldOfView /2f);
    }
}
