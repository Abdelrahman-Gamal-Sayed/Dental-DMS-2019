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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.CrystalReports;
using Microsoft.Win32;

namespace ONA_Clinics
{
    /// <summary>
    /// Interaction logic for Doctor.xaml
    /// </summary>
    public partial class Doctor : Window
    {
        DB db = new DB();
        DataDB tdb = new DataDB();

        public Doctor()
        {
            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
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
                DataTable dtinformation = db.RunReader("select  to_char(BIRTH_DATE,'DD-MM-YYYY'),C_COMP_ID,CLASS_CODE,NVL(to_char(SPECIFIC_DATE,'DD-MM-YYYY'),to_char(INS_START_DATE,'DD-MM-YYYY')),to_char(INS_END_DATE,'DD-MM-YYYY'),TERMINATE_FLAG ,EMP_ANAME_ST ,EMP_ANAME_SC,EMP_ANAME_TH ,EMP_ENAME_ST ,EMP_ENAME_SC,EMP_ENAME_TH, to_char(INS_START_DATE,'DD-MM-YYYY'), to_char(TERMINATE_DATE,'DD-MM-YYYY') from COMP_EMPLOYEESS where CARD_ID='" + txtCard.Text + "' order by ins_start_date DESC");

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

                    DataTable dgetdegree = new DataTable();

                    dgetdegree = db.RunReader(@"SELECT COVER_RELATION, BS_ANAME FROM dms_test.COMP_CONTRACT_CLASS, dms_test.BASIC_DATA 
                                                                 WHERE COMP_CONTRACT_CLASS.COVER_RELATION = BASIC_DATA.BS_CODE AND SOURCE_MOD = 'PRDEG' 
                                                                    AND C_COMP_ID= '" + Patient.Comp_id + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "'");
                    if (dgetdegree.Rows.Count > 0)
                        Patient.CardDegree = dgetdegree.Rows[0][0].ToString();


                    string cononmed = "", cononothr = "";
                    Patient.Card_NO = txtCard.Text;
                    txtBirthDate.Text = dtinformation.Rows[0][0].ToString();
                    Patient.Comp_id = dtinformation.Rows[0][1].ToString();
                    txtCompName.Text = db.RunReader(@"select C_ENAME from V_COMPANIES  WHERE  C_COMP_ID  = '" + Patient.Comp_id + "'").Rows[0][0].ToString();
                    txtAge.Text = (DateTime.Now.Year - Convert.ToDateTime(dtinformation.Rows[0][0]).Year).ToString();
                    Patient.Class_Code = dtinformation.Rows[0][2].ToString();
                    txtContractStartDate.Text = dtinformation.Rows[0][3].ToString();
                    txtContractEndtDate.Text = dtinformation.Rows[0][4].ToString();
                    Patient.AName = dtinformation.Rows[0][6].ToString() + " " + dtinformation.Rows[0][7].ToString() + " " + dtinformation.Rows[0][8].ToString();
                    Patient.EName = dtinformation.Rows[0][9].ToString() + " " + dtinformation.Rows[0][10].ToString() + " " + dtinformation.Rows[0][11].ToString();

                    txtEName.Text = Patient.EName;
                    txtAName.Text = Patient.AName;

                    Patient.Contract_NO = db.RunReader("select max(contract_no) from COMP_EMPLOYEESS where C_COMP_ID='" + Patient.Comp_id + "'").Rows[0][0].ToString();
                    Patient.Max_Amount = db.RunReader("select MAX_AMOUNT from V_P_COMP_CONTRACT_CLASS where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "'").Rows[0][0].ToString();

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

                        filldata();
                        SecreenData.IsEnabled = true;
                        calculatepercent();
                        //    DataTable dtservok = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT from DMS_TEST.COMP_CUSTOMIZED_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO+ "' and CLASS_CODE='" + Patient.Class_Code + "' and D_SERV_CODE= 111");

                        //if(dtservok.Rows.Count > 0)
                        //{
                        //ServiceType.IsEnabled = true;
                        //ServiceType.ItemsSource = db.RunReader("SELECT SERV_CODE, SERV_ENAME FROM SERVICES WHERE SERV_CODE IN('111', '112', '11104')").DefaultView;
                        ////}
                        //else
                        //{
                        //    ServiceType.Text = "";
                        //    ServiceType.IsEnabled = false;
                        //}
                        User.Provider_Code = "11205";
                        checkhistory();
                        SaveData.IsEnabled = true;
                        //aaa = db.RunReader("select CEILING_PERT,CEILING_AMT, CARR_AMT from V_P_COMP_CUSTOMIZED_D where C_COMP_ID='" + cbxindtyp1.Text + "' and CONTRACT_NO='" + txtindcode1_Copy6.Text + "' and CLASS_CODE='" + txtindcode1_Copy.Text + "' and D_SERV_CODE='" + cbxindtyp1_Copy3.Text + "'").Result;

                    }
                    catch { }
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
        int flghist = 0;
        void checkhistory()
        {
            DataTable dthist = db.RunReader(@"SELECT D_DETECTION.CODE, CLAIM_NO, D_DETECTION.CREATED_DATE, NEXT_CONSULT FROM D_DETECTION, D_ONLINE_DATA 
                                              WHERE D_DETECTION.CODE = D_ONLINE_DATA.CODE AND D_DETECTION.CARD_ID = '" + Patient.Card_NO + "' AND D_DETECTION.COMP_ID = '" + Patient.Comp_id + "' AND D_DETECTION.CONTRACT_NO =  '" + Patient.Contract_NO + "'AND D_DETECTION.PROVIDER_CODE = '" + User.Provider_Code + "'      "
                                        + "   AND (TRUNC(TO_DATE(D_DETECTION.CREATED_DATE)) - TRUNC(TO_DATE(SYSDATE)) < 7 OR TRUNC(TO_DATE(NEXT_CONSULT)) = TRUNC(TO_DATE(SYSDATE))) AND SERV_CODE = 11205         "
                                        + "   ORDER BY D_DETECTION.CREATED_DATE DESC");
            if(dthist != null && dthist.Rows.Count > 0)
            {
                NumberCheckup.Text = dthist.Rows[0][0].ToString();
                CheckupType.Text = "إستشارة";
                LastCheckup.Text = dthist.Rows[0][3].ToString();
                ShowHistoryScreen.Visibility = Visibility.Visible;
                flghist = 2;                
            }
            else
            {
                NumberCheckup.Text = "";
                CheckupType.Text = "كشف جديد";
                LastCheckup.Text = "";
                ShowHistoryScreen.Visibility = Visibility.Hidden;
                flghist = 1;
            }
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();

        }

        private void ServiceType_DropDownClosed(object sender, EventArgs e)
        {
            
        }


        void calculatepercent()
        {
            DataTable dtperc = new DataTable();
            dtperc = db.RunReader("SELECT NVL(CEILING_PERT,0), CEILING_AMT, NVL(CARR_AMT, 0), SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D_EMP where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV= 11205 AND CARD_ID = '" + Patient.Card_NO + "'");
            if (dtperc.Rows.Count == 0)
                db.RunReader("SELECT NVL(CEILING_PERT,0), CEILING_AMT, NVL(CARR_AMT, 0), SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV = 11205");

            if (dtperc.Rows.Count != 0)
            {
                Patient.PercServ = (100 - Convert.ToDouble(dtperc.Rows[0][0])).ToString();                
                Patient.MaxAmtServ = dtperc.Rows[0][1].ToString();
                Patient.CARR_AMT = dtperc.Rows[0][2].ToString();
            }

            labelPercent.Content = "يتحمل المريض " + Patient.PercServ + "% من قيمة الكشف";
            labelAmt.Content = "يتم تحصيل من المريض مبلغ قدره " + Patient.CARR_AMT + " جنيه ";
        }
        DataTable dtsrvdetials;
        void setsubservdata()
        {
            if (dtsrvdetials == null)
            {
                dtsrvdetials = new DataTable();
                dtsrvdetials.Columns.Add("CODE_SERV");
                dtsrvdetials.Columns.Add("NAME_SERV");
                dtsrvdetials.Columns.Add("DESCRIPTION");
                dtsrvdetials.Columns.Add("AMT", typeof(double));
                //  dtsrvdetials.Columns.Add("FINAL_AMT", typeof(double));
            }
        }
        private void PackIcon_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (DataSubService.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataSubService.SelectedItem;

                dtsrvdetials.Rows.RemoveAt(DataSubService.SelectedIndex);
                DataSubService.ItemsSource = dtsrvdetials.DefaultView;
            }
        }
        private void StackPanel_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (DataSubService.SelectedItem != null)
            {
                dtsrvdetials.Rows.RemoveAt(DataSubService.SelectedIndex);
                DataSubService.ItemsSource = dtsrvdetials.DefaultView;
            }
        }
        DataTable dtbill;
        int getcodserv(Int32 serv)
        {
            int cod = 0;
            switch(serv)
            {
                case 111:
                    cod = 6;
                    break;
                case 112:
                    cod = 7;
                    break;
                case 11204:
                    cod = 3;
                    break;
                case 11105:
                case 11205:
                    cod = 4;
                    break;
                case 114:
                    cod = 8;
                    break;
                case 115:
                    cod = 9;
                    break;
                case 113:
                    cod = 10;
                    break;
                case 11206:
                    cod = 1;
                    break;
                case 11201:
                    cod = 2;
                    break;

                default:
                    cod = 0;
                    break;

            }
            return cod;
        }

        DataTable dtdiagnosis;
        DataTable dtservice;
        DataTable dtsubservice;
        private void CalculateBill_Click(object sender, RoutedEventArgs e)
        {
            /* DataView view = new DataView(dtsrvdetials);
             DataTable dtclsname = view.ToTable(true, "كود الفئة", "اسم الفئة");
             dtclsname.Columns[0].ColumnName = "CLASS_CODE";
             dtclsname.Columns[1].ColumnName = "CLASS_ENAME";
             */

            // int sum = Convert.ToInt32(dtsrvdetials.Compute("SUM(Salary)", "EmployeeId > 2"));
            /* DataRow[] dr = dtbl.Select("SUM(Amount)");
             txtTotalAmount.Text = Convert.ToString(dr[0]);
             */

            //DataTable _newDataTable = yurDateTable.Select(_sqlWhere, _sqlOrder).CopyToDataTable();
            if (DataSubService.Items.Count > 0)
            {
                if (dtbill == null)
                {
                    dtbill = new DataTable();
                    dtbill.Columns.Add("SERV");
                    dtbill.Columns.Add("SERV_NAME");
                    dtbill.Columns.Add("AMT", typeof(double));
                    dtbill.Columns.Add("Cash", typeof(double));
                    dtbill.Columns.Add("Credit", typeof(double));
                }
                dtbill.Clear();
                try
                {

                    // DataRow[] getservname = dtsrvdetials.Select("DISTINCT CODE_SERV");

                    DataView view = new DataView(dtsrvdetials);
                    DataTable dtservdisc = view.ToTable(true, "CODE_SERV", "NAME_SERV");

                    DataTable dtperc = new DataTable();
                    double perc = 0, mxamtserv = 0, valserv = 0, sum = 0, finl = 0, finldms = 0;

                    foreach (DataRow row in dtservdisc.Rows)
                    {
                        DataRow dr = dtbill.NewRow();
                        perc = 0; mxamtserv = 0; valserv = 0; sum = 0; finl = 0; finldms = 0;

                        sum = Convert.ToInt32(dtsrvdetials.Compute("SUM(AMT)", "CODE_SERV = '" + row[0].ToString() + "'"));

                        dr[0] = row[0].ToString();
                        dr[1] = row[1].ToString();
                        dr[2] = sum;

                        dtperc = db.RunReader("SELECT CEILING_PERT, NVL(CEILING_AMT,0), NVL(CARR_AMT,0), SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D_EMP where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='" + row[0].ToString() + "' AND CARD_ID = '" + Patient.Card_NO + "'");
                        if (dtperc.Rows.Count == 0)
                            db.RunReader("SELECT CEILING_PERT, NVL(CEILING_AMT,0), NVL(CARR_AMT,0), SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='" + row[0].ToString() + "'");

                        if (dtperc.Rows.Count != 0)
                        {
                            perc = Convert.ToDouble(dtperc.Rows[0][0]);
                            mxamtserv = Convert.ToDouble(dtperc.Rows[0][1]);
                            valserv = Convert.ToDouble(dtperc.Rows[0][2]);
                        }

                        finldms = sum * Convert.ToDouble(perc) / 100;

                        finl += valserv;
                        finldms -= valserv;

                        int codserv = getcodserv(Int32.Parse(row[0].ToString()));
                        DataTable dtcodserv = new DataTable();
                        dtcodserv = db.RunReader(@"SELECT CONSUMPTION FROM CONS_SUMMARY WHERE COMP_ID = '" + Patient.Comp_id + "' AND CARD_ID = '" + Patient.Card_NO + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "' AND GROUP_NO = '" + codserv + "'");
                        double conserv = 0, conservall = 0, conservaval = 0;

                        if (dtcodserv.Rows.Count > 0)
                            conserv = Convert.ToDouble(dtcodserv.Rows[0][0]);
                        else
                            conserv = 0;

                        conservall = conserv + finldms;
                        conservaval = mxamtserv - conserv;

                        if (mxamtserv > 0 && conservall > mxamtserv)
                        {
                            finl += conservall - mxamtserv;
                            finldms = conservaval;
                        }

                        dr[3] = finl;
                        dr[4] = finldms;

                        // if (int.Parse(Patient.CardDegree) < int.Parse(User.Degree_Code))
                        if (!(((int.Parse(Patient.CardDegree) == 1 || int.Parse(Patient.CardDegree) == 4) && (int.Parse(User.Degree_Code) >= 1 && int.Parse(User.Degree_Code) <= 3)) ||// degprovid != "1" || degprovid != "2" || degprovid != "3"
                           (int.Parse(Patient.CardDegree) == 2 && (int.Parse(User.Degree_Code) >= 2 && int.Parse(User.Degree_Code) <= 3)) || (int.Parse(Patient.CardDegree) == 3 && (int.Parse(User.Degree_Code) == 3))))
                        {
                            dr[3] = sum;
                            dr[4] = "0";
                        }

                        dtbill.Rows.Add(dr);
                    }

                    double finalamt = Convert.ToDouble(dtbill.Compute("SUM(Cash)", ""));
                    double finalamtdms = Convert.ToDouble(dtbill.Compute("SUM(Credit)", ""));
                    //Testtttt
                    if (finalamtdms > double.Parse(Patient.Max_Amount))
                    {
                        finalamt = finalamt + (finalamtdms - double.Parse(Patient.Max_Amount));
                        finalamtdms = double.Parse(Patient.Available);// double.Parse(Patient.Max_Amount);
                    }
                    Patient.FinalAmtDms = finalamtdms.ToString();
                    //FinalAmount.Text = finalamt.ToString();
                    //FinalAmountDms.Text = finalamtdms.ToString();
                    //DataBillDetails.ItemsSource = dtbill.DefaultView;
                    //SaveBill.Visibility = Visibility.Visible;
                }

                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
                MessageBox.Show("لم تقم بإختيار شيء");
        }
        private void StackPanel2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtCard.Text = "";
            txtBirthDate.Text = "";
            txtCompName.Text = "";
            txtAge.Text = "";
            txtContractStartDate.Text = "";
            txtContractEndtDate.Text = "";
            txtEName.Text = "";
            txtAName.Text = "";
           
            clearscreen();
        }
        void clearscreen()
        {            
            MainScreen.IsEnabled = true;

            txtCard.Text = "";
            txtBirthDate.Text = "";
            
            txtCompName.Text = "";
            txtAge.Text = "";
            txtContractStartDate.Text = "";
            txtContractEndtDate.Text = "";
            
            txtEName.Text = "";
            txtAName.Text = "";
            CheckupType.Text = "";

            LastCheckup.Text = "";
            NextMedicalConsultatio.Text = "";
            AllMoney.Text = "";
            CreditMony.Text = "";
            CashMoney.Text = "";
            Note.Text = "";

            Diagnosis.Text = "";
            Service.Text = "";
            ServiceType.Text = "";
            SubService.Text = "";

            Numberpotion.Text = "";
            Duration.Text = "";
            
            NumberSession.Text = "";
            NumberOrgn.Text = "";
            labelPercent.Content = "";
            labelAmt.Content = "";

            SecreenData.IsEnabled = false;
            SaveData.IsEnabled = false;

            ShowHistoryScreen.Visibility = Visibility.Hidden;

            dtdiagnosis = null;
            dtservice = null;
            dtsubservice = null;
            
            Midcation.IsChecked = false;
            Ray.IsChecked = false;
            Lab.IsChecked = false;
            Physical.IsChecked = false;
            
            Service.ItemsSource = null;
            SubService.ItemsSource = null;
            Diagnosis.ItemsSource = null;
            
            DataDiagnosis.ItemsSource = null;
            DataService.ItemsSource = null;
            DataSubService.ItemsSource = null;

            NumberCheckup.Text = "";
            DiagnosisHistory.Document.Blocks.Clear();
            ServiceHistory.Document.Blocks.Clear();

            NotesSubService.Text = "";
            NotesScreen.Visibility = Visibility.Hidden;
            SubServiceHistory.ItemsSource = null;
            DataHistory.ItemsSource = null;
            HistoryScreen.Visibility = Visibility.Hidden;
            ShowHistoryScreen.Visibility = Visibility.Hidden;

        }
        void filldata()
        {
            Diagnosis.ItemsSource = db.RunReader("select DIA_CODE,DIA_ANAME,DIA_ENAME from DIAGNOSIS where  ROWNUM <= 50 order by DIA_CODE").DefaultView;
            //if (User.DOCTOR_SPE == "11" || User.DOCTOR_SPE == "21")
            //    ServiceType.ItemsSource = db.RunReader("SELECT SERV_CODE GROUP_NO, SERV_ANAME GROUP_ANAME, SERV_ENAME FROM SERVICES WHERE SERV_CODE IN ('11201', '11206', '116', '11204') ORDER BY SERV_ANAME").DefaultView;
            //else
            //    ServiceType.ItemsSource = db.RunReader(" SELECT SERV_CODE GROUP_NO, SERV_ANAME GROUP_ANAME, SERV_ENAME FROM SERVICES WHERE SERV_CODE IN ('11201', '11206', '116') ORDER BY SERV_ANAME").DefaultView;

            if (User.DOCTOR_SPE == "11" || User.DOCTOR_SPE == "21")
                Physical.Visibility = Visibility.Hidden;
            else
                Physical.Visibility = Visibility.Visible;

            Service.ItemsSource = db.RunReader("SELECT SERV_CODE, SERV_ANAME, SERV_ENAME FROM SERVICES WHERE SERV_CODE_H LIKE '11203%' AND ROWNUM < 100 ORDER BY SERV_ANAME").DefaultView;
            //    db.RunReader("select DIA_CODE,DIA_ANAME,DIA_ENAME from DIAGNOSIS where DIA_CODE like '%" + cbxindtyp1_Copy2.Text + "%' or upper(DIA_ANAME) like '%" + cbxindtyp1_Copy2.Text.ToUpper() + "%' or upper(DIA_ENAME) like '%" + cbxindtyp1_Copy2.Text.ToUpper() + "%' and rownum<50");
        }
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearscreen();
        }
        private void DataApproval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataApproval.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataApproval.SelectedItem;
            }
        }
        private void AddDiagnosis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(dtdiagnosis == null)
            {
                dtdiagnosis = new DataTable();
                dtdiagnosis.Columns.Add("CODE");
                dtdiagnosis.Columns.Add("Name");
            }

            DataRow dr = dtdiagnosis.NewRow();
            DataRowView row = (DataRowView)Diagnosis.SelectedItem;

            dr[0] = row[0].ToString();
            dr[1] = row[1].ToString();

            dtdiagnosis.Rows.Add(dr);
            DataDiagnosis.ItemsSource = dtdiagnosis.DefaultView;
            Diagnosis.Text = "";
        }
        private void AddService_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dtservice == null)
            {
                dtservice = new DataTable();
                dtservice.Columns.Add("CODE");
                dtservice.Columns.Add("Name");
            }

            DataRow dr = dtservice.NewRow();
            DataRowView row = (DataRowView)Service.SelectedItem;

            dr[0] = row[0].ToString();
            dr[1] = row[1].ToString();

            dtservice.Rows.Add(dr);
            DataService.ItemsSource = dtservice.DefaultView;
            Service.Text = "";
        }


        string getservcode()
        {
            string serv = "";
            
            if (Midcation.IsChecked == true)
                serv = "116";
            else if (Lab.IsChecked == true)
                serv = "11206";
            else if (Ray.IsChecked == true)
                serv = "11201";
            else if (Physical.IsChecked == true)
                serv = "11204";

            return serv;
        }

        private void AddSubService_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string serv = getservcode();

            if (serv != string.Empty && SubService.Text != string.Empty)
            {
                if ((User.DOCTOR_SPE == "11" || User.DOCTOR_SPE == "21") && dtsubservice == null)
                {
                    dtsubservice = new DataTable();
                    dtsubservice.Columns.Add("Service");
                    dtsubservice.Columns.Add("Code");
                    dtsubservice.Columns.Add("Name");
                    dtsubservice.Columns.Add("Status");
                    dtsubservice.Columns.Add("Number");
                    dtsubservice.Columns.Add("Duration");
                    dtsubservice.Columns.Add("NumberOrgan");
                    dtsubservice.Columns.Add("NumberSession");
                }
                else if(dtsubservice == null) //if(User.DOCTOR_SPE == "" && dtsubservice == null)
                {
                    dtsubservice = new DataTable();
                    dtsubservice.Columns.Add("Service");
                    dtsubservice.Columns.Add("Code");
                    dtsubservice.Columns.Add("Name");
                    dtsubservice.Columns.Add("Status");
                    dtsubservice.Columns.Add("Number");
                    dtsubservice.Columns.Add("Duration");
                }

                DataRow dr = dtsubservice.NewRow();
                DataRowView row = (DataRowView)SubService.SelectedItem;

                dr[0] = serv;// ServiceType.Text;
                dr[1] = row[0].ToString();
                dr[2] = row[1].ToString();
                dr[3] = row[2].ToString();
                dr[4] = Numberpotion.Text;
                dr[5] = Duration.Text;

                if (User.DOCTOR_SPE == "11" || User.DOCTOR_SPE == "21")
                {
                    dr[6] = NumberSession.Text;
                    dr[7] = NumberOrgn.Text;
                }

                dtsubservice.Rows.Add(dr);
                DataSubService.ItemsSource = dtsubservice.DefaultView;

                Numberpotion.Text = "";
                Duration.Text = "";
                NumberSession.Text = "";
                NumberOrgn.Text = "";
                SubService.Text = "";
            }
        }
        private void CancelPhysicalData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PhysicalScreen.Visibility = Visibility.Hidden;
           // ServiceType.SelectedItem = null;
            SubService.SelectedItem = null;
            SubService.Text = "";
        }
        private void CancelMedicineData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MedicineScreen.Visibility = Visibility.Hidden;
            //ServiceType.SelectedItem = null;
            SubService.SelectedItem = null;
            SubService.Text = "";
        }
        private void OkMedicineData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Numberpotion.Text != string.Empty && Duration.Text != string.Empty)
                MedicineScreen.Visibility = Visibility.Hidden;
            else
                MessageBox.Show("من فضلك أكمل البيانات");
        }
        private void OkPhysicalData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (NumberSession.Text != string.Empty && Duration.Text != string.Empty)
                PhysicalScreen.Visibility = Visibility.Hidden;
            else
                MessageBox.Show("من فضلك أكمل البيانات");
        }
        private void ServiceType_DropDownClosed_1(object sender, EventArgs e)
        {
            if (ServiceType.Text == "11201")
                SubService.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H, 'YES' STATUS FROM v_services where rownum <= 50 and serv_code like '11201%'").DefaultView;
            else if (ServiceType.Text == "11206")
                SubService.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H, 'YES' STATUS FROM v_services where rownum <= 50 and serv_code like '11206%'").DefaultView;
            else if (ServiceType.Text == "11204")
                SubService.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H, 'YES' STATUS FROM v_services where rownum <= 50 and serv_code like '11204%'").DefaultView;
            else if (ServiceType.Text == "116")
                SubService.ItemsSource = db.RunReader(@"SELECT M_CODE SERV_CODE, TRADE_NAME SERV_ENAME, NVL(GRUOP_TYPE, 'YES') STATUS  FROM DMS_TEST.MEDICINE_DATA 
                                                        LEFT OUTER JOIN MEDICINE_GRUOP ON MEDICINE_DATA.MED_GROUP = MEDICINE_GRUOP.gruop_id WHERE rownum < 100").DefaultView;
        }
        private void SubService_DropDownClosed(object sender, EventArgs e)
        {
            if (Physical.IsChecked == true && SubService.SelectedIndex != -1)
                PhysicalScreen.Visibility = Visibility.Visible;
            else if (Midcation.IsChecked == true && SubService.SelectedIndex != -1)
                MedicineScreen.Visibility = Visibility.Visible;
            else
            {
                Numberpotion.Text = "";
                Duration.Text = "";
                NumberSession.Text = "";
                NumberOrgn.Text = "";               
            }
        }
        private void PackIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataDiagnosis.SelectedItem != null)
            {
                dtdiagnosis.Rows.RemoveAt(DataDiagnosis.SelectedIndex);
                DataDiagnosis.ItemsSource = dtdiagnosis.DefaultView;
            }
        }
        private void PackIcon_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {
            if (DataService.SelectedItem != null)
            {
                dtservice.Rows.RemoveAt(DataService.SelectedIndex);
                DataService.ItemsSource = dtservice.DefaultView;
            }
        }
        private void PackIcon_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e)
        {
            if (DataSubService.SelectedItem != null)
            {
                dtsubservice.Rows.RemoveAt(DataSubService.SelectedIndex);
                DataSubService.ItemsSource = dtsubservice.DefaultView;
            }
        }

        private void SaveData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataDB ddbb = new DataDB();
            //public void savedoctor(string birth, int typ, int chktyp, string lstchk, string nxtcon, int perc, string nts, string amt, string crdt, string csh)

            int flg = ddbb.savedoctor(4, AllMoney.Text, CreditMony.Text, CashMoney.Text);
            if (flg == 1)
            {

                string cod = db.RunReader(@"SELECT CODE FROM D_DETECTION WHERE COMP_ID = '" + Patient.Comp_id + "' AND CARD_ID = '" + Patient.Card_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "' AND CONTRACT_NO = '" + Patient.Contract_NO + "'AND SERV_CODE = 11205 AND PROVIDER_CODE =11205").Rows[0][0].ToString();
                ddbb.savedoctor2(cod, txtBirthDate.Text, 4, flghist, LastCheckup.Text, NextMedicalConsultatio.Text, 80, Note.Text, AllMoney.Text, CreditMony.Text, CashMoney.Text);
                if (dtsubservice != null)
                    foreach (DataRow r in dtsubservice.Rows)
                    {
                        if (User.DOCTOR_SPE == "11" || User.DOCTOR_SPE == "21")
                            db.RunNonQuery(@"INSERT INTO D_DOCTOR_SERVICE (CODE, D_SERV_CODE, SERV_CODE, SERV_NAME, STATUS, MED_NUMBER, MED_DURATION, ORGAN_NUMBER, SESSION_DURATION)
                                                           VALUES ('" + cod + "', '" + r[0].ToString() + "', '" + r[1].ToString() + "', '" + r[2].ToString() + "','" + r[3].ToString() + "','" + r[4].ToString() + "','" + r[5].ToString() + "','" + r[6].ToString() + "','" + r[7].ToString() + "')");
                        else
                            db.RunNonQuery(@"INSERT INTO D_DOCTOR_SERVICE (CODE, D_SERV_CODE, SERV_CODE, SERV_NAME, STATUS, MED_NUMBER, MED_DURATION)
                                                           VALUES ('" + cod + "', '" + r[0].ToString() + "', '" + r[1].ToString() + "', '" + r[2].ToString() + "','" + r[3].ToString() + "','" + r[4].ToString() + "','" + r[5].ToString() + "')");

                    }

                if (dtdiagnosis != null)
                    foreach (DataRow r in dtdiagnosis.Rows)
                    {
                        db.RunNonQuery(@"INSERT INTO D_DOCTOR_DIAGNOSIS (CODE, DIAG_CODE, DIAG_NAME)
                                                                VALUES ('" + cod + "', '" + r[0].ToString() + "', '" + r[1].ToString() + "')");
                    }

                if (dtservice != null)
                    foreach (DataRow r in dtservice.Rows)
                    {
                        db.RunNonQuery(@"INSERT INTO D_DOCTOR_SERVICE2 (CODE, SERV_CODE, SERV_NAME)
                                                                VALUES ('" + cod + "', '" + r[0].ToString() + "', '" + r[1].ToString() + "')");
                    }

                MessageBox.Show("تم الحفظ بنجاح");
                SaveData.IsEnabled = false;
            }
        }

        private void NewData_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearscreen();
        }

        private void AllMoney_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.Key == Key.Enter)
            //{
            //    if (AllMoney.Text != string.Empty)
            //    {
            //       // Patient.Patient_carrying_ratio = 80;

            //        CreditMony.Text = (Convert.ToDouble(AllMoney.Text) * Patient.Patient_carrying_ratio / 100).ToString();
            //        CashMoney.Text = (Convert.ToDouble(AllMoney.Text) * (100 - Patient.Patient_carrying_ratio) / 100).ToString();

            //        SaveData.IsEnabled = true;
            //    }
            //    else
            //    {
            //        MessageBox.Show("من فضلك أدخل المبلغ أولا");
            //        SaveData.IsEnabled = false;
            //    }

            //}
        }

        private void Diagnosis_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Diagnosis.ItemsSource = db.RunReader("select DIA_CODE,DIA_ANAME,DIA_ENAME from DIAGNOSIS where DIA_CODE like '%" + Diagnosis.Text + "%' or upper(DIA_ANAME) like '%" + Diagnosis.Text.ToUpper() + "%' or upper(DIA_ENAME) like '%" + Diagnosis.Text.ToUpper() + "%' and rownum<50").DefaultView;
                Diagnosis.IsDropDownOpen = true;
            }
        }
        private void Service_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
                Service.ItemsSource = db.RunReader("SELECT SERV_CODE, SERV_ANAME, SERV_ENAME FROM SERVICES WHERE (SERV_CODE like '%" + Service.Text + "%' OR UPPER(SERV_ANAME) like '%" + Service.Text.ToUpper() + "%' OR UPPER(SERV_ENAME) like '%" + Service.Text.ToUpper() + "%') AND SERV_CODE_H LIKE '11203%' AND ROWNUM < 100 ORDER BY SERV_ANAME").DefaultView;
                Service.IsDropDownOpen = true;
                //Service.ItemsSource = db.RunReader("select DIA_CODE,DIA_ANAME,DIA_ENAME from DIAGNOSIS where DIA_CODE like '%" + Diagnosis.Text + "%' or upper(DIA_ANAME) like '%" + Diagnosis.Text.ToUpper() + "%' or upper(DIA_ENAME) like '%" + Diagnosis.Text.ToUpper() + "%' and rownum<50").DefaultView;

            }
        }

        private void Midcation_Checked(object sender, RoutedEventArgs e)
        {
            SubService.ItemsSource = db.RunReader(@"SELECT M_CODE SERV_CODE, TRADE_NAME SERV_ENAME, NVL(GRUOP_TYPE, 'YES') STATUS  FROM DMS_TEST.MEDICINE_DATA 
                                                    LEFT OUTER JOIN MEDICINE_GRUOP ON MEDICINE_DATA.MED_GROUP = MEDICINE_GRUOP.gruop_id WHERE rownum < 100").DefaultView;
        }

        private void Ray_Checked(object sender, RoutedEventArgs e)
        {
            SubService.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H, 'YES' STATUS FROM SERVICES where rownum <= 50 and serv_code like '11201%'").DefaultView;
        }

        private void Lab_Checked(object sender, RoutedEventArgs e)
        {
            SubService.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H, 'YES' STATUS FROM SERVICES where rownum <= 50 and serv_code like '11206%'").DefaultView;
        }

        private void Physical_Checked(object sender, RoutedEventArgs e)
        {
            SubService.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H, 'YES' STATUS FROM SERVICES where rownum <= 50 and serv_code like '11204%'").DefaultView;          
        }

        private void SubService_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string serv = getservcode();
                if (serv != string.Empty && serv != "116")
                {
                    SubService.ItemsSource = db.RunReader("SELECT SERV_CODE, SERV_ANAME, SERV_ENAME, 'YES' STATUS FROM SERVICES WHERE (SERV_CODE like '%" + SubService.Text + "%' OR UPPER(SERV_ANAME) like '%" + SubService.Text.ToUpper() + "%' OR UPPER(SERV_ENAME) like '%" + SubService.Text.ToUpper() + "%') AND SERV_CODE LIKE '" + serv + "%' AND ROWNUM < 100 ORDER BY SERV_ANAME").DefaultView;
                    SubService.IsDropDownOpen = true;
                }
                else if (serv != string.Empty && serv == "116")
                {

                    SubService.ItemsSource = db.RunReader(@"SELECT M_CODE SERV_CODE, TRADE_NAME SERV_ENAME, NVL(GRUOP_TYPE, 'YES') STATUS FROM DMS_TEST.MEDICINE_DATA
                                                            LEFT OUTER JOIN MEDICINE_GRUOP ON MEDICINE_DATA.MED_GROUP = MEDICINE_GRUOP.gruop_id WHERE (M_CODE like '%" + SubService.Text + "%' OR UPPER(TRADE_NAME) like '%" + SubService.Text.ToUpper() + "%') AND rownum < 100").DefaultView;
                    SubService.IsDropDownOpen = true;
                    
                }
                //Service.ItemsSource = db.RunReader("select DIA_CODE,DIA_ANAME,DIA_ENAME from DIAGNOSIS where DIA_CODE like '%" + Diagnosis.Text + "%' or upper(DIA_ANAME) like '%" + Diagnosis.Text.ToUpper() + "%' or upper(DIA_ENAME) like '%" + Diagnosis.Text.ToUpper() + "%' and rownum<50").DefaultView;

            }
        }

        private void ExitHistoryScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HistoryScreen.Visibility = Visibility.Hidden;
        }
        void historydatacard()
        {
            DataTable dtdiaghist = new DataTable();
            DataTable dtservhist = new DataTable();
            DataTable dtsubservhist = new DataTable();

            dtdiaghist = db.RunReader(@"SELECT CODE, DIAG_CODE, DIAG_NAME FROM D_DOCTOR_DIAGNOSIS WHERE code = '" + NumberCheckup.Text + "' ");
            dtdiaghist = db.RunReader(@"SELECT CODE, SERV_CODE, SERV_NAME FROM D_DOCTOR_SERVICE2 WHERE code = '" + NumberCheckup.Text + "' ");
            dtsubservhist = db.RunReader(@"SELECT CODE, D_SERV_CODE, DECODE(D_SERV_CODE, '11201', 'أشعات', '11204', 'علاج طبيعي', '11206', 'تحاليل', '116', 'أدوية') NAME, SERV_CODE, SERV_NAME, STATUS, MED_NUMBER, MED_DURATION, ORGAN_NUMBER, SESSION_DURATION, NOTES, UPDATED_DATE  FROM D_DOCTOR_SERVICE WHERE code = '" + NumberCheckup.Text + "' ");
            string adtxt = "1- "; int flg = 2;
            if (dtdiaghist.Rows.Count > 0)
            {
                foreach (DataRow r in dtdiaghist.Rows)
                {
                    adtxt = adtxt + r[2].ToString() + " \n" + flg.ToString() + "- ";
                    flg++;
                }

                DiagnosisHistory.Document.Blocks.Clear();
                DiagnosisHistory.Document.Blocks.Add(new Paragraph(new Run(adtxt)));
            }
            else
                DiagnosisHistory.Document.Blocks.Clear();

            adtxt = "1- ";
            flg = 2;
            if (dtservhist.Rows.Count > 0)
            {
                foreach (DataRow r in dtservhist.Rows)
                {
                    adtxt = adtxt + r[2].ToString() + " \n" + flg.ToString() + "- ";
                    flg++;
                }

                ServiceHistory.Document.Blocks.Clear();
                ServiceHistory.Document.Blocks.Add(new Paragraph(new Run(adtxt)));
            }
            else
                ServiceHistory.Document.Blocks.Clear();


            SubServiceHistory.ItemsSource = dtsubservhist.DefaultView;
        }
        private void ShowHistoryScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HistoryScreen.Visibility = Visibility.Visible;
            historydatacard();
        }
        private void StackPanel_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            NotesScreen.Visibility = Visibility.Visible;
        }
        private void CancelNotes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NotesScreen.Visibility = Visibility.Hidden;
            NotesSubService.Text = "";
        }
        private void OkNotes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SubServiceHistory.SelectedItem != null)
            {
                DataRowView row = (DataRowView)SubServiceHistory.SelectedItem;
                db.RunNonQuery(@"UPDATE D_DOCTOR_SERVICE SET NOTES = '" + NotesSubService.Text + "', UPDATED_BY = '" + User.Name + "', UPDATED_DATE = SYSDATE WHERE CODE = '" + NumberCheckup.Text + "' AND D_SERV_CODE = '" + row[1].ToString() + "' AND SERV_CODE = '" + row[3].ToString() + "'");

                NotesSubService.Text = "";
                NotesScreen.Visibility = Visibility.Hidden;

                SubServiceHistory.ItemsSource = db.RunReader(@"SELECT CODE, D_SERV_CODE, DECODE(D_SERV_CODE, '11201', 'أشعات', '11204', 'علاج طبيعي', '11206', 'تحاليل', '116', 'أدوية') NAME, SERV_CODE, SERV_NAME, STATUS, MED_NUMBER, MED_DURATION, ORGAN_NUMBER, SESSION_DURATION, NOTES, UPDATED_DATE  FROM D_DOCTOR_SERVICE WHERE code = '" + NumberCheckup.Text + "' ").DefaultView;

            }
        }

        private void StackPanel_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            if (txtCard.Text != string.Empty)
            {
                DataTable dthist = new DataTable();
                dthist = db.RunReader(@"SELECT CLAIM_NO, CODE, COMP_ID, CARD_ID, CONTRACT_NO, CLASS_CODE, PROVIDER_CODE, PROVIDER_NAME, CREATED_BY, CREATED_DATE FROM D_DETECTION WHERE CARD_ID = '" + Patient.Card_NO + "' AND COMP_ID =  '" + Patient.Comp_id + "'AND SERV_CODE = 11205");

                if (dthist.Rows.Count > 0)
                {
                    DataHistory.ItemsSource = dthist.DefaultView;
                    HistoryScreen.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("لا توجد كشوفات سابقة لهذا المريض");
                    HistoryScreen.Visibility = Visibility.Hidden;
                }
            }
            else
                MessageBox.Show("من فضلك أدخل رقم الكارت أولا");
        }

        private void DataHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DataHistory.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataHistory.SelectedItem;
                NumberCheckup.Text = row[1].ToString();
                historydatacard();
            }
        }
    }
}


