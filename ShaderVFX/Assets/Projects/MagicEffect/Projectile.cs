using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private MagicEffect ME_Sc;
    [HideInInspector]public GameObject projectilePrefab; // prefab for the projectile to be spawned
    [HideInInspector]public Transform spawnPoint; // the point from which the projectiles will be spawned
    [HideInInspector]public Transform target; // the target towards which the projectiles will curve
    [HideInInspector]public float projectileSpeed; // the speed of the projectile
    [HideInInspector]public float curveStrength; // the strength of the curve
    private float stoppingDistance = 1f;
    private bool currentProjectile;
    Quaternion rotation;
    Vector3 faceDirection;
    void Start()
    {
        currentProjectile = true;
        ME_Sc = GameObject.Find("Player#").GetComponent<MagicEffect>();
    }

    void Update()
    {
       
        faceDirection = target.position - transform.position;
        //goingToPosition = destWithOffset;

        rotation = Quaternion.LookRotation(-faceDirection);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10 * Time.deltaTime);
        transform.rotation = rotation;
        float step = projectileSpeed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, step);
//------------
        if (Vector3.Distance(this.transform.position, target.position) < stoppingDistance)
        {
            ME_Sc.WaterExplode(0);
            Destroy(this.gameObject, .1f);
        }
    }
}
