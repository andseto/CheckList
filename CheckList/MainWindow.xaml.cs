using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace CheckList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AddTask window
            AddTask addTaskWindow = new AddTask();

            // Show the AddTask window as a dialog and check if it returns true
            if (addTaskWindow.ShowDialog() == true)
            {
                // Retrieve the new task name and description from the AddTask window
                string taskName = addTaskWindow.TaskName.Text;
                string taskDesc = addTaskWindow.TaskDesc.Text;

                // Create a new Border to hold the task information
                Border taskBorder = new Border
                {
                    Background = new SolidColorBrush(Colors.LightGray),
                    BorderBrush = new SolidColorBrush(Colors.DarkGray),
                    BorderThickness = new Thickness(1),
                    Padding = new Thickness(10),
                    Margin = new Thickness(5)
                };

                // Create a Grid to arrange the task info and remove button horizontally
                Grid taskGrid = new Grid();
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Task info
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) }); // Remove button

                // Create a StackPanel to hold tasks name and description
                StackPanel taskPanel = new StackPanel();

                // Task name
                TextBlock taskNameBlock = new TextBlock
                {
                    Text = taskName,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap
                };

                // Task description
                TextBlock taskDescBlock = new TextBlock
                {
                    Text = taskDesc,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                // Add the text blocks to the StackPanel
                taskPanel.Children.Add(taskNameBlock);
                taskPanel.Children.Add(taskDescBlock);

                // Add the task panel to the Grid in the first column
                Grid.SetColumn(taskPanel, 0);
                taskGrid.Children.Add(taskPanel);

                // Create the "Remove" button
                Button removeButton = new Button
                {
                    Content = "Remove",
                    Background = new SolidColorBrush(Colors.Gray),
                    Foreground = new SolidColorBrush(Colors.White),
                    Width = 50,
                    Height = 30,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                // Add the remove button to the Grid in the second column
                Grid.SetColumn(removeButton, 1);
                taskGrid.Children.Add(removeButton);

                // Add the Grid to the Border
                taskBorder.Child = taskGrid;

                // Add the Border to the TaskStackPanel (in ScrollViewer)
                TaskStackPanel.Children.Add(taskBorder);

                // Attach the click event to remove the task
                removeButton.Click += (s, args) =>
                {
                    TaskStackPanel.Children.Remove(taskBorder); // Remove the task when button is clicked
                };
            }
        }

        // Loads the ToDoList with tasks from the MySQL server
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Command to connect with MySql Server from "FreeSQLdatabase
                string connectionString = "Server=sql3.freesqldatabase.com;Database=sql3733358;User=sql3733358;Password=xaBWQerfQ7;Port=3306;";
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    string retrieve_data = "SELECT Name, `Desc` FROM ToDoList";
                    MySqlCommand cmd = new MySqlCommand(retrieve_data, con);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    TaskStackPanel.Children.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        // Retrieve the task name and description
                        string taskName = row["Name"].ToString();
                        string taskDesc = row["Desc"].ToString();

                        // Create a new Border to hold the task information
                        Border taskBorder = new Border
                        {
                            Background = new SolidColorBrush(Colors.LightGray),
                            BorderBrush = new SolidColorBrush(Colors.DarkGray),
                            BorderThickness = new Thickness(1),
                            Padding = new Thickness(10),
                            Margin = new Thickness(5)
                        };

                        // Create a Grid to arrange the task info and remove button horizontally
                        Grid taskGrid = new Grid();
                        taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Task info
                        taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) }); // Remove button

                        // Create a StackPanel to hold task name and description
                        StackPanel taskPanel = new StackPanel();

                        // Task name
                        TextBlock taskNameBlock = new TextBlock
                        {
                            Text = taskName,
                            FontSize = 16,
                            FontWeight = FontWeights.Bold,
                            TextWrapping = TextWrapping.Wrap
                        };

                        // Task description
                        TextBlock taskDescBlock = new TextBlock
                        {
                            Text = taskDesc,
                            FontSize = 14,
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(0, 5, 0, 0)
                        };

                        // Add the text blocks to the StackPanel
                        taskPanel.Children.Add(taskNameBlock);
                        taskPanel.Children.Add(taskDescBlock);

                        // Add the task panel to the Grid in the first column
                        Grid.SetColumn(taskPanel, 0);
                        taskGrid.Children.Add(taskPanel);

                        // Create the "Remove" button
                        Button removeButton = new Button
                        {
                            Content = "Remove",
                            Background = new SolidColorBrush(Colors.Gray),
                            Foreground = new SolidColorBrush(Colors.White),
                            Width = 50,
                            Height = 30,
                            HorizontalAlignment = HorizontalAlignment.Right
                        };

                        // Add the remove button to the Grid in the second column
                        Grid.SetColumn(removeButton, 1);
                        taskGrid.Children.Add(removeButton);

                        // Add the Grid to the Border
                        taskBorder.Child = taskGrid;

                        // Add the Border to the TaskStackPanel (in ScrollViewer)
                        TaskStackPanel.Children.Add(taskBorder);

                        // Attach the click event to remove the task
                        removeButton.Click += (s, args) =>
                        {
                            // Remove the task from the UI
                            TaskStackPanel.Children.Remove(taskBorder);

                            // Open a new connection for the deletion operation
                            try
                            {
                                // Create a new MySQL connection instance for the deletion process
                                // Free Data base to connect to, Change information BELOW as needed.
                                // Change credentials as needed, database, username, password, etc
                                // FreeSQLdatabase
                                using (MySqlConnection con2 = new MySqlConnection("Server=CHANGEME;Database=CHANGEME;User Id=CHANGEME;Password=CHANGEME;Port=3306;"))
                                {
                                    // Open the connection
                                    con2.Open();

                                    // SQL query to remove the task from the database
                                    string delete_query = "DELETE FROM ToDoList WHERE Name = @Name AND `Desc` = @Desc";

                                    // Prepare the SQL command
                                    MySqlCommand deleteCmd = new MySqlCommand(delete_query, con2);
                                    deleteCmd.Parameters.AddWithValue("@Name", taskName);
                                    deleteCmd.Parameters.AddWithValue("@Desc", taskDesc);

                                    // Execute the query to remove the task from the database
                                    int rowsAffected = deleteCmd.ExecuteNonQuery();
                                }
                            }
                            catch (MySqlException ex)
                            {
                                MessageBox.Show("MySQL Error: " + ex.Message);
                            }
                        };
                    }

                    con.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error: " + ex.Message);
            }
        }
    }
}
