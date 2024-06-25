using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform centerTransform;
    [SerializeField]
    DrawLine drawLine;
    [SerializeField]
    Player player;
    private float rate = 1f;

    void Start()
    {
        GameObject lineRenderer = GameObject.Find("LineRenderer");
        drawLine = lineRenderer.GetComponent<DrawLine>();
        centerTransform = lineRenderer.transform.Find("Rotation").transform.Find("Center").transform;
        player = GameObject.Find("Player").GetComponent<Player>();
        drawLine.OnMapChanged += Move;
    }

    void OnDestroy()
    {
        //Debug.LogWarning("destroy");
        drawLine.OnMapChanged -= Move;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(float zoomRate)
    {
        /*Debug.LogWarning($"pos:{transform.position}, center:{centerTransform.position}");
        Debug.LogWarning($"lpos:{transform.localPosition}");
        Debug.LogWarning($"zr:{zoomRate}, rate:{rate}, ");*/
        transform.position = zoomRate*(transform.position - centerTransform.position);
        transform.localScale*=rate*zoomRate;
        //rate = 1.01f;
        //Debug.Log($"scale {this.transform.localScale.sqrMagnitude}");
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Destroy")
        {
            Destroy(this.gameObject);
        }else if(other.gameObject.tag == "Player")
        {
            Debug.LogWarning($"Damage");
            player.Damage();
            Destroy(this.gameObject);
        }
    }
}
