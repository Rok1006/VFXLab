using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEffect : MonoBehaviour
{
    [SerializeField]private GameObject AllCam;
    bool CamSwitch = true;
    [Header("WaterEffect")]
    [SerializeField] private KeyCode input_SmallWaterAttack;
    [SerializeField] private KeyCode input_WaterAttack;
    [SerializeField]private GameObject Portal;
    [SerializeField]private Animator PortalAnim;
    [SerializeField]private GameObject Book;
    public Animator bookAnim;
    [SerializeField]private GameObject MagicGlobe_Water;
    Animator MGAnim;
    [SerializeField]private GameObject WaterBullet;
    [SerializeField]private GameObject WaterGround;
    Animator WGAnim;
    [SerializeField]private GameObject WaterDrop;
    [SerializeField]private GameObject WaterSplat;
    [SerializeField]private GameObject[] ExplosionImpact;

    [Header("Attack")]
    [SerializeField] private GameObject CurvedProjectile; // prefab for the projectile to be spawned
    [SerializeField] private GameObject StraightProjectile;
    [SerializeField] private GameObject SmallProjectile;
    [Header("For Straight")]
    [SerializeField] private Transform Origin;
    [SerializeField] private Transform spawnPoint; // the point from which the projectiles will be spawned
    [SerializeField] private Transform target; // the target towards which the projectiles will curve
    [SerializeField] private float projectileSpeed = 10f; // the speed of the projectile
    [SerializeField] private float SmallProjectileSpeed = 10f;
    [SerializeField] private float curveStrength = 5f; // the strength of the curve

    bool up = false;
    //public bool arrived = false;

    void Start()
    {
       // bookAnim = Book.GetComponent<Animator>();
        MGAnim = MagicGlobe_Water.GetComponent<Animator>();
        WGAnim = WaterGround.GetComponent<Animator>();

        Book.SetActive(false);
        MagicGlobe_Water.SetActive(false);
        WaterGround.SetActive(false);
        WaterDrop.SetActive(false);
        WaterSplat.SetActive(false);
        Portal.SetActive(false);
        ExplosionImpact[0].SetActive(false);
        ExplosionImpact[1].SetActive(false);
        ExplosionImpact[2].SetActive(false);
        AllCam.SetActive(true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)){
            if(CamSwitch){
                AllCam.SetActive(false);
                CamSwitch = false;
            }else{ //false
                AllCam.SetActive(true);
                CamSwitch = true;
            }
            
        }
        if(Input.GetKeyDown(input_WaterAttack)){
            //StartCoroutine("WaterAttack"); 
            //weaponAnim.SetTrigger("move");
            Portal.SetActive(true);
        }
        if(Input.GetKeyDown(input_SmallWaterAttack)){
            CreateStraightBullet(SmallProjectile,Origin,target,SmallProjectileSpeed);
        }
        if (PortalAnim.GetCurrentAnimatorStateInfo(0).IsName("Portal_BookUp")&&!up)
        {
            Book.SetActive(true);
            StartCoroutine("WaterAttack");
            up = true;
        }
        if(Input.GetKeyDown(KeyCode.L)){
            CreateCurvedBullet(CurvedProjectile,spawnPoint,target);
            CreateStraightBullet(StraightProjectile,spawnPoint,target,projectileSpeed);
        }
        
    }

    IEnumerator WaterAttack(){
        //open a small portal
        //PortalAnim.SetTrigger("Close");
        yield return new WaitForSeconds(1f);
        WaterGround.SetActive(true);
        yield return new WaitForSeconds(.5f);
        MagicGlobe_Water.SetActive(true); //ORb comes up
        WaterSplat.SetActive(true);
        yield return new WaitForSeconds(.1f);
        WGAnim.SetTrigger("waterGroundorbenter");
        MGAnim.SetTrigger("pulse");
        yield return new WaitForSeconds(.3f);
        WaterDrop.GetComponent<ParticleSystem>().Emit(0);
        yield return new WaitForSeconds(1f);
        MGAnim.SetTrigger("LitUp");
        yield return new WaitForSeconds(1f);
        CreateCurvedBullet(CurvedProjectile,spawnPoint,target);
        CreateStraightBullet(StraightProjectile,spawnPoint,target,projectileSpeed);
    }
    void CreateCurvedBullet(GameObject _prefab, Transform _spawnPoint, Transform _targetPt){
        // spawn the projectile at the spawn point
        GameObject projectile = Instantiate(_prefab, _spawnPoint.position, transform.rotation);
        // GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        var ProjectileSc = projectile.GetComponent<MissileBehavior>();
        ProjectileSc.target = _targetPt;
        var destinationWithOffset = target.position;
        ProjectileSc.GetComponent<MissileBehavior>().destWithOffset = destinationWithOffset;

    }
    void CreateStraightBullet(GameObject _prefab, Transform _spawnPoint, Transform _targetPt, float _speed){
        // spawn the projectile at the spawn point
        GameObject projectile = Instantiate(_prefab, _spawnPoint.position, transform.rotation);
        // GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        var ProjectileSc = projectile.GetComponent<Projectile>();
        ProjectileSc.target = _targetPt;
        // var destinationWithOffset = target.position;
        // ProjectileSc.GetComponent<MissileBehavior>().destWithOffset = destinationWithOffset;
        ProjectileSc.spawnPoint = _spawnPoint;
        ProjectileSc.target = _targetPt;
        ProjectileSc.projectileSpeed = _speed;
        //ProjectileSc.curveStrength = _curveStregth;
    }

    public void WaterExplode(int index){
        ExplosionImpact[index].SetActive(true);
        Invoke("ExplodeOff",3f);
    }
    void ExplodeOff(){
        ExplosionImpact[0].SetActive(false);
        ExplosionImpact[1].SetActive(false);
        ExplosionImpact[2].SetActive(false);
        MGAnim.SetTrigger("perish");
        bookAnim.SetTrigger("Close");
        Portal.SetActive(false);
        Portal.SetActive(true);
    }

}
