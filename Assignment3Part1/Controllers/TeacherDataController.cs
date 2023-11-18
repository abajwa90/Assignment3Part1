using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Assignment3Part1.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext schoolDB = new SchoolDbContext();

        [HttpGet]
        [Route("api/teacherdata/listteachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            // Create connection to database
            MySqlConnection Conn = schoolDB.AccessDatabase();

            // Open database connection
            Conn.Open();

            // Create command for the database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query to retrieve teachers based on search key
            cmd.CommandText = "SELECT * FROM teachers WHERE LOWER(teacherfname) LIKE LOWER(@key) OR LOWER(teacherlname) LIKE LOWER(@key) OR CONCAT(teacherfname, ' ', teacherlname) LIKE LOWER(@key)";
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            // store result in variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // List to store teacher data
            List<Teacher> TeachersList = new List<Teacher> ();

            // loop for result set
            while (ResultSet.Read())
            {
                // use column values
                string TeacherName = ResultSet["teacherfname"] + " " + ResultSet["teacherlname"];
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);

                // new Teacher var
                Teacher newTeacher = new Teacher();
                newTeacher.TeacherId = TeacherId;
                newTeacher.TeacherName = TeacherName;
                newTeacher.Salary = Salary;

                // Add the Teacher object to the list
                Teachers.Add(newTeacher);
            }

            // Close database connection
            Conn.Close();

            // Return list of teachers
            return TeachersList;
        }
        
        [HttpGet]
        [Route("api/teacherdata/findteacher/{teacherid}")]
        public Teacher FindTeacher(int TeacherId)
        {
            // Initialize a new Teacher object
            Teacher chooseTeacher = new Teacher();

            // Create a connection to the database
            MySqlConnection Conn = schoolDB.AccessDatabase();

            // Open the database connection
            Conn.Open();

            // Create a command to interact with the database
            MySqlCommand cmd = Conn.CreateCommand();

            // SQL query to retrieve information about a specific teacher
            cmd.CommandText = "SELECT t.teacherid, CONCAT(t.teacherfname, t.teacherlname) AS teachername , t.salary AS salary, t.hiredate AS hiredate, c.classname AS classname FROM teachers t JOIN classes c ON c.teacherid = t.teacherid WHERE t.teacherid =" + TeacherId + " GROUP BY t.teacherid;";

            // Execute the query and store the result
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            // Process the result set
            while (ResultSet.Read())
            {
                // Extract column values
                chooseTeacher.TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                chooseTeacher.TeacherName = ResultSet["teachername"].ToString();
                chooseTeacher.Salary = Convert.ToDecimal(ResultSet["salary"]);
                chooseTeacher.HireDate = ResultSet["hiredate"].ToString();
                chooseTeacher.ClassName = ResultSet["classname"].ToString();
            }

            // Close the database connection
            Conn.Close();

            // Return the selected Teacher object
            return chooseTeacher;
        }
    }
}

