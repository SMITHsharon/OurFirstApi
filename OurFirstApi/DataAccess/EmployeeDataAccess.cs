using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using OurFirstApi.Models;

namespace OurFirstApi.DataAccess
{
    public class EmployeeDataAccess : IRepository<EmployeeListResult>
    {
        public List<EmployeeListResult> GetAll()
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                    connection.Open();

                    var result = connection.Query<EmployeeListResult>("select * " +
                                                                      "from Employee");

                    return result.ToList();
            }
        }



        public EmployeeListResult Get(int id)
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var result = connection.QueryFirstOrDefault<EmployeeListResult>
                                ("Select * From Employee where EmployeeId = @id",
                                 new { id = id });

                return result;
            }
        }



        public EmployeeListResult Get(string firstName)
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var result = connection.QueryFirstOrDefault<EmployeeListResult>
                                ("Select * From Employee where FirstName = @firstname",
                            new { firstName });

                return result;
            }
        }


        // Post
        public int AddEmployee(EmployeeListResult employee)
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var affectedRows = connection.Execute
                                ("Insert into Employee(FirstName, LastName) " +
                                 "Values(@firstName, @lastName)",
                                  new { FirstName = employee.FirstName, LastName = employee.LastName });

                return affectedRows;
            }
        }


        // Put
        public int Update(int id, EmployeeListResult employee)
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var affectedRows = connection.Execute
                               ("Update Employee " +
                                "set FirstName = @changedFirstName, " +
                                "LastName = @changedLastName " +
                                "Where EmployeeId = @employeeId " +
                                "Values(@changedFirstName, @changedLastName, employeeId = id)",
                                 new { changedFirstName = employee.FirstName, changedLastName = employee.LastName });

                return affectedRows;
                
            }
        }



        public int Delete(int id)
        {
            using (var connection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var affectedRows = connection.Execute
                                ("delete from Employee where EmployeeId = @thisEmployee",
                                  new { thisEmployee = id });

                return affectedRows;
            }
        }
    }
}



// generic interface :: 
public interface IRepository<T>
{
    List<T> GetAll();
    T Get(int id);
    int AddEmployee(T entityToAdd);
    int Update(int id, T entityToUpdate);
    int Delete(int id);
}