using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script is for all effect
public class Ability_Garna : MonoBehaviour
{
    [SerializeField] private GameObject Lines_BK;
    [SerializeField] private GameObject Lines_FR;
    [Header("Ability_Stinger")]
    [SerializeField] private KeyCode input_Stinger;
    [SerializeField] private GameObject needlePack; //needle prefab
    [SerializeField] private GameObject StingerEmitPt;
    public List<GameObject> needle = new List<GameObject>();
    public GameObject stingerHitEffect; //enemy get hit effect
    [SerializeField] private GameObject effectPt;
    public List<GameObject> HitEffect = new List<GameObject>();
    [SerializeField] private GameObject sparkRing;
    [SerializeField] private GameObject ST_HitEffect;
    private GameObject target;
    [SerializeField] private int speed;

    [Header("Ability_AmberDust")]
    [SerializeField] private KeyCode input_AmberDust;
    [SerializeField] private GameObject amberDust;

    [Header("Ability_DashOfVengenance")]
    int state = 0;
    [SerializeField] private KeyCode input_DashOVen;
    [SerializeField] private GameObject origin;
    [SerializeField] private GameObject midPt;
    [SerializeField] private GameObject attackPt;
    [SerializeField] private GameObject lanceSwingObj;
    [SerializeField] private GameObject lance;
    public GameObject slashV1;
    [SerializeField] private GameObject slashV2;
    public GameObject slashV2_2;
    public GameObject DV_HitEffect;
    [SerializeField] private GameObject DV_HitEffect_2;
    Animator lanceAnim;
    [SerializeField] private int V2_speed;
    bool moveForward = false;
    public bool slashOut = false;

    [Header("Ability_BloomingGold_V1")]
    [SerializeField] private KeyCode input_BloomingGold_v1;
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject smallFlowerPrefab;
    [SerializeField] private GameObject lancePrefab;
    [SerializeField] private GameObject groundFlower;
    public GameObject BG_HitEffect;
    [SerializeField] private int BG_speed;
    public GameObject[] Location;
    public bool[] ready;
    public List<GameObject> Flower = new List<GameObject>();
    public List<GameObject> Lance = new List<GameObject>();
    public List<GameObject> SmallFlower = new List<GameObject>();

    void Start()
    {
        ST_HitEffect.SetActive(false);
        amberDust.SetActive(false);
        sparkRing.SetActive(false);
        slashV1.SetActive(false);
        slashV2.SetActive(false);
        slashV2_2.SetActive(false);
        DV_HitEffect_2.SetActive(false);
        Lines_BK.SetActive(false);
        Lines_FR.SetActive(false);
        groundFlower.SetActive(false);
        BG_HitEffect.SetActive(false);

        Location[0].transform.GetChild(0).gameObject.SetActive(false);
        Location[1].transform.GetChild(0).gameObject.SetActive(false);
        Location[2].transform.GetChild(0).gameObject.SetActive(false);
        Location[3].transform.GetChild(0).gameObject.SetActive(false);
        Location[4].transform.GetChild(0).gameObject.SetActive(false);

        lanceAnim = lance.GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(input_Stinger)){
            StartCoroutine("Stinger"); 
        }
        if(Input.GetKeyDown(input_AmberDust)){
            StartCoroutine("AmberDust"); 
        }
        if(Input.GetKeyDown(input_DashOVen)){
            if(state==0){
                StartCoroutine("DashOfVengence_V1");  
                Lines_BK.SetActive(true);
            }else{
                StartCoroutine("DashOfVengence_V2");
                Lines_FR.SetActive(true);
            } 
        }
        if(Input.GetKeyDown(input_BloomingGold_v1)){
            StartCoroutine("BloomingGold"); 
            StartCoroutine("BloomingGold_Lance"); 
        }
//-----------------------
        if(needle.Count>0&&needle[0]!=null&&needle[0].GetComponent<Stinger>().arrived){
            Debug.Log("penetrate");
            // stingerHitEffect.transform.position = effectPt.transform.position;
            // stingerHitEffect.SetActive(true);
            GameObject e = Instantiate(stingerHitEffect, effectPt.transform.position, Quaternion.identity);
            ST_HitEffect.SetActive(true);
            //stingRemain.SetActive(true);
            HitEffect.Add(e);
            needle[0].GetComponent<Stinger>().arrived = false;
        }
        if(moveForward){
            Move();
            if(this.transform.position == target.transform.position){
                moveForward = false;
            }
        }
        if(slashOut){
            SlashOut();
        }
        if(Lance.Count>3){
            if(ready[0]==true){
                LanceOut(Lance[0]);
            } 
            if(ready[1]==true){
                LanceOut(Lance[1]);
            }
            if(ready[2]==true){
                LanceOut(Lance[2]);
            }
            if(ready[3]==true){
                LanceOut(Lance[3]);
            }
            if(ready[4]==true){
                LanceOut(Lance[4]);
            }
        }
    }
//Ability - Stinger
    IEnumerator Stinger(){
        GameObject s = Instantiate(needlePack, StingerEmitPt.transform.position, Quaternion.identity);
        needle.Add(s.transform.GetChild(0).gameObject);  
        needle.Add(s.transform.GetChild(1).gameObject); 
        needle.Add(s.transform.GetChild(2).gameObject);   
        yield return new WaitForSeconds(3f); //wait for animation to finish
        s.GetComponent<Animator>().enabled = false;
        sparkRing.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        
        needle[0].GetComponent<Stinger>().move = true;
        // yield return new WaitForSeconds(1f);
        needle[1].GetComponent<Stinger>().move = true;
        // yield return new WaitForSeconds(1f);
        needle[2].GetComponent<Stinger>().move = true;
        yield return new WaitForSeconds(.1f);
        s.SetActive(false);
        yield return new WaitForSeconds(2f);
        
        Destroy(HitEffect[0]);
        HitEffect.TrimExcess();
        HitEffect.Clear();
        needle.TrimExcess();
        needle.Clear();
        Destroy(s);
        ST_HitEffect.SetActive(false);
        sparkRing.SetActive(false);
        //stingRemain.SetActive(false);
         //s.shoot out in order
    }
//Ability - AmberDust
    IEnumerator AmberDust(){
        yield return new WaitForSeconds(0f);
        amberDust.SetActive(true);
        yield return new WaitForSeconds(15f);
        amberDust.SetActive(false);
    }
//Ability - DashOfVengence
    IEnumerator DashOfVengence_V1(){
        lanceSwingObj.transform.rotation = Quaternion.Euler(-17.863f, 0f, 0f);
        yield return new WaitForSeconds(0f);
        target = attackPt;
        moveForward = true;
        yield return new WaitForSeconds(.3f);
        lanceAnim.SetTrigger("Swing");
        yield return new WaitForSeconds(1f);
        state = 1;
    }
    IEnumerator DashOfVengence_V2(){
        lanceSwingObj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        yield return new WaitForSeconds(0f);
        target = midPt;
        moveForward = true;
        yield return new WaitForSeconds(.05f);
        lanceAnim.SetTrigger("BackSwing");
        slashV2.SetActive(true);
        yield return new WaitForSeconds(1f);
        ResetDashOfVengence();
    }
    void ResetDashOfVengence(){
        target=origin;
        state = 0;
        moveForward = true;
        slashOut = false;
        lanceAnim.SetTrigger("Reset");
        slashV1.SetActive(false);
        slashV2.SetActive(false);
        slashV2_2.SetActive(false);
        DV_HitEffect.SetActive(false);
        DV_HitEffect_2.SetActive(false);
        slashV2.transform.position = this.transform.position;
        Lines_BK.SetActive(false);
        Lines_FR.SetActive(false);
    }
    void Move(){
        float step = speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, step);
    }
    void SlashOut(){
        slashV2.transform.Translate(Vector3.left * V2_speed * Time.deltaTime);
    }
//Ability - Blooming Gold
    IEnumerator BloomingGold(){
        yield return new WaitForSeconds(0f);
        Location[0].transform.GetChild(0).gameObject.SetActive(true); //GrdBreakObj
        RandomZ();
        yield return new WaitForSeconds(.5f);

        groundFlower.SetActive(true);
        GenerateSmallFlowers();
        GenerateFlowers(0,.7f);
        Location[1].transform.GetChild(0).gameObject.SetActive(true); //GrdBreakObj
        yield return new WaitForSeconds(.5f);
        GenerateFlowers(1,.8f);
        Location[2].transform.GetChild(0).gameObject.SetActive(true); //GrdBreakObj
        yield return new WaitForSeconds(.5f);
        GenerateFlowers(2,.9f);
        Location[3].transform.GetChild(0).gameObject.SetActive(true); //GrdBreakObj
        yield return new WaitForSeconds(.5f);
        GenerateFlowers(3,1);
        Location[4].transform.GetChild(0).gameObject.SetActive(true); //GrdBreakObj
        yield return new WaitForSeconds(.5f);
        GenerateFlowers(4,1.2f);
        //Ending----------------
        yield return new WaitForSeconds(.5f); ///Ending-----
        FlowerDisappear(0);
        yield return new WaitForSeconds(.5f); ///Ending-----
        FlowerDisappear(1);
        yield return new WaitForSeconds(.5f); ///Ending-----
        FlowerDisappear(2);
        yield return new WaitForSeconds(.5f); ///Ending-----
        FlowerDisappear(3);
        yield return new WaitForSeconds(.5f); ///Ending-----
        FlowerDisappear(4);
        yield return new WaitForSeconds(.5f);
        for(int i = 0; i<SmallFlower.Count;i++){
            Animator a = SmallFlower[i].transform.GetChild(0).gameObject.GetComponent<Animator>();
            a.SetTrigger("Disappear");
        }
        yield return new WaitForSeconds(2.5f); //Cool Down Time
        StartCoroutine("Reset"); 

    }
    IEnumerator BloomingGold_Lance(){
        yield return new WaitForSeconds(1.5f);
        ready[0] = true;
        yield return new WaitForSeconds(.5f);
        Flower[0].transform.GetChild(3).gameObject.SetActive(true); //Petal Burst Obj
        yield return new WaitForSeconds(.5f);
        ready[1] = true;
        Flower[1].transform.GetChild(3).gameObject.SetActive(true); //Petal Burst Obj
        yield return new WaitForSeconds(.5f);
        ready[2] = true;
        Flower[2].transform.GetChild(3).gameObject.SetActive(true); //Petal Burst Obj
        yield return new WaitForSeconds(.5f);
        ready[3] = true;
        Flower[3].transform.GetChild(3).gameObject.SetActive(true); //Petal Burst Obj
        yield return new WaitForSeconds(.5f);
        ready[4] = true;
        Flower[4].transform.GetChild(3).gameObject.SetActive(true); //Petal Burst Obj

    }
    void GenerateSmallFlowers(){
        for(int i = 0; i<10; i++){
            float ranS = Random.Range(0.05f, 0.3f);
            Vector3 location = new Vector3(Random.Range(-9, 14), 3.19f ,Random.Range(-3.5f, 3.5f));
            GameObject f0 = Instantiate(smallFlowerPrefab, location, Quaternion.identity);
            f0.transform.localScale = new Vector3(ranS,ranS,ranS);
            SmallFlower.Add(f0);
        }
    }
    void GenerateFlowers(int i, float size){
        GameObject f = Instantiate(flowerPrefab, Location[i].transform.position, Quaternion.identity);
        Flower.Add(f);
        f.transform.localScale = new Vector3(size,size,size);
        f.transform.GetChild(1).gameObject.SetActive(true);
        GenerateLance(f);
    }
    void GenerateLance(GameObject flower){
        GameObject pt = flower.transform.GetChild(2).gameObject;
        GameObject L = Instantiate(lancePrefab, pt.transform.position, Quaternion.identity);
        L.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        Lance.Add(L);
    }
    void FlowerDisappear(int i){
        Animator a = Flower[i].transform.GetChild(0).gameObject.GetComponent<Animator>();
        a.SetTrigger("Disappear");
        Flower[i].transform.GetChild(4).gameObject.SetActive(true);
    }
    private void RandomZ(){
        float r0 = Random.Range(-1,1);
        Location[0].transform.position = new Vector3(Location[0].transform.position.x, Location[0].transform.position.y, r0);
        float r1 = Random.Range(-1,1);
        Location[1].transform.position = new Vector3(Location[1].transform.position.x, Location[1].transform.position.y, r1);
        float r2 = Random.Range(-1,1);
        Location[2].transform.position = new Vector3(Location[2].transform.position.x, Location[2].transform.position.y, r2);
        float r3 = Random.Range(-1,1);
        Location[3].transform.position = new Vector3(Location[3].transform.position.x, Location[3].transform.position.y, r3);
        float r4 = Random.Range(0,0);
        Location[4].transform.position = new Vector3(Location[4].transform.position.x, Location[4].transform.position.y, r4);
    }
    void LanceOut(GameObject a){
        a.transform.Translate(-Vector3.forward * BG_speed * Time.deltaTime);
    }
    IEnumerator Reset(){
        yield return new WaitForSeconds(0);
        for(int i = 0; i<Flower.Count;i++){
            Destroy(Flower[i]);
        }
        for(int i = 0; i<Lance.Count;i++){
            Destroy(Lance[i]);
        }
        for(int i = 0; i<ready.Length;i++){
            ready[i] = false;
        }
        Flower.Clear();
        Flower.TrimExcess();
        Lance.Clear();
        Lance.TrimExcess();
        BG_HitEffect.SetActive(false);

        Location[0].transform.GetChild(0).gameObject.SetActive(false);
        Location[1].transform.GetChild(0).gameObject.SetActive(false);
        Location[2].transform.GetChild(0).gameObject.SetActive(false);
        Location[3].transform.GetChild(0).gameObject.SetActive(false);
        Location[4].transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        for(int i = 0; i<SmallFlower.Count;i++){
            Destroy(SmallFlower[i],.5f);
        }
        SmallFlower.Clear();
        SmallFlower.TrimExcess();
        groundFlower.SetActive(false);
    }
    // private void GetRandomX(float x, float y, float z){
    //     int ranX = Random.Range(-1,1);
    //     y = -3.2f;
    //     return Vector3(x,y,z)
    // }
 

    

    // public void Project(GameObject i){

    // }
}
