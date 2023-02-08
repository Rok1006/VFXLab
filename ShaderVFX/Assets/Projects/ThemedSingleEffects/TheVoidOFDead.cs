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
    public List<GameObject> Ghost = new List<GameObject>();
    public List<bool> Ready = new List<bool>();
    int ranNum;

    void Start()
    {
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
        //if(Ready.Count>1){
        // for(int i = 0; i<Ghost.Count; i++){
        //     if(Ghost[i].transform.position == EnemyReceivePt.transform.position){
        //         //Ready[i] = false;
        //         Rigidbody rb = Ghost[i].GetComponent<Rigidbody>();
        //         rb.velocity = Vector3.zero;
        //         //Ghost[i].SetActive(false);
        //     }
        // }
        //}
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
                //Ghost[i].SetActive(false);
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
        yield return new WaitForSeconds(0f);
        CreateGhost(0);
        yield return new WaitForSeconds(0.5f);
        CreateGhost(1);
        yield return new WaitForSeconds(0.5f);
        CreateGhost(2);
        yield return new WaitForSeconds(0.5f);
        CreateGhost(3);
        yield return new WaitForSeconds(3f);
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
