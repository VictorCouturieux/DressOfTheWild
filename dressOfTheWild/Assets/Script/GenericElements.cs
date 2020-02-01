using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericElements : MonoBehaviour {
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int rejectionSamples = 30;
    public float displayRadius = 1;

    public static List<Vector2> points;

    public GameObject[] objectTypes;
    public GameObject minePrefab;

    public static List<GameObject> grassList;
    public static List<GameObject> treeList;
    public static List<GameObject> mineList;

    void OnValidate() {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(regionSize / 2, regionSize);
        if (points != null) {
            foreach (Vector2 point in points) {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }

    private void Awake() {
        points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        if (points != null) {
            int index=0;
            foreach (Vector2 point in points) {
//                var randMine = Random.value;
                if (index%100 == 1) {
                    if (minePrefab != null) {
                        Instantiate(minePrefab, point, Quaternion.identity, 
                            transform.GetChild(1).transform);
                    }
                }
                else {
                    int rand = Random.Range(0, objectTypes.Length);
                    Instantiate(objectTypes[rand], point, Quaternion.identity, 
                        transform.GetChild(0).transform);
                }
                index++;
            }
        }
    }
}