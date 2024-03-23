using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script: Stinger object
public class Stinger : MonoBehaviour
{
    [SerializeField] private Ability_Garna AG;
    [SerializeField] private GameObject target;
    [SerializeField] private float _speed;
    public bool move = false;
    public bool arrived = false;
    //[SerializeField] private float _force;
    
    void Start()
    {
        target = GameObject.FindWithTag("Target");
        AG = GameObject.FindWithTag("Player").GetComponent<Ability_Garna>();
    }

    void Update()
    {
        // if(this.transform.position.z >= target.transform.position.z){
        //     Invoke("AutoDestruct", .5f);
        //     arrived = true;
        // }
        if(move){
            if(target!= null){
                float step = _speed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, step);
            } 
        }
    }
    // IEnumerator Move(){
    //     // yield return new WaitForSeconds(.3f);
    //     // rb.useGravity = true;
    //     // rb.AddForce(transform.up * _force, ForceMode.Impulse);
    //     // move = true;
    // }
    void AutoDestruct(){
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider col) {
        // Debug.Log("penetrate");
        if(col.gameObject.tag==("Enemy")){
            arrived = true;
        }
        
        // GameObject s = Instantiate(AG.stingerHitEffect, col.transform.position, Quaternion.identity);
    }
}
