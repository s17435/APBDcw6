using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw4.DAL;
using cw4.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cw4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        // Do połączenia się z bazą danych wykorzystałem Dockera
        // Połączenie do MS SQL przez VPN na MACu niestety mnie przerosło
        private const string ConString = "Server=localhost,1433; Database=Master; User Id=SA; Password= Pa55w0rd";
        //private const string ConString = "Server=db-mssql.pjwstk.edu.pl; Initial Catalog=s17435; User ID=PJWSTK/s17435; Password= ";
        //private const string ConString = "Server=jdbc:jtds:sqlserver://db-mssql.pjwstk.edu.pl/s17435; Initial Catalog=s17435; User ID=s17435; Password= ";


        private IDbService _dbService;



        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
      


        }






        [HttpGet]
        public IActionResult GetStudent(string orderBy = "Nazwisko")


        {
            List<Student> list = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT IndexNumber, FirstName, LastName, BirthDate, Name, Semester FROM Student INNER JOIN Enrollment E on Student.IdEnrollment = E.IdEnrollment INNER JOIN Studies S on E.IdStudy = S.IdStudy";

                con.Open();
                SqlDataReader sqlDataReader = com.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Student student = new Student();
                    student.IndexNumber = sqlDataReader["IndexNumber"].ToString();

                    student.FirstName = sqlDataReader["FirstName"].ToString();
                    student.LastName = sqlDataReader["LastName"].ToString();
                    student.BirthDate = (DateTime) sqlDataReader["BirthDate"];

                   
                    list.Add(student);
                }

                con.Close();
            }

            
            return Ok(list);
        }




        // Używając polecenia np: https://localhost:5001/api/students/2; DROP TABLE STUDENTS
        // Bardzo  łatwo usunąć tabelę

        [HttpGet("{id}")]
        public IActionResult GetStudentById(string id)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
         
                com.CommandText = "SELECT IndexNumber, FirstName, LastName, BirthDate, Name, Semester FROM Student INNER JOIN Enrollment E on Student.IdEnrollment = E.IdEnrollment INNER JOIN Studies S on E.IdStudy = S.IdStudy WHERE IndexNumber = " + @id;
                com.Parameters.AddWithValue("id", id);
                con.Open();
                SqlDataReader sqlDataReader = com.ExecuteReader();
               

                if(sqlDataReader.Read())
                {
                    Student student = new Student();
                    student.IndexNumber = sqlDataReader["IndexNumber"].ToString();

                    student.FirstName = sqlDataReader["FirstName"].ToString();
                    student.LastName = sqlDataReader["LastName"].ToString();
                    student.BirthDate = (DateTime)sqlDataReader["BirthDate"];





                    return Ok(student);
                } else
                { return NotFound("Nie ma takiego ucznia"); }

            }
        }



        [HttpGet("en/{id}")]
        public IActionResult GetEnrollmentByStudentId(string id)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;

                com.CommandText = "SELECT Name, Semester FROM Student INNER JOIN Enrollment E on Student.IdEnrollment = E.IdEnrollment INNER JOIN Studies S on E.IdStudy = S.IdStudy WHERE IndexNumber = " + @id;
                com.Parameters.AddWithValue("id", id);
                con.Open();
                SqlDataReader sqlDataReader = com.ExecuteReader();

                if (sqlDataReader.Read())
                {
         
                    String response = "Semestr: "+ sqlDataReader["Semester"].ToString() + " Studia: " + sqlDataReader["Name"].ToString();
         




                    return Ok(response);
                }
                else
                { return NotFound("Nie ma takiego ucznia"); }

            }
        }





        //[HttpPost]
        //public IActionResult CreateStudent(Student student)
        //{
        //    student.IndexNumber = $"s{new Random().Next(1, 20000)}";
        //    _dbService.AddStudent(student);


        //    return Ok(student);
        //}


        //[HttpPut("{id}")]
        //public IActionResult UpdateStudent(int id, Student tmpstudent)
        //{

        //    Student existingstudent = _dbService.GetStudentById(id);

        //    if (existingstudent is null)
        //    {
        //        return NotFound("Nie ma ucznia o takim id");
        //    }
        //    else
        //    {

        //        _dbService.UpdateStudent(id, tmpstudent);

        //        return Ok("Aktualizacja dokończona");
        //    }



        //}



        ////[HttpPut("{id}")]
        ////public IActionResult UpdateStudent(int id)
        ////{

        ////    return Ok("Aktualizacja dokończona");
        ////}




        //[HttpDelete("{id}")]
        //public IActionResult DeleteStudent(int id)
        //{
        //    Student existingstudent = _dbService.GetStudentById(id);

        //    if (existingstudent is null)
        //    {
        //        return NotFound("Nie ma ucznia o takim id");
        //    } else
        //    {
        //        _dbService.DeleteStudent(id);

        //        return Ok("Usuwanie ukończone");
        //    }


        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteStudent(int id)
        //{
        //    return Ok("Usuwanie ukończone");
        //}


    }
}
