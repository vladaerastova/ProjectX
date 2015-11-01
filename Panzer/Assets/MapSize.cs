using UnityEngine;
using System.Collections;

public class MapSize : MonoBehaviour {
    public TerrainData obj;
    public Vector3 size;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        size = obj.size;
	}
}
