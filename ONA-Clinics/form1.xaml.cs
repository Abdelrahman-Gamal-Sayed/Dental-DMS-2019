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
    /// Interaction logic for form1.xaml
    /// </summary>
    public partial class form1 : Window
    {
        DB db = new DB();
        DataDB tdb = new DataDB();

        public form1()
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


                        //    DataTable dtservok = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT from DMS_TEST.COMP_CUSTOMIZED_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO+ "' and CLASS_CODE='" + Patient.Class_Code + "' and D_SERV_CODE= 111");

                        //if(dtservok.Rows.Count > 0)
                        //{
                        ServiceType.IsEnabled = true;
                        ServiceType.ItemsSource = db.RunReader("SELECT SERV_CODE, SERV_ENAME FROM SERVICES WHERE SERV_CODE IN('111', '112', '11104')").DefaultView;
                        //}
                        //else
                        //{
                        //    ServiceType.Text = "";
                        //    ServiceType.IsEnabled = false;
                        //}



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
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();

        }

        private void ServiceType_DropDownClosed(object sender, EventArgs e)
        {
            if (ServiceType.Text == "111")
            {
                User.Provider_Code = "7";
                DataTable dtgetaprov = new DataTable();
                dtgetaprov = db.RunReader(@"SELECT CODE, REPLAY, VALUE_AFTER FROM MEDICAL_APPROVALS WHERE CARD_NO = '" + Patient.Card_NO + "' and COMP_CONTRACT_NO = '" + Patient.Contract_NO + "' AND CLASS_CODE = '" + Patient.Class_Code + "' AND PROVIDER_ID = '" + User.Provider_Code + "' AND (SYSDATE - TO_DATE(CREATED_DATE)) < 7  AND SERVECE_TYP = 'Inpatient'");

                if(dtgetaprov.Rows.Count > 0)
                {
                    DataApproval.ItemsSource = dtgetaprov.DefaultView;
                    DataApproval.Visibility = Visibility.Visible;
                }
                else
                {
                    DataApproval.Visibility = Visibility.Hidden;
                    MessageBox.Show("لا توجد موافقات للكارت");
                }                
                ScreenApprovalNo.Visibility = Visibility.Visible;
                ScreenSubServiceType.Visibility = Visibility.Hidden;
                AcceptApproval.Visibility = Visibility.Hidden;
                RejectApproval.Visibility = Visibility.Hidden;
                
            }
            else if (ServiceType.Text == "112")
            {
                ScreenApprovalNo.Visibility = Visibility.Hidden;
                ScreenSubServiceType.Visibility = Visibility.Visible;
                DataApproval.Visibility = Visibility.Hidden;
                SubServiceType.ItemsSource = db.RunReader(@"select GROUP_NO,GROUP_ANAME, GROUP_ENAME from SERVICES_GROUP where SUPER_GROUP_CODE= 112 order by GROUP_NO").DefaultView;
            }
        }

        private void ApprovalNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (ApprovalNo.Text != string.Empty)
            {
                DataTable dtaproval = db.RunReader(@"SELECT CODE, VALUE_AFTER FROM MEDICAL_APPROVALS WHERE CODE = '" + ApprovalNo.Text + "' ");
                if (dtaproval.Rows.Count > 0)
                {
                    ValueApproval.Text = dtaproval.Rows[0][1].ToString();
                    AcceptApproval.Visibility = Visibility.Visible;
                    RejectApproval.Visibility = Visibility.Hidden;
                    MainScreen.IsEnabled = true;

                }
                else
                {
                    RejectApproval.Visibility = Visibility.Visible;
                    AcceptApproval.Visibility = Visibility.Hidden;
                    ValueApproval.Text = "";
                    MainScreen.IsEnabled = false;

                }
            }
        }

        private void PrintApproval_Click(object sender, RoutedEventArgs e)
        {
            string code = ApprovalNo.Text;

            string serv = "", diag = "";

            ReportInPatient.Visibility = Visibility.Visible;
            BillOutPatient.Visibility = Visibility.Hidden;

            DataTable dtsrev = db.RunReader(@"select    APPROVAL_SUB_SERV.S_SERV_NAME, CASE WHEN (S_SERV_NAME = 'الاقامة'OR S_SERV_CODE LIKE '114%') 
                                                                        THEN CONCAT(APPROVAL_SUB_SERV.DETAILS, APPROVAL_SUB_SERV.DISCRIPTION) ELSE APPROVAL_SUB_SERV.DETAILS END
                                                             FROM      APPROVAL_SUB_SERV  WHERE    APPROVAL_SUB_SERV.CODE = '" + code + "'");

            DataTable dtdiag = db.RunReader(@"select    MEDICAL_APPROVALS.APROVAL_IMAG, APPROVAL_DIAG.DIAG_NAME FROM      MEDICAL_APPROVALS, APPROVAL_DIAG
                                                              WHERE     MEDICAL_APPROVALS.CODE = APPROVAL_DIAG.CODE  AND MEDICAL_APPROVALS.CODE = '" + code + "' ");

            if (dtsrev.Rows.Count != 0)
            {
                for (int i = 0; i < dtsrev.Rows.Count; i++)
                    serv = serv + dtsrev.Rows[i][0].ToString() + " : " + dtsrev.Rows[i][1].ToString() + "\n";
            }
            if (dtdiag.Rows.Count != 0)
            {
                for (int i = 0; i < dtdiag.Rows.Count; i++)
                    diag = diag + dtdiag.Rows[i][1].ToString() + "\n";

            }

            ReportApproval repo = new ReportApproval();

            repo.SetDatabaseLogon("APP", "12369");

            repo.SetParameterValue("cod", code);
            repo.SetParameterValue("crd", Patient.Card_NO);
            repo.SetParameterValue("diag", diag);
            repo.SetParameterValue("service", serv);

            //crystalReportsViewer1.ViewerCore.ReportSource = repo;
            //crystalReportsViewer1.Visibility = Visibility.Visible;

            db.RunNonQuery(@"INSERT INTO APPROVAL_PRINT (APPROVAL_CODE, PRINTED_BY, PRINTED_DATE) VALUES ('" + code + "' , '" + User.Name + "' , SYSDATE)");

            //if (MessageBox.Show("Do you want save report to pdf file", "Save pdf file", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    ExportOptions exp = new ExportOptions();
            //    DiskFileDestinationOptions dis = new DiskFileDestinationOptions();

            //    PdfFormatOptions expdf = new PdfFormatOptions();
            //    string sa = "";

            //    SaveFileDialog sfd = new SaveFileDialog();
            //    sfd.Filter = "Pdf file|*.pdf";
            //    sfd.FileName ="";

            //    if (sfd.ShowDialog() == true)
            //        sa = sfd.FileName;

            //    dis.DiskFileName = sa;

            //    exp = repo.ExportOptions;
            //    exp.ExportDestinationType = ExportDestinationType.DiskFile;
            //    exp.ExportFormatType = ExportFormatType.PortableDocFormat;
            //    exp.ExportFormatOptions = expdf;
            //    exp.ExportDestinationOptions = dis;
            //    repo.Export();

            //    MessageBox.Show("Successfull Export to Pdf");
            //}
            //else
            //    MessageBox.Show("Thank you");
        }
        private void NumberOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            e.Handled = reg.IsMatch(e.Text);

        }
        void calculatepercent()
        {
            DataTable dtperc = new DataTable();
            dtperc = db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT, SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D_EMP where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='" + SubServiceType.Text + "' AND CARD_ID = '" + Patient.Card_NO + "'");
            if (dtperc.Rows.Count == 0)
                db.RunReader("SELECT CEILING_PERT,CEILING_AMT, CARR_AMT, SER_SERV from DMS_TEST.COMP_CUSTOMIZED_D_D where C_COMP_ID='" + Patient.Comp_id + "' and CONTRACT_NO='" + Patient.Contract_NO + "' and CLASS_CODE='" + Patient.Class_Code + "' and SER_SERV='" + SubServiceType.Text + "'");

            if (dtperc.Rows.Count != 0)
                Patient.PercServ = dtperc.Rows[0][0].ToString();
        }
        private void SubServiceType_DropDownClosed(object sender, EventArgs e)
        {
            if (SubServiceType.Text == "11201")
            {
                SubSubServiceType.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H FROM v_services where rownum <= 50 and serv_code like '11201%'").DefaultView;
                //   calculatepercent();
                SecreenSubSubService.Visibility = Visibility.Visible;
                DataSubService.Visibility = Visibility.Visible;

                ReportInPatient.Visibility = Visibility.Hidden;
                BillOutPatient.Visibility = Visibility.Visible;
            }
            else if (SubServiceType.Text == "11206")
            {
                SubSubServiceType.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H FROM v_services where rownum <= 50 and serv_code like '11206%'").DefaultView;
                //  calculatepercent();
                SecreenSubSubService.Visibility = Visibility.Visible;
                DataSubService.Visibility = Visibility.Visible;

                ReportInPatient.Visibility = Visibility.Hidden;
                BillOutPatient.Visibility = Visibility.Visible;
            }
            else if (SubServiceType.Text == "11204")
            {
                SubSubServiceType.ItemsSource = db.RunReader("select SERV_CODE, SERV_ANAME, SERV_ENAME, SERV_CODE_H FROM v_services where rownum <= 50 and serv_code like '11204%'").DefaultView;
                //     calculatepercent();
                SecreenSubSubService.Visibility = Visibility.Visible;
                DataSubService.Visibility = Visibility.Visible;

                ReportInPatient.Visibility = Visibility.Hidden;
                BillOutPatient.Visibility = Visibility.Visible;
            }
            else
            {
                SecreenSubSubService.Visibility = Visibility.Hidden;
                DataSubService.Visibility = Visibility.Hidden;
            }
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
        private void AddSubServDetails_Click(object sender, RoutedEventArgs e)
        {
            if (SubServiceType.Text != string.Empty && SubSubServiceType.Text != string.Empty && ValueSubService.Text != string.Empty)
            {
                setsubservdata();
                DataRow dr = dtsrvdetials.NewRow();
                DataRowView row = (DataRowView)SubServiceType.SelectedItem;

                dr[0] = row[0].ToString();
                dr[1] = row[1].ToString();
                dr[2] = SubSubServiceType.Text;
                dr[3] = Convert.ToDouble(ValueSubService.Text);
                // dr[4] = Convert.ToDouble(ValueSubService.Text) - (Convert.ToDouble(ValueSubService.Text) * Convert.ToDouble(Patient.PercServ) / 100);

                dtsrvdetials.Rows.Add(dr);
                DataSubService.ItemsSource = dtsrvdetials.DefaultView;

                SubSubServiceType.Text = "";
                ValueSubService.Text = "";
            }
            else
                MessageBox.Show("من فضلك أكمل البيانات");
        }
        private void PackIcon_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (DataSubService.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataSubService.SelectedItem;

                SubServiceType.Text = row[0].ToString();
                SubSubServiceType.Text = row[2].ToString();
                ValueSubService.Text = row[3].ToString();

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


                        Patient.CardDegree = "1";
                        User.Degree_Code = "1";

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
                    Patient.FinalAmt = finalamt.ToString();
                    FinalAmount.Text = finalamt.ToString();
                    FinalAmountDms.Text = finalamtdms.ToString();
                    DataBillDetails.ItemsSource = dtbill.DefaultView;
                    SaveBill.Visibility = Visibility.Visible;
                    
                }

                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
            else
                MessageBox.Show("لم تقم بإختيار شيء");
        }
        private void SaveBill_Click(object sender, RoutedEventArgs e)
        {
            if(FinalAmountDms.Text != string.Empty && FinalAmount.Text != string.Empty)
            {
                



                SaveBill.Visibility = Visibility.Hidden;
            }


            
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
            ServiceType.Text = "";
            clearscreen();
        }
        void clearscreen()
        {
            ScreenApprovalNo.Visibility = Visibility.Hidden;
            ScreenSubServiceType.Visibility = Visibility.Hidden;
            RejectApproval.Visibility = Visibility.Hidden;
            AcceptApproval.Visibility = Visibility.Hidden;
            ValueApproval.Text = "";
            MainScreen.IsEnabled = true;
            SubSubServiceType.Text = "";
            ValueSubService.Text = "";
            SecreenSubSubService.Visibility = Visibility.Hidden;
            DataSubService.Visibility = Visibility.Hidden;
            DataSubService.ItemsSource = null;
            DataBillDetails.ItemsSource = null;
            DataApproval.Visibility = Visibility.Hidden;
            DataApproval.ItemsSource = null;

            if (dtbill != null)
                dtbill.Clear();

            if (dtsrvdetials != null)
                dtsrvdetials.Clear();

            SaveBill.Visibility = Visibility.Hidden;
            FinalAmount.Text = "";
            BillOutPatient.Visibility = Visibility.Hidden;
            ReportInPatient.Visibility = Visibility.Hidden;
            DataGetBill.Visibility = Visibility.Hidden;

            if (billgetype != null)
                billgetype.Clear();

            ScreenGetBill.Visibility = Visibility.Hidden;
        }
        DataTable billgetype;
        void setbillgetype()
        {
            billgetype = new DataTable();
            billgetype.Columns.Add("TYPE");
            billgetype.Columns.Add("VALUE", typeof(double));
        }
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearscreen();
            ScreenGetBill.Visibility = Visibility.Visible;
            DataGetBill.Visibility = Visibility.Visible;

        }
        private void AddGetBillDetails_Click(object sender, RoutedEventArgs e)
        {
            if (billgetype == null)
                setbillgetype();

            DataRow dr = billgetype.NewRow();

            dr[0] = GetBillType.Text;
            dr[1] = double.Parse(ValueGetBillType.Text);

            billgetype.Rows.Add(dr);
            DataGetBill.ItemsSource = billgetype.DefaultView;

        }

        private void DataApproval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataApproval.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DataApproval.SelectedItem;
                ValueApproval.Text = row[2].ToString();
                ApprovalNo.Text = row[0].ToString();
                AcceptApproval.Visibility = Visibility.Visible;

                if (row[1].ToString() == "Y")
                {
                    ShowReplyApproval.Content = "تمت الموافقة";
                    SaveNoApproval.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("تم رفض الموافقة");
                    ShowReplyApproval.Content = "تم رفض الموافقة";
                    SaveNoApproval.Visibility = Visibility.Hidden;
                }

                //    if (row[1].ToString() == "Y")
                //{
                ////    ValueApproval.Text = dtaproval.Rows[0][1].ToString();
                //    AcceptApproval.Visibility = Visibility.Visible;
                //    RejectApproval.Visibility = Visibility.Hidden;
                //    MainScreen.IsEnabled = true;

                //}
                //else
                //{
                //    RejectApproval.Visibility = Visibility.Visible;
                //    AcceptApproval.Visibility = Visibility.Hidden;
                //    ValueApproval.Text = "";
                //    MainScreen.IsEnabled = false;

                //}



            }
        }

        private void SaveNoApproval_Click(object sender, RoutedEventArgs e)
        {
            if (ApprovalNo.Text != string.Empty)
            {
                DataTable dtaprov = new DataTable();

                dtaprov = db.RunReader(@"SELECT CODE FROM D_DETECTION WHERE APPROVAL_NO = '" + ApprovalNo.Text + "'");

                if (dtaprov.Rows.Count > 0)
                    MessageBox.Show("تم حفظ هذه الموافقة من قبل");
                else
                {
                    int flg;
                    flg = tdb.saveapproval(ApprovalNo.Text, ValueApproval.Text);
                    if (flg == 1)
                    {
                        MessageBox.Show("تم الحفظ بنجاح");
                        ValueApproval.Text = "";
                        ApprovalNo.Text = "";
                        AcceptApproval.Visibility = Visibility.Hidden;
                        DataApproval.ItemsSource = null;
                        DataApproval.Visibility = Visibility.Hidden;
                    }
                    else
                        MessageBox.Show("حدثت مشكلة أثناء الحفظ");
                }
            }
        }
    }
}
