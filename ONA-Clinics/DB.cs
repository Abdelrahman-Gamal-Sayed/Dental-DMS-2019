using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Windows;
namespace ONA_Clinics {
    class DB {//171.0.1.96 171.0.1.96 
        string connectionStr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)
                                            (HOST=**********)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                                            (SERVICE_NAME=ora11g)));User Id=app;Password=******";

        ////connection
        //OracleConnection conn = new OracleConnection(connectionStr);
        ////queries
        //public OracleCommand cmd = new OracleCommand();

        //public void SetCommand(string SQLStatement) {

        //    // cmd = new OracleCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = SQLStatement;
        //}

        public bool RunNonQuery(string SQLStatement, string Message = "")
        {
            //  Waiting waiting = new Waiting();
            //waiting.Show();

        
        OracleConnection conn = new OracleConnection(connectionStr);        
        OracleCommand cmd = new OracleCommand();


        bool test = false;
            try {
                //SetCommand(SQLStatement);

                cmd.Connection = conn;
                cmd.CommandText = SQLStatement;
                
                conn.Open();
                //bnfz el queries

                cmd.ExecuteNonQuery();
                if(Message != "") {
                    MessageBox.Show(Message);
                    test = true;
                }
                // waiting.Close();
                return test;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // waiting.Close();
                return test;
            }
            finally
            {
                conn.Dispose();
                conn.Close();
                // waiting.Close();
            }
        }

        public  DataTable RunReader(string Selectstatement)
        {
            //  Waiting waiting = new Waiting();
            //waiting.Show();
            // return data in tables

            OracleConnection conn = new OracleConnection(connectionStr);
            OracleCommand cmd = new OracleCommand();
            OracleDataAdapter da;


            DataTable tbl = new DataTable();
            //  SetCommand(Selectstatement);

            cmd.Connection = conn;
            cmd.CommandText = Selectstatement;

            conn.Open();
            
            try {
                //read from database

                //   tbl.Load(cmd.ExecuteReader());

                // waiting.Close();

                da = new OracleDataAdapter(cmd);
                da.Fill(tbl);

                return tbl;

            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.ToString());
                // waiting.Close();
                return tbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // waiting.Close();
                return tbl;
            }
            finally
            {
             //   cmd.Dispose();
                conn.Dispose();
                conn.Close();
                //OracleConnection.ClearAllPools();
            }
        }



    }
}
