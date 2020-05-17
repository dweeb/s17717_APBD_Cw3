using Cw3.DTOs.Requests;
using Cw3.DTOs.Responses;
using Cw3.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public class PsqlEnrollmentService : IDbEnrollmentService
    {
        private readonly string connectionString;
        public PsqlEnrollmentService()
        {
            connectionString = System.IO.File.ReadLines("auth\\pg.cstr").First();
            // constring in gitignored file because security
            // "Username=;Password=;Host=;Port=;Database=;SSL Mode=Prefer";
        }
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest req)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    command.CommandText = "select idStudy from studies where name=@name;";
                    command.Parameters.AddWithValue("name", req.Studies);
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        transaction.Rollback();
                        var res = new EnrollStudentResponse();
                        res.responseText = "Studies specified do not exist";
                        res.enrollment = null;
                        return res;
                    }
                    int idStudy = (int)reader["idstudy"];
                    command.Parameters.Clear();
                    string selectStr = "select idEnrollment, semester, idStudy, startDate from enrollment where idStudy = @idStudy and semester = 1;";
                    reader.Close();
                    command.CommandText = selectStr;
                    command.Parameters.AddWithValue("idStudy", idStudy);
                    reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        command.CommandText = "insert into Enrollment (idEnrollment, semester, idStudy, startdate) " +
                                                "values (nextval('idEnrollSeq'), 1, @idStudy, @startDate);";
                        command.Parameters.AddWithValue("startDate", DateTime.Now);
                        if(command.ExecuteNonQuery() > 0)
                        {
                            reader.Close();
                            command.CommandText = selectStr;
                            reader = command.ExecuteReader();
                            reader.Read();
                        }
                    }
                    Enrollment enrollment = new Enrollment();
                    enrollment.IdEnrollment = (int)reader["idEnrollment"];
                    enrollment.Semester = (int)reader["semester"];
                    enrollment.IdStudy = (int)reader["idStudy"];
                    enrollment.StartDate = reader["startDate"].ToString();
                    reader.Close();
                    command.Parameters.Clear();
                    command.CommandText = "insert into Student (indexNumber, firstName, lastName, birthDate, idEnrollment) " +
                                            "values (@indexNumber, @firstName, @lastName, to_date(@birthDate, 'DD.MM.YYYY'), @idEnrollment);";
                    //made it conform to the date format in spec, but at what cost
                    command.Parameters.AddWithValue("indexNumber", req.IndexNumber);
                    command.Parameters.AddWithValue("firstName", req.FirstName);
                    command.Parameters.AddWithValue("lastName", req.LastName);
                    command.Parameters.AddWithValue("birthDate", req.Birthdate);
                    command.Parameters.AddWithValue("idEnrollment", enrollment.IdEnrollment);
                    if(command.ExecuteNonQuery() < 1)
                    {
                        transaction.Rollback();
                        var res = new EnrollStudentResponse();
                        res.responseText = "this shouldn't happen";
                        res.enrollment = null;
                        return res;
                    }
                    var result = new EnrollStudentResponse();
                    result.enrollment = enrollment;
                    result.responseText = "Student enrolled.";
                    transaction.Commit();
                    return result;
                }
                catch (NpgsqlException e)
                {
                    transaction.Rollback();
                    var res = new EnrollStudentResponse();
                    res.responseText = e.ToString();
                    res.enrollment = null;
                    return res;
                }
            }
        }
        public Enrollment GetEnrollment(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select * from Enrollment where idEnrollment = @id";
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if(!reader.Read()) return null;
                Enrollment enrollment = new Enrollment();
                enrollment.IdEnrollment = (int)reader["idEnrollment"];
                enrollment.Semester = (int)reader["semester"];
                enrollment.IdStudy = (int)reader["idStudy"];
                enrollment.StartDate = reader["startDate"].ToString();
                return enrollment;
            }
        }
    }
}
