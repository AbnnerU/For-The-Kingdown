using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundTrigger : MonoBehaviour
{

    [SerializeField] private AiAtack aiAtack;
    [SerializeField] private HealthManager healthManager;

    [SerializeField] private string atackSoundManagerTag;
  
    [SerializeField] private string healthSoundManagerTag;

    [SerializeField] private AudioClip atackSound;

    [SerializeField] private AudioClip deathSound;

    [SerializeField] private AudioClip hittedSound;

    [SerializeField] private AudioClip healSound;

    private Transform _transform;

    private AudioManager healthSoundManager;

    private AudioManager atackSoundManager;

    

    private void Awake()
    {
        healthSoundManager = GameObject.FindGameObjectWithTag(healthSoundManagerTag).GetComponent<AudioManager>();
        atackSoundManager = GameObject.FindGameObjectWithTag(atackSoundManagerTag).GetComponent<AudioManager>();

        _transform = GetComponent<Transform>();

        if(aiAtack)
            aiAtack.OnExecuteAtack += AtackSound;

        if (healthManager)
        {
            healthManager.OnHitted += OnHittedSound;

            healthManager.OnHeal += OnHealSound;

            healthManager.OnDeath += OnDeathSound;
        }
    }

    private void OnDeathSound(GameObject obj)
    {
        if(deathSound)
            healthSoundManager.TryPlayClip(_transform.position,deathSound);
    }

    private void OnHealSound()
    {
        if(healSound)
            healthSoundManager.TryPlayClip(_transform.position, healSound);
    }

    private void AtackSound()
    {
        if(atackSound)
        atackSoundManager.TryPlayClip(_transform.position, atackSound);
    }

    private void OnHittedSound()
    {
        if(hittedSound)
        healthSoundManager.TryPlayClip(_transform.position, hittedSound);
    }
}
