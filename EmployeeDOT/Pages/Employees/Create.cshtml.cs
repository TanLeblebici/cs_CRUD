using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace EmployeeDOT.Pages.Employees
{
    public class CreateModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            employeeInfo.name = Request.Form["name"];
            employeeInfo.email = Request.Form["email"];
            employeeInfo.phone = Request.Form["phone"];
            employeeInfo.department = Request.Form["department"];

            if(employeeInfo.name.Length == 0 || employeeInfo.email.Length == 0 
                || employeeInfo.phone.Length == 0 || employeeInfo.department.Length == 0)
            {
                errorMessage = "All information must be filled";
                return;
            }

            //new employee
            try
            {
                //String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Employee;Integrated Security=True";
                String connectionString = "Server=localhost;Database=Employees;Trusted_Connection=true;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO employees" +
                        "(name, email, phone, department) VALUES " +
                        "(@name, @email, @phone, @department);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", employeeInfo.name);
                        command.Parameters.AddWithValue("@email", employeeInfo.email);
                        command.Parameters.AddWithValue("@phone", employeeInfo.phone);
                        command.Parameters.AddWithValue("@department", employeeInfo.department);
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            employeeInfo.name = ""; employeeInfo.email = ""; employeeInfo.phone = ""; employeeInfo.department = "";

            Response.Redirect("/Employees/Index");
        }
    }
}
