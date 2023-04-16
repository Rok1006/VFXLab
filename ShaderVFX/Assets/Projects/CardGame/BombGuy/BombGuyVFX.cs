using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Plan 
Main: Throw bombs on three enemy towards its owns side in one round
1. emit 3 berry at a time shoot towards enemy but with a force that goes upward
Passive: heal (buff + attack) himself a bit by eating the blueberry
1. One big blue berry pop up on the top of the card 
2. vibrates a few sec 
3. either being eat out bit by bit(2D) or explode (3D)
4. self buff effect

Note:
Need in other script add all detected enemy into the enemy list
*/
public class BombGuyVFX : MonoBehaviour
{
    public enum AbilityState{MAIN, PASSIVE};
    public AbilityState currentState = AbilityState.MAIN;
    [Header("OBJ")]
    public GameObject smallBerry;  //prefab
    public GameObject bigBerry;  //prefab
    public GameObject mainEmitPt;
    public GameObject passiveEmitPt;
    public GameObject trail;
    public List<GameObject> enemy = new List<GameObject>();
    public List<GameObject> berry = new List<GameObject>();
    public List<GameObject> enemySparks = new List<GameObject>();

    private GameObject enemySparks1;
    private GameObject enemySparks2;
    private GameObject enemySparks3;

    int index = 0;
    public int count = 1;
    bool check = false;
    [Header("Values")]
    [SerializeField]private int effectPos;
    [SerializeField]private float force; //up Dip force
    [SerializeField]private float throwSpeed;

    void Start()
    {
        DetectSparks();
        trail.SetActive(false);
    }
    public void DetectSparks(){
        //if(enemy.Count>0){ //put it in a way that it reassign new enemy's efect
            enemySparks.Add(enemy[0].transform.GetChild(effectPos).gameObject);
            enemySparks.Add(enemy[1].transform.GetChild(effectPos).gameObject);
            enemySparks.Add(enemy[2].transform.GetChild(effectPos).gameObject);
            enemySparks[0].SetActive(false);
            enemySparks[1].SetActive(false);
            enemySparks[2].SetActive(false);
        //}
    }

    void Update()
    {
        //Dev shit
        if(Input.GetKeyDown(KeyCode.Space)){
            //DetectSparks();
            StartCoroutine("Attack"); 
        }
//---------------------------------
        switch(currentState){
            case AbilityState.MAIN:  //Main
            if(berry.Count>2){
                for(int i = 0; i<berry.Count;i++){ //try use while
                    if(berry[i].transform.position==enemy[i].transform.position){
                        enemy[i].GetComponent<SpriteRenderer>().color = Color.green;
                        enemySparks[i].SetActive(true);
                        enemy[i].GetComponent<Animator>().SetBool("stun",true);
                        continue;
                    }
                }    
                if(berry[2].transform.position==enemy[2].transform.position){
                    Debug.Log("Done");
                    Invoke("ResetAttack",.1f);
                    Invoke("OffHitEffect",1f);
                    break;
                }    
            }
            break;
            case AbilityState.PASSIVE:  //Passive
                if(trail.activeSelf){
                    Invoke("OffTrail", 5f);
                }
            break;
        } 
    }
    IEnumerator Attack(){
        yield return new WaitForSeconds(0);
        if(currentState == AbilityState.MAIN&&enemy.Count>0){
            BerryCreate(smallBerry, enemy[0]);
            yield return new WaitForSeconds(.2f);
            BerryCreate(smallBerry, enemy[1]);
            yield return new WaitForSeconds(.2f);
            BerryCreate(smallBerry, enemy[2]);
            check = true;
        }else if(currentState == AbilityState.PASSIVE){ //DragonMode-----------------
            GameObject s = Instantiate(bigBerry, passiveEmitPt.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(.2f);
            s.GetComponent<Animator>().SetTrigger("eaten");
        }
    }
    public void BerryCreate(GameObject prefab, GameObject target){
        GameObject s = Instantiate(prefab, mainEmitPt.transform.position, Quaternion.identity);
        //s.transform.localEulerAngles = new Vector3(90, 0, 0);
        berry.Add(s);
        var sc = s.GetComponent<Berry>();
        sc.target = target;
        sc._speed = throwSpeed;
        sc._force = force;
    }
    void ResetAttack(){
        berry.Clear();
        berry.TrimExcess();
        // enemy.Clear();
        // enemy.TrimExcess();
    }
    void OffHitEffect(){
        enemySparks[0].SetActive(false);
        enemySparks[1].SetActive(false);
        enemySparks[2].SetActive(false);
        enemy[0].GetComponent<Animator>().SetBool("stun",false);
        enemy[1].GetComponent<Animator>().SetBool("stun",false);
        enemy[2].GetComponent<Animator>().SetBool("stun",false);
        enemySparks.Clear();
        enemySparks.TrimExcess();
        // berry.Clear();
        // berry.TrimExcess();
    }
    void OffTrail(){
        trail.SetActive(false);
    }

}
