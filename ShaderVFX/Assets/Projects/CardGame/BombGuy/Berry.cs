using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Put this script on the Small Berry Prefab
public class Berry : MonoBehaviour
{
    public GameObject target;
    public float _speed;
    public float _force;
    bool move = false;
    Rigidbody rb;
    Animator anim;
    
    void Start() {
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        StartCoroutine("Move"); 
    }

    void Update()
    {
        if(this.transform.position.z >= target.transform.position.z){
            Invoke("AutoDestruct", .5f);
        }
        if(move){
            this.transform.Rotate(Vector3.left * -50 * 5 * Time.deltaTime); 
            if(target!= null){
                float step = _speed * Time.deltaTime;
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, step);
            } 
        }
    }
    IEnumerator Move(){
        yield return new WaitForSeconds(.3f);
        rb.useGravity = true;
        rb.AddForce(transform.up * _force, ForceMode.Impulse);
        move = true;
    }
    void AutoDestruct(){
        Destroy(this.gameObject, .5f);
    }
}
