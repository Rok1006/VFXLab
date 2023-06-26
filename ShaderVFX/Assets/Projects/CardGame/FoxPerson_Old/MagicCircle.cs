using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Put this script on the All the Magic Circle Prefab - Fox Person
public class MagicCircle : MonoBehaviour
{
    public float _deathTime;
    Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        StartCoroutine("Destroy"); 
    }
    IEnumerator Destroy(){
        yield return new WaitForSeconds(_deathTime);
        anim.SetTrigger("die");
        Destroy(this.gameObject,.5f);
    }

}
