using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissileBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private MagicEffect ME_Sc;
    public Transform target;
    public Vector3 destWithOffset;
    public string[] collisionTagsToCheck;
    public float duration, rotationSpeed, beforeTurnSpeed, afterTurnSpeed, defaultDestinationDistance, distanceBeforeTurn, destroyDelay;
    //5, 13, 60, 50, 5, 2, 2
    Vector3 startPosition, faceDirection, goingToPosition;
    float distance;

    Quaternion rotation;
    ParticleSystem loopFX, impactFX;
    public int explosionIndex;

    bool hasTurned = false, isDead = false;
    [SerializeField] private GameObject FrontWhite;
    [SerializeField] private GameObject Tunnel;
    [SerializeField] private GameObject Head;
    [SerializeField] private GameObject Trail;
    [SerializeField] private GameObject BaseTrail;
    void Start()
    {
     //   loopFX = transform.Find("MissileLoop").GetComponent<ParticleSystem>();
      //  impactFX = transform.Find("Impact").GetComponent<ParticleSystem>();

        startPosition = transform.position;
        ME_Sc = GameObject.Find("Player#").GetComponent<MagicEffect>();

        var rot = transform.rotation.eulerAngles;
        rot.y += 90;
        rot.x = Random.Range(-150, -170); //210
        transform.rotation = Quaternion.Euler(rot);
        Debug.Log(rot);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) { return; }

        if (hasTurned)
        {
            if (destWithOffset != null)
            {
                faceDirection = destWithOffset - transform.position;
                goingToPosition = destWithOffset;
            }

            rotation = Quaternion.LookRotation(faceDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            transform.Translate(transform.forward * afterTurnSpeed * Time.deltaTime, Space.World);
            //transform.position.y = 0;
            //transform.position = new Vector3(transform.position.x, goingToPosition.y, transform.position.z);
            distance = Vector3.Distance(transform.position, goingToPosition);
            Debug.Log(distance);
            if (distance <= 1f)
            {
                Explode();
            }
        }
        else
        {
            transform.Translate(transform.forward * beforeTurnSpeed * Time.deltaTime, Space.World);
            //transform.Translate(Vector3.up * beforeTurnSpeed * Time.deltaTime, Space.World);
            distance = Vector3.Distance(transform.position, startPosition);
            if (distance > distanceBeforeTurn)
            {
                hasTurned = true;
            }
        }

        if(duration <= 0)
        {
            //Explode();
        }
        else
        {
            duration -= Time.deltaTime;
        }
       
    }

    void Explode()
    {
        isDead = true;
        // loopFX.Stop();
        // impactFX.Play();
        ME_Sc.WaterExplode(explosionIndex);
        Debug.Log("Boom");
        FrontWhite.SetActive(false);
        Tunnel.SetActive(false);
        Head.SetActive(false);
        BaseTrail.SetActive(false);
        Trail.SetActive(false);
        Destroy(this.gameObject, .5f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (collisionTagsToCheck.Contains(other.tag) && !isDead)
        {
            //can damage player here
            //Explode();
        }
    }
}
