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
using System.Windows;

namespace ONA_Clinics
{
    /// <summary>
    /// Interaction logic for Forms_Switch.xaml
    /// </summary>
    public partial class Forms_Switch : UserControl
    {
        public Forms_Switch()
        {
            InitializeComponent();

            checkouthotry();


         

         

        }


        private void checkouthotry()
        {

            if (!(User.DENTAL_FRM == "Y" || User.DENTAL_FRM == "y"))DentalBTN.Visibility = Visibility.Collapsed;
            if (!(User.ADMIN_FRM == "Y" || User.ADMIN_FRM == "y")) AdminBTN.Visibility = Visibility.Collapsed;
            if (!(User.HOSPITAL_FRM == "Y" || User.HOSPITAL_FRM == "y")) HospitalsBTN.Visibility = Visibility.Collapsed;
            if (!(User.PHYSICAL_THERAPY_FRM == "Y" || User.PHYSICAL_THERAPY_FRM == "y"))PhysicalBTN.Visibility = Visibility.Collapsed;
            if (!(User.DOCTOR_FRM == "Y" || User.DOCTOR_FRM == "y"))doctorBTN.Visibility = Visibility.Collapsed;
                
       
              
             
                
                      
               
        }


        private void SwitchBTN_Click(object sender, RoutedEventArgs e)
        {

            MainWindow ss=new MainWindow();
            ss.Show();
            foreach (Window f in System.Windows.Application.Current.Windows)
            {
                if (f != ss)
                    f.Hide();
            }


        }

    

        private void LogOutBTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AdminBTN_Click(object sender, RoutedEventArgs e)
        {
            

            AdminFrm ss = new AdminFrm();
            ss.Show();
            foreach (Window f in System.Windows.Application.Current.Windows)
            {
                if (f != ss)
                    f.Hide();
            }
        }

        private void HospitalsBTN_Click(object sender, RoutedEventArgs e)
        {


            form1 ss = new form1();
            ss.Show();
            foreach(Window f in System.Windows.Application.Current.Windows) {
                if(f != ss)
                   f.Hide();
            }

        }

        private void DentalBTN_Click(object sender, RoutedEventArgs e)
        {
           

            ClinicsFrm ss = new ClinicsFrm();
            ss.Show();
            foreach (Window f in System.Windows.Application.Current.Windows)
            {
                if (f != ss)
                   f.Hide();
            }
        }

        private void PhysicalBTN_Click(object sender, RoutedEventArgs e)
        {
            frmPhysicalTherapy ss = new frmPhysicalTherapy();
            ss.Show();
            foreach (Window f in System.Windows.Application.Current.Windows)
            {
                if (f != ss)
                   f.Hide();
            }
        }

        private void doctorBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
