using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSourceSlots;

    public void TryPlayClip(AudioClip audioClip)
    {

        int index = FindAnNotUsingSource();

        if (index >= 0)
        {
            print("SOM");
            audioSourceSlots[index].clip=audioClip;

            audioSourceSlots[index].Play();
        }
       
    }

    public void TryPlayClip(Vector3 playPosition, AudioClip audioClip)
    {
        int index = FindAnNotUsingSource();

        if (index >= 0)
        {
            audioSourceSlots[index].transform.position = playPosition;

            audioSourceSlots[index].clip = audioClip;

            audioSourceSlots[index].Play();
        }
    }

    public int FindAnNotUsingSource()
    {
        int id = 0;

        foreach(AudioSource audio in audioSourceSlots)
        {
            if (audio.isPlaying == false)
            {
                return id;
                break;
            }

            id++;
        }

        return  -1;
    }
}
