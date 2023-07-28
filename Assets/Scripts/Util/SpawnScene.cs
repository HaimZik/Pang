﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnScene : MonoBehaviour
{
    public string sceneName;
    public bool playAudioBeforeLoad = false;
    public LoadSceneMode loadMode = LoadSceneMode.Single;

    public void Spawn()
    {
        if (sceneName != "")
        {
            if (playAudioBeforeLoad)
            {
                StartCoroutine(PlaySoundThenSpawn());
            }
            else
            {
                SceneManager.LoadScene(sceneName, loadMode);
            }
        }
    }

    IEnumerator PlaySoundThenSpawn()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip, 0.5f);
        //Wait until clip finish playing
        yield return new WaitForSeconds(audioSource.clip.length - 2f);
        SceneManager.LoadScene(sceneName, loadMode);
    }

    public void SetSceneName(string name)
    {
        sceneName = name;
    }

}
