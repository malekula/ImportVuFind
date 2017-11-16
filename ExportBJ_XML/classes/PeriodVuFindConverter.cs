using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Data;

namespace ExportBJ_XML.classes
{
    public class PeriodVuFindConverter : VuFindConverter
    {
        public override void Export()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////PERIOD/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            string allFields = "";
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\period.xml");
            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=PERIOD;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 120";
            DataSet ds = new DataSet();
            int cnt = da.Fill(ds, "t");
            DataTable val = new DataTable();
            foreach (DataRow row in ds.Tables["t"].Rows)
            {
                allFields = "";
                string pin = row["POLE"].ToString();
                if (pin == "")
                    continue;
                if ((Convert.ToInt32(row["POLE"]) < 2305) || (Convert.ToInt32(row["POLE"]) > 2308))
                {
                    //continue;
                }
                //title
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 121 and VVERH = " + row["IDZ"].ToString();
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("title", val.Rows[0]["POLE"].ToString());
                    AddField("title_short", val.Rows[0]["POLE"].ToString());
                    AddField("title_sort", val.Rows[0]["POLE"].ToString());
                }
                //вид издания
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 124 and VVERH = " + row["IDZ"].ToString();
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_ModeOfPublication", val.Rows[0]["POLE"].ToString());//добавить в индекс
                }
                //язык
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 128 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_Language", val.Rows[0]["POLE"].ToString());//добавить в индекс
                }
                //периодичность
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 130 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_Periodicity", val.Rows[0]["POLE"].ToString());//добавить в индекс
                }
                //года 
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 131 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    string result = "";
                    foreach (DataRow r in val.Rows)
                    {
                        result = result + r["POLE"].ToString() + "; ";

                    }
                    AddField("period_Years", result);//добавить в индекс
                }
                //гиперссылки 
                da.SelectCommand.CommandText = "select E.POLE " +
                                               "  from PERIOD..PI A " +
                                               "  left join PERIOD..PI B on A.IDZ = B.VVERH " +
                                               "  left join PERIOD..PI C on B.IDZ = C.VVERH " +
                                               "  left join PERIOD..PI D on C.IDZ = D.VVERH " +
                                               "  left join PERIOD..PI E on C.IDZ = E.VVERH " +
                                               "  where A.IDF = 120 and A.POLE = '" + row["POLE"] + "' " +
                                               " and D.IDF = 363  " +
                                               " and D.POLE = 'e-book' " +
                                               "  and E.IDF = 219";
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    string result = "";
                    foreach (DataRow r in val.Rows)
                    {
                        result = result + r["POLE"].ToString() + "; ";
                    }
                    AddField("period_HyperLink", result);//добавить в индекс
                }



                AddField("id", "period_" + row["POLE"]);
                AddField("fund", GetRusFund("period"));
                AddField("allfields", allFields);
                allFields = "";
                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");

                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = "period_" + row["POLE"];
                OnRecordExported(args);
            }
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }

        public override void ExportSingleRecord(int idmain)
        {
            throw new NotImplementedException();
        }
        public override void ExportCovers()
        {
            throw new NotImplementedException();
        }
    }
}
