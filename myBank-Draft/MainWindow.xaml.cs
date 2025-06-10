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
        private void verifyBTN_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(passwordTB.Text) || string.IsNullOrEmpty(userTB.Text))
            {
                MessageBox.Show("L");
            }

            if (UserExists(out role))
            {
                if (VerifyPassword(role))
                {
                    OpenWindow(role);
                }
            }


        }
        bool UserExists(out string role)
        {
            var customerList = from c in db.Customers
                               select c;

            var adminList = from a in db.Admins
                            select a;

            foreach (var c in customerList)
            {
                if (userTB.Text == c.Customer_Email)
                {
                    role = "customer";
                    return true;
                }
            }

            foreach (var a in adminList)
            {
                if (userTB.Text == a.Admin_Email)
                {
                    role = "admin";
                    return true;
                }
            }

            MessageBox.Show("X");

            role = "";
            return false;
        }
        bool VerifyPassword(string role)
        {
            if (role == "customer")
            {
                var customer = (from c in db.Customers
                               where c.Customer_Email == userTB.Text
                               select c).FirstOrDefault();

                if (customer.Customer_Password == passwordTB.Text)
                {
                    return true;
                }
            }
            else if (role == "admin")
            {
                var admin = (from a in db.Admins
                             where a.Admin_Email == userTB.Text
                             select a).FirstOrDefault();

                if (admin.Admin_Password == passwordTB.Text)
                {
                    return true;
                }
            }

            MessageBox.Show("Y");
            return false;
        }
        void OpenWindow(string role)
        {
            if (role == "customer")
            {
                CustomerWindow customerWindow = new CustomerWindow();
                customerWindow.Show();
                this.Close();
            }
            else if(role == "admin")
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }
        }
    }
}
