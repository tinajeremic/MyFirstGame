using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] string _levelName;
    Monster[] _monsters;    

    void OnEnable()
    {
        _monsters = FindObjectsOfType<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MonstersAreAllDead())
            GoToNextLevel();
    }

    bool MonstersAreAllDead()
    {
        foreach (var monster in _monsters)
        {
            if (monster.gameObject.activeSelf)
                return false;
        }

        return true;
    }

    void GoToNextLevel()
    {
        Debug.Log("Go to Level " + _levelName);
        UnlockLevel();
        SceneManager.LoadScene(_levelName);
    }

    void UnlockLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
