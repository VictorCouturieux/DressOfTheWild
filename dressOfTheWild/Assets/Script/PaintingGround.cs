using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingGround : MonoBehaviour
{
    public float lenghtOfThatRay = 1.5f;
    public LayerMask layerMaskIfNeeded;

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



        lastPosition = this.transform.position;
    }
}
