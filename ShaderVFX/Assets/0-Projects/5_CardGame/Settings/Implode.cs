using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Implode : MonoBehaviour
{
    public GameObject Top;
    void Start() {
         StartCoroutine(ImplodeP());
     }
     
     IEnumerator ImplodeP() {
         if (!GetComponent<ParticleSystem>().isPlaying) {
             GetComponent<ParticleSystem>().Play();
         }
         
         //let the system do it's thing for a bit
         yield return new WaitForSeconds(0f);
         
         //stop emission
         //GetComponent<ParticleSystem>().emissionRate = 0f;
         
         //allocate reference array
         ParticleSystem.Particle[] particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
         
         
         //this loop executes over several frames
         // - get particle list
         // - update each particle's position
         // - set particle list
         // - wait one frame
         for (float t = 0f; t < 1f; t += 0.1f) {
             int count = GetComponent<ParticleSystem>().GetParticles(particles);
             for (int i=0; i<count; i++) {
                 particles[i].position = Vector3.Lerp(particles[i].position, Top.transform.position, t);
             }
             GetComponent<ParticleSystem>().SetParticles(particles, count);
             
             yield return null;
         }
         
         //once loop is finished, clear particles
         GetComponent<ParticleSystem>().Clear();
     }
 }