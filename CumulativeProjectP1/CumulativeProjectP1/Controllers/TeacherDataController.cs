using CumulativeProjectP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using Renci.SshNet.Security.Cryptography;
using Org.BouncyCastle.Asn1.Cmp;

namespace CumulativeProjectP1.Controllers
{
    public class TeacherDataController : ApiController
    {
        // the context class allows us to comunicate with the SQL DB
        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the authors table of our blog database.
        /// <summary>
        /// Returns a list of Teachers in the system. if a search= key is included we filter where the teacher infromation is like the key
        /// </summary>
        /// <param name="search_key">An optional search key for teacher name information</param>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <example>GET api/TeacherData/ListTeachers?search_key</example>
        /// <returns>
        /// ({teacherId,teacherId, teacherfname,teacherHireDate, teacherSalary},{teacherId,teacherId, teacherfname,teacherHireDate, teacherSalary})
        /// </returns>
        [HttpGet]
        [Route("api/teacher/list/{search_key}")]
        public IEnumerable<teacher> listTeachers(string search_key)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatatbase();

            //open connection between the server and the database
            Conn.Open();

            //sql query to be sent to the database
            string query = "select * from teachers where teacherfname like @key or teacherlname like @key or employeenumber like @key or hiredate like @key or salary like @key or teacherid like @key";

            //output the query to the debug console
            Debug.WriteLine("the query is " + query);

            //establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //sql query will be the query defined above
            cmd.CommandText = query;

            //fill the parameters of the query to keep it safe from SQL injection
            cmd.Parameters.AddWithValue("@key", "%" + search_key + "%");

            cmd.Prepare();

            //gather resultset of the query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create an empty list of teachers
            List<teacher> teachers = new List<teacher>();


            while (ResultSet.Read())
            {
                //access column information by the DB column name as an index
                int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string teacherfname = ResultSet["teacherfname"].ToString();
                string teacherlname = ResultSet["teacherlname"].ToString();
                string employeenumber = ResultSet["employeenumber"].ToString();
                DateTime hiredate = Convert.ToDateTime(ResultSet["hiredate"]);
                decimal salary = Convert.ToDecimal(ResultSet["salary"].ToString());




                teacher Newteacher = new teacher();

                Newteacher.teacherid = teacherId;
                Newteacher.teacherfname = teacherfname;
                Newteacher.teacherlname = teacherlname;
                Newteacher.employeenumber = employeenumber;
                Newteacher.hiredate = hiredate;
                Newteacher.salary = salary;
                teachers.Add(Newteacher);
            }

            //close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //display the list of teachers to the user
            return teachers;

        }

        /// <summary>
        /// returns a teachers information when provided their ID
        /// </summary>
        /// <param name="TeacherId"> the primary key of teachers in the database</param>
        /// <returns>
        /// {teacherfname,teacherHireDate, teacherSalary}
        /// </returns>
        [HttpGet]
        [Route("api/teacherdata/FindTeacher/{TeacherId}")]

        public teacher FindTeacher(int TeacherId)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatatbase();

            //Open the connection to the database
            Conn.Open();

            //SQL query to be sent to the database
            string query = "Select * from teachers where teacherId= @key";

            //establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            //clean up the input from the user to prevent SQL injection
            cmd.Parameters.AddWithValue("key", TeacherId);

            //gather resultset of the query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //create an empty list of teachers
            teacher SelectedTeacher = new teacher();

            while (ResultSet.Read())
            {
                //access column information by the DB column name as an index
                SelectedTeacher.teacherid = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.teacherfname = ResultSet["teacherfname"].ToString();
                SelectedTeacher.teacherlname = ResultSet["teacherlname"].ToString();
                SelectedTeacher.employeenumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.hiredate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.salary = Convert.ToDecimal(ResultSet["salary"].ToString());

            }
            //close database connection
            Conn.Close();
            //display the list of teachers to the user
            return SelectedTeacher;
        }

        //Add teacher, delete teacher

        /// <summary>
        /// Adds teachers into the system
        /// </summary>
        /// <param name="NewTeacher"> artical object input</param>
        /// <returns></returns>
        [HttpPost]

        public void AddTeacher(teacher NewTeacher)
        {

            //creates an instance of a connection
            MySqlConnection Conn = School.AccessDatatbase();
            //opens the connection to the database
            Conn.Open();

            //create the sql command to insert a new teacher
            string query = "INSERT INTO teachers(teacherfname, teacherlname, employeenumber, hiredate, salary)" +
                " VALUES (@teacherfname,@teacherlname,@employeenumber,@hiredate,@salary);";



            //establish a new command for the database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;
            //add parameters to the query to prevent SQL injection
            cmd.Parameters.AddWithValue("@teacherfname", NewTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@teacherlname", NewTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.employeenumber);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.salary);
            cmd.Parameters.AddWithValue("@hiredate", NewTeacher.hiredate);


            cmd.Prepare();

            cmd.ExecuteNonQuery();
            //close the connection with the database
            Conn.Close();
        }


        /// <summary>
        /// Delete a teacher in the system
        /// </summary>
        /// <param name="teacherid">the primary key of the teacher to delete</param>
        /// <returns>
        /// </returns>
        /// <example>
        /// POST api/teacherdata/deleteteacher/{teacherid}
        /// </example>

        [HttpPost]
        [Route("api/teacherdata/deleteteacher/{teacherid}")]
        public void DeleteTeacher(int teacherid) //void means no return
        {
            Debug.WriteLine("trying to delete teacher with id" + teacherid);

            MySqlConnection Conn = School.AccessDatatbase();

            Conn.Open();

            string query = "delete from teachers where teacherid=@id";

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@id", teacherid);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

            //query
            //delete from teachers where teacherid=id
        }

        /// <summary>
        /// Update a teacher in the system
        /// <param name="teacherid"/>the id of the teacher in the systme
        /// <param name="UpdatedTeacher">post content</paramref>
        /// <example>
        /// api/teacherdata/updateteacher/18
        /// POST: POST CONTENT/ FORM BODY/ REQUEST BODY
        /// {"teacherfname" : "Bassil"}
        /// </example>
        /// </summary>
        [HttpPost]
        [Route("api/teacherdata/updateteacher/{teacherid}")]
        public void UpdateTeacher(int teacherid, [FromBody]teacher UpdatedTeacher)
        {
            Debug.WriteLine("updating" + teacherid);
            Debug.WriteLine(UpdatedTeacher.teacherfname);
            Debug.WriteLine(UpdatedTeacher.teacherlname);

            //create an instance of a connection to the school database
            MySqlConnection conn = School.AccessDatatbase();

            // opens the connection
            conn.Open();

            //SQL query to be sent to the database
            string query = "update teachers set teacherfname=@teacherfname, teacherlname=@teacherlname, employeenumber=@employeenumber, hiredate=@hiredate, salary=@salary where teacherid=@teacherid";
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = query;
            //add the updtated teacher information to the database
            cmd.Parameters.AddWithValue("@teacherfname", UpdatedTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@teacherlname", UpdatedTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", UpdatedTeacher.employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", UpdatedTeacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", UpdatedTeacher.salary);
            cmd.Parameters.AddWithValue("@teacherid", teacherid);

            
            cmd.Prepare();

            cmd.ExecuteNonQuery();
            //close the connection to the database
            conn.Close();

            //connection closed

        }


    }
}

