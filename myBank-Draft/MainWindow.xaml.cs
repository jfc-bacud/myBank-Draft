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
        string role;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginBTN_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(userIN.Text) || String.IsNullOrEmpty(passIN.Password))
            {
                MessageBox.Show("Error 1");
                return;
            }

            if (UserExists(out role))
            {
                if (VerifiedPassword(role))
                {
                    OpenWindow(role);
                }

                MessageBox.Show("Error 3");
            }

            MessageBox.Show("Error 2");
        }

        private bool UserExists(out string role)
        {
            var customers = db.Customers.ToList();
            var admins = db.Admins.ToList();

            foreach (var c in customers)
            {
                if (c.Customer_Email == userIN.Text.ToString())
                {
                    role = "Customer";
                    return true;
                }
            }

            foreach (var a in admins)
            {
                if (a.Admin_Email == userIN.Text.ToString())
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
            using (var db = new databaseDataContext())
            {
                if (role == "Customer")
                {
                    var user = (from c in db.Customers
                                where c.Customer_Email == userIN.Text
                                select c).FirstOrDefault();

                    if (user.Customer_Password == passIN.Password)
                    {
                        return true;
                    }
                }
                else if (role == "Admin")
                {
                    var user = (from a in db.Admins
                                where a.Admin_Email == userIN.Text
                                select a).FirstOrDefault();

                    if (user.Admin_Password == passIN.Password)
                    {
                        return true;
                    }
                }

                return false;
            }
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
