using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Polybrush;
using System.Linq;
using UnityEditor.Polybrush;

public class PaintingGround : MonoBehaviour
{
    //Raycast
    public float lenghtOfThatRay = 1.5f;
    public LayerMask layerMaskIfNeeded;

    //Brush
    public float maxDistance = 3f;
    public AnimationCurve theFalloutOfTheCurve;

    //Herb
    public List<GameObject> aRandomHerb = new List<GameObject>();
    private Vector3 lastSpawnPoint = Vector3.zero;
    public Transform folderHerb;

    MeshFilter meshMesh;

    private Vector3 lastPosition = Vector3.zero;

    //Mesh with you
    public List<MeshFilter> meshTouch = new List<MeshFilter>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("default"))
        {
            MeshFilter meshFil = other.GetComponent<MeshFilter>();
            if (meshFil == null)
                Debug.LogError("WHAAAAT ???(enter)" + other.name, other.gameObject);
            if (!meshTouch.Contains(meshFil))
                meshTouch.Add(meshFil);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("default"))
        {
            MeshFilter meshFil = other.GetComponent<MeshFilter>();
            if (meshFil == null)
                Debug.LogError("WHAAAAT ???(exit)" + other.name, other.gameObject);
            if (meshTouch.Contains(meshFil))
                meshTouch.Remove(meshFil);
        }
    }




    public void RaycastGround(float velocity)
    {
        if ((lastPosition - transform.position).sqrMagnitude < 0.05f)
            return;

        Debug.Log("Raycast ?");

        RaycastHit hitRay;

        Vector3 startPositionForTheRay = this.transform.position + Vector3.back;
        Vector3 direction = Vector3.forward * lenghtOfThatRay;

        Ray rayForWall = new Ray(startPositionForTheRay, direction);//use the current upVector to check.
#if UNITY_EDITOR
        Debug.DrawRay(startPositionForTheRay, direction, Color.cyan, 3f);
#endif
        if (Physics.Raycast(rayForWall, out hitRay, lenghtOfThatRay, layerMaskIfNeeded))
        {
            Debug.Log("Raycast work !");
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

            Vector3 worldPos = meshMesh.transform.TransformPoint(pos);
            BrushIt(meshMesh, pos, maxDistance * velocity, false);
            //Is there other Mesh touch ?
            foreach (MeshFilter meshFil in meshTouch)
            {
                if(meshFil.name != meshMesh.name)
                {
                    pos = meshFil.transform.InverseTransformPoint(worldPos);
                    BrushIt(meshFil, pos, maxDistance * velocity, false);
                }
            }

        }

        if ((lastSpawnPoint - this.transform.position).sqrMagnitude > 1f*1f && Random.Range(0,100) < 10)
        {
            Instantiate(aRandomHerb[Random.Range(0, aRandomHerb.Count)], this.transform.position, Quaternion.identity, folderHerb);
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
            MeshFilter meshHere;
            meshHere = hitRay.collider.GetComponent<MeshFilter>();


            int indexTriangle = hitRay.triangleIndex;
            Vector3 barycenterPoint = hitRay.barycentricCoordinate;
            int[] triangles = meshHere.mesh.triangles;
            Vector3[] vertex = meshHere.mesh.vertices;

            Vector3 pos = vertex[triangles[indexTriangle * 3 + 0]];
            pos += vertex[triangles[indexTriangle * 3 + 1]];
            pos += vertex[triangles[indexTriangle * 3 + 2]];
            pos /= 3f;

            StartCoroutine(GrowExplosion(meshHere, pos, 0.1f, 4f));

        }
    }

    IEnumerator GrowExplosion(MeshFilter mesh, Vector3 positionCenter, float distanceDepart, float distanceMax)
    {
        while (distanceDepart < distanceMax)
        {
            BrushIt(mesh, positionCenter, distanceDepart, true);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void BrushIt(MeshFilter meshFil, Vector3 localPosition, float distance, bool sad)
    {
        Color[] colorVertex = meshFil.mesh.colors;
        Vector3[] vertex = meshFil.mesh.vertices;
        int[] triangles = meshFil.mesh.triangles;
       
        for (int indx = 0; indx < colorVertex.Length; indx++)
        {
            //Distance square
            Vector3 vec = vertex[indx] - localPosition;
            float distanceSquare = vec.sqrMagnitude;
            float value = Mathf.Max((((distance * distance) - distanceSquare) / (distance * distance)), 0);

            value = Mathf.Max(theFalloutOfTheCurve.Evaluate(value), colorVertex[indx].g);

            colorVertex[indx] = new Color(1 - value, value, 0, 1);
        }

        Debug.Log("Just paint :" + meshFil.name);

        meshFil.mesh.colors = colorVertex;
    }

    
}
