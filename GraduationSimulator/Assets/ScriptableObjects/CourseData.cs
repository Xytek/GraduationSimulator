using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewCourse", menuName ="CourseData")]
public class CourseData : ScriptableObject
{
    public CourseFactory.CourseTypes type;
    public string[] UpgradeDescriptions;
    public int[] prices;
    public int UpgradeLevel;
}
