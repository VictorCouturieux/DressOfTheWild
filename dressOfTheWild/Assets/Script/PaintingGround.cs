using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Polybrush;
using System.Linq;
using UnityEditor.Polybrush;

public class PaintingGround : MonoBehaviour
{
    public float lenghtOfThatRay = 1.5f;
    public LayerMask layerMaskIfNeeded;

    public float maxDistance = 3f;
    public AnimationCurve theFalloutOfTheCurve;

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

            //else, fetch one the mesh.
            if (meshMesh == null || meshMesh.name != hitRay.collider.name)
                meshMesh = hitRay.collider.GetComponent<MeshFilter>();
            

            int indexTriangle = hitRay.triangleIndex;
            Vector3 barycenterPoint = hitRay.barycentricCoordinate;
            int[] triangles = meshMesh.mesh.triangles;
            Vector3[] vertex = meshMesh.mesh.vertices;

            Vector3 pos = vertex[triangles[indexTriangle * 3 + 0]] ;
            pos += vertex[triangles[indexTriangle * 3 + 1]] ;
            pos += vertex[triangles[indexTriangle * 3 + 2]];
            pos /= 3f;
            
            BrushIt(meshMesh, pos, false);
            //Is there other Mesh touch ?

        }

        if ((lastSpawnPoint - this.transform.position).sqrMagnitude > 1f*1f && Random.Range(0,100) < 10)
        {
            Instantiate(aRandomHerb[Random.Range(0, aRandomHerb.Count)], this.transform.position, Quaternion.identity);
        }

        lastPosition = this.transform.position;
    }

    public void explodeIt(Vector3 transformPosition)
    {
        RaycastHit hitRay;

        Vector3 startPositionForTheRay = transformPosition;
        Vector3 direction = Vector3.forward * lenghtOfThatRay;

        Ray rayForWall = new Ray(startPositionForTheRay, direction);//use the current upVector to check.
#if UNITY_EDITOR
        Debug.DrawRay(startPositionForTheRay, direction, Color.red, 2f);
#endif
        if (Physics.Raycast(rayForWall, out hitRay, lenghtOfThatRay, layerMaskIfNeeded))
        {

            //else, fetch one the mesh.
            if (meshMesh == null || meshMesh.name != hitRay.collider.name)
                meshMesh = hitRay.collider.GetComponent<MeshFilter>();


            int indexTriangle = hitRay.triangleIndex;
            Vector3 barycenterPoint = hitRay.barycentricCoordinate;
            int[] triangles = meshMesh.mesh.triangles;
            Vector3[] vertex = meshMesh.mesh.vertices;

            Vector3 pos = vertex[triangles[indexTriangle * 3 + 0]];
            pos += vertex[triangles[indexTriangle * 3 + 1]];
            pos += vertex[triangles[indexTriangle * 3 + 2]];
            pos /= 3f;

            BrushIt(meshMesh, pos, true);
            //Is there other Mesh touch ?

        }
    }

    public void BrushIt(MeshFilter mesh, Vector3 localPosition, bool sad)
    {
        Color[] colorVertex = meshMesh.mesh.colors;
        Vector3[] vertex = meshMesh.mesh.vertices;
        int[] triangles = meshMesh.mesh.triangles;
       
        for (int indx = 0; indx < colorVertex.Length; indx++)
        {
            //Distance square
            Vector3 vec = vertex[indx] - localPosition;
            float distanceSquare = vec.sqrMagnitude;
            float value = Mathf.Max(((maxDistance - distanceSquare) / (maxDistance * maxDistance)), 0);

            value = Mathf.Max(theFalloutOfTheCurve.Evaluate(value), colorVertex[indx].g);

            colorVertex[indx] = new Color(1 - value, value, 0, 1);
        }

        meshMesh.mesh.colors = colorVertex;
    }

    
}
