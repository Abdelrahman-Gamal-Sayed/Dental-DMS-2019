using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Windows;


namespace ONA_Clinics
{
    class DB

    {//171.0.1.96 171.0.1.96 
        public static string connectionStr = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)
                                            (HOST=171.0.1.96)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)
                                            (SERVICE_NAME=ora11g)));User Id=app;Password=12369";


        //connection
        OracleConnection conn = new OracleConnection(connectionStr);
        //queries
        public OracleCommand cmd = new OracleCommand();

        public void SetCommand(string SQLStatement)
        {

            // cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = SQLStatement;
        }

        public bool RunNonQuery(string SQLStatement, string Message = "")
        {
            //  Waiting waiting = new Waiting();
            //waiting.Show();
            bool test = false;
            try
            {
                SetCommand(SQLStatement);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Close();
                conn.Open();
                //bnfz el queries

                cmd.ExecuteNonQuery();
                if (Message != "")
                {
                    MessageBox.Show(Message);
                    test = true;
                }
                // waiting.Close();
                return test;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
                // waiting.Close();
                return test;
            }
            finally

            {
                conn.Close();
                // waiting.Close();
            }
        }
        public bool RunNonQuery2(string SQLStatement, string Message = "")
        {
            bool test = false;
            try
            {
                SetCommand(SQLStatement);
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Open();
                }
                else if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                conn.Close();
                conn.Open();
                //bnfz el queries

                cmd.ExecuteNonQuery();
                if (Message != "")
                {
                    MessageBox.Show(Message);
                    test = true;
                }
                return test;
            }
            catch
            {
                conn.Close();
                return test;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable RunReader(string Selectstatement)
        {
            //  Waiting waiting = new Waiting();
            //waiting.Show();
            // return data in tables
            DataTable tbl = new DataTable();
            SetCommand(Selectstatement);
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Open();
            }
            else if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            try
            {
                //read from database

                tbl.Load(cmd.ExecuteReader());

                conn.Close();
                // waiting.Close();
                return tbl;

            }
            catch (OracleException ex)
            {

                MessageBox.Show(ex.ToString());
                conn.Close();
                // waiting.Close();
                return tbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
                // waiting.Close();
                return tbl;
            }
            finally { conn.Close(); }
        }
    }
}
