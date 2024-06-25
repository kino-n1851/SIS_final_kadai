using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DrawLine : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject linePrefab;
    [SerializeField]
    Player player;
    [SerializeField]
    GameObject rotationObject;
    static int poly_num = 6;
    float initTheta = (poly_num % 2 == 0) ? Mathf.PI/poly_num : Mathf.PI/2;
    List<GameObject> lineObjects = new List<GameObject>();
    float user_angle=0;
    float scale = 12.0f;
    float angle_sensitivity = 5.0f;

    public delegate void MapChanged(float dScale);
    public event MapChanged OnMapChanged;
    
    void Start()
    {
        Screen.SetResolution(1920, 1080, false, 60);

        DrawTube(0, 12.0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        user_angle = angle_sensitivity*player.dPos.x;
        float currentScale = scale;
        scale += player.speed*0.1f;
        if(scale > 16.2f)
        {
            currentScale = 12.0f - player.speed*0.1f;
            scale=12.0f;
        }
        DeleteLine();
        DrawTube(user_angle, scale);
        OnMapChanged?.Invoke(scale/currentScale);
    }
    void DrawTube(float rotation, float max_scale)
    {
        float scale = max_scale;
        float maxWidth = 0.1f;
        float minWidth = 0.01f;
        int loop_time = 40;
        float angle_step_deg = 30.0f;
        float angle = 0;
        rotationObject.transform.Rotate(new Vector3(0,0,rotation));
        float b_c = 0.8f;
        float intensity = -0.8f + max_scale*0.27f;//0.26
        var factor = Mathf.Pow(2, intensity);
        for (int i = 0; i < loop_time; i++)
        {
            if(i>6){
                b_c*=0.8f;
            }
            scale*=0.86f;
            angle+=angle_step_deg;
            DrawPoly(scale, angle, Mathf.Clamp((0.15f - 0.05f*Mathf.Log(i, 2)), 0.01f, 0.3f), new Color(b_c*factor, b_c*factor, 1*factor,1));
        }
    }

    void DeleteLine()
    {
        for(int i = 0; i < lineObjects.Count; i++)
        {
            LineRenderer lineRenderer = lineObjects[i].GetComponent<LineRenderer>();
            int len = lineRenderer.materials.Length;
            for(int j = 0; j < len;j++){
                Destroy(lineRenderer.materials[j]);
            }

            Destroy(lineObjects[i].gameObject);
            Destroy(lineObjects[i]);
        }
        lineObjects = new List<GameObject>();
    }

    void DrawPoly(float scale, float rotation, float pxSize, Color color)
    {
        for (float i = 0; i < Mathf.PI*2; i+=Mathf.PI*2/poly_num){
            GameObject line = Instantiate(linePrefab, rotationObject.transform) as GameObject;
            LineRenderer renderer = line.GetComponent<LineRenderer>();
            // 線の幅
            renderer.SetWidth(pxSize, pxSize);
            Material material = renderer.material;
            material.color = color;
            // 頂点の数
            renderer.SetVertexCount(2);
            // 頂点を設定
            float angle1 = initTheta + i + rotation*Mathf.Deg2Rad;
            float angle2 = initTheta + i + 2*Mathf.PI/poly_num + rotation*Mathf.Deg2Rad;
            renderer.SetPosition(0, new Vector3(scale*Mathf.Cos(angle1), scale*Mathf.Sin(angle1), 50));
            renderer.SetPosition(1, new Vector3(scale*Mathf.Cos(angle2), scale*Mathf.Sin(angle2), 50));
            lineObjects.Add(line);
        }
    }
}
