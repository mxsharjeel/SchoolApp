using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class Student
    {
        [Key]
        public virtual int StudentId { get; set; }

        [Required]
        public virtual string Name { get; set; }
        public virtual ICollection<StudentCourse> StudentCourse { get; set; }
    }
}
