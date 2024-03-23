using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manger_LP : MonoBehaviour
{
    [SerializeField] private GameObject Cam1;
    [SerializeField] private GameObject Cam2;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            if(index==1){
                Cam1.SetActive(false);
                Cam2.SetActive(true);
                index=2;
            }else if(index==2){
                Cam2.SetActive(false);
                Cam1.SetActive(true);
                index=1;
            }
        }
    }
}
