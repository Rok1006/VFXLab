using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Put this script on the HealPt Prefab - Fox Person
public class HealPt : MonoBehaviour
{
    public float _shootSpeed;
    public float _force;
    public GameObject target;
    Rigidbody rb;
    bool move = false;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        StartCoroutine("Move"); 
    }

    void Update()
    {       
        if(this.transform.position.z == target.transform.position.z){
            Invoke("AutoDestruct", .5f);
        }
        if(move){
            float step = _shootSpeed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, step);
        }
    }
    IEnumerator Move(){
        yield return new WaitForSeconds(0f);
        rb.AddForce(transform.up * _force, ForceMode.Impulse);
        move = true;
    }
    void AutoDestruct(){
        Destroy(this.gameObject);
    }
}
