public static class CourseFactory
{
    public static Course GetCourse(CourseTypes courseName)
    {
        switch (courseName)
        {
            case CourseTypes.Science:
                return new ScienceCourse();
            case CourseTypes.Psychology:
                return new PsychologyCourse();
            case CourseTypes.Hacking:
                return new HackingCourse();
            case CourseTypes.Research:
                return new ResearchCourse();
            case CourseTypes.Sports:
                return new SportCourse();
            default:
                return null;
        }
    }
}

