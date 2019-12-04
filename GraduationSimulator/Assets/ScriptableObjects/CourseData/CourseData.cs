using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="NewCourse", menuName ="CourseData")]
public class CourseData : ScriptableObject
{
    public CourseTypes type;
    public string[] UpgradeDescriptions;
    public int[] prices;
    public Sprite icon;
}
