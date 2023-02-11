using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheVoidOFDead : MonoBehaviour
{
    [SerializeField] private KeyCode input_TVOD;
    [SerializeField] private GameObject ghostPrefab;  //with trails
    [SerializeField] private GameObject PlayerEmitPt;
    [SerializeField] private GameObject EnemyReceivePt;
    [SerializeField] private int speed;
    [SerializeField] private float force;
    [SerializeField] private GameObject puddle;
    Animator puddleAnim;
    [SerializeField] private GameObject inkCircle;

    public List<GameObject> Ghost = new List<GameObject>();
    public List<bool> Ready = new List<bool>();
    int ranNum;
    

    void Start()
    {
        puddleAnim = puddle.GetComponent<Animator>();
        puddle.SetActive(false);
        inkCircle.SetActive(false);
        puddle.SetActive(true);
    }

    void Update()
    {
        if(Input.GetKeyDown(input_TVOD)){
            StartCoroutine("GhostSend"); 
        }
        //if(Ready.Count>1){
        //}
        // if(Ghost!=null&&Ghost[Ghost.Count].transform.position == EnemyReceivePt.transform.position){
        //     StartCoroutine("Reset");
        // }
        // if(Ghost.Count>0){
        //     if(Ghost[0].transform.position == EnemyReceivePt.transform.position){
        //         puddle.SetActive(true);
        //         // puddleAnim.SetTrigger()
        //     }
        //     if(Ghost[1].transform.position == EnemyReceivePt.transform.position){
        //         puddleAnim.SetTrigger("p2");
        //     }
        //     if(Ghost[2].transform.position == EnemyReceivePt.transform.position){
        //         puddleAnim.SetTrigger("p3");
        //     }
        //     if(Ghost[3].transform.position == EnemyReceivePt.transform.position){
        //         puddleAnim.SetTrigger("p4");
        //     }
        //     if(Ghost[4].transform.position == EnemyReceivePt.transform.position){
        //         puddleAnim.SetTrigger("p5");
        //     }
        //     // if(Ghost[5].transform.position == EnemyReceivePt.transform.position){
        //     //     puddleAnim.SetTrigger("p6");
        //     // }
        // }
    }
    private void FixedUpdate() {
        for(int i = 0; i<Ready.Count; i++){
            if(Ready[i] == true){
                Rigidbody rb = Ghost[i].GetComponent<Rigidbody>();
                int ranNum = Random.Range(0,2);;
                if(ranNum==0){
                    rb.AddForce(new Vector3(0,0,-force), ForceMode.Impulse);
                }else{
                    rb.AddForce(new Vector3(0,0,force), ForceMode.Impulse);
                }
                float step = speed * Time.deltaTime;
                Ghost[i].transform.position = Vector3.MoveTowards(Ghost[i].transform.position, EnemyReceivePt.transform.position, step);
                //Ready[i] = false;
            }
        }
        for(int i = 0; i<Ghost.Count; i++){
            if(Ghost[i].transform.position == EnemyReceivePt.transform.position){
                //Ready[i] = false;
                Rigidbody rb = Ghost[i].GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                Ghost[i].SetActive(false);
            }
        }

    }
    void CreateGhost(int i){
        GameObject g1 = Instantiate(ghostPrefab, PlayerEmitPt.transform.position, Quaternion.identity);
        Ghost.Add(g1);
        g1.transform.rotation = Quaternion.Euler(90f, 0f, -90f);
        Ready.Add(false);
        Ready[i] = true;
        //ranNum = Random.Range(0,2);
    }
    IEnumerator GhostSend(){
        
        //yield return new WaitForSeconds(.5f);
        
        yield return new WaitForSeconds(.5f);
        CreateGhost(0);
        yield return new WaitForSeconds(.1f);
        puddleAnim.SetTrigger("p1");
        yield return new WaitForSeconds(.5f);
        CreateGhost(1);
        puddleAnim.SetTrigger("p2");
        
        yield return new WaitForSeconds(1f);
        CreateGhost(2);
        puddleAnim.SetTrigger("p3");
        
        yield return new WaitForSeconds(1f);
        CreateGhost(3);
        puddleAnim.SetTrigger("p4");

        yield return new WaitForSeconds(1f);
        
        puddleAnim.SetTrigger("p5");
        //yield return new WaitForSeconds(.5f);
        //CreateGhost(4);
        inkCircle.SetActive(true);
        //yield return new WaitForSeconds(.2f);
        
        yield return new WaitForSeconds(5f);
        StartCoroutine("Reset"); 
    }

    IEnumerator Reset(){
        yield return new WaitForSeconds(0f);
        for(int i = 0; i<Ghost.Count;i++){
            Destroy(Ghost[i]);
            //Ready.Remove(Ready[i]);
        }
        Ghost.TrimExcess();
        Ghost.Clear();
        Ready.TrimExcess();
        Ready.Clear();

    }
}
