using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    WallGenerator wallGenerator;
    [SerializeField]
    AudioPlayer audioPlayer;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI endText;
    [SerializeField]
    private GameObject endBG;
    public int HP {get; private set;}
    private Vector2 velocity;
    private Vector2 acceleration;
    public Vector2 dPos{get; private set;}
    public float speed{get;private set;}
    private float mass = 0.1f;
    private float force = 100.0f;
    private List<Color> colorList = new List<Color>();
    private float intensity = Mathf.Pow(2, 2.4f);
    Material myMaterial;
    private bool isDead = false;
    private float acceleration_front = 0.006f;
    float score = 0;
    // Start is called before the first frame update
    void Start()
    {
        endBG.SetActive(false);
        velocity = Vector2.zero;
        dPos = Vector2.zero;
        acceleration = Vector2.zero;
        speed = 4.0f;
        colorList.Add(new Color(0.496f*intensity, 1*intensity, 0.496f*intensity, 1));
        colorList.Add(new Color(0.746f*intensity, 1*intensity, 0.496f*intensity, 1));
        colorList.Add(new Color(1.0f*intensity, 1*intensity, 0.496f*intensity, 1));
        colorList.Add(new Color(1.0f*intensity, 0.746f*intensity, 0.496f*intensity, 1));
        colorList.Add(new Color(1.0f*intensity, 0.496f*intensity, 0.496f*intensity, 1));
        HP = colorList.Count;
        Debug.Log($"HP:{HP}");
        myMaterial = transform.Find("Line").GetComponent<LineRenderer>().sharedMaterial;
        Debug.LogWarning($"mat:{myMaterial}");
        myMaterial.color = colorList[colorList.Count - HP];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(isDead)
        {
            velocity *= 0.96f;
            this.speed *= 0.86f;
            this.transform.localPosition += new Vector3(0, -0.02f, 0);
            //Debug.LogWarning($"{this.transform.localPosition.y}");
            if(this.transform.localPosition.y < -3.5)
            {
                End();
            }
        }else{
            score += acceleration_front*50;
            scoreText.text = "Score: " + ((int)score).ToString();

            wallGenerator.genInterval -= acceleration_front*0.08f;
            this.speed += acceleration_front;
            velocity = (velocity + acceleration*Time.deltaTime)*0.8f;
        }
        dPos = velocity*Time.deltaTime;
        acceleration = Vector2.zero;
        if(Mathf.Abs(this.transform.localPosition.x - -0.12f) > 0.2f)
        {
            this.transform.localPosition += new Vector3((this.transform.localPosition.x - -0.12f)*0.5f, 0, 0);
        }
    }

    public void OnMove(InputValue value)
    {
        if(isDead) return;
        Vector2 input = value.Get<Vector2>();
        acceleration = new Vector2(-force/mass*input.x, 0);
        if(Mathf.Abs(input.x) > 0.02f)
        {
            this.gameObject.transform.localPosition = new Vector3(-0.12f + Mathf.Sign(input.x)*0.6f*Mathf.Clamp(Mathf.Pow(input.x, 2), -0.4f, 0.4f), this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
        }
    }

    public void OnShot()
    {
        Debug.Log("sjot");
    }
    
    public void Damage()
    {
        if(HP > 1){
            HP--;
            Debug.Log($"HP:{HP}");
            myMaterial.color = colorList[colorList.Count - HP];
            audioPlayer.PlayHit();
        }
        else if(!isDead){
            death();
            audioPlayer.PlayHit();
        }
    }

    private void death()
    {
        myMaterial.color = new Color(0.2f,0.2f,0.2f);
        velocity = new Vector2(40.0f, -0.4f);
        isDead = true;
        wallGenerator.Stop();
        audioPlayer.PlayDead();
    }
    private void End(){
        endBG.SetActive(false);
        scoreText.gameObject.SetActive(false);
        endText.text = "Score: " + ((int)score).ToString()+"\n Press R to Restart";
        //Application.Quit();
    }
}
