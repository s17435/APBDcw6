using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cw4.DAL;
using cw4.DTOs.Requests;
using cw4.DTOs.Responses;
using cw4.Models;
using cw4.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw4.Controllers
{
    [Route("api/enrollments")]
    [ApiController] //-> implicit model validation
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDBService _service;


        public EnrollmentsController(IStudentDBService service)
        {
            _service = service;
        }

        
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request){
            
//         {
//             var student = new Student();
//             student.IndexNumber = request.IndexNumber;
//             student.FirstName = request.FirstName;
//             student.LastName = request.LastName;
//             student.BirthDate = request.BirthDate;
//             
//             Enrollment enrollment = new Enrollment();
//
//             
//             
//             if (!ModelState.IsValid)
//             {
//                 var d = ModelState;
//                 return BadRequest("!!!");
//             }
//
//             using (var con = new SqlConnection(ConString))
//             {
//                 using (var com = new SqlCommand())
//                 {
//                     com.Connection = con;
//                     con.Open();
//
//                     var transaction = con.BeginTransaction();
//                     com.Transaction = transaction;
//
//                     try
//                     {
//                         Console.WriteLine(request.Studies);
//                         // Sprawdzenie czy studia istnieją
//                         com.CommandText = "SELECT IdStudy from studies where name = @name";
//                         com.Parameters.AddWithValue("name", request.Studies);
//
//                         var dr = com.ExecuteReader();
//
//                         if (!dr.Read())
//                         {
//                             dr.Close();
//                             transaction.Rollback();
//                             return BadRequest("Studia nie istnieją");
//                             //...
//                         }
//
//                         int idStudies = (int) dr["IdStudy"];
//                         dr.Close();
//
//                         enrollment.IdStudy = idStudies;
//
//
//
//                         // Odnalezienie najnowszego wpisu 
//                         com.CommandText =
//                             "SELECT * FROM ENROLLMENT WHERE IdEnrollment = (SELECT MAX(IdEnrollment) FROM Enrollment WHERE IdStudy = @idStudies AND Semester = 1);";
//                         com.Parameters.AddWithValue("idStudies", idStudies);
//                         var dr1 = com.ExecuteReader();
//                         int idEnrollment;
//                         // Sprawdzenie czy wpis istnieje, jeżeli nie, jestem zobowiązany go utworzyć
//                         if (!dr1.Read())
//                         {
//                             dr1.Close();
//                             com.CommandText = "SELECT MAX(IdEnrollment) + 1 FROM Enrollment";
//                            idEnrollment = Convert.ToInt32(com.ExecuteScalar());
//                            Console.WriteLine(idEnrollment);
//                             DateTime myDateTime = DateTime.Now;
//                             string dateFormat = myDateTime.ToString("yyyy-MM-dd");
//                             Console.WriteLine(dateFormat);
//                             Console.WriteLine(idStudies);
//                         
//                             com.CommandText =
// //                                "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES ({idEnrollment},1,{idStudies}, '2000-12-12')";
//                             "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) " +
//                             $"VALUES ({idEnrollment},1,{idStudies}, '{dateFormat}')";
//
//                             // com.Parameters.AddWithValue("idEnrollment", idEnrollment);
//                             // com.Parameters.AddWithValue("date", dateFormat);
//                             // com.Parameters.AddWithValue("idStudies", idStudies);
//                             com.ExecuteNonQuery();
//                         
//                             enrollment.Semester = 1;
//                             enrollment.IdEnrollment = idEnrollment;
//                             enrollment.IdStudy = idStudies;
//                             enrollment.StartDate = myDateTime;
//                         
//                         }
//                         else
//                         {
//                             idEnrollment = (int) dr1["IdEnrollment"];
//                             enrollment.IdEnrollment = idEnrollment;
//                             enrollment.Semester = (int) dr1["Semester"];
//                             enrollment.IdStudy = (int) dr1["IdStudy"];
//                             enrollment.StartDate = (DateTime) dr1["StartDate"];
//                         
//                         }
//                         dr1.Close();
//
//                         
//                         
//                         // Sprawdzenie, czy student ma unikalne ID
//                         com.CommandText = "SELECT * FROM Student WHERE IndexNumber = @index";
//                         com.Parameters.AddWithValue("index", request.IndexNumber);
//                         var dr3 = com.ExecuteReader();
//                         if (dr3.Read())
//                         {
//                             dr3.Close();
//                             transaction.Rollback();
//                             return BadRequest("Numer indeksu nie jest unikalny");
//                         }
//                         
//                         dr3.Close();
//                         
//                         
//                         
//                         
//                         com.CommandText =
//                             "INSERT INTO Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@studentindex, @fname, @lname, @dob, @idEnrollment)";
//                         
//                         
//                         com.Parameters.AddWithValue("studentindex", request.IndexNumber);
//                         com.Parameters.AddWithValue("fname", request.FirstName);
//                         com.Parameters.AddWithValue("lname", request.LastName);
//                         com.Parameters.AddWithValue("dob", request.BirthDate);
//                         com.Parameters.AddWithValue("idEnrollment", idEnrollment);
//                         
//                         com.ExecuteNonQuery();
//
//                         transaction.Commit();
//
//                     }
//                     catch (SqlException exc)
//                     {
//                         transaction.Rollback();
//                         Console.WriteLine("SQL ERROR and rollback");
//                         Console.WriteLine(exc);
//
//
//                     }
//
//
//
//
//
//
//
//
//
//
//                 }
//             }

            if (!ModelState.IsValid) 
            {
                 var d = ModelState;
                 return BadRequest("!!!");
             }
            
       
        var response = _service.EnrollStudent(request);

        if (response.Status == 201)
        {
            return CreatedAtAction(response.Message, response.enrollment);
        }

        
            return BadRequest(response.Message);
        
          

    
        }


        [HttpPost("/api/enrollments/promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            
            if (!ModelState.IsValid)
            {
                var d = ModelState;
                return BadRequest("!!!");
            }

            // using (var con = new SqlConnection(ConString))
            // {
            //     using (var com = new SqlCommand())
            //     {
            //         com.Connection = con;
            //         con.Open();
            //
            //         var transaction = con.BeginTransaction();
            //         com.Transaction = transaction;
            //         
            //         //1. Sprawdzam czy w tabeli enrollment istnieje wpis o podanej wartości Studies i Semester, W przeciwnym razie zwracam kod 404 Not Found
            //         
            //         com.CommandText = "SELECT * FROM Enrollment" +
            //                           " INNER JOIN Studies" +
            //                           " ON Studies.IdStudy = Enrollment.IdStudy" +
            //                           " WHERE Enrollment.Semester = @semester" +
            //                           " AND Studies.Name = @studies";
            //         com.Parameters.AddWithValue("semester", request.Semester);
            //         com.Parameters.AddWithValue("studies", request.Studies);
            //
            //         var dr = com.ExecuteReader();
            //
            //         if (!dr.Read())
            //         {
            //             dr.Close();
            //
            //             return new NotFoundResult();
            //         }
            //         dr.Close();
            //         
            //         // Jeżeli wszystko poszło dobrze uruchamiam procedurę składową
            //
            //         com.CommandText = "promoteStudents";
            //         com.CommandType = CommandType.StoredProcedure;
            //         dr = com.ExecuteReader();
            //         if (dr.Read())
            //         {
            //             enrollment.IdEnrollment = (int) dr["IdEnrollment"];
            //             enrollment.Semester = (int) dr["Semester"];
            //             enrollment.IdStudy = (int) dr["IdStudy"];
            //             enrollment.StartDate = (DateTime) dr["StartDate"];
            //             dr.Close();
            //         }
            //
            //
            //     }
            // }

            var response = _service.PromoteStudent(request);
            if (response.Status == 404)
            {
                return NotFound();
            }
            
            

            return CreatedAtAction(response.Message, response.enrollment);
        }
        
    }
}