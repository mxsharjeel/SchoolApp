using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SchoolApp.Models;
using SchoolApp.Data;
using Microsoft.EntityFrameworkCore;
namespace SchoolsApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("-----------------Main Menu----------------");
                Console.WriteLine("1. Manage Students");
                Console.WriteLine("2. Manage Courses");
                Console.WriteLine("3. Exit");
                Console.Write("Enter Your Choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            studentHandler();
                            break;
                        case 2:
                            courseHandler();
                            break;
                        case 3:
                            return;
                        default:
                            Console.WriteLine("Invalid Choice, Please choose between 1 and 3");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please Enter a number.");
                };


            }
        }

        //student handler
        public static  void studentHandler()
        {
            while (true)
            {
                Console.WriteLine("---------------Student Menu---------------");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Retrieve Student");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Register Course");
                Console.WriteLine("5. Withdraw Course");
                Console.WriteLine("6. Delete Student");
                Console.WriteLine("7. Main Menu");
                Console.WriteLine("Enter your choice");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            try
                            {
                                {
                                    string name;
                                    do
                                    {
                                        Console.WriteLine("Enter student Name: ");
                                        name = Console.ReadLine();
                                        if (string.IsNullOrWhiteSpace(name))
                                        {
                                            Console.WriteLine("Name must not be null. Please input a valid name.");
                                        }
                                    } while (string.IsNullOrWhiteSpace(name));
                                    addStudent(new Student { Name = name });
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;


                        case 2:
                            retrieveStudents();
                            break;

                        case 3:
                            try
                            {
                                string name;
                                Console.WriteLine("Enter Student ID: ");
                                int id = int.Parse(Console.ReadLine());
                                if (!CheckStudentExists(id))
                                {
                                    Console.WriteLine($"No student found with {id}. Please check ID and try again.");
                                    return;
                                }
                                do
                                {
                                    Console.WriteLine("Enter Student Name: ");
                                    name = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(name))
                                    {
                                        Console.WriteLine("Name must not be null. Please input a valid name.");
                                    }
                                }
                                while (string.IsNullOrWhiteSpace(name));
                                updateStudent(id, new Student { Name = name });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 4:
                            try
                            {
                                Console.WriteLine("Enter Student ID: ");
                                int sid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Course ID: ");
                                int cid = int.Parse(Console.ReadLine());
                                if (!CheckStudentExists(sid))
                                {
                                    Console.WriteLine($"No student found with {sid}. Please check ID and try again.");
                                    return;
                                }
                                using (var context = new SchoolContext())
                                {
                                    var courseExists = context.Courses.Any(c => c.CourseId == cid);
                                    if (!courseExists)
                                    {
                                        Console.WriteLine($"No course found with {cid} . Please check ID and try again.");
                                        return;
                                    }
                                    var studentCourseExist = context.StudentCourses.Any(sc => sc.CourseId == cid && sc.StudentId == sid);
                                   if(studentCourseExist)
                                    {
                                        Console.WriteLine($"Student {sid} is already registered in Course {cid}. Please check the IDs and try again.");
                                        return;
                                    }
                                }
                                registerCourse(new StudentCourse { CourseId = cid, StudentId = sid });
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Error: Invalid input format. Please enter valid numerical IDs.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 5:
                            try
                            {
                                Console.WriteLine("Enter Student Id: ");
                                int sid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Course Id: ");
                                int cid = int.Parse(Console.ReadLine());
                                using (var context = new SchoolContext())
                                {
                                    var studentExists = context.StudentCourses.Any(s => s.StudentId == sid && s.CourseId == cid);
                                    if (!studentExists)
                                    {
                                        Console.WriteLine($"No student found with {sid} and course with {cid}. Please check ID and try again.");
                                        return;
                                    }
                                }
                                removeCourse(sid, cid);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 6:
                            try
                            {
                                Console.WriteLine("Enter Student ID");
                                int id = int.Parse(Console.ReadLine());
                                if (!CheckStudentExists(id))
                                {
                                    Console.WriteLine($"No student found with {id}. Please check ID and try again.");
                                    return;
                                }
                                deleteStudent(id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 7:
                            return;
                        default:
                            Console.WriteLine("Invalid Choice. Please choose between 1 and 7.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input. Please Enter a number.");
                }
            }
        }

        //course handler
        public static void courseHandler()
        {
            while (true)
            {
                Console.WriteLine("---------------Course Menu----------------");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. Retrieve Course");
                Console.WriteLine("3. Update Course");
                Console.WriteLine("4. Register Student");
                Console.WriteLine("5. Remove Student");
                Console.WriteLine("6. Delete Course");
                Console.WriteLine("7. Main Menu");
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            try
                            {
                                string title;
                                do
                                {
                                    Console.WriteLine("Enter Course Title: ");
                                    title = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(title))
                                    {
                                        Console.WriteLine("Course Title cannot be null. Please enter a valid title.");
                                    }
                                }
                                while (string.IsNullOrWhiteSpace(title));
                                addCourse(new Course { Title = title });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;

                        case 2:
                            try
                            {
                                retrieveCourse();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 3:
                            try
                            {
                                Console.WriteLine("Enter Course ID: ");
                                int id = int.Parse(Console.ReadLine());
                                using (var context = new SchoolContext())
                                {
                                    var courseExists = context.Courses.Any(c => c.CourseId == id);
                                    if (!courseExists)
                                    {
                                        Console.WriteLine($"No course found with {id}. Please check ID and try again.");
                                        return;
                                    }
                                }
                                Console.WriteLine("Enter Course Title: ");
                                string title;
                                do
                                {
                                    title = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(title))
                                    {
                                        Console.WriteLine("Course title cannot be null. Please Enter a valid Course title.");
                                    }
                                }
                                while (string.IsNullOrWhiteSpace(title));
                                updateCourse(id, new Course { Title = title });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;

                        case 4:
                            try
                            {
                                Console.WriteLine("Enter Course ID: ");
                                int cid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Student ID: ");
                                int sid = int.Parse(Console.ReadLine());
                                using (var context = new SchoolContext())
                                {

                                    var studentExist = context.Courses.Any(c => c.CourseId==cid);
                                    if(!studentExist)
                                    {
                                        Console.WriteLine($"Course {cid} does not exist. Please check ID and try again.");
                                    }
                                    if (!CheckStudentExists(sid))
                                    {
                                        Console.WriteLine($"Student {sid} does not exist. Please check ID and try again.");
                                    }
                                    var studentCourseExist = context.StudentCourses.Any(sc => sc.CourseId == cid && sc.StudentId == sid);
                                    if (studentCourseExist)
                                    {
                                        Console.WriteLine($"Student {sid} is already registered in Course {cid}. Please check the IDs and try again.");
                                        return;
                                    }
                                }
                               
                                registerStudent(new StudentCourse { CourseId = cid, StudentId = sid });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 5:
                            try
                            {
                                Console.WriteLine("Enter Course ID: ");
                                int cid = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Student ID: ");
                                int sid = int.Parse(Console.ReadLine());

                                using (var context = new SchoolContext())
                                {
                                    // Check if the student exists
                                    if (!CheckStudentExists(sid))
                                    {
                                        Console.WriteLine($"No student found with ID {sid}. Please check the ID and try again.");
                                        return;
                                    }

                                    // Check if the course exists
                                    if (!context.Courses.Any(c => c.CourseId == cid))
                                    {
                                        Console.WriteLine($"No course found with ID {cid}. Please check the ID and try again.");
                                        return;
                                    }

                                    // Check if the student is registered in the course
                                    if (!context.StudentCourses.Any(sc => sc.StudentId == sid && sc.CourseId == cid))
                                    {
                                        Console.WriteLine($"Student ID {sid} is not registered in Course ID {cid}. Please check the IDs and try again.");
                                        return;
                                    }
                                }

                                // Remove the student from the course
                                removeStudent(sid, cid);
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Error: Invalid input format. Please enter valid numerical IDs.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;

                        case 6:
                            try
                            {
                                Console.WriteLine("Enter course ID to delete: ");
                                int id = int.Parse(Console.ReadLine());
                                using (var context = new SchoolContext())
                                {
                                    var courseExists = context.Courses.Any(c => c.CourseId == id);
                                    if (!courseExists)
                                    {
                                        Console.WriteLine($"No course found with {id}. Please check ID and try again.");
                                        return;
                                    }
                                }
                                deleteCourse(id);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            break;
                        case 7:
                            return;
                        default:
                            Console.WriteLine("Inavlid Input. Please enter number between 1 and 5");
                            break;
                    }
                }
            }
        }


        //add student
        public static void addStudent(Student student)
        {
            using (var context = new SchoolContext())
            {
                context.Students.Add(student);
                context.SaveChanges();
                Console.WriteLine($"Student Added Successfully.");
            }
        }
        //delete student
        public static void deleteStudent(int id)
        {
            using (var context = new SchoolContext())
            {
                var student = context.Students.Find(id);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                    Console.WriteLine("Student Deleted Successfully.");
                }
            }
        }

        //retrieve student
        public static void retrieveStudents()
        {
            using (var context = new SchoolContext())
            {
                var students = context.Students
                    .Include(s => s.StudentCourse)
                    .ThenInclude(sc => sc.Course)
                    .Select(s => new
                    {
                        s.StudentId,
                        s.Name,
                        Courses = s.StudentCourse.Select(sc => new
                        {
                            sc.Course.Title,
                            sc.Course.CourseId
                        })
                    })
                    .ToList();

                Console.WriteLine("----------------- Students -----------------");
                foreach (var student in students)
                {
                    Console.WriteLine("==========================================");
                    Console.WriteLine($"Student ID: {student.StudentId}");
                    Console.WriteLine($"Student Name: {student.Name}");

                    foreach (var course in student.Courses)
                    {
                        Console.WriteLine($"Course Title: {course.Title}");
                        Console.WriteLine($"Course ID: {course.CourseId}");
                    }
                    Console.WriteLine("==========================================");
                    Console.WriteLine();
                }
            }
        }



        //update student
        public static void updateStudent(int id, Student student)
        {
            using (var context = new SchoolContext())
            {
                var students = context.Students.Find(id);
                if (students != null)
                {
                    students.Name = student.Name;

                    context.SaveChanges();
                    Console.WriteLine("Updated Successfully");
                }
            }
        }

        //add course
        public static void addCourse(Course course)
        {
            using (var context = new SchoolContext())
            {
                context.Courses.Add(course);
                context.SaveChanges();
                Console.WriteLine("Course Added Successfully");
            }
        }

        //retrieve course
        public static void retrieveCourse()
        {

            using (var context = new SchoolContext())
            {
                var courses = context.Courses
                                .Include(sc => sc.StudentCourse)
                                .ThenInclude(s => s.Student)
                                .ToList();
                Console.WriteLine("-----------------Courses------------------");
                foreach (var course in courses)
                {
                    Console.WriteLine("==========================================");
                    Console.WriteLine($"Course ID: {course.CourseId}");
                    Console.WriteLine($"Course Title: {course.Title}");
                    foreach (var j in course.StudentCourse)
                    {
                        Console.WriteLine($"Student ID: {j.Student.StudentId}");
                        Console.WriteLine($"Student Name: {j.Student.Name}");
                    }
                    Console.WriteLine("==========================================");
                    Console.WriteLine();
                }
            }
        }

        //update course
        public static void updateCourse(int id, Course course)
        {
            using (var context = new SchoolContext())
            {
                var courseupdate = context.Courses.Find(id);
                if (courseupdate != null)
                {
                    courseupdate.Title = course.Title;
                    context.SaveChanges();
                }
            }
        }

        //delete course
        public static void deleteCourse(int id)
        {
            using (var context = new SchoolContext())
            {
                var course = context.Courses.Find(id);
                if (course != null)
                {
                    context.Courses.Remove(course);
                    context.SaveChanges();
                    Console.WriteLine("Course deleted successfully.");
                }
            }
        }

        //update student course
        public static void registerCourse(StudentCourse sc)
        {
            using (var context = new SchoolContext())
            {

                context.StudentCourses.Add(sc);
                context.SaveChanges();
                Console.WriteLine("Course Registered Successfully.");
            }
        }

        //remove student course
        public static void removeCourse(int sid, int cid)
        {
            using (var context = new SchoolContext())
            {
                var rc = context.StudentCourses.SingleOrDefault(s => s.StudentId == sid && s.CourseId == cid);
                context.StudentCourses.Remove(rc);
                context.SaveChanges();
                Console.WriteLine("Course withdrawn successfully.");
            }
        }

        //register student

        public static void registerStudent(StudentCourse sc)
        {
            using (var context = new SchoolContext())
            {
                context.StudentCourses.Add(sc);
                context.SaveChanges();
                Console.WriteLine("Student Registered Successfully.");
            }
        }
        //remove course student
        public static void removeStudent(int sid, int cid)
        {
            using (var context = new SchoolContext())
            {
                var studentCourse = context.StudentCourses.SingleOrDefault(s => s.StudentId == sid && s.CourseId == cid);
                if (studentCourse == null)
                {
                    Console.WriteLine($"No registration found for Student ID {sid} in Course ID {cid}.");
                    return;
                }

                context.StudentCourses.Remove(studentCourse);
                context.SaveChanges();
                Console.WriteLine("Student removed successfully.");
            }
        }



        //students exists
        public static bool CheckStudentExists(int id)
        {
            using (var context = new SchoolContext())
            {
                return context.Students.Any(s => s.StudentId == id);
            }
        }


    }


}
