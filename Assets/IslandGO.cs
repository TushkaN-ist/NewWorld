using UnityEngine;
using System.Collections;
using GenerateTest01;

public class IslandGO : MonoBehaviour {
    Island _island;
    public float wight;
    public Island island
    {
        get
        {
            return _island;
        }
        set
        {
            _island = value;
            name = _island.Name + "_" + _island.Type;
            wight = _island.Weight;
            transform.position=new Vector3(_island.x,_island.y);
        }
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        for (int c = 0; c < transform.childCount; ++c)
        {
            Debug.DrawLine(transform.position, transform.GetChild(c).position);
            
        }
	    
	}
}
