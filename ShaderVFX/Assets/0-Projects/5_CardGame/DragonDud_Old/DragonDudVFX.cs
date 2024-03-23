using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Plan 
Cat Mode: VShape attack 
1. two consecutive big slice vfx infront of player card 
2. shoot out slice towards enemy  and leave trail on ground
3. when arrive at enemy two separate diagonal slice in front of/on enemy card
Dragon Mode: Straight, STRONG high attack on single enemy
1. bring one of the slices in cat mode and amplify it
Passive: Dragon Tail appear on card with particles; defence - Takes less damage OR all player get less damage
Conditions:
if the card in front is gone already: 

*/
public class DragonDudVFX : MonoBehaviour
{
    public enum AbilityState{CATMODE, DRAGONMODE, PASSIVE};
    public AbilityState currentState = AbilityState.CATMODE;
    [Header("OBJ")]
    public GameObject theSlash;
    public GameObject center; //the pos in front of player card
    public GameObject leftEnemy;
    public GameObject rightEnemy;
    public GameObject frontEnemy;
    public GameObject singleSlashPrefab; //assign the prefab
    public GameObject doubleSlashPrefab; //assign the prefab
    public GameObject Tail;
    public GameObject tailSparks;
    public GameObject lightDust;
    Animator tailAnim;

    int index = 0;
    public List<GameObject> slash = new List<GameObject>();
    private GameObject previousTarget;
    private GameObject currentTarget;
    bool move = false;
    Animator EnemyAnim1;
    Animator EnemyAnim2;
    Animator EnemyAnim3;
    private GameObject enemySparks1;
    private GameObject enemySparks2;
    private GameObject enemySparks3;
    private GameObject darkExplo;

    [Header("Values")]
    [SerializeField]private int effectPos = 0;
    [SerializeField]private int darkExploPos = 2;
    [SerializeField]private float catSpeed;
    [SerializeField]private float dragonSpeed;

    void Start()
    {
        Tail.SetActive(false);
        tailAnim = Tail.GetComponent<Animator>();
        tailSparks.SetActive(false);
        lightDust.SetActive(false);
        //abt Enemy
        if(leftEnemy!=null&&rightEnemy!=null){
            EnemyAnim1 = leftEnemy.GetComponent<Animator>();
            EnemyAnim2 = rightEnemy.GetComponent<Animator>();
            enemySparks1 = leftEnemy.transform.GetChild(effectPos).gameObject; //later refine this part to not too long
            enemySparks1.SetActive(false);
            enemySparks2 = rightEnemy.transform.GetChild(effectPos).gameObject;
            enemySparks2.SetActive(false);
        }
        if(frontEnemy!=null){
            EnemyAnim3 = frontEnemy.GetComponent<Animator>();
            enemySparks3 = frontEnemy.transform.GetChild(effectPos).gameObject;
            enemySparks3.SetActive(false);
            darkExplo = frontEnemy.transform.GetChild(darkExploPos).gameObject;
            darkExplo.SetActive(false);
        }
    }
    public void CatModeDetect(GameObject enemy1, GameObject enemy2){ //put this somewhere
        frontEnemy = enemy1;
        leftEnemy = enemy2;
    }
    public void DragonModeDetect(GameObject enemy1){ //put this somewhere
        frontEnemy = enemy1;
    }

    void Update()
    {//Dev shit
        if(Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine("Attack"); 
        }
//---------------------------------
        switch(currentState){
            case AbilityState.CATMODE:  //Main: CAT MODE
                if(slash.Count>0&&slash[0]!=null && slash[0].transform.position.z>=leftEnemy.transform.position.z){   //arrive at first enemy
                    //fisrt enemy shake and attacked reaction
                    leftEnemy.GetComponent<SpriteRenderer>().color = Color.green; //Sample
                    enemySparks1.SetActive(true);
                    EnemyAnim1.SetBool("stun",true);
                }
                if(slash.Count>1&&slash[1]!=null && slash[1].transform.position.z>=rightEnemy.transform.position.z){   //arrive at first enemy
                    //fisrt enemy shake and attacked reaction
                    rightEnemy.GetComponent<SpriteRenderer>().color = Color.green; //Sample
                    enemySparks2.SetActive(true);
                    EnemyAnim2.SetBool("stun",true);
                    Invoke("ResetAttack",.1f);
                    Invoke("OffHitEffect",3f);
                }
            break;
            case AbilityState.DRAGONMODE:  //Main: DRAGON MODE
                if(slash.Count>0&&slash[0]!=null && slash[0].transform.position.z>=frontEnemy.transform.position.z){   //arrive at first enemy
                    //fisrt enemy shake and attacked reaction
                    frontEnemy.GetComponent<SpriteRenderer>().color = Color.green; //Sample
                    enemySparks3.SetActive(true);
                    darkExplo.SetActive(true);
                    EnemyAnim3.SetBool("stun",true);
                    Invoke("ResetAttack",.1f);
                    Invoke("OffHitEffect",1f);
                }
            break;
            case AbilityState.PASSIVE:  //PASSIVE
                if(Tail.activeSelf){
                    Invoke("TailDown",4f);//this exccute before next round
                }
            break;
        }
    }

    IEnumerator Attack(){
        if(currentState == AbilityState.CATMODE){//CatMODE---------------- refine these part
            ResetAttack();
            yield return new WaitForSeconds(0);
            GameObject s = Instantiate(singleSlashPrefab, center.transform.position, Quaternion.identity);
            slash.Add(s);
            var sc = s.GetComponent<Slash>();
            sc.target = leftEnemy;
            sc.speed = catSpeed;
            yield return new WaitForSeconds(.5f);
            EnemyAnim1.SetBool("stun",false);
            GameObject s2 = Instantiate(singleSlashPrefab, center.transform.position, Quaternion.identity);
            slash.Add(s2);
            var sc2 = s2.GetComponent<Slash>();
            sc2.target = rightEnemy;
            sc2.speed = catSpeed;
        }else if(currentState == AbilityState.DRAGONMODE){ //DragonMode-----------------
            ResetAttack();
            yield return new WaitForSeconds(0);
            GameObject s = Instantiate(doubleSlashPrefab, center.transform.position, Quaternion.identity);
            slash.Add(s);
            var sc = s.GetComponent<Slash>();
            sc.target = frontEnemy;
            sc.speed = dragonSpeed;
        }else if(currentState == AbilityState.PASSIVE){
            yield return new WaitForSeconds(0);
            Tail.SetActive(true);
            lightDust.SetActive(true);
            yield return new WaitForSeconds(.5f);
            tailSparks.SetActive(true);
        }
    }

    void ResetAttack(){
        slash.Clear();
        slash.TrimExcess();
        EnemyAnim1.SetBool("stun",false);
        EnemyAnim2.SetBool("stun",false);
        EnemyAnim3.SetBool("stun",false);
        
    }
    void OffHitEffect(){
        enemySparks1.SetActive(false);
        enemySparks2.SetActive(false);
        enemySparks3.SetActive(false);
        darkExplo.SetActive(false);
    }
    void ResetPassive(){
        Tail.SetActive(false);
    }
    void TailDown(){
        tailAnim.SetTrigger("down");
        tailSparks.SetActive(false);
        lightDust.SetActive(false);
        Invoke("ResetPassive",1f);
    }
}
