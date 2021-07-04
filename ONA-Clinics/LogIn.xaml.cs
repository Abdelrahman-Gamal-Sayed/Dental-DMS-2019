using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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


            oldUser();



        }
       private void oldUser() {
            try {
                txtEName.Text = File.ReadAllText("User.txt");
                PasswordBox.Password = File.ReadAllText("pass.txt");
            } catch { }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //new ClinicsFrm().Show();
            //this.Close();
            validationFun();
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
        DB db = new DB();
        void validationFun()
        {

            try
            {
                string path = @"User.txt";
                File.WriteAllText(path, txtEName.Text.Trim());
                string path2 = @"pass.txt";
                File.WriteAllText(path2, PasswordBox.Password.Trim());
                DataTable s = db.RunReader(@"select USER_NAME ,PASSWORD ,SHOW_NAME ,PROVIDER_CODE ,
PROVIDER_ENAME ,PROVIDER_ANAME ,DEGREE_CODE ,DEGREE_NAME  ,USER_AUTHORITY_CODE ,USER_ID ,PROVIDER_TYPE  from USERS_HOSPITAL where USER_NAME = '" + txtEName.Text.Trim().ToUpper() + "'");
                if (s.Rows.Count <= 0)
                {
                    MessageBox.Show("Please check your Name");
                    return;
                }

                if (s.Rows[0][1].ToString() != PasswordBox.Password.ToString())
                {
                    MessageBox.Show("Please check your Password");
                    return;
                }

                User.Name = s.Rows[0][0].ToString();
                User.Name_Show=s.Rows[0][2].ToString();
                User.Provider_Code= s.Rows[0][3].ToString();
                User.Provider_English_Name = s.Rows[0][4].ToString();
                User.Provider_Arabic_Name= s.Rows[0][5].ToString();
                User.Degree_Code = s.Rows[0][6].ToString();
                User.Degree_Name = s.Rows[0][7].ToString();
                User.ID = s.Rows[0][9].ToString();
                User.Provider_Type = s.Rows[0][10].ToString();
                User.AUTHORITY_CODE = s.Rows[0][8].ToString();
                DataTable ss = db.RunReader("select NAME ,DENTAL_FRM ,HOSPITAL_FRM ,PHYSICAL_THERAPY_FRM,DOCTOR_FRM,ADMIN_FRM  from USER_AUTHORITYS WHERE CODE ='" + User.AUTHORITY_CODE + "'");
                User.AUTHORITY_NAME = ss.Rows[0][0].ToString();
                User.DENTAL_FRM = ss.Rows[0][1].ToString();
                User.HOSPITAL_FRM = ss.Rows[0][2].ToString();
                User.PHYSICAL_THERAPY_FRM = ss.Rows[0][3].ToString();
                User.DOCTOR_FRM = ss.Rows[0][4].ToString();
                User.ADMIN_FRM = ss.Rows[0][5].ToString();


                System.Data.DataTable ssa = db.RunReader("select DISTINCT PR_CODE, PR_ANAME, PR_ENAME, PROV_DEGREE, BS_ANAME from SERV_PROVIDERS, dms_test.BASIC_DATA  where PR_CODE='" + User.Provider_Code + "' AND SERV_PROVIDERS.PROV_DEGREE = BASIC_DATA.BS_CODE AND SOURCE_MOD = 'PRDEG' ");
                User.Provider_Arabic_Name = ssa.Rows[0][1].ToString();
                User.Provider_English_Name = ssa.Rows[0][2].ToString();
                User.Degree_Code = ssa.Rows[0][3].ToString();
                User.Degree_Name = ssa.Rows[0][4].ToString();



                if (User.ADMIN_FRM == "Y" || User.ADMIN_FRM == "y") new AdminFrm().Show();
                else if (User.DENTAL_FRM == "Y" || User.DENTAL_FRM == "y") new ClinicsFrm().Show();
                else if (User.HOSPITAL_FRM == "Y" || User.HOSPITAL_FRM == "y") ;
                else if (User.PHYSICAL_THERAPY_FRM == "Y" || User.PHYSICAL_THERAPY_FRM == "y") new frmPhysicalTherapy().Show();
                else if (User.DOCTOR_FRM == "Y" || User.DOCTOR_FRM == "y") ;
                else MessageBox.Show("برجاء المحاولة مرة اخرى");
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                validationFun();
            }

        }
    }
}
