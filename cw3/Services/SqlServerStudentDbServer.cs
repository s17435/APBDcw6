using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using cw4.DTOs.Requests;
using cw4.DTOs.Responses;
using cw4.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace cw4.Services
{
    public class SqlServerStudentDbServer : IStudentDBService
    {
        private const string ConString = "Server=localhost,1433; Database=Master; User Id=SA; Password= Pa55w0rd";


        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {

            var response = new EnrollStudentResponse();


            using (var con = new SqlConnection(ConString))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();

                    var transaction = con.BeginTransaction();
                    com.Transaction = transaction;

                    try
                    {
                        
                        // Sprawdzenie czy studia istnieją
                        com.CommandText = "SELECT IdStudy from studies where name = @name";
                        com.Parameters.AddWithValue("name", request.Studies);

                        var dr = com.ExecuteReader();

                        if (!dr.Read())
                        {
                            dr.Close();
                            transaction.Rollback();
                            response.Status = 400;
                            response.Message = "Nie istnieją studia o podanej nazwie:: " + request.Studies;
                            return response;

                        }

                        int idStudies = (int) dr["IdStudy"];
                        dr.Close();

                       // enrollment.IdStudy = idStudies;



                        // Odnalezienie najnowszego wpisu 
                        com.CommandText =
                            "SELECT * FROM ENROLLMENT WHERE IdEnrollment = (SELECT MAX(IdEnrollment) FROM Enrollment WHERE IdStudy = @idStudies AND Semester = 1);";
                        com.Parameters.AddWithValue("idStudies", idStudies);
                        var dr1 = com.ExecuteReader();
                        int idEnrollment;
                        // Sprawdzenie czy wpis istnieje, jeżeli nie, jestem zobowiązany go utworzyć
                        if (!dr1.Read())
                        {
                            dr1.Close();
                            com.CommandText = "SELECT MAX(IdEnrollment) + 1 FROM Enrollment";
                            idEnrollment = Convert.ToInt32(com.ExecuteScalar());
                            Console.WriteLine(idEnrollment);
                            DateTime myDateTime = DateTime.Now;
                            string dateFormat = myDateTime.ToString("yyyy-MM-dd");
                            Console.WriteLine(dateFormat);
                            Console.WriteLine(idStudies);

                            com.CommandText =
//                                "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES ({idEnrollment},1,{idStudies}, '2000-12-12')";
                                "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) " +
                                $"VALUES ({idEnrollment},1,{idStudies}, '{dateFormat}')";

                            com.Parameters.AddWithValue("idEnrollment", idEnrollment);
                            com.Parameters.AddWithValue("date", dateFormat);
                            com.Parameters.AddWithValue("idStudies", idStudies);
                            com.ExecuteNonQuery();

                            var enrollment = new Enrollment();

                            enrollment.Semester = 1;
                            enrollment.IdEnrollment = idEnrollment;
                            enrollment.IdStudy = idStudies;
                            enrollment.StartDate = myDateTime;
                            response.enrollment = enrollment;

                        }
                        else
                        {
                            var enrollment = new Enrollment();

                            idEnrollment = (int) dr1["IdEnrollment"];
                            enrollment.IdEnrollment = idEnrollment;
                            enrollment.Semester = (int) dr1["Semester"];
                            enrollment.IdStudy = (int) dr1["IdStudy"];
                            enrollment.StartDate = (DateTime) dr1["StartDate"];
                            response.enrollment = enrollment;

                        }

                        dr1.Close();



                        // Sprawdzenie, czy student ma unikalne ID
                        com.CommandText = "SELECT * FROM Student WHERE IndexNumber = @index";
                        com.Parameters.AddWithValue("index", request.IndexNumber);
                        Console.WriteLine(request.IndexNumber);
                        var dr3 = com.ExecuteReader();
                        if (dr3.Read())
                        {
                            dr3.Close();
                            transaction.Rollback();
                            response.Status = 400;
                            response.Message = "Numer indeksu nie jest unikalny!";
                            return response;
                        }
                        
                        dr3.Close();




                        com.CommandText =
                            "INSERT INTO Student (IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@studentindex, @fname, @lname, @dob, @idEnrollment)";


                        com.Parameters.AddWithValue("studentindex", request.IndexNumber);
                        com.Parameters.AddWithValue("fname", request.FirstName);
                        com.Parameters.AddWithValue("lname", request.LastName);
                        com.Parameters.AddWithValue("dob", request.BirthDate);
                        com.Parameters.AddWithValue("idEnrollment", idEnrollment);

                        com.ExecuteNonQuery();

                        transaction.Commit();
                        response.Status = 201;

                    }
                    catch (SqlException exc)
                    {
                        response.Status = 400;
                        response.Message = "Wystąpił problem z bazą danych";

                        transaction.Rollback();
                        Console.WriteLine("SQL ERROR and rollback");
                        Console.WriteLine(exc);


                    }
                    
                }
            }

            return response;
        }
    

    public PromoteStudentsResponse PromoteStudent(PromoteStudentsRequest request)
    {

        var response = new PromoteStudentsResponse();
            using (var con = new SqlConnection(ConString))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();

                    var transaction = con.BeginTransaction();
                    com.Transaction = transaction;
                    
                    //1. Sprawdzam czy w tabeli enrollment istnieje wpis o podanej wartości Studies i Semester, W przeciwnym razie zwracam kod 404 Not Found
                    
                    com.CommandText = "SELECT * FROM Enrollment" +
                                      " INNER JOIN Studies" +
                                      " ON Studies.IdStudy = Enrollment.IdStudy" +
                                      " WHERE Enrollment.Semester = @semester" +
                                      " AND Studies.Name = @studies";
                    com.Parameters.AddWithValue("semester", request.Semester);
                    com.Parameters.AddWithValue("studies", request.Studies);

                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        response.Status = 404;
                        response.Message = "Nie istnieje wpis Studies i Semester o podanej wartości";

                        return response;
                    }
                    dr.Close();
                    
                    // Jeżeli wszystko poszło dobrze uruchamiam procedurę składową

                    com.CommandText = "promoteStudents";
                    com.CommandType = CommandType.StoredProcedure;
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        var enrollment = new Enrollment();
                        enrollment.IdEnrollment = (int) dr["IdEnrollment"];
                        enrollment.Semester = (int) dr["Semester"];
                        enrollment.IdStudy = (int) dr["IdStudy"];
                        enrollment.StartDate = (DateTime) dr["StartDate"];
                        response.enrollment = enrollment;
                        response.Status = 201;
                        //response.Message = "Ok";
                        dr.Close();
                    }


                }
            }

            return response;
    }

    public Student GetStudent(string index)
    {
       Console.WriteLine("Jestem w metodzie GEtStudent");
        using (var con = new SqlConnection(ConString))
        {
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                var transaction = con.BeginTransaction();
                com.Transaction = transaction;
                
                com.CommandText = "SELECT * from student where IndexNumber = @index";
                com.Parameters.AddWithValue("index", index);

                var dr = com.ExecuteReader();

                if (!dr.Read())
                {
                    Console.WriteLine("nie ma");
                    dr.Close();
                    return null;
                }
                var std = new Student();
                std.IndexNumber = (string) dr["IndexNumber"];
                std.FirstName = (string) dr["FirstName"];
                std.LastName = (string) dr["LastName"];
                std.BirthDate = (DateTime) dr["BirthDate"];
                std.IdEnrollment = (int) dr["IdEnrollment"];
                Console.WriteLine(std.FirstName);
                dr.Close();

                return std;
            }
        }
    }
    
    }
}