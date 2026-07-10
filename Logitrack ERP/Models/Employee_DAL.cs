using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Employee_DAL
    {
        // 1. CREATE (With Auto-Payroll Generation)
        public void AddEmployee(string conn, Employee emp)
        {
            // Ek hi query mein Employee bhi insert hoga aur uski Default Payroll bhi ban jayegi
            string query = @"
                -- 1. Pehle Naya Employee Insert Karein
                INSERT INTO EMPLOYEE 
                (EmployeeName, Department, Position, Phone, Email, HireDate, Status)
                VALUES 
                (@name, @dept, @position, @phone, @email, @hireDate, @status);

                -- 2. Naye Ban'ne Walay Employee ka 'EmployeeID' Get Karein
                DECLARE @NewEmployeeID INT = SCOPE_IDENTITY();

                -- 3. Usi EmployeeID ki madad se AUTOMATIC PAYROLL banayein
                -- Default Salary 0.00 rakhi hai, jise HR baad mein update kar sakta hai
                INSERT INTO PAYROLL 
                (EmployeeID, SalaryAmount, Bonus, Deduction, NetSalary, PayrollDate, Status)
                VALUES 
                (@NewEmployeeID, 0.00, 0.00, 0.00, 0.00, GETDATE(), 'Pending');
            ";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@dept", emp.Department);
                cmd.Parameters.AddWithValue("@position", emp.Position);
                cmd.Parameters.AddWithValue("@phone", emp.Phone);
                cmd.Parameters.AddWithValue("@email", emp.Email);
                cmd.Parameters.AddWithValue("@hireDate", emp.HireDate);
                cmd.Parameters.AddWithValue("@status", emp.Status ?? "Active");

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<Employee> GetAllEmployees(string conn)
        {
            List<Employee> list = new List<Employee>();
            string query = "SELECT * FROM EMPLOYEE;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Employee
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        Department = reader["Department"].ToString(),
                        Position = reader["Position"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        // Format date for HTML <input type="date">
                        HireDate = Convert.ToDateTime(reader["HireDate"]).ToString("yyyy-MM-dd"),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public Employee GetEmployeeById(string conn, int id)
        {
            Employee emp = null;
            string query = "SELECT * FROM EMPLOYEE WHERE EmployeeID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    emp = new Employee
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        Department = reader["Department"].ToString(),
                        Position = reader["Position"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        HireDate = Convert.ToDateTime(reader["HireDate"]).ToString("yyyy-MM-dd"),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return emp;
        }

        // 4. UPDATE
        public void UpdateEmployee(string conn, Employee emp)
        {
            string query = @"UPDATE EMPLOYEE 
                             SET EmployeeName = @name, 
                                 Department = @dept, 
                                 Position = @position, 
                                 Phone = @phone,
                                 Email = @email,
                                 HireDate = @hireDate,
                                 Status = @status
                             WHERE EmployeeID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", emp.EmployeeID);
                cmd.Parameters.AddWithValue("@name", emp.EmployeeName);
                cmd.Parameters.AddWithValue("@dept", emp.Department);
                cmd.Parameters.AddWithValue("@position", emp.Position);
                cmd.Parameters.AddWithValue("@phone", emp.Phone);
                cmd.Parameters.AddWithValue("@email", emp.Email);
                cmd.Parameters.AddWithValue("@hireDate", emp.HireDate);
                cmd.Parameters.AddWithValue("@status", emp.Status);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteEmployee(string conn, int id)
        {
            string query = "DELETE FROM EMPLOYEE WHERE EmployeeID = @id;";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}