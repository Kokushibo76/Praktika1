using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using Praktika_1;

namespace Praktika
{
    public partial class MainWindow : Window
    {
        private PraktikaContext _context = new PraktikaContext();

        public MainWindow()
        {
            InitializeComponent();

            employeesDGR.ItemsSource = _context.Employees.ToList();
            employeesCBX.ItemsSource = _context.JobTitles.ToList();
        }

        private void employeesDGR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dannye = (employeesDGR.SelectedItem as Employee);
            NameTBX.Text = dannye.Name_of_employee;
            employeesCBX.SelectedValue = dannye.ID_JobTitle;
        }
    }

    public partial class JobTitle
    {
        private DataSet _ds = new DataSet();

        public JobTitle()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=Praktika;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Job_titles";
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                {
                    adapter.Fill(_ds, "Job_titles");
                }

                sql = "SELECT * FROM Employees";
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                {
                    adapter.Fill(_ds, "Employees");
                }
            }

            this.employees = new HashSet<Employee>();

            foreach (DataRow row in _ds.Tables["Job_titles"].Rows)
            {
                this.ID_JobTitle = Convert.ToInt32(row["ID_JobTitle"]);
                this.Job_title = row["Job_title"].ToString();
            }

            foreach (DataRow row in _ds.Tables["Employees"].Rows)
            {
                employees.Add(new Employee
                {
                    ID_Employee = Convert.ToInt32(row["ID_Employee"]),
                    Name_of_employee = row["Name_of_employee"].ToString(),
                    ID_JobTitle = Convert.ToInt32(row["ID_JobTitle"])
                });
            }
        }

        public int ID_JobTitle { get; set; }

        public string Job_title { get; set; }

        public virtual ICollection<Employee> employees { private get; set; }
    }
}