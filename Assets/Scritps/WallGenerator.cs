using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    [SerializeField]
    Transform gen1;
    [SerializeField]
    Transform gen2;
    [SerializeField]
    Transform gen3;
    [SerializeField]
    Transform gen4;
    [SerializeField]
    Transform gen5;
    [SerializeField]
    Transform gen6;

    [SerializeField]
    GameObject wallPrefab;

    List<Transform> generators = new List<Transform>();

    private int wallNumber = 6;
    public float genInterval{get; set;}
    private float elapsedTime;
    bool enabled = true;
    // Start is called before the first frame update
    void Start()
    {
        generators.Add(gen1);
        generators.Add(gen2);
        generators.Add(gen3);
        generators.Add(gen4);
        generators.Add(gen5);
        generators.Add(gen6);
        elapsedTime = 0;
        genInterval = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(!enabled)return;
        elapsedTime += Time.deltaTime;
        if(elapsedTime > genInterval)
        {
            Transform generatorTrans = generators[Random.Range(0, wallNumber)];
            GameObject wallObj = Instantiate(wallPrefab, generatorTrans) as GameObject;
            elapsedTime = 0;
        }
    }

    public void Stop()
    {
        enabled = false;
    }
}
