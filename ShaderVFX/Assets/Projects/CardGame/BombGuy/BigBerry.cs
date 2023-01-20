using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Put this script on the BigBerry Prefab
public class BigBerry : MonoBehaviour
{
    public GameObject effect;
    private BombGuyVFX BGV;
    private GameObject BG; //bomb guy
    void Start()
    {
        BG = GameObject.FindGameObjectWithTag("BombGuy");
        if(BG!=null){
            BGV = BG.GetComponent<BombGuyVFX>();
        }
        effect.SetActive(false);
        this.transform.eulerAngles = new Vector3(90f, 0f, 0f);
    }
    public void EffectEmit(){
        GameObject s = Instantiate(effect, this.transform.position, Quaternion.identity);
        s.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        s.SetActive(true);
        Destroy(s.gameObject,1f);
    }
    public void Destroy(){
        BGV.trail.SetActive(true);
        Destroy(this.gameObject);
    }
    void Update() {
    }
}
