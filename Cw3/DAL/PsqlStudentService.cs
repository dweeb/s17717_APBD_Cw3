﻿using Cw3.Models;
using System.Collections.Generic;
using Npgsql;
using System.Linq;

namespace Cw3.DAL
{
    public class PsqlStudentService : IDbStudentService
    {
        private readonly string connectionString;
        public PsqlStudentService()
        {
            connectionString = System.IO.File.ReadLines("auth\\pg.cstr").First();
            // constring in gitignored file because security
            // "Username=;Password=;Host=;Port=;Database=;SSL Mode=Prefer";
        }
        // Student
        public Enrollment GetStudentEnrollment(string id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText =   "select e.IdEnrollment, e.Semester, e.IdStudy, e.StartDate from Student as s " +
                                        "join Enrollment e on s.IdEnrollment = e.IdEnrollment " +
                                        "where s.IndexNumber = @id;";
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                // currently only *:1 relation is possible between Student and Enrollment but not sure if that's the correct busines logic
                reader.Read();
                var enrollment = new Enrollment();
                enrollment.IdEnrollment = (int)reader["IdEnrollment"];
                enrollment.Semester = (int)reader["Semester"];
                enrollment.IdStudy = (int)reader["IdStudy"];
                enrollment.StartDate = reader["StartDate"].ToString();
                // could've used a proper constructor to 1-line it but it's whatever
                return enrollment;
            }
        }

        public Student GetStudent(string id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, u.Name, e.Semester from Student as s " +
                        "join Enrollment e on s.IdEnrollment = e.IdEnrollment join Studies u on e.IdStudy = u.IdStudy " +
                        "where IndexNumber = @id;";
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                var s = new Student();
                s.FirstName = reader["FirstName"].ToString();
                s.LastName = reader["LastName"].ToString();
                s.BirthDate = reader["BirthDate"].ToString();
                s.StudiesName = reader["Name"].ToString();
                s.Semester = (int)reader["Semester"];
                return s;
            }
        }

        public List<Student> GetStudents()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, u.Name, e.Semester from Student as s " +
                        "join Enrollment e on s.IdEnrollment = e.IdEnrollment join Studies u on e.IdStudy = u.IdStudy;";
                connection.Open();
                var reader = command.ExecuteReader();
                List<Student> returnList = new List<Student>();
                while (reader.Read())
                {
                    var s = new Student();
                    s.FirstName = reader["FirstName"].ToString();
                    s.LastName = reader["LastName"].ToString();
                    s.BirthDate = reader["BirthDate"].ToString();
                    s.StudiesName = reader["Name"].ToString();
                    s.Semester = (int) reader["Semester"];
                    returnList.Add(s);
                }
                return returnList;
            }
        }
    }
}