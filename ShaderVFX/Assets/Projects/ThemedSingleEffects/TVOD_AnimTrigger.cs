using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVOD_AnimTrigger : MonoBehaviour
{
    [SerializeField] private TheVoidOFDead TVOD;
    
    public void PuddleSplat_Big(){
        TVOD.ForPuddle_Big.SetActive(true);
    }
    public void ClosePuddleSplat_Big(){
        TVOD.ForPuddle_Big.SetActive(false);
    }
    public void PuddleSplat_Small(){
        TVOD.ForPuddle_Small.SetActive(true);
    }
    public void ClosePuddleSplat_Small(){
        TVOD.ForPuddle_Small.SetActive(false);
    }
}
