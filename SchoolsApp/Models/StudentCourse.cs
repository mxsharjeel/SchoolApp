using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolApp.Models
{
    public class StudentCourse
    {
        public virtual int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public virtual int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }
}
