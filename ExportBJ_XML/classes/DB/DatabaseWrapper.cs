﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ExportBJ_XML.QueriesText;

namespace ExportBJ_XML.classes.DB
{
    class DatabaseWrapper
    {

        public static string Fund { get; set; }
        public DatabaseWrapper(string fund)
        {
            Fund = fund;
        }

        private DataTable ExecuteSelectQuery(SqlDataAdapter da)
        {
            DataSet ds = new DataSet();
            while (true)
            {
                try
                {
                    da.Fill(ds, "t");
                    break;
                }
                catch (SqlException ex)
                {
                    if (ex.Number != -2) throw;//таймаут подключения. 

                    //это событиями переделать
                    //VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                    //args.RecordId = _lastID.ToString();

                    Thread.Sleep(5000);
                    continue;
                }
            }
            return ds.Tables[0];
        }

        internal DataTable GetBJRecord(int idmain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.SELECT_RECORD_QUERY, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int ).Value = idmain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_10a(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.IMPORT_CLARIFY_10a, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_517a(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.IMPORT_CLARIFY_517a, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_205a_1(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.IMPORT_CLARIFY_205a_1, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_205a_2(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.IMPORT_CLARIFY_205a_2, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_205a_3(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.IMPORT_CLARIFY_205a_3, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_606a(int idchain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.IMPORT_CLARIFY_606a, connection);
                dataAdapter.SelectCommand.Parameters.Add("idchain", SqlDbType.Int).Value = idchain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetAllExemplars(int idmain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_ALL_EXEMPLARS, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = idmain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetExemplar(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_EXEMPLAR, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetHyperLink(int IDMAIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_HYPERLINK, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetBookScanInfo(int IDMAIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_BOOK_SCAN_INFO, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                dataAdapter.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
                if (DatabaseWrapper.Fund == "BJVVV")
                {
                    dataAdapter.SelectCommand.Parameters["idbase"].Value = 1;
                }
                else if (DatabaseWrapper.Fund == "REDKOSTJ")
                {
                    dataAdapter.SelectCommand.Parameters["idbase"].Value = 2;
                }
                else
                {
                    dataAdapter.SelectCommand.Parameters["idbase"].Value = 0;
                }
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetAllIdmainWithImages()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_ALL_IDMAIN_WITH_IMAGES, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetImage(int idmain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_IMAGE, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = idmain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetParentIDMAIN(int ParentPIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_PARENT_PIN, connection);
                dataAdapter.SelectCommand.Parameters.Add("ParentPIN", SqlDbType.Int).Value = ParentPIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetMaxIDMAIN()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(QueriesText.Bibliojet.GET_MAX_IDMAIN, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }
    }
}