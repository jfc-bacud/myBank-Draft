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

namespace myBank_Draft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        databaseDataContext db = new databaseDataContext(Properties.Settings.Default.MyBankConnectionString);
        IQueryable<Customer> customers;
        IQueryable<Admin> admins;
        string role;

        public MainWindow()
        {
            InitializeComponent();  
        }

        public void LoadDatabase()
        {
            customers = db.Customers;
            admins = db.Admins;
        }



        private void loginBTN_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(userIN.Text) || String.IsNullOrEmpty(passIN.Password))
            {
                // Error status
            }

            if (UserExists(out role))
            {
                if (VerifiedPassword(role))
                {
                    OpenWindow(role);
                }
            }
        }

        private bool UserExists(out string role)
        {
            foreach (var c in customers)
            {
                if (c.Customer_Email == userIN.Text)
                {
                    role = "Customer";
                    return true;
                }
            }

            foreach (var a in admins)
            {
                if (a.Admin_Email == userIN.Text)
                {
                    role = "Admin";
                    return true;
                }
            }

            role = null;
            return false;
        }
        private bool VerifiedPassword(string role)
        {
            if (role == "Customer")
            {
                var user = (from c in customers
                           where c.Customer_Email == userIN.Text
                           select c).FirstOrDefault();

                if (user.Customer_Password == passIN.Password)
                {
                    return true;
                }
            }
            else if (role == "Admin")
            {
                var user = (from a in admins
                            where a.Admin_Email == userIN.Text
                            select a).FirstOrDefault();

                if (user.Admin_Password == passIN.Password)
                {
                    return true;
                }
            }

            return false;
        }

        private void OpenWindow(string role)
        {
            if (role == "Customer")
            {
                CustomerWindow customerWindow = new CustomerWindow();
                customerWindow.Show();
                this.Close();
            }
            else if (role == "Admin")
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }
        }
    }
}
