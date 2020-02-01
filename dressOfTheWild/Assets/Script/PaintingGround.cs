using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingGround : MonoBehaviour
{
    public float lenghtOfThatRay = 1.5f;
    public LayerMask layerMaskIfNeeded;

    public List<Transform> raycastPoint;

    public List<GameObject> aRandomHerb = new List<GameObject>();
    public Vector3 lastSpawnPoint = Vector3.zero;

    MeshFilter meshMesh;

    public Vector3 lastPosition = Vector3.zero;

    public void RaycastGround()
    {
        if ((lastPosition - transform.position).sqrMagnitude < 0.05f)
            return;

        RaycastHit hitRay;

        Vector3 startPositionForTheRay = this.transform.position;
        Vector3 direction = Vector3.forward * lenghtOfThatRay;

        Ray rayForWall = new Ray(startPositionForTheRay, direction);//use the current upVector to check.
#if UNITY_EDITOR
        Debug.DrawRay(startPositionForTheRay, direction, Color.cyan);
#endif
        if (Physics.Raycast(rayForWall, out hitRay, lenghtOfThatRay, layerMaskIfNeeded))
        {
            int indexTriangle = hitRay.triangleIndex;
            Vector3 barycenterPoint = hitRay.barycentricCoordinate;

            //else, fetch one the mesh.
            if (meshMesh == null || meshMesh.name != hitRay.collider.name)
                meshMesh = hitRay.collider.GetComponent<MeshFilter>();

            int[] triangles = meshMesh.mesh.triangles;
            Color[] colorVertex = meshMesh.mesh.colors;
            
            colorVertex[triangles[indexTriangle * 3 + 0]] = Color.green;
            colorVertex[triangles[indexTriangle * 3 + 1]] = Color.green;
            colorVertex[triangles[indexTriangle * 3 + 2]] = Color.green;

            meshMesh.mesh.colors = colorVertex;
        }

        if((lastSpawnPoint - this.transform.position).sqrMagnitude > 1f*1f && Random.Range(0,100) < 10)
        {
            Instantiate(aRandomHerb[Random.Range(0, aRandomHerb.Count)], this.transform.position, Quaternion.identity);
        }

        lastPosition = this.transform.position;
    }
}
