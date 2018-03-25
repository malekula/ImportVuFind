﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Drawing;
using ExportBJ_XML.classes.BJ;
using ExportBJ_XML.ValueObjects;
using ExportBJ_XML.classes.DB;

namespace ExportBJ_XML.classes
{
    public class BJVuFindConverter : VuFindConverter
    {

        public BJVuFindConverter(string fund)
        {
            this.Fund = fund;
            DatabaseWrapper.Fund = fund;
        }

        private int _lastID = 1;


        public override void Export()
        {

            _objXmlWriter = XmlTextWriter.Create(@"F:\import\"+Fund.ToLower()+".xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////BJVVV/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1;
            this.StartExportFrom( _lastID );
        }

        private void StartExportFrom( int previous )
        {
            int step = 1;
            int MaxIDMAIN = GetMaxIDMAIN();
            List<string> errors = new List<string>();
            for (int i = previous; i < MaxIDMAIN; i += step)
            {
                _lastID = i;
                DatabaseWrapper db = new DatabaseWrapper(this.Fund);
                DataTable record = db.GetBJRecord(_lastID);
                if (record.Rows.Count == 0) continue;
                try
                {
                    int check = CreateBJDoc( record );
                    if (check == 1) continue;
                }
                catch (Exception ex)
                {
                    //errors.Add(this.Fund + "_" + i);
                    //_doc = _exportDocument.CreateElement("doc");
                    continue;
                }

                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");

                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = this.Fund + "_" + i;
                OnRecordExported(args);
            }

            _objXmlWriter.Flush();
            _objXmlWriter.Close();
            File.WriteAllLines(@"f:\import\importErrors\" + this.Fund + "Errors.txt", errors.ToArray());

        }
        public override void ExportSingleRecord( int idmain )
        {
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\singleRecords\" + this.Fund + "_" + idmain + ".xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////TEST/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            //DB.DatabaseWrapper( int.Parse(idmain) );
            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);
            DataTable table = dbWrapper.GetBJRecord(idmain);
            int check = CreateBJDoc( table );
            if (check == 1) return;
            _doc.WriteTo(_objXmlWriter);
            _doc = _exportDocument.CreateElement("doc");
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }

        private int CreateBJDoc( DataTable BJBook )
        {
            int currentIDMAIN = (int)BJBook.Rows[0]["IDMAIN"];
            string level = BJBook.Rows[0]["Level"].ToString();
            string level_id = BJBook.Rows[0]["level_id"].ToString();
            int lev_id = int.Parse(level_id);
            if (lev_id < 0) return 1;
            string allFields = "";
            AuthoritativeFile AF_all = new AuthoritativeFile();
            bool wasTitle = false;//встречается ошибка: два заглавия в одном пине
            bool wasAuthor = false;//был ли автор. если был, то сортировочное поле уже заполнено
            string description = "";//все 3хх поля
            DataTable clarify;
            string query = "";
            string Annotation = "";
            string CarrierCode = "";
            //BJBookInfo book = new BJBookInfo();
            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);
            foreach (DataRow r in BJBook.Rows)
            {

                allFields += " " + r["PLAIN"].ToString();
                switch (r["code"].ToString())
                {
                    //=======================================================================Родные поля вуфайнд=======================
                    case "200$a":
                        if (wasTitle) break;
                        AddField("title", r["PLAIN"].ToString());
                        AddField("title_short", r["PLAIN"].ToString());
                        AddField("title_sort", r["SORT"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        wasTitle = true;
                        break;
                    case "700$a":
                        AddField("author", r["PLAIN"].ToString());
                        if (!wasAuthor)
                        {
                            AddField("author_sort", r["SORT"].ToString());
                        }
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        wasAuthor = true;
                        //забрать все варианты написания автора из авторитетного файла и вставить в скрытое, но поисковое поле
                        break;
                    case "701$a":
                        AddField("author2", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "710$a":
                        AddField("author_corporate", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "710$4":
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        AddField("author_corporate_role", r["PLAIN"].ToString());
                        break;
                    case "700$4":
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        AddField("author_role", r["PLAIN"].ToString());
                        break;
                    case "701$4":
                        AddField("author2_role", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "921$a":
                        CarrierCode = GetCarrierCode(r["PLAIN"].ToString());
                        AddField("format", CarrierCode);
                       // book.Fields.AddField(CarrierCode, (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "922$e":
                        AddField("genre", r["PLAIN"].ToString());
                        AddField("genre_facet", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "10$a":
                        clarify = dbWrapper.Clarify_10a((int)r["IDDATA"]);
                        string add = r["PLAIN"].ToString();
                        if (clarify.Rows.Count != 0)
                        {
                            add = r["PLAIN"].ToString() + " (" + clarify.Rows[0]["PLAIN"].ToString() + ")";
                        }
                        AddField("isbn", add);
                        //book.Fields.AddField(add, (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "11$a":
                        AddField("issn", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "101$a":
                        query = " select NAME from " + this.Fund + "..LIST_1 " + " where ID = " + r["IDINLIST"].ToString();
                        clarify = dbWrapper.Clarify_10a((int)r["IDDATA"]);
                        if (clarify.Rows.Count == 0)
                        {
                            AddField("language", r["PLAIN"].ToString());
                         //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        }
                        else
                        {
                            AddField("language", clarify.Rows[0]["NAME"].ToString());
                          //  book.Fields.AddField(clarify.Rows[0]["NAME"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        }
                        break;
                    case "2100$d":
                        AddField("publishDate", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "210$c":
                        AddField("publisher", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "517$a":
                        clarify = dbWrapper.Clarify_517a((int)r["IDDATA"]);
                        string fieldValue;
                        fieldValue = (clarify.Rows.Count != 0) ?
                            "(" + clarify.Rows[0]["PLAIN"].ToString() + ")" + r["PLAIN"].ToString() :
                            r["PLAIN"].ToString();
                        AddField("title_alt", fieldValue);
                        //book.Fields.AddField(fieldValue, (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        //нужно специальным образом обрабатывать
                        break;
                    //=======================================================================добавленные в индекс=======================
                    case "210$a":
                        AddField("PlaceOfPublication", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$6":
                        AddField("Title_another_chart", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$b":
                        AddField("Title_same_author", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$d":
                        AddField("Parallel_title", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$e":
                        AddField("Info_pertaining_title", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$f":
                        AddField("Responsibility_statement", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$h":
                        AddField("Part_number", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$i":
                        AddField("Part_title", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "200$z":
                        AddField("Language_title_alt", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "500$a":
                        AddField("Title_unified", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "500$3":
                        AF_all = GetAFAll( (int)r["AFLINKID"], "AFHEADERVAR");
                        AddField("Title_unified", AF_all.ToString());
                       // book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "517$e":
                        AddField("Info_title_alt", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "517$z":
                        AddField("Language_title_alt", r["PLAIN"].ToString());
                        break;
                    case "700$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        foreach (string av in AF_all.AFValues)
                        {
                            AddField("author_variant", av);//хранить но не отображать
                        }
                       // book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "701$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        AddField("Another_author_AF_all", AF_all.ToString());//хранить но не отображать
                       // book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "501$a":
                        AddField("Another_title", r["PLAIN"].ToString());
                        //book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "501$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFHEADERVAR");
                        AddField("Another_title_AF_All", AF_all.ToString());
                        //book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "503$a":
                        AddField("Unified_Caption", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "503$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFHEADERVAR");
                        AddField("Unified_Caption_AF_All", AF_all.ToString());
                      //  book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "700$6":
                        AddField("Author_another_chart", r["PLAIN"].ToString());
                      //  book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "702$a":
                        AddField("Editor", r["PLAIN"].ToString());
                     //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "702$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        AddField("Editor_AF_all", AF_all.ToString());
                       // book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "702$4":
                        AddField("Editor_role", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "710$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        AddField("Collective_author_all", AF_all.ToString());
                       // book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "710$9":
                        AddField("Organization_nature", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "11$9":
                        AddField("Printing", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "205$a":
                        string PublicationInfo = r["PLAIN"].ToString();

                        // 205$b
                         
                        clarify = dbWrapper.Clarify_205a_1((int)r["IDDATA"]);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        // 205$f
                        clarify = dbWrapper.Clarify_205a_2((int)r["IDDATA"]);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += " / " + rr["PLAIN"].ToString();
                        }
                        // 205$g
                        clarify = dbWrapper.Clarify_205a_3((int)r["IDDATA"]);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        AddField("Publication_info", PublicationInfo);
                      //  book.Fields.AddField(PublicationInfo, (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "921$b":
                        AddField("EditionType", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "102$a":
                        AddField("Country", r["PLAIN"].ToString());
                      //  book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "210$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        AddField("PlaceOfPublication_AF_All", AF_all.ToString());
                      //  book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "2110$g":
                        AddField("PrintingHouse", r["PLAIN"].ToString());
                       // book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "2110$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        AddField("PrintingHouse_AF_All", AF_all.ToString());
                       // book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "2111$e":
                        AddField("GeoNamePlaceOfPublication", r["PLAIN"].ToString());
                     //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "2111$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFGEOVAR");
                        AddField("GeoNamePlaceOfPublication_AF_All", AF_all.ToString());
                     //   book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "10$z":
                        AddField("IncorrectISBN", r["PLAIN"].ToString());
                     //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "11$z":
                        AddField("IncorrectISSN", r["PLAIN"].ToString());
                      //  book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "11$y":
                        AddField("CanceledISSN", r["PLAIN"].ToString());
                      //  book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "101$b":
                        AddField("IntermediateTranslateLanguage", r["PLAIN"].ToString());
                     //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "101$d":
                        AddField("SummaryLanguage", r["PLAIN"].ToString());
                     //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "101$e":
                        AddField("TableOfContentsLanguage", r["PLAIN"].ToString());
                     //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "101$f":
                        AddField("TitlePageLanguage", r["PLAIN"].ToString());
                     //   book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "101$g":
                        AddField("BasicTitleLanguage", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "101$i":
                        AddField("AccompayingMaterialLanguage", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "215$a":
                        AddField("Volume", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "215$b":
                        AddField("Illustrations", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "215$c":
                        AddField("Dimensions", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "215$d":
                        AddField("AccompayingMaterial", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "225$a":
                        if (r["PLAIN"].ToString() == "") break;
                        if (r["PLAIN"].ToString() == "-1") break;
                        //AddHierarchyFields(Convert.ToInt32(r["PLAIN"]), Convert.ToInt32(r["IDMAIN"]));
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "225$h":
                        AddField("NumberInSeries", r["PLAIN"].ToString());
                  //      book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "225$v":
                        AddField("NumberInSubseries", r["PLAIN"].ToString());
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "300$a":
                    case "301$a":
                    case "316$a":
                    case "320$a":
                    case "326$a":
                    case "336$a":
                    case "337$a":
                        description += r["PLAIN"].ToString() + " ; ";
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "327$a":
                    case "330$a":
                        Annotation += r["PLAIN"].ToString() + " ; ";
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "830$a":
                        AddField("CatalogerNote", r["PLAIN"].ToString());
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "831$a":
                        AddField("DirectoryNote", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "924$a":
                        AddField("AdditionalBibRecord", r["PLAIN"].ToString());
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "940$a":
                        AddField("HyperLink", r["PLAIN"].ToString());
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "606$a"://"""""" • """"""
                        clarify = dbWrapper.Clarify_606a(Convert.ToInt32(r["SORT"]));
                        if (clarify.Rows.Count == 0) break;
                        string TPR = "";
                        foreach (DataRow rr in clarify.Rows)
                        {
                            TPR += rr["VALUE"].ToString() + " • ";
                        }
                        TPR = TPR.Substring(0, TPR.Length - 2);
                        AddField("topic", TPR);
                        AddField("topic_facet", TPR);
                   //     book.Fields.AddField(TPR, (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "3000$a":
                        AddField("OwnerPerson", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "3000$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        AddField("OwnerPerson_AF_All", AF_all.ToString());
                   //     book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "3001$a":
                        AddField("OwnerOrganization", r["PLAIN"].ToString());
                   //     book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "3001$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        AddField("OwnerOrganization_AF_All", AF_all.ToString());
                    //    book.Fields.AddField(r["AFLINKID"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString(), AF_all);
                        break;
                    case "3002$a":
                        AddField("Ownership", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "3003$a":
                        AddField("OwnerExemplar", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                    case "3200$a":
                        AddField("IllustrationMaterial", r["PLAIN"].ToString());
                    //    book.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                        break;
                }

            }
            AddField("id", this.Fund + "_" + currentIDMAIN);
           // book.ID = this.Fund + "_" + currentIDMAIN;
            string rusFund = GetFundId(this.Fund);

            AddField("fund", rusFund);
            AddField("allfields", allFields);
            AddField("Level", level);
            AddField("Level_id", level_id);
            AddField("Annotation", Annotation);

            if (description != "")
            {
                AddField("description", description);
            }

            AddExemplarFields(currentIDMAIN, _exportDocument, this.Fund);

            return 0;
        }


        private void AddExemplarFields(int idmain, XmlDocument _exportDocument, string fund)
        {

            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);
            DataTable table = dbWrapper.GetAllExemplars(idmain);
            if (table.Rows.Count == 0) return;
            int IDMAIN = (int)table.Rows[0]["IDMAIN"];

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);

            //3901 - проверить, почему у этого пина удаленный доступ

            //{"1":
            //    {
            //     "exemplar_location":"Абонемент",
            //     "exemplar_collection":"ОФ",
            //     "exemplar_placing_cipher":"08.B 4650",
            //     "exemplar_carrier":"Бумага",
            //     "exemplar_inventory_number":"2494125",
            //     "exemplar_class_edition":"Для выдачи"
            //    },
            // "2":
            //    {
            //        "exemplar_location":"Абонемент",
            //        "exemplar_collection":"ОФ",
            //        "exemplar_placing_cipher":"08.B 4651",
            //        "exemplar_carrier":"Бумага",
            //        "exemplar_inventory_number":"2494126",
            //        "exemplar_class_edition":"Для выдачи"
            //    }
            //}


            writer.WriteStartObject();

            DataTable exemplar;
            int cnt = 1;
            string CarrierCode = "";
            foreach (DataRow iddata in table.Rows)
            {
                exemplar = dbWrapper.GetExemplar((int)iddata["IDDATA"]);
                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();

                //ExemplarInfo bjExemplar = new ExemplarInfo((int)iddata["IDDATA"]);

                foreach (DataRow r in exemplar.Rows)
                {
                    string code = r["MNFIELD"].ToString() + r["MSFIELD"].ToString();
                    switch (code)
                    {
                        case "899$a":
                            string UL = KeyValueMapping.UnifiedLocation.GetValueOrDefault(r["PLAIN"].ToString(), "отсутствует в словаре");
                            writer.WritePropertyName("exemplar_location");

                            int LocationCode = KeyValueMapping.UnifiedLocationCode.GetValueOrDefault(UL, 2999);
                            writer.WriteValue(LocationCode);
                            AddField("Location", UL);

                            //f_899a = r["PLAIN"].ToString();
                            //bjExemplar.Fields.AddField(LocationCode.ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "482$a":
                            //Exemplar += "Приплетено к:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_interlaced_to");
                            writer.WriteValue(r["PLAIN"].ToString());
                            
                            writer.WritePropertyName("exemplar_convolute");
                            ExemplarInfo Convolute = ExemplarInfo.GetExemplarByInventoryNumber(r["PLAIN"].ToString(), this.Fund);
                            writer.WriteValue(Convolute.IDMAIN);

                            //f_482a = r["PLAIN"].ToString();
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "899$b"://тут надо оставить только коллекции
                            string fnd = r["PLAIN"].ToString();

                            //f_899b = r["PLAIN"].ToString();

                            //Exemplar += "Наименование фонда или коллекции:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_collection");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());

                            break;
                        case "899$c":
                            //Exemplar += "Местонахождение стеллажа:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_rack_location");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "899$d":
                            //Exemplar += "Направление временного хранения:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_direction_temporary_storage");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "899$j":
                            //Exemplar += "Расстановочный шифр:" + r["PLAIN"].ToString() + "#";
                            AddField("PlacingCipher", r["PLAIN"].ToString());
                            writer.WritePropertyName("exemplar_placing_cipher");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "899$p":
                            //Exemplar += "Инвентарный номер:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_inventory_number");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //f_899p = r["PLAIN"].ToString();
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "899$w":
                            //Exemplar += "Штрихкод:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_barcode");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "899$x":
                            //Exemplar += "Примечание к инвентарному номеру:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_inv_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //f_899x = r["PLAIN"].ToString();
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "921$a":
                            //Exemplar += "Носитель:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_carrier");
                            CarrierCode = GetCarrierCode(r["PLAIN"].ToString());
                            writer.WriteValue(CarrierCode);
                            //f_921a = r["PLAIN"].ToString();
                            //bjExemplar.Fields.AddField(CarrierCode, (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "921$c":
                            //Exemplar += "Класс издания:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_class_edition");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //f_921c = r["PLAIN"].ToString();
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "921$d":
                            //writer.WritePropertyName("exemplar_class_edition");
                            //writer.WriteValue(r["PLAIN"].ToString());
                            //f_921d = r["PLAIN"].ToString();
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        //case "922$b":
                            //Exemplar += "Трофей\\Принадлежность к:" + r["PLAIN"].ToString() + "#";
                            //writer.WritePropertyName("exemplar_trophy");
                           // writer.WriteValue(r["PLAIN"].ToString());
                           // break;
                        case "3300$a":
                            //Exemplar += "Вид переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_kind");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$b":
                            //Exemplar += "Век переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_age");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$c":
                            //Exemplar += "Тип переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_type");
                            writer.WriteValue(r["PLAIN"].ToString());
                           // bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$d":
                            //Exemplar += "Материал крышек:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_cover_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$e":
                            //Exemplar += "Материал на крышках:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_material_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$f":
                            //Exemplar += "Материал корешка:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_spine_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$g":
                            //Exemplar += "Бинты:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_bandages");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$h":
                            //Exemplar += "Тиснение на крышках:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_stamping_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                           // bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$i":
                            //Exemplar += "Тиснение на корешке:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_embossing_on_spine");
                            writer.WriteValue(r["PLAIN"].ToString());
                           // bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$j":
                            //Exemplar += "Фурнитура:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_fittings");
                            writer.WriteValue(r["PLAIN"].ToString());
                           // bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$k":
                            //Exemplar += "Жуковины:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_bugs");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$l":
                            //Exemplar += "Форзац:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_forth");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$m":
                            //Exemplar += "Обрез:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_cutoff");
                            writer.WriteValue(r["PLAIN"].ToString());
                           // bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$n":
                            //Exemplar += "Состояние:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_condition");
                            writer.WriteValue(r["PLAIN"].ToString());
                           // bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$o":
                            //Exemplar += "Футляр:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_case");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$p":
                            //Exemplar += "Тиснение на канте:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_embossing_on_edge");
                            writer.WriteValue(r["PLAIN"].ToString());
                           // bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                        case "3300$r":
                            //Exemplar += "Примечание о переплете:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            //bjExemplar.Fields.AddField(r["PLAIN"].ToString(), (int)r["MNFIELD"], r["MSFIELD"].ToString());
                            break;
                    }

                }
                //Exemplar += "exemplar_id:" + ds.Tables["t"].Rows[0]["IDDATA"].ToString() + "#";
                writer.WritePropertyName("exemplar_id");
                writer.WriteValue(iddata["IDDATA"].ToString());

                ExemplarInfo bjExemplar = ExemplarInfo.GetExemplarByIdData((int)iddata["IDDATA"], this.Fund);


                
                AddField("MethodOfAccess", bjExemplar.ExemplarAccess.MethodOfAccess.ToString());
                writer.WritePropertyName("exemplar_access");
                writer.WriteValue(bjExemplar.ExemplarAccess.Access);
                writer.WritePropertyName("exemplar_access_group");
                writer.WriteValue(KeyValueMapping.AccessCodeToGroup[bjExemplar.ExemplarAccess.Access]);


                writer.WriteEndObject();
            }

            //смотрим есть ли гиперссылка
            table = dbWrapper.GetHyperLink(IDMAIN);
            if (table.Rows.Count != 0)//если есть - вставляем отдельным экземпляром. после электронной инвентаризации этот кусок можно будет убрать
            {
                //ExemplarInfo bjExemplar = new ExemplarInfo(-1);
                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();


                //Exemplar += "Электронная копия: есть#";
                writer.WritePropertyName("exemplar_electronic_copy");
                writer.WriteValue("да");
                //Exemplar += "Гиперссылка: " + ds.Tables["t"].Rows[0]["PLAIN"].ToString() + " #";
                writer.WritePropertyName("exemplar_hyperlink");
                writer.WriteValue(table.Rows[0]["PLAIN"].ToString());

                DataTable hyperLinkTable = dbWrapper.GetBookScanInfo(IDMAIN);
                bool ForAllReader = false;
                if (hyperLinkTable.Rows.Count != 0)
                {
                    ForAllReader = (bool)hyperLinkTable.Rows[0]["ForAllReader"];
                    //Exemplar += "Авторское право: " + ((ds.Tables["t"].Rows[0]["ForAllReader"].ToString() == "1") ? "нет" : "есть");
                    writer.WritePropertyName("exemplar_copyright");
                    writer.WriteValue( (ForAllReader) ? "нет" : "есть" );
                    //Exemplar += "Ветхий оригинал: " + ((ds.Tables["t"].Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_old_original");
                    writer.WriteValue(((hyperLinkTable.Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Наличие PDF версии: " + ((ds.Tables["t"].Rows[0]["PDF"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_PDF_exists");
                    writer.WriteValue(((hyperLinkTable.Rows[0]["PDF"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Доступ: Заказать через личный кабинет";
                    writer.WritePropertyName("exemplar_access");
                    writer.WriteValue(
                        (ForAllReader) ?
                        "1001" : "1002");
                        //"Для прочтения онлайн необходимо перейти по ссылке" :
                        //"Для прочтения онлайн необходимо положить в корзину и заказать через личный кабинет");
                    writer.WritePropertyName("exemplar_access_group");
                    writer.WriteValue((ForAllReader) ? KeyValueMapping.AccessCodeToGroup[1001] : KeyValueMapping.AccessCodeToGroup[1002]);

                    writer.WritePropertyName("exemplar_carrier");
                    //writer.WriteValue("Электронная книга");
                    writer.WriteValue("3011");

                    writer.WritePropertyName("exemplar_id");
                    writer.WriteValue("ebook");//для всех у кого есть электронная копия. АПИ когда это значение встретит, сразу вернёт "доступно"
                    writer.WritePropertyName("exemplar_location");
                    writer.WriteValue("2030");

                }
                writer.WriteEndObject();
                AddField("MethodOfAccess", "4002");

                //определить структуру, в которую полностью запись ложится и здесь её проверять, чтобы правильно вычисляемые поля проставить.
            }
            writer.WriteEndObject();
            writer.Flush();
            writer.Close();

            AddField("Exemplar", sb.ToString());
        }

        private string GetCarrierCode(string CarrierName)
        {
            switch (CarrierName)
            {
                case "Аудиокассета":
                    return "3000";
                case "Бумага":
                    return "3001";
                case "Видеокассета":
                    return "3002";
                case "Грампластинка":
                    return "3003";
                case "Дискета":
                    return "3004";
                case "Комплект(бумага+)":
                    return "3005";
                case "Магнитная лента":
                    return "3006";
                case "Микрофильм":
                    return "3007";
                case "Микрофиша":
                    return "3008";
                case "СD/DVD":
                    return "3009";
                case "Слайд":
                    return "3010";
                case "Электронная копия":
                    return "3011";
                case "Электронное издание":
                    return "3012";
            }
            return "3001";
        }


      
        public override void ExportCovers()
        {
            StringBuilder sb = new StringBuilder();
            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);
            DataTable table = dbWrapper.GetAllIdmainWithImages();
            List<string> errors = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                DataTable pics = dbWrapper.GetImage((int)row["IDMAIN"]);
                int i = 0;
                foreach (DataRow pic in pics.Rows)
                {
                    byte[] p = (byte[])pic["PIC"];
                    MemoryStream ms = new MemoryStream(p);
                    Image img = Image.FromStream(ms);
                    string path = @"f:\import\covers\"+this.Fund.ToLower()+"\\" + row["IDMAIN"].ToString() + @"\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (i == 0)
                    {
                        i++;
                        try
                        {
                            img.Save(path + "cover.jpg");
                        }
                        catch (Exception ex)
                        {
                            errors.Add(pic["PIC"].ToString() + " " + ex.Message + ex.InnerException);

                        }
                    }
                    else
                    {
                        img.Save(path + "cover"+i++.ToString() + ".jpg");
                    }
                }

            }
            File.WriteAllLines(@"f:\import\importErrors\" + this.Fund + "Errors.txt", errors.ToArray());

        }

        private void AddHierarchyFields(int ParentPIN, int CurrentPIN)
        {
            string query = " select * from " + this.Fund + "..DATAEXT " +
                               " where IDMAIN = " + ParentPIN;
            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);
            DataTable table = dbWrapper.GetBJRecord(ParentPIN);
            int TopHierarchyId = GetTopId( ParentPIN );
            AddField("hierarchy_top_id", this.Fund + "_" + TopHierarchyId);

            table = dbWrapper.GetTitle(TopHierarchyId);
            if (table.Rows.Count != 0)
            {
                string hierarchy_top_title = table.Rows[0]["PLAIN"].ToString();
                AddField("hierarchy_top_title", hierarchy_top_title);
            }
            AddField("hierarchy_parent_id", this.Fund + "_" + ParentPIN);

            table = dbWrapper.GetTitle(ParentPIN);
            if (table.Rows.Count != 0)
            {
                string hierarchy_parent_title = table.Rows[0]["PLAIN"].ToString();
                AddField("hierarchy_parent_title", hierarchy_parent_title);
            }

            bool metka = false;
            foreach (XmlNode n in _doc.ChildNodes)
            {
                if (n.Attributes["name"].Value == "is_hierarchy_id")
                {
                    metka = true;
                }
            }
            if (!metka)
            {
                AddField("is_hierarchy_id", this.Fund + "_" + CurrentPIN);//пометка о том, что это серия
            }

            table = dbWrapper.GetTitle(CurrentPIN);
            if (table.Rows.Count != 0)
            {
                string is_hierarchy_title = table.Rows[0]["PLAIN"].ToString();

                metka = false;
                foreach (XmlNode n in _doc.ChildNodes)
                {
                    if (n.Attributes["name"].Value == "is_hierarchy_id")
                    {
                        metka = true;
                    }
                }
                if (!metka) AddField("is_hierarchy_title", is_hierarchy_title);
            }
        }
        private int GetTopId(int ParentPIN)
        {
            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);
            DataTable table = dbWrapper.GetParentIDMAIN(ParentPIN);
            if (table.Rows.Count == 0)
            {
                return ParentPIN;
            }
            ParentPIN = Convert.ToInt32(table.Rows[0]["SORT"]);
            return GetTopId(ParentPIN);
        }
        private int GetMaxIDMAIN()
        {
            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);
            DataTable table = dbWrapper.GetMaxIDMAIN();
            return int.Parse(table.Rows[0][0].ToString());
        }
        private AuthoritativeFile GetAFAll( int AFLinkId, string AFTable)
        {
            //NAMES: (1) Персоналии
            //ORGS: (2) Организации
            //HEADER: (3) Унифицированное заглавие
            //DEL: (5) Источник списания
            //GEO: (6) Географическое название
            DatabaseWrapper dbWrapper = new DatabaseWrapper(this.Fund);

            AuthoritativeFile result = new AuthoritativeFile();
            DataTable table = dbWrapper.GetAFAllValues(AFTable, AFLinkId);
            foreach (DataRow r in table.Rows)
            {
                result.Add(r["PLAIN"].ToString());
            }
            return result;
        }




    }
}
