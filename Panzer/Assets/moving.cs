using UnityEngine;
using System.Collections;

public class moving : MonoBehaviour {
    public GameObject obj;
    public Vector3 position;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
            transform.Translate(1, 0, 0);
        position = obj.transform.position;
	}
}
