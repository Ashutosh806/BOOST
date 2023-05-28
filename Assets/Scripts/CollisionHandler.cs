using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
   [SerializeField]float Delay = 1f;
   [SerializeField]AudioClip Success,Crash;
   [SerializeField] ParticleSystem successParticles,crashParticles;

   AudioSource audioSource;
   
   bool isTransistioning = false;
   bool collisionDisabled = false;

   void Start() 
   {
     audioSource = GetComponent<AudioSource>();   
   }

   void Update() 
   {
      RespondToDebugKeys();
   }

   void RespondToDebugKeys()
   {
      if(Input.GetKeyDown(KeyCode.L))
      {
        NextLevel();
      }
      
      else if(Input.GetKeyDown(KeyCode.C))
      {
        collisionDisabled = !collisionDisabled;
      }
   }
   
   void OnCollisionEnter(Collision other) 
   {
     if(isTransistioning || collisionDisabled)
     { 
      return; 
     }

    switch(other.gameObject.tag)
    {
        case "Friendly":
           Debug.Log("This thing is friendly");
           break;
        case "Finish":
           StartSuccessSequence();
           break;
        default:
           StartCrashSequence();
           break;   
    }
   }

    void StartSuccessSequence()
    {
      isTransistioning = true;
      audioSource.Stop();
      audioSource.PlayOneShot(Success);
      successParticles.Play();
      GetComponent<Movement>().enabled = false;
      Invoke("NextLevel", Delay);  
    }

    void StartCrashSequence()
   {
      isTransistioning = true;
      audioSource.Stop();
      audioSource.PlayOneShot(Crash);
      crashParticles.Play();
      GetComponent<Movement>().enabled = false;
      Invoke("ReloadLevel", Delay);
   }

   void ReloadLevel()
    {
      int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(currentSceneIndex);
    }

    void NextLevel()
    {
       int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       int nextSceneIndex = currentSceneIndex + 1;
       if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
       {
         nextSceneIndex = 0; 
       }
       SceneManager.LoadScene(nextSceneIndex);
    }
}
