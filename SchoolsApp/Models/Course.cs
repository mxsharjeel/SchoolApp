using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class Course
    {
        [Key]
        public virtual int CourseId { get; set; }
        public virtual string Title { get; set; }
        public virtual ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
