using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace EmployeeDOT.Pages.Employees
{
    public class EditModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string errorMessage = "";
        //public string successMessage = "";
        
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Server=localhost;Database=Employees;Trusted_Connection=true;TrustServerCertificate=true;";
                using (SqlConnection connection= new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM employees WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.name = "" + reader.GetString(1);
                                employeeInfo.email = "" + reader.GetString(2);
                                employeeInfo.phone = "" + reader.GetString(3);
                                employeeInfo.department = "" + reader.GetString(4);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            employeeInfo.id = Request.Form["id"];
            employeeInfo.name = Request.Form["name"];
            employeeInfo.email = Request.Form["email"];
            employeeInfo.phone = Request.Form["phone"];
            employeeInfo.department = Request.Form["department"];

            if (employeeInfo.name.Length == 0 ||
                employeeInfo.email.Length == 0 || employeeInfo.phone.Length == 0 ||
                employeeInfo.department.Length == 0)
            {
                errorMessage = "All info must be filled";
                return;
            }

            try
            {
                String connectionString = "Server=localhost;Database=Employees;Trusted_Connection=true;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE employees " +
                                "SET name=@name, email=@email, phone=@phone, department=@department " +
                                "WHERE id=@id;";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", employeeInfo.name);
                        command.Parameters.AddWithValue("@email", employeeInfo.email);
                        command.Parameters.AddWithValue("@phone", employeeInfo.phone);
                        command.Parameters.AddWithValue("@department", employeeInfo.department);
                        command.Parameters.AddWithValue("@id", employeeInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Employees/Index");
        }    
    }
}




