using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Ability_Garna AG;
    [SerializeField] private GameObject DV_HitEffect_2;
    private void Awake() {
        AG = GameObject.FindGameObjectWithTag("Player").GetComponent<Ability_Garna>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag==("Projectile")){
            GameObject g = col.gameObject.transform.parent.gameObject;
            g.SetActive(false);
            DV_HitEffect_2.SetActive(true);
        }
        if(col.gameObject.tag==("Lance")){
            AG.BG_HitEffect.SetActive(true);
        }
    }
}
