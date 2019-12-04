﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UpgradePlayer : MonoBehaviour
{
    private Dictionary<CourseTypes, Course> activeCourses;
    private Player _player;
    private PlayerStats _playerStats;

    public void Awake()
    {     
        activeCourses = new Dictionary<CourseTypes, Course>();
        _player = this.gameObject.GetComponent<Player>();
        if (_player == null) Debug.LogError("No player found");
        _playerStats = this.gameObject.GetComponent<PlayerStats>();
        if (_playerStats == null) Debug.LogError("No player stats found");
    }

    public void AddUpgrade(CourseData data)
    {        
        CourseTypes type = data.type;

        // check if that course is already in the dictionary       
        Course activeCourse = null;        
        foreach (KeyValuePair<CourseTypes, Course> entry in activeCourses)
            if(entry.Key == type)
                activeCourse = entry.Value;

        // if not create Course and add it to the dictionary
        if (activeCourse == null)
        {            
            activeCourse = CourseFactory.GetCourse(type);
            activeCourses.Add(type, activeCourse);
            activeCourse.Initialize(type);
        }

        // upgrade and pay                
        activeCourse.Upgrade();
        int amount = data.prices[activeCourse.UpgradeLevel - 1];
        if (_playerStats.Credits - amount > 0)
            _playerStats.UpdateCredits(-amount, true);
    }
}
