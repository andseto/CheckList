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
using System.Windows.Shapes;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace CheckList
{
    /// <summary>
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        public AddTask()
        {
            InitializeComponent();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            try
            {
                // MySQL connection string
                // Free Data base to connect to, Change information BELOW as needed.
                // Change credentials as needed, database, username, password, etc
                // FreeSQLdatabase
                MySqlConnection con = new MySqlConnection("Server=CHANGEME;Database=CHANGEME;User Id=CHANGEME;Password=CHANGEME;Port=3306;");
                con.Open();

                // MySQL insert query
                string add_data = "INSERT INTO ToDoList (Name, `Desc`) VALUES(@Name, @Desc)";
                MySqlCommand cmd = new MySqlCommand(add_data, con);

                // Adding parameters
                cmd.Parameters.AddWithValue("@Name", TaskName.Text);
                cmd.Parameters.AddWithValue("@Desc", TaskDesc.Text);

                // Execute query
                int rowsAffected = cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error: " + ex.Message);
            }

            this.Close();
        }
    }
}
