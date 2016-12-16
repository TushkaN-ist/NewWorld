using UnityEngine;
using System.Collections;
using GenerateTest01;

public class GenerateInGame : MonoBehaviour {

    public IslandGO islandObject;

    public int islandsMin=50, lifeCicles=25;
    [Range(1f,45f)]
    public float percentDropTrash;
    public float arroundStick=3;

    Program program;
	// Use this for initialization
    IEnumerator GenerateWorld()
    {
        program = new Program();
        program.GenerateRun("100x100", islandsMin, lifeCicles, percentDropTrash, arroundStick / 20f);
        while (program.Progress<1f)
        {
            Debug.Log(program.Progress);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log(program.islandsOut.Count);
        for (int c = 0; c < program.islandsOut.Count; ++c)
        {
            CreateObject(program.islandsOut[c]);
        }
        yield return null;
    }
    IslandGO CreateObject(Island isl)
    {
        IslandGO go = Instantiate(islandObject);
        go.island = isl;
        for (int c = 0; c < isl.Childrens.Count;++c )
        {
            CreateObject(isl.Childrens[c]).transform.SetParent(go.transform);
        }
        return go;
    }
	void Start () {
        StartCoroutine(GenerateWorld());
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
