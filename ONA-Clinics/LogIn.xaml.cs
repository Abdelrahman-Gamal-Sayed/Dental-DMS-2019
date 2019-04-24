using System;
using System.Collections.Generic;
using System.Data;
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

namespace ONA_Clinics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ClinicsFrm().Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    //    DB db = new DB();
  

    //    private void Window_KeyDown(object sender, KeyEventArgs e)
    //    {
    //        try
    //        {
    //            if (e.Key == Key.Enter)
    //            {
    //                loginnow();
    //            }
    //        }catch(Exception ex)
    //        { MessageBox.Show(ex.Message); }
    //    }

    //    private void Button_Click(object sender, RoutedEventArgs e)
    //    {
    //        try
    //        {
    //            loginnow();
    //        }
    //        catch (Exception ex)
    //        { MessageBox.Show(ex.Message); }

    //    }

    //    void loginnow()
    //    {
    //        DataTable s = db.RunReader("select id,name,password,type from [USER]  WHERE NAME = '" + txtEName.Text + "'");
    //        if (s.Rows.Count <= 0)
    //            MessageBox.Show("اسم الامستخدم غير صحيح");
    //        else
    //        {
      
    //            if (s.Rows[0][2].ToString().Trim() != passbox.Password.ToString())
    //                MessageBox.Show("كلمة مرور غير صحيحة");
    //            else
    //            {
    //                if (s.Rows[0][3].ToString() == "1")
    //                {
    //                    //Form1 aa = new Form1();
    //                    //aa.ShowDialog();
    //                //    this.Close();
    //                }
                  
                  
    //            }

    //        }
    //    }

    //    private void safedsgfshgd(object sender, KeyEventArgs e)
    //    {
    //        if(e.Key==Key.Enter)
    //        loginnow();
    //    }
    }
}
