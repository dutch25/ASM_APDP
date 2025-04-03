using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASM_APDP.Models;

namespace ASM_APDP.ViewModels
{
    public class AssignCoursesToStudentsViewModel
    {
        public int ClassId { get; set; } // Original ClassId
        public int NewClassId { get; set; } // New ClassId from dropdown
        public int StudentId { get; set; }
        public string ClassName { get; set; }
        public int CourseId { get; set; }
        public List<User> Students { get; set; }
        public List<Class> Classes { get; set; }
        public List<Course> Courses { get; set; }
    }
}