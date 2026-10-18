using System;
using System.Collections.Generic;
using Npgsql; // Ubah dari Microsoft.Data.SqlClient
using StudentManager.Models;

namespace StudentManager.Data;

public class StudentRepository
{
    private readonly string connectionString =
        "Host=localhost;Port=5432;Database=StudentDB;Username=postgres;Password=Mirzard22;";

    public List<Student> GetAll()
    {
        var students = new List<Student>();
        using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        string sql = @"SELECT ""Id"", ""NIM"", ""Nama"", ""Jurusan"", ""Gender"", ""Email"" 
                       FROM ""Students"" ORDER BY ""Id"" DESC";
        using NpgsqlCommand command = new NpgsqlCommand(sql, connection);

        connection.Open();
        using NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                NIM = reader["NIM"].ToString()!,
                Nama = reader["Nama"].ToString()!,
                Jurusan = reader["Jurusan"].ToString()!,
                Gender = reader["Gender"].ToString()!,
                Email = reader["Email"].ToString()!
            });
        }
        return students;
    }

    public void Insert(Student student)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        string sql = @"INSERT INTO ""Students"" (""NIM"", ""Nama"", ""Jurusan"", ""Gender"", ""Email"") 
                       VALUES (@NIM, @Nama, @Jurusan, @Gender, @Email)";
        using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@NIM", student.NIM);
        command.Parameters.AddWithValue("@Nama", student.Nama);
        command.Parameters.AddWithValue("@Jurusan", student.Jurusan);
        command.Parameters.AddWithValue("@Gender", student.Gender);
        command.Parameters.AddWithValue("@Email", student.Email);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Update(Student student)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        string sql = @"UPDATE ""Students"" 
                       SET ""NIM""=@NIM, ""Nama""=@Nama, ""Jurusan""=@Jurusan, ""Gender""=@Gender, ""Email""=@Email 
                       WHERE ""Id""=@Id";
        using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", student.Id);
        command.Parameters.AddWithValue("@NIM", student.NIM);
        command.Parameters.AddWithValue("@Nama", student.Nama);
        command.Parameters.AddWithValue("@Jurusan", student.Jurusan);
        command.Parameters.AddWithValue("@Gender", student.Gender);
        command.Parameters.AddWithValue("@Email", student.Email);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        using NpgsqlCommand command = new NpgsqlCommand(@"DELETE FROM ""Students"" WHERE ""Id""=@Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public List<Student> Search(string keyword)
    {
        var students = new List<Student>();
        using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        // ILIKE digunakan di PostgreSQL agar pencarian tidak bersifat case-sensitive
        string sql = @"SELECT ""Id"", ""NIM"", ""Nama"", ""Jurusan"", ""Gender"", ""Email"" 
                       FROM ""Students"" 
                       WHERE ""NIM"" ILIKE @Keyword OR ""Nama"" ILIKE @Keyword OR ""Jurusan"" ILIKE @Keyword 
                       ORDER BY ""Id"" DESC";
        using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

        connection.Open();
        using NpgsqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                NIM = reader["NIM"].ToString()!,
                Nama = reader["Nama"].ToString()!,
                Jurusan = reader["Jurusan"].ToString()!,
                Gender = reader["Gender"].ToString()!,
                Email = reader["Email"].ToString()!
            });
        }
        return students;
    }
}