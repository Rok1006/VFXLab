using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[SerializedField]
/*Plan
PreAttack:
1. small bird with Trails circling around the card
Ready Attack:
1. a build up effect on the player card itself that accentuate at the top and form step2 - one gameObject
2. a small bird/ ball with shader effect formed > wait 0.2 sec > do a curved low dip and penetrate center of first card
3. Stay at the first card with sparks effect and go through second card smoothly
Conditions:
if the card in front is gone already: detect if enemy card is still there, if no change the target? or a different particles?
Passive: 
*/
public class BirdManVFX : MonoBehaviour
{
    public enum AbilityState{MAIN, PASSIVE};
    public AbilityState currentState = AbilityState.MAIN;
    [Header("OBJ")]
    public GameObject theBird; //Another object prefab 
    public GameObject top; //the pos on top of player card
    public GameObject firstEnemy;
    public GameObject secondEnemy;
    public GameObject bird; //the bird mesh inside theBird
    public GameObject buildUP; //prefab
    public GameObject blingTrail; //prefab

    private GameObject previousTarget;
    private GameObject target;
    Rigidbody rb;
    private float speed;
    bool move = false;
    TrailRenderer tr;
    MeshRenderer mr;
    private GameObject sparks; //Sparks
    private GameObject sparks2; //SparksFlat
    private GameObject enemySparks;
    private GameObject enemySparks2;
    Animator EnemyAnim1;
    Animator EnemyAnim2;
    Animator birdAnim;

    [Header("Values")]
    [SerializeField]private int effectPos = 0;
    [SerializeField]private float force; //dip force
    [SerializeField]private float shootWaitTime = 1.4f; //2.79
    [SerializeField]private float dipWaitTime = 0.2f;
    [SerializeField]private float secondShootWaitTime = 0.7f;
    [SerializeField]private float ph0Speed = 2f; //1
    [SerializeField]private float ph1Speed = 12f;
    [SerializeField]private float ph2Speed = 12f;

    void Start()
    {
        rb = theBird.GetComponent<Rigidbody>();
        theBird.transform.position = this.transform.position;
        tr = bird.GetComponent<TrailRenderer>();
        tr.enabled = false;
        mr = bird.GetComponent<MeshRenderer>();
        mr.enabled = false;
        sparks = theBird.transform.GetChild(0).gameObject;
        sparks.SetActive(false);
        sparks2 = theBird.transform.GetChild(1).gameObject;
        sparks2.SetActive(false);
        bird.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        birdAnim = bird.GetComponent<Animator>();
        buildUP.SetActive(false);
        blingTrail.SetActive(false);
        // ABT ENEMY
        if(firstEnemy!=null&&secondEnemy!=null){
            enemySparks = firstEnemy.transform.GetChild(effectPos).gameObject;
            enemySparks.SetActive(false);
            enemySparks2 = secondEnemy.transform.GetChild(effectPos).gameObject;
            enemySparks2.SetActive(false);
            EnemyAnim1 = firstEnemy.GetComponent<Animator>();
            EnemyAnim2 = secondEnemy.GetComponent<Animator>();
        }
    }
    public void DetectEnemy(GameObject enemy1, GameObject enemy2){ //put this somewhere
        firstEnemy = enemy1;
        secondEnemy = enemy2;
    }

    void Update()
    {
        //Dev shit
        if(Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine("Attack"); 
        }
        switch(currentState){
            case AbilityState.MAIN:  //Main
                if(move){
                    Move();
                }
                if(target == firstEnemy&&theBird.transform.position==target.transform.position ){   //arrive at first enemy
                    //fisrt enemy shake and attacked reaction
                    firstEnemy.GetComponent<SpriteRenderer>().color = Color.green; //Sample
                    enemySparks.SetActive(true);
                    EnemyAnim1.SetBool("stun",true);
                }
                if(target == secondEnemy&&theBird.transform.position==target.transform.position ){   //arrive at first enemy
                    //fisrt enemy shake and attacked reaction
                    secondEnemy.GetComponent<SpriteRenderer>().color = Color.green; //Sample
                    enemySparks2.SetActive(true);
                    EnemyAnim2.SetBool("stun",true);
                    birdAnim.SetTrigger("Normal");//disappear

                    Invoke("ResetAttack", .5f);
                }
            break;
            case AbilityState.PASSIVE:  //Passive

            break;
        } 
    }

    void Move(){
        float step = speed * Time.deltaTime;
        theBird.transform.position = Vector3.MoveTowards(theBird.transform.position, target.transform.position, step);
    }

    IEnumerator Attack(){
        yield return new WaitForSeconds(0);
        if(currentState == AbilityState.MAIN){
//Phase0----------------------------------------------------------
        buildUP.SetActive(true);
        speed = ph0Speed; //2
        previousTarget = theBird;
        target = top;
        move = true;
        //StartCoroutine("Appear");
        Invoke("Appear", 2f);
//Phase1----------------------------------------------------------
        yield return new WaitForSeconds(shootWaitTime);  //1
        theBird.transform.position=target.transform.position;
        speed = ph1Speed; //7
        previousTarget = top;
        target = firstEnemy;
        sparks.SetActive(true);
        sparks2.SetActive(true);
        blingTrail.SetActive(true);
        yield return new WaitForSeconds(dipWaitTime); //.4 wait time before apply low dip
        rb.AddForce(-transform.up * force, ForceMode.Impulse);
//Phase2----------------------------------------------------------  
        yield return new WaitForSeconds(secondShootWaitTime); //0.7
        theBird.transform.position=target.transform.position;
        rb.AddForce(transform.up * 3, ForceMode.Impulse);
        speed = ph2Speed; //7
        previousTarget = firstEnemy;
        target = secondEnemy;
        }else if(currentState == AbilityState.PASSIVE){
            yield return new WaitForSeconds(0);
            //sth here
        }
    }
    void Appear(){
        bird.transform.rotation = Quaternion.Euler(184f, 0f, 0f);
        mr.enabled = true;
        tr.enabled = true; 
        birdAnim.SetTrigger("Appear");
    }

    void ResetAttack(){
        theBird.transform.position = this.transform.position;
        speed = ph0Speed;
        previousTarget = theBird;
        target = top;
        move = false;
        mr.enabled = false;
        tr.enabled = false; 
        birdAnim.SetTrigger("Normal");
        bird.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        sparks.SetActive(false);
        sparks2.SetActive(false);
        enemySparks.SetActive(false);
        enemySparks2.SetActive(false);
        buildUP.SetActive(false);
        blingTrail.SetActive(false);
    }
}
