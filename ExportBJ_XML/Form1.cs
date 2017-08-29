using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using System.IO;
using System.Security;
using System.Xml.Linq;

namespace ExportBJ_XML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //XmlWriter objXmlWriter = XmlTextWriter.Create(new BufferedStream(new FileStream(@"F:\bjvvv.xml", FileMode.Create, System.Security.AccessControl.FileSystemRights.Write, FileShare.None, 1024, FileOptions.SequentialScan)), new XmlWriterSettings { Encoding = Encoding.Unicode, Indent = true, CloseOutput = true });
            XmlWriter objXmlWriter = XmlTextWriter.Create(@"F:\bjvvv.xml");
            using (objXmlWriter)
            {

                XmlDocument main = new XmlDocument();
                XmlNode docNode = main.CreateXmlDeclaration("1.0", "UTF-8", null);
                main.AppendChild(docNode);
                docNode.WriteTo(objXmlWriter);

                XmlNode root = main.CreateElement("add");
                main.AppendChild(root);
                objXmlWriter.WriteStartElement("add");
                //root.WriteTo(objXmlWriter);

                XmlNode doc = main.CreateElement("doc");

                int MaxIDMAIN_BJVVV = GetMaxIDMAIN("BJVVV");
                //MaxIDMAIN_BJVVV = 10000;
                for (int i = 1; i < MaxIDMAIN_BJVVV; i += 1000)
                {
                    string q = GetQuery("BJVVV", i);
                    int check = CreateBJDocs("BJVVV", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");

                }

                int MaxIDMAIN_REDKOSTJ = GetMaxIDMAIN("REDKOSTJ");
                //MaxIDMAIN_REDKOSTJ = 10000;
                for (int i = 1; i < MaxIDMAIN_REDKOSTJ; i += 1000)
                {
                    string q = GetQuery("REDKOSTJ", i);
                    int check = CreateBJDocs("REDKOSTJ", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    
                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_ACC = GetMaxIDMAIN("BJACC");
                
                for (int i = 1; i < MaxIDMAIN_ACC; i += 1000)
                {
                    string q = GetQuery("BJACC", i);
                    int check = CreateBJDocs("BJACC", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");

                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_FCC = GetMaxIDMAIN("BJFCC");
                
                for (int i = 1; i < MaxIDMAIN_FCC; i += 1000)
                {
                    string q = GetQuery("BJFCC", i);
                    int check = CreateBJDocs("BJFCC", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");

                }
                /////////////////////////////////////////////////////////////////////////////////////////////
                int MaxIDMAIN_BRIT = GetMaxIDMAIN("BRIT_SOVET");

                for (int i = 1; i < MaxIDMAIN_BRIT; i += 1000)
                {
                    string q = GetQuery("BRIT_SOVET", i);
                    int check = CreateBJDocs("BJFCC", q, main, doc, root, objXmlWriter);
                    if (check == 1) continue;
                    //doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");

                }


                //main.Save(@"f:\bjvvv.xml");
                Close();
            }
        }
        private string GetQuery(string baza, int count)
        {
           return "select A.IDMAIN,cast(A.MNFIELD as nvarchar(10))+A.MSFIELD code,A.SORT,B.PLAIN " +
                                   " from "+baza+"..DATAEXT A" +
                                   " left join "+baza+"..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                   " where A.IDMAIN between " + count + " and " + (count + 999).ToString() +
                //" and exists (select 1 from BRIT_SOVET..DATAEXT C where A.IDMAIN = C.IDMAIN and C.MNFIELD = 899 and C.MSFIELD = '$p')" +
                                   " order by IDMAIN";
        }
        private int GetMaxIDMAIN(string p)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = "select max(ID) from "+p+"..MAIN";
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            return int.Parse(ds.Tables["t"].Rows[0][0].ToString());
        }
        public void AddField(XmlDocument main,XmlNode doc_, string name,string val)
        {
            
            XmlNode field = main.CreateElement("field");
            XmlAttribute att = main.CreateAttribute("name");
            att.Value = name;
            field.Attributes.Append(att);
            //field.InnerText = SecurityElement.Escape(val);
            if (val.Contains("Tendances dans l'economie mondiale;"))
            {
                field.InnerText = SecurityElement.Escape(val);
                //field.Value = SecurityElement.Escape(val);
                //val = XmlConvert.EncodeName(val);

            }
            //XElement xe = new XElement("dummy", val);
            //field.InnerText = xe.Value;
            //val = val.Replace("\\0", "").Replace("/","");
            field.InnerText = SecurityElement.Escape(val);
            //field.Value = SecurityElement.Escape(val);
            //val = XmlConvert.EncodeName(val);
            val = XmlCharacterWhitelist(val);
            field.InnerText = val;
            //XNode xn = 

            //field.InnerText = val.Replace("&", "").Replace("\v", "").Replace("#","");
            //XmlElement xe = new XmlElement();
            
            doc_.AppendChild(field);
        }
        public static string XmlCharacterWhitelist(string in_string)
        {
            if (in_string == null) return null;

            StringBuilder sbOutput = new StringBuilder();
            char ch;

            for (int i = 0; i < in_string.Length; i++)
            {
                ch = in_string[i];
                if ((ch >= 0x0020 && ch <= 0xD7FF) ||
                    (ch >= 0xE000 && ch <= 0xFFFD) ||
                    ch == 0x0009 ||
                    ch == 0x000A ||
                    ch == 0x000D)
                {
                    sbOutput.Append(ch);
                }
            }
            return sbOutput.ToString();
        }
        public int CreateBJDocs(string fund, string query,XmlDocument main,XmlNode doc,XmlNode root, XmlWriter objXmlWriter)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
            da.SelectCommand.CommandText = query;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            if (i == 0) return 1;
            string CurrentIDMAIN = ds.Tables["t"].Rows[0]["IDMAIN"].ToString();
            string allFields = "";
            foreach (DataRow r in ds.Tables["t"].Rows)
            {
                if (r["IDMAIN"].ToString() != CurrentIDMAIN)
                {
                    AddField(main, doc, "id", fund + "_" + CurrentIDMAIN);
                    AddField(main, doc, "fund", fund);
                    AddField(main, doc, "allfields", allFields);
                    allFields = "";
                    //root.AppendChild(doc);
                    doc.WriteTo(objXmlWriter);
                    doc = main.CreateElement("doc");
                    CurrentIDMAIN = r["IDMAIN"].ToString();
                }
                //else
                {
                    allFields += " " + r["PLAIN"].ToString();
                    switch (r["code"].ToString())
                    {
                        case "200$a":
                            AddField(main, doc, "title", r["PLAIN"].ToString());
                            break;
                        case "700$a":
                            AddField(main, doc, "author", r["PLAIN"].ToString());
                            AddField(main, doc, "author_sort", r["SORT"].ToString());
                            break;
                        case "701$a":
                            AddField(main, doc, "author2", r["PLAIN"].ToString());
                            break;
                        case "710$a":
                            AddField(main, doc, "author_corporate", r["PLAIN"].ToString());
                            break;
                        case "710$4":
                            AddField(main, doc, "author_corporate_role", r["PLAIN"].ToString());
                            break;
                        case "700$4":
                            AddField(main, doc, "author_role", r["PLAIN"].ToString());
                            break;
                        case "701$4":
                            AddField(main, doc, "author2_role", r["PLAIN"].ToString());
                            break;
                        case "330$a":
                            AddField(main, doc, "description", r["PLAIN"].ToString());
                            break;
                        case "205$a":
                            AddField(main, doc, "edition", r["PLAIN"].ToString());
                            break;
                        case "921$a":
                            AddField(main, doc, "format", r["PLAIN"].ToString());
                            break;
                        case "922$e":
                            AddField(main, doc, "genre", r["PLAIN"].ToString());
                            break;
                        case "10$a":
                            AddField(main, doc, "isbn", r["PLAIN"].ToString());
                            break;
                        case "11$a":
                            AddField(main, doc, "issn", r["PLAIN"].ToString());
                            break;
                        case "101$a":
                            AddField(main, doc, "language", r["PLAIN"].ToString());
                            break;
                        case "2100$d":
                            AddField(main, doc, "publishDate", r["PLAIN"].ToString());
                            break;
                        case "210$c":
                            AddField(main, doc, "publisher", r["PLAIN"].ToString());
                            break;
                        case "517$a":
                            AddField(main, doc, "title_alt", r["PLAIN"].ToString());
                            break;
                        case "210$a":
                            AddField(main, doc, "PlaceOfPublication", r["PLAIN"].ToString());
                            break;

                    }
                }
            }
            AddField(main, doc, "id", fund + "_" + CurrentIDMAIN);
            AddField(main, doc, "fund", fund);
            AddField(main, doc, "allfields", allFields);
            //root.AppendChild(doc);
            doc.WriteTo(objXmlWriter);
            return 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = "Tendances dans l'economie mondiale; &#x0; 23/ Conseil scientifique hongrois d'economie mondaile";
            XElement x = new XElement("bb",text);
            string xmlText = SecurityElement.Escape(text);
        }
    }
}
