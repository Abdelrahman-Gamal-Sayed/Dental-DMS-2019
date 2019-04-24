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
using System.Data;
using System.Globalization;
using System.Threading;
using MaterialDesignThemes.Wpf;

namespace ONA_Clinics
{
    public struct GroupNo
    {
        public string serial;
        public string serviceCode;
        public string serviceName;
        public string toothNumber;
        public string subServiceCode;
        public string subServiceName;
        public string subServicePrice;
        public string DmsDiscForSubServ;
        public string paitiontCash;


    }
    /// <summary>
    /// Interaction logic for form1.xaml
    /// </summary>
    public partial class ClinicsFrm : Window
    {
        DB db = new DB();
        DataDB tdb = new DataDB();

        public ClinicsFrm()
        {
            InitializeComponent();
            User.Provider_Code = "1082";

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            filldata();
        }
        
        void filldata()
        {
            groupno_tabl = db.RunReader("select GROUP_NO,GROUP_ANAME, GROUP_ENAME from SERVICES_GROUP where SUPER_GROUP_CODE='114' order by GROUP_NO");

            cbxServies.ItemsSource = groupno_tabl.DefaultView;

        }
        
       
                //Dentaltooth.subservies = cbxServies.Text;
       

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9.]+");
            e.Handled = reg.IsMatch(e.Text);

        }
        private void NumberOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            e.Handled = reg.IsMatch(e.Text);

        }
        private void LogOutBTN_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenMenueBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenMenueBTN.Visibility = Visibility.Collapsed;
            CloseMenueBTN.Visibility = Visibility.Visible;
        }

        private void CloseMenueBTN_Click(object sender, RoutedEventArgs e)
        {
            OpenMenueBTN.Visibility = Visibility.Visible;
            CloseMenueBTN.Visibility = Visibility.Collapsed;

        }

     

        private void PackIcon_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void PackIcon_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

   

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
         
                this.WindowState = WindowState.Maximized;
            btnMax.Visibility = Visibility.Collapsed;
            btnRestore.Visibility = Visibility.Visible;

        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            btnMax.Visibility = Visibility.Visible;
            btnRestore.Visibility = Visibility.Collapsed;
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void txtCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataTable dtinformation = db.RunReader("select  to_char(BIRTH_DATE,'DD-MM-YYYY'),C_COMP_ID,CLASS_CODE,NVL(to_char(SPECIFIC_DATE,'DD-MM-YYYY'),to_char(INS_START_DATE,'DD-MM-YYYY')),to_char(INS_END_DATE,'DD-MM-YYYY'),TERMINATE_FLAG ,EMP_ANAME_ST ,EMP_ANAME_SC,EMP_ANAME_TH ,EMP_ENAME_ST ,EMP_ENAME_SC,EMP_ENAME_TH, to_char(INS_START_DATE,'DD-MM-YYYY'), to_char(TERMINATE_DATE,'DD-MM-YYYY') from  dms_test.COMP_EMPLOYEES where CARD_ID='" + txtCard.Text + "' order by ins_start_date DESC");

                if (dtinformation.Rows.Count > 0 && Convert.ToDateTime(dtinformation.Rows[0][4]).Date < DateTime.Now.Date)
                    MessageBox.Show("خارج التغطية التامينية");
                else if (dtinformation.Rows.Count > 0 && dtinformation.Rows[0][5].ToString() == "Y" && Convert.ToDateTime(dtinformation.Rows[0][13]).Date < DateTime.Now.Date)
                {
                    MessageBox.Show("أنتهى التعاقد مع هذا الموظف بتاريخ " + dtinformation.Rows[0][13].ToString());
                }
                else if (dtinformation.Rows.Count > 0)
                {
                    //string comp = "", cont = "", cls = "", maxamt = "", conirs = "", conon = "", cononothr = "", cononmed = "", conuse = "", avilabl = "";
                    //comp = txtCard.Text.Substring(0, txtCard.Text.IndexOf('-'));

                    string cononmed = "", cononothr = "";
                    Patient.Card_NO = txtCard.Text;
                    txtBirthDate.Text = dtinformation.Rows[0][0].ToString();
                    Patient.Comp_id = dtinformation.Rows[0][1].ToString();
                    txtCompName.Text = db.RunReader(@"select C_ENAME from dms_test.CONTRACT_COMP  WHERE  C_COMP_ID  = '" + Patient.Comp_id + "'").Rows[0][0].ToString();
                    txtAge.Text = (DateTime.Now.Year - Convert.ToDateTime(dtinformation.Rows[0][0]).Year).ToString();
                    Patient.Class_Code = dtinformation.Rows[0][2].ToString();
                    txtContractStartDate.Text = dtinformation.Rows[0][3].ToString();
                    txtContractEndtDate.Text = dtinformation.Rows[0][4].ToString();
                    Patient.AName = dtinformation.Rows[0][6].ToString() + " " + dtinformation.Rows[0][7].ToString() + " " + dtinformation.Rows[0][8].ToString();
                    Patient.EName = dtinformation.Rows[0][9].ToString() + " " + dtinformation.Rows[0][10].ToString() + " " + dtinformation.Rows[0][11].ToString();

                    txtEName.Text = Patient.EName;
                    txtAName.Text = Patient.AName;

                    Patient.Contract_NO = db.RunReader("select max(contract_no) from  dms_test.COMP_EMPLOYEES  where C_COMP_ID='" + Patient.Comp_id + "'").Rows[0][0].ToString();
                    Patient.Max_Amount = db.RunReader("select MAX_AMOUNT from DMS_TEST.COMP_CONTRACT_CLASS where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "'").Rows[0][0].ToString();

                    DataTable dtirs = db.RunReader("select TOT_NET_AMT from IRS_CONSUMPTION where CARD_NO = '" + Patient.Comp_id + "' and CONTRACT_NO = (select max(CONTRACT_NO) FROM IRS_CONSUMPTION where CARD_NO = '" + Patient.Card_NO + "') AND to_date(CON_END_DATE) > to_date(sysdate) ");
                    // System.Data.DataTable dton = db.RunReader("select TOT_AMOUNT from ONLINE_CONS_02 where CARD_NO = '" + cbxindtyp1_Copy1.Text + "'").Result;



                    System.Data.DataTable dton1 = tdb.getonlinevalue(txtCard.Text, Convert.ToDateTime(dtinformation.Rows[0][12].ToString()), Convert.ToDateTime(dtinformation.Rows[0][4].ToString()), 1);
                    System.Data.DataTable dton2 = tdb.getonlinevalue(txtCard.Text, Convert.ToDateTime(dtinformation.Rows[0][12].ToString()), Convert.ToDateTime(dtinformation.Rows[0][4].ToString()), 2);

                    if (dtirs.Rows.Count == 0 || dtirs.Rows[0][0].ToString() == string.Empty)
                        Patient.Consumption_IRS = "0";
                    else
                        Patient.Consumption_IRS = dtirs.Rows[0][0].ToString();

                    try
                    {
                        cononmed = dton1.Rows[0][0].ToString();
                        cononothr = dton2.Rows[0][0].ToString();

                        Patient.Consumption_Online = (double.Parse(dton1.Rows[0][0].ToString()) + double.Parse(dton2.Rows[0][0].ToString())).ToString();

                        Patient.Consumption_Total = (Convert.ToDouble(Patient.Consumption_IRS) + Convert.ToDouble(Patient.Consumption_Online)).ToString();

                        Patient.Available = (Convert.ToDouble(Patient.Max_Amount) - Convert.ToDouble(Patient.Consumption_Total)).ToString();

                    }
                    catch { }

                    DataTable aaa = new DataTable();
                    aaa = db.RunReader(@"SELECT COPAY_PERC, MAX_AMOUNT, COPAY_AMT, SERV_CODE FROM COMP_CONTRACT_CLASS_PROVIDER 
                                         WHERE COMP_ID= '" + Patient.Comp_id + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "' AND PR_CODE = '" + User.Provider_Code + "' AND SERV_CODE = '11204'");
                    if (aaa.Rows.Count == 0)
                        aaa = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT, SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D_EMP where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='11204' AND CARD_ID = '" + txtCard.Text + "'");
                    else if (aaa.Rows.Count == 0)
                        aaa = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT, SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='11204'");
                    //   else if (aaa.Rows.Count == 0)
                    // aaa = db.RunReader("select CEILING_PERT,CEILING_AMT, CARR_AMT from V_P_COMP_CUSTOMIZED_D where C_COMP_ID='" + Patient.Comp_id  + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='11204'");

                    else if (aaa.Rows.Count == 0)
                    {
                        MessageBox.Show("هذا الكارت غير متاح له هذه الخدمة");
                    }
                }
                else
                    MessageBox.Show("هذا الكارت غير موجود من فضلك تأكد من رقم الكارت");
            }
        }

        private void SwitchBTN_Click(object sender, RoutedEventArgs e)
        {
          
                new MainWindow().Show();
            this.Close();
        }        
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        
    }

        private void refBTN_Click(object sender, RoutedEventArgs e)
        {
            //  MessageBox.Show("aaaaaaaaa");
            Console.WriteLine("SAMPLE 2: Closing dialog with parameter:");

            //MaterialMessageBox.Show("Your cool message here", "The awesome message title");
        }



        /* ======================================== toooth here =========================================== */
        string detailgroup;
        List<GroupNo> grouplist = new List<GroupNo>();
        System.Data.DataTable dtooth;
        System.Data.DataTable groupno_tabl;

      


        void AddToothService(object sender, RoutedEventArgs e)
        {
           
            System.Windows.Controls.CheckBox c = sender as System.Windows.Controls.CheckBox;

            string toothnum = c.Name.Substring(1, 2);



            //foreach (GroupNo item in grouplist)
            //{
            //    if (item.toothNumber == toothnum && item.serviceCode == "11423")
            //    {
            //        MessageBox.Show("لايمكن طلب خدمة لسن سيتم خلعة");
            //        c.IsChecked = false;
            //        return;
            //    }
            //}

            int tstoth = TestToothExist(toothnum);
            int tsttothnow = TestToothExistNow(toothnum);




            if (tstoth == 0 && tsttothnow == 0)
            {
                detailgroup = toothnum;
                tooth.IsEnabled = false;
                TeethType.Visibility = Visibility.Visible;
            }

        

        }



      



        void SubToothService(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox c = sender as System.Windows.Controls.CheckBox;

            string tothnum = c.Name.Substring(1, 2);
            if (tooth.Visibility == Visibility.Visible)
            {
                foreach (GroupNo item in grouplist)
                {
                    if (tothnum == item.toothNumber)
                    {
                        grouplist.Remove(item);
                        break;
                    }
                }
            }
        }

        private void cbxServies_DropDownClosed(object sender, EventArgs e)
        {
            if (cbxServies.Text != string.Empty)
            {

                System.Data.DataTable aaa = new System.Data.DataTable();



                string servprov = cbxServies.Text;

                try
                {
                    aaa = db.RunReader(@"SELECT COPAY_PERC, MAX_AMOUNT, COPAY_AMT, SERV_CODE FROM COMP_CONTRACT_CLASS_PROVIDER 
                                         WHERE COMP_ID= '" + Patient.Comp_id + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "' AND PR_CODE = '" + User.Provider_Code + "' AND SERV_CODE = '" + servprov + "'");

                    if (aaa.Rows.Count == 0)
                        aaa = db.RunReader(@"SELECT COPAY_PERC, MAX_AMOUNT, COPAY_AMT, SERV_CODE FROM COMP_CONTRACT_CLASS_PROVIDER 
                                         WHERE COMP_ID= '" + Patient.Comp_id + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "' AND PR_CODE = '" + User.Provider_Code + "' AND SERV_CODE = '" + cbxServies.Text + "'");


                    if (aaa.Rows.Count == 0)
                    {

                        aaa = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT, SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D_EMP where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='" + cbxServies.Text + "' AND CARD_ID = '" + txtCard.Text + "'");

                        //if (aaa.Rows.Count == 0)
                        //    aaa = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT, SER_SERV FROM APP.COMP_CUSTOMIZED_D_D_COST_CNTR where C_COMP_ID='" + comp_code + "' and CONTRACT_NO='" + contract_num + "' and CLASS_CODE='" + class_code + "' and SER_SERV='" + cbxServies.Text + "' AND COST_CODE = '" + CostCenterCodeApproval.Text + "'");
                        //if (aaa.Rows.Count == 0)
                            aaa = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT, SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='" + cbxServies.Text + "'");



                        if ((aaa.Rows.Count == 0) || (aaa.Rows.Count != 0 && aaa.Rows[0][0].ToString() == string.Empty))
                            aaa = db.RunReader("select CEILING_PERT,CEILING_AMT, CARR_AMT from V_P_COMP_CUSTOMIZED_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and D_SERV_CODE='" + cbxServies.Text + "'");

                    }


                    dtooth = db.RunReader(@"select APPROVAL_SUB_SERV.S_SERV_CODE, APPROVAL_SUB_SERV.S_SERV_NAME, APPROVAL_SUB_SERV.details, MEDICAL_APPROVALS.CREATED_DATE,MEDICAL_APPROVALS.CODE from APPROVAL_SUB_SERV, MEDICAL_APPROVALS 
                                                            where APPROVAL_SUB_SERV.code = MEDICAL_APPROVALS.code
                                                            AND MEDICAL_APPROVALS.card_no = '" + txtCard.Text + "' AND MEDICAL_APPROVALS.COMP_CONTRACT_NO ='" + Patient.Contract_NO + "' AND APPROVAL_SUB_SERV.S_SERV_CODE ='" + cbxServies.Text + "' AND MEDICAL_APPROVALS.active = 'Y'");

                }
                catch { }
                //if (aaa.Rows.Count == 0)
                //    aaa = db.RunReader("select CEILING_PERT,CEILING_AMT from V_P_COMP_CUSTOMIZED_D_D where C_COMP_ID='" + comp_code + "' and CONTRACT_NO='" + contract_num + "' and CLASS_CODE='" + class_code + "' and D_SERV_CODE='" + cbxServies.Text + "'");

                if (aaa.Rows.Count != 0 && aaa.Rows[0][0].ToString() != string.Empty)
                {
                    //  txtindcode1_Copy1.Text = aaa.Rows[0][0].ToString() + " %";// نسبة التحمل
                    //  txtindcode1_Copy2.Text = aaa.Rows[0][1].ToString(); // الحد الاقصى للخدمة

                    //okaprovpercnt = txtindcode1_Copy1.Text;
                    //flagapproval = "YES";

                    //if (aaa.Rows[0][2].ToString() != string.Empty)
                    //    copayamtaprov = " يتحمل المريض مبلغ وقدره" + aaa.Rows[0][2].ToString() + " جنيه";
                    //else
                    //    copayamtaprov = "";

                   

                  
                    tooth.Visibility = Visibility.Visible;
                    NameTeethSearch_Copy.ItemsSource= NameTeethSearch.ItemsSource = db.RunReader(@"select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H FROM v_services where SERV_CODE like '" + cbxServies.Text + "%' order by SERV_ENAME").DefaultView;

                }
                else
                {
                    System.Data.DataTable dtrecolaprov = new System.Data.DataTable();
                    dtrecolaprov = db.RunReader(@"SELECT RECOLLECTION FROM PAYMENT_DET_D WHERE C_COMP_ID = '" + Patient.Comp_id + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' ");
                    if (dtrecolaprov.Rows.Count > 0 && dtrecolaprov.Rows[0][0].ToString() == "N")
                    {
                        MessageBox.Show("هذه الخدمة غير مغطاه لهذا المريض");

                    }
                    else
                    {
                        MessageBox.Show("هذه الخدمة غير مغطاه لهذا المريض");
                    }

                }
            }
        }

       

        int TestToothExist(string toth)
        {
            int tm = 0;
            if (dtooth.Rows.Count != 0)
            {
                for (int i = 0; i < dtooth.Rows.Count; i++)
                {
                    if (dtooth.Rows[i][2].ToString() == toth)
                    {
                        MessageBox.Show("لقد تم عمل " + dtooth.Rows[i][1].ToString() + "للسن " + dtooth.Rows[i][2].ToString() + "بتاريخ " + dtooth.Rows[i][3].ToString() + "وبرقم موافقة " + dtooth.Rows[i][4].ToString());
                        cbxServies.Text = "";
                    
                        detailgroup = "";
                        tm = 1;
                        break;
                    }

                }
            }
            return tm;
        }

        private void NameTeethSearch_DropDownClosed(object sender, EventArgs e)
        {
            NameTeethSearch_Copy.SelectedItem = NameTeethSearch.SelectedItem;
        }

        private void TeethType_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
                addSubservfun();
           

        }


        void addSubservfun()
        {
            GroupNo temp;
            if (txtSalary.Text.Trim() == "")
            {
                MessageBox.Show("برجاء ادخال سعر العملية");
                return;
            }

            if (NameTeethSearch.Text != string.Empty && NameTeethSearch.SelectedIndex >= 0)
            {
                foreach (DataRow item in groupno_tabl.Rows)
                {
                    if (item[0].ToString() == cbxServies.Text)
                    {

                        temp.serial = (grouplist.Count + 1).ToString();
                        temp.serviceCode = cbxServies.Text;
                        temp.serviceName = item[1].ToString();
                        temp.subServicePrice = txtSalary.Text;

                        temp.toothNumber = detailgroup;

                        temp.subServiceName = NameTeethSearch.Text;
                        temp.subServiceCode = NameTeethSearch_Copy.Text;
                        temp.subServicePrice = txtSalary.Text;
                        temp.DmsDiscForSubServ = "0 %";
                        temp.paitiontCash= txtSalary.Text;
                        grouplist.Add(temp);


                        NameTeethSearch_Copy.Text = "";

                        txtSalary.Text = "";
                        detailgroup = "";
                        NameTeethSearch.Text = "";

                        break;
                    }
                }
                TeethType.Visibility = Visibility.Hidden;
                tooth.IsEnabled = true;
            }
            else
                MessageBox.Show("من فضلك أختر العملية المطلوبة من القائمة");
        }
        int TestToothExistNow(string toth)
        {
            int tm = 0;
          //  GroupNo temp;


            foreach (GroupNo item in grouplist)
            {
                if ((cbxServies.Text == item.serviceCode) && (toth == item.toothNumber))
                {
                    MessageBox.Show("تمت اضافتة من قبل");
                    tm = 1;
                    break;
                }
            }

            return tm;
        }

        private void ToothService(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            tooth.Visibility = Visibility.Collapsed;





            DataTable dt = new DataTable();
            dt.Columns.Add("رقم", typeof(string));
            dt.Columns.Add("كود الخدمة", typeof(string));
            dt.Columns.Add("اسم الخدمة", typeof(string));
       //     dt.Columns.Add("cod2", typeof(string));
            dt.Columns.Add("السن", typeof(string));
          
            dt.Columns.Add("كود العملية", typeof(string));
            dt.Columns.Add("اسم العملية", typeof(string));
            dt.Columns.Add("سعر العملية", typeof(string));
            dt.Columns.Add("نسبة الخصم", typeof(string));
            dt.Columns.Add("بعد الخصم", typeof(string));

            double total = 0;
            //   txtindcode1_Service.Text = "";
            foreach (GroupNo item in grouplist)
            {
                //  txtindcode1_Service.Text = txtindcode1_Service.Text + item.name + " " + item.discr + item.detail + item.typopera + " " + "\n";

                total += Convert.ToDouble(item.subServicePrice);
                //    dt.Columns.Add("name", typeof(string));
                dt.Rows.Add(item.serial, item.serviceCode, item.serviceName, item.toothNumber, item.subServiceCode, item.subServiceName, item.subServicePrice, item.DmsDiscForSubServ, item.paitiontCash);
            //    dgTotal.ItemsSource = groupno_tabl.DefaultView;

            }
            txtAName1.Text = total.ToString();
            txtAName2.Text = "0";
            txtAName3.Text = total.ToString();
            dgTotal.ItemsSource = dt.DefaultView;
            //  cbxindtyp1_Copy3.IsEnabled = false;
            // requesthrsearch3_addservice.IsEnabled = false;
            //cbxServies.IsEnabled = false;







            _11.IsChecked = false;
            _12.IsChecked = false;
            _13.IsChecked = false;
            _14.IsChecked = false;
            _15.IsChecked = false;
            _16.IsChecked = false;
            _17.IsChecked = false;
            _18.IsChecked = false;
            _21.IsChecked = false;
            _22.IsChecked = false;
            _23.IsChecked = false;
            _24.IsChecked = false;
            _25.IsChecked = false;
            _26.IsChecked = false;
            _27.IsChecked = false;
            _28.IsChecked = false;
            _31.IsChecked = false;
            _32.IsChecked = false;
            _33.IsChecked = false;
            _34.IsChecked = false;
            _35.IsChecked = false;
            _36.IsChecked = false;
            _37.IsChecked = false;
            _38.IsChecked = false;
            _41.IsChecked = false;
            _42.IsChecked = false;
            _43.IsChecked = false;
            _44.IsChecked = false;
            _45.IsChecked = false;
            _46.IsChecked = false;
            _47.IsChecked = false;
            _48.IsChecked = false;
         

        }


        private void Cancel_Teeth_Click(object sender, RoutedEventArgs e)
        {
            cbxServies.Text = "";
          
            detailgroup = "";
            grouplist.Clear();
            tooth.Visibility = Visibility.Collapsed;
         
          //  reqaddhrfalse_Copy3.IsEnabled = true;

            _11.IsChecked = false;
            _12.IsChecked = false;
            _13.IsChecked = false;
            _14.IsChecked = false;
            _15.IsChecked = false;
            _16.IsChecked = false;
            _17.IsChecked = false;
            _18.IsChecked = false;
            _21.IsChecked = false;
            _22.IsChecked = false;
            _23.IsChecked = false;
            _24.IsChecked = false;
            _25.IsChecked = false;
            _26.IsChecked = false;
            _27.IsChecked = false;
            _28.IsChecked = false;
            _31.IsChecked = false;
            _32.IsChecked = false;
            _33.IsChecked = false;
            _34.IsChecked = false;
            _35.IsChecked = false;
            _36.IsChecked = false;
            _37.IsChecked = false;
            _38.IsChecked = false;
            _41.IsChecked = false;
            _42.IsChecked = false;
            _43.IsChecked = false;
            _44.IsChecked = false;
            _45.IsChecked = false;
            _46.IsChecked = false;
            _47.IsChecked = false;
            _48.IsChecked = false;
        }


        private void Ok_Teeth_Click(object sender, RoutedEventArgs e)
        {
            addSubservfun();
        }



    }
}
