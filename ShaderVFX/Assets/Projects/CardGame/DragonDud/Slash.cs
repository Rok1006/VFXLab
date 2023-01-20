using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Put this script on Double Slash and Single Slash
public class Slash : MonoBehaviour
{
    //public static Slash Instance;
    public GameObject target;
    public float speed;
    private float damping = 100;
    Vector3 dir;

    void Start()
    {
        dir = target.transform.position - this.transform.position;
    }

    void Update()
    {   if(target!= null){
            
            //dir.y = 0;
            var rotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping); 
            // float step = speed * Time.deltaTime;//change this to go pass target
            // this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, step);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if(this.transform.position.z >= target.transform.position.z){
            Invoke("AutoDestruct", 1f);
        }
    }

    void AutoDestruct(){
        Destroy(this.gameObject);
    }
}
