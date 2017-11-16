using System;
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

namespace ExportBJ_XML.classes
{
    public class BJVuFindConverter : VuFindConverter
    {

        public BJVuFindConverter(string fund)
        {
            this.Fund = fund;
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
                string query = GetQuery( i );
                DataTable record = ExecuteQuery(query);
                if (record.Rows.Count == 0) continue;
                try
                {
                    int check = CreateBJDoc( record );
                    if (check == 1) continue;
                }
                catch (Exception ex)
                {

                    //_f1.textBox1.Text += DateTime.Now.ToShortTimeString() + " - " + ex.Message + " ;\r\n ";
                    //OnExportError
                    //здесь записать пин в файл ошибок и продолжить.
                    errors.Add(this.Fund + "_" + i);
                    _doc = _exportDocument.CreateElement("doc");
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
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\" + this.Fund + "_" + idmain + ".xml");

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
            string q = GetQuery( idmain );
            DataTable table = ExecuteQuery(q);
            int check = CreateBJDoc( table );
            if (check == 1) return;
            _doc.WriteTo(_objXmlWriter);
            _doc = _exportDocument.CreateElement("doc");
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }

        private int CreateBJDoc( DataTable BJBook )
        {
            string currentIDMAIN = BJBook.Rows[0]["IDMAIN"].ToString();
            string level = BJBook.Rows[0]["Level"].ToString();
            string level_id = BJBook.Rows[0]["level_id"].ToString();
            int lev_id = int.Parse(level_id);
            if (lev_id < 0) return 1;
            string allFields = "";
            string AF_all = "";
            bool wasTitle = false;//встречается ошибка: два заглавия в одном пине
            bool wasAuthor = false;//был ли автор. если был, то сортировочное поле уже заполнено
            string description = "";//все 3хх поля
            DataTable clarify;
            string query = "";
            string Annotation = "";
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

                        wasTitle = true;
                        break;
                    case "700$a":
                        AddField("author", r["PLAIN"].ToString());
                        if (!wasAuthor)
                        {
                            AddField("author_sort", r["SORT"].ToString());
                        }
                        wasAuthor = true;
                        //забрать все варианты написания автора из авторитетного файла и вставить в скрытое, но поисковое поле
                        break;
                    case "701$a":
                        AddField("author2", r["PLAIN"].ToString());
                        break;
                    case "710$a":
                        AddField("author_corporate", r["PLAIN"].ToString());
                        break;
                    case "710$4":
                        AddField("author_corporate_role", r["PLAIN"].ToString());
                        break;
                    case "700$4":
                        AddField("author_role", r["PLAIN"].ToString());
                        break;
                    case "701$4":
                        AddField("author2_role", r["PLAIN"].ToString());
                        break;
                    case "921$a":
                        AddField("format", r["PLAIN"].ToString());
                        break;
                    case "922$e":
                        AddField("genre", r["PLAIN"].ToString());
                        AddField("genre_facet", r["PLAIN"].ToString());
                        break;
                    case "10$a":
                        query = " select * from " + this.Fund + "..DATAEXT A " +
                           " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                           " where A.MNFIELD = 10 and A.MSFIELD = '$b' and A.IDDATA = " + r["IDDATA"].ToString();
                        clarify = ExecuteQuery(query);
                        string add = r["PLAIN"].ToString();
                        if (clarify.Rows.Count != 0)
                        {
                            add = r["PLAIN"].ToString() + " (" + clarify.Rows[0]["PLAIN"].ToString() + ")";
                        }
                        AddField("isbn", add);
                        break;
                    case "11$a":
                        AddField("issn", r["PLAIN"].ToString());
                        break;
                    case "101$a":
                        query = " select NAME from " + this.Fund + "..LIST_1 " + " where ID = " + r["IDINLIST"].ToString();
                        clarify = ExecuteQuery(query);
                        if (clarify.Rows.Count == 0)
                        {
                            AddField("language", r["PLAIN"].ToString());
                        }
                        else
                        {
                            AddField("language", clarify.Rows[0]["NAME"].ToString());
                        }
                        break;
                    case "2100$d":
                        AddField("publishDate", r["PLAIN"].ToString());
                        break;
                    case "210$c":
                        AddField("publisher", r["PLAIN"].ToString());
                        break;
                    case "517$a":
                        query = " select B.PLAIN from " + this.Fund + "..DATAEXT A " +
                                " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where MNFIELD = 517 and MSFIELD = '$b' and A.IDDATA = " + r["IDDATA"].ToString();
                        clarify = ExecuteQuery(query);
                        if (clarify.Rows.Count != 0)
                        {
                            AddField("title_alt", "(" + clarify.Rows[0]["PLAIN"].ToString() + ")" + r["PLAIN"].ToString());
                        }
                        else
                        {
                            AddField("title_alt", r["PLAIN"].ToString());
                        }
                        //нужно специальным образом обрабатывать
                        break;
                    //=======================================================================добавленные в индекс=======================
                    case "210$a":
                        AddField("PlaceOfPublication", r["PLAIN"].ToString());
                        break;
                    case "200$6":
                        AddField("Title_another_chart", r["PLAIN"].ToString());
                        break;
                    case "200$b":
                        AddField("Title_same_author", r["PLAIN"].ToString());
                        break;
                    case "200$d":
                        AddField("Parallel_title", r["PLAIN"].ToString());
                        break;
                    case "200$e":
                        AddField("Info_pertaining_title", r["PLAIN"].ToString());
                        break;
                    case "200$f":
                        AddField("Responsibility_statement", r["PLAIN"].ToString());
                        break;
                    case "200$h":
                        AddField("Part_number", r["PLAIN"].ToString());
                        break;
                    case "200$i":
                        AddField("Part_title", r["PLAIN"].ToString());
                        break;
                    case "200$z":
                        AddField("Language_title_alt", r["PLAIN"].ToString());
                        break;
                    case "500$a":
                        AddField("Title_unified", r["PLAIN"].ToString());
                        break;
                    case "500$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField("Title_unified", AF_all);
                        break;
                    case "517$e":
                        AddField("Info_title_alt", r["PLAIN"].ToString());
                        break;
                    case "517$z":
                        AddField("Language_title_alt", r["PLAIN"].ToString());
                        break;
                    case "700$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFNAMESVAR");
                        string[] author_variants = AF_all.Split(';');
                        foreach (string av in author_variants)
                        {
                            AddField("author_variant", av);//хранить но не отображать
                        }
                        break;
                    case "701$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField("Another_author_AF_all", AF_all);//хранить но не отображать
                        break;
                    case "501$a":
                        AddField("Another_title", r["PLAIN"].ToString());
                        break;
                    case "501$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField("Another_title_AF_All", AF_all);
                        break;
                    case "503$a":
                        AddField("Unified_Caption", r["PLAIN"].ToString());
                        break;
                    case "503$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFHEADERVAR");
                        AddField("Unified_Caption_AF_All", AF_all);
                        break;
                    case "700$6":
                        AddField("Author_another_chart", r["PLAIN"].ToString());
                        break;
                    case "702$a":
                        AddField("Editor", r["PLAIN"].ToString());
                        break;
                    case "702$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField("Editor_AF_all", AF_all);
                        break;
                    case "702$4":
                        AddField("Editor_role", AF_all);
                        break;
                    case "710$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField("Collective_author_all", AF_all);
                        break;
                    case "710$9":
                        AddField("Organization_nature", r["PLAIN"].ToString());
                        break;
                    case "11$9":
                        AddField("Printing", r["PLAIN"].ToString());
                        break;
                    case "205$a":
                        string PublicationInfo = r["PLAIN"].ToString();

                        // 205$b
                        query = " select * from " + this.Fund + "..DATAEXT A " +
                                " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where A.MNFIELD = 205 and A.MSFIELD = '$b' and A.IDDATA = " + r["IDDATA"].ToString();
                        clarify = ExecuteQuery(query);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        // 205$f
                        query = " select * from " + this.Fund + "..DATAEXT A " +
                                " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where A.MNFIELD = 205 and A.MSFIELD = '$f' and A.IDDATA = " + r["IDDATA"].ToString();
                        clarify = ExecuteQuery(query);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += " / " + rr["PLAIN"].ToString();
                        }
                        // 205$g
                        query = " select * from " + this.Fund + "..DATAEXT A " +
                                " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where A.MNFIELD = 205 and A.MSFIELD = '$g' and A.IDDATA = " + r["IDDATA"].ToString();
                        clarify = ExecuteQuery(query);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        AddField("Publication_info", r["PLAIN"].ToString());
                        break;
                    case "921$b":
                        AddField("EditionType", r["PLAIN"].ToString());
                        break;
                    case "102$a":
                        AddField("Country", r["PLAIN"].ToString());
                        break;
                    case "210$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField("PlaceOfPublication_AF_All", r["PLAIN"].ToString());
                        break;
                    case "2110$g":
                        AddField("PrintingHouse", r["PLAIN"].ToString());
                        break;
                    case "2110$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField("PrintingHouse_AF_All", r["PLAIN"].ToString());
                        break;
                    case "2111$e":
                        AddField("GeoNamePlaceOfPublication", r["PLAIN"].ToString());
                        break;
                    case "2111$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFGEOVAR");
                        AddField("GeoNamePlaceOfPublication_AF_All", r["PLAIN"].ToString());
                        break;
                    case "10$z":
                        AddField("IncorrectISBN", r["PLAIN"].ToString());
                        break;
                    case "11$z":
                        AddField("IncorrectISSN", r["PLAIN"].ToString());
                        break;
                    case "11$y":
                        AddField("CanceledISSN", r["PLAIN"].ToString());
                        break;
                    case "101$b":
                        AddField("IntermediateTranslateLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$d":
                        AddField("SummaryLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$e":
                        AddField("TableOfContentsLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$f":
                        AddField("TitlePageLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$g":
                        AddField("BasicTitleLanguage", r["PLAIN"].ToString());
                        break;
                    case "101$i":
                        AddField("AccompayingMaterialLanguage", r["PLAIN"].ToString());
                        break;
                    case "215$a":
                        AddField("Volume", r["PLAIN"].ToString());
                        break;
                    case "215$b":
                        AddField("Illustrations", r["PLAIN"].ToString());
                        break;
                    case "215$c":
                        AddField("Dimensions", r["PLAIN"].ToString());
                        break;
                    case "215$d":
                        AddField("AccompayingMaterial", r["PLAIN"].ToString());
                        break;
                    case "225$a":
                        if (r["PLAIN"].ToString() == "") break;
                        if (r["PLAIN"].ToString() == "-1") break;
                        AddHierarchyFields(r["PLAIN"].ToString(), r["IDMAIN"].ToString());
                        break;
                    case "225$h":
                        AddField("NumberInSeries", r["PLAIN"].ToString());
                        break;
                    case "225$v":
                        AddField("NumberInSubseries", r["PLAIN"].ToString());
                        break;
                    case "300$a":
                    case "301$a":
                    case "316$a":
                    case "320$a":
                    case "326$a":
                    case "336$a":
                    case "337$a":
                        description += r["PLAIN"].ToString() + " ; ";
                        break;
                    case "327$a":
                    case "330$a":
                        Annotation += r["PLAIN"].ToString() + " ; ";
                        break;
                    case "830$a":
                        AddField("CatalogerNote", r["PLAIN"].ToString());
                        break;
                    case "831$a":
                        AddField("DirectoryNote", r["PLAIN"].ToString());
                        break;
                    case "924$a":
                        AddField("AdditionalBibRecord", r["PLAIN"].ToString());
                        break;
                    case "940$a":
                        AddField("HyperLink", r["PLAIN"].ToString());
                        AddField("Location", "Удалённый доступ");
                        break;
                    case "606$a"://"""""" • """"""
                        query = "select * " +
                                " from BJVVV..TPR_CHAIN A " +
                                " left join BJVVV..TPR_TES B on A.IDTES = B.ID " +
                                " where A.IDCHAIN = " + r["SORT"].ToString() +
                                " order by IDORDER";
                        clarify = ExecuteQuery(query);
                        if (clarify.Rows.Count == 0) break;
                        string TPR = "";
                        foreach (DataRow rr in clarify.Rows)
                        {
                            TPR += rr["VALUE"].ToString() + " • ";
                        }
                        TPR = TPR.Substring(0, TPR.Length - 2);
                        AddField("topic", TPR);
                        AddField("topic_facet", TPR);
                        break;
                    case "3000$a":
                        AddField("OwnerPerson", r["PLAIN"].ToString());
                        break;
                    case "3000$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFNAMESVAR");
                        AddField("OwnerPerson_AF_All", AF_all);
                        break;
                    case "3001$a":
                        AddField("OwnerOrganization", r["PLAIN"].ToString());
                        break;
                    case "3001$3":
                        AF_all = GetAFAll( r["AFLINKID"].ToString(), "AFORGSVAR");
                        AddField("OwnerOrganization_AF_All", AF_all);
                        break;
                    case "3002$a":
                        AddField("Ownership", r["PLAIN"].ToString());
                        break;
                    case "3003$a":
                        AddField("OwnerExemplar", r["PLAIN"].ToString());
                        break;
                    case "3200$a":
                        AddField("IllustrationMaterial", r["PLAIN"].ToString());
                        break;
                    case "899$a":
                        AddField("Location", r["PLAIN"].ToString());
                        break;
                }

            }
            AddField("id", this.Fund + "_" + currentIDMAIN);

            string rusFund = GetRusFund(this.Fund);

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

        private void AddExemplarFields(string idmain, XmlDocument _exportDocument, string fund)
        {

            string query = " select distinct A.IDMAIN, A.IDDATA from " + fund + "..DATAEXT A" +
                            " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                            " where A.IDMAIN = " + idmain + " and (A.MNFIELD = 899 and A.MSFIELD = '$p' or A.MNFIELD = 899 and A.MSFIELD = '$a' or A.MNFIELD = 899 and A.MSFIELD = '$w') " +
                            " and not exists (select 1 from BJVVV..DATAEXT C where C.IDDATA = A.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c' and C.SORT = 'Списано')";
            DataTable table = ExecuteQuery(query);
            if (table.Rows.Count == 0) return;
            string IDMAIN = table.Rows[0]["IDMAIN"].ToString();

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
            //ser.Serialize(
            foreach (DataRow iddata in table.Rows)
            {
                query = " select * from " + fund + "..DATAEXT A" +
                        " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                        " where A.IDDATA = " + iddata["IDDATA"];
                exemplar = ExecuteQuery(query);
                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();



                foreach (DataRow r in exemplar.Rows)
                {
                    string code = r["MNFIELD"].ToString() + r["MSFIELD"].ToString();
                    switch (code)
                    {
                        case "899$a":
                            string UnifiedLocation = GetUnifiedLocation(r["PLAIN"].ToString());
                            writer.WritePropertyName("exemplar_location");
                            writer.WriteValue(UnifiedLocation);
                            AddField("Location", UnifiedLocation);
                            break;
                        case "482$a":
                            //Exemplar += "Приплетено к:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_interlaced_to");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$b"://тут надо оставить только коллекции
                            string fnd = r["PLAIN"].ToString();
                            if ((fnd == "ОФ") || (fnd == "Фонд редкой книги") || (fnd == "Фонд Редкой книги") || (fnd == "ОФ - Восток"))//надо определить что коллекция, что фонд, а что на дом.
                            {
                                AddField("MethodOfAccess", "В помещении библиотеки");
                                //Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет в зал библиотеки");
                            }
                            else
                            {
                                AddField("collection", r["PLAIN"].ToString());
                                //Exemplar += "Доступ: Заказать через личный кабинет в зал библиотеки#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет в зал библиотеки");
                            }
                            if (fnd == "Абонемент")
                            {
                                AddField("MethodOfAccess", "На дом");
                                //Exemplar += "Доступ: Заказать через личный кабинет на дом#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Заказать через личный кабинет на дом");
                            }
                            if ((fund == "BJFCC") || (fund == "BJACC") || (fund == "BRIT_SOVET") || (fund == "BJSCC"))
                            {
                                AddField("MethodOfAccess", "На дом");
                                //break;//тут надо оставить только коллекции
                                //Exemplar += "Доступ: Проследовать в указанный зал для получения на дом#";
                                writer.WritePropertyName("Access");
                                writer.WriteValue("Проследовать в указанный зал для получения на дом");
                            }

                            //Exemplar += "Наименование фонда или коллекции:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_collection");
                            writer.WriteValue(r["PLAIN"].ToString());

                            break;
                        case "899$c":
                            //Exemplar += "Местонахождение стеллажа:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_rack_location");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$d":
                            //Exemplar += "Направление временного хранения:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_direction_temporary_storage");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$j":
                            //Exemplar += "Расстановочный шифр:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_placing_cipher");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$p":
                            //Exemplar += "Инвентарный номер:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_inventory_number");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$w":
                            //Exemplar += "Штрихкод:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_barcode");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$x":
                            //Exemplar += "Примечание к инвентарному номеру:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_inv_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "921$a":
                            //Exemplar += "Носитель:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_carrier");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "921$c":
                            //Exemplar += "Класс издания:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_class_edition");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "922$b":
                            //Exemplar += "Трофей\\Принадлежность к:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_trophy");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$a":
                            //Exemplar += "Вид переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_kind");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$b":
                            //Exemplar += "Век переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_age");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$c":
                            //Exemplar += "Тип переплёта:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_type");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$d":
                            //Exemplar += "Материал крышек:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_cover_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$e":
                            //Exemplar += "Материал на крышках:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_material_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$f":
                            //Exemplar += "Материал корешка:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_spine_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$g":
                            //Exemplar += "Бинты:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_bandages");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$h":
                            //Exemplar += "Тиснение на крышках:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_stamping_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$i":
                            //Exemplar += "Тиснение на корешке:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_embossing_on_spine");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$j":
                            //Exemplar += "Фурнитура:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_fittings");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$k":
                            //Exemplar += "Жуковины:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_bugs");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$l":
                            //Exemplar += "Форзац:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_forth");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$m":
                            //Exemplar += "Обрез:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_cutoff");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$n":
                            //Exemplar += "Состояние:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_condition");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$o":
                            //Exemplar += "Футляр:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_case");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$p":
                            //Exemplar += "Тиснение на канте:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_embossing_on_edge");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$r":
                            //Exemplar += "Примечание о переплете:" + r["PLAIN"].ToString() + "#";
                            writer.WritePropertyName("exemplar_binding_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                    }

                }
                //Exemplar += "exemplar_id:" + ds.Tables["t"].Rows[0]["IDDATA"].ToString() + "#";
                writer.WritePropertyName("exemplar_id");
                writer.WriteValue(iddata["IDDATA"].ToString());
                writer.WriteEndObject();
            }

            //смотрим есть ли гиперссылка
            query = " select * from " + fund + "..DATAEXT A" +
                    " left join " + fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                    " where A.MNFIELD = 940 and A.MSFIELD = '$a' and A.IDMAIN = " + IDMAIN;
            table = ExecuteQuery(query);
            if (table.Rows.Count != 0)//если есть - вставляем отдельным экземпляром.
            {
                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();


                //Exemplar += "Электронная копия: есть#";
                writer.WritePropertyName("exemplar_electronic_copy");
                writer.WriteValue("да");
                //Exemplar += "Гиперссылка: " + ds.Tables["t"].Rows[0]["PLAIN"].ToString() + " #";
                writer.WritePropertyName("exemplar_hyperlink");
                writer.WriteValue(table.Rows[0]["PLAIN"].ToString());
                if (fund == "BJVVV")
                {
                    query = " select * from BookAddInf..ScanInfo A" +
                    " where A.IDBase = 1 and A.IDBook = " + IDMAIN;
                }
                if (fund == "REDKOSTJ")
                {
                    query = " select * from BookAddInf..ScanInfo A" +
                    " where A.IDBase = 2 and A.IDBook = " + IDMAIN;
                }
                DataTable hyperLinkTable = ExecuteQuery(query);
                if (hyperLinkTable.Rows.Count != 0)
                {
                    //Exemplar += "Авторское право: " + ((ds.Tables["t"].Rows[0]["ForAllReader"].ToString() == "1") ? "нет" : "есть");
                    writer.WritePropertyName("exemplar_copyright");
                    writer.WriteValue(((hyperLinkTable.Rows[0]["ForAllReader"].ToString() == "1") ? "нет" : "есть"));
                    //Exemplar += "Ветхий оригинал: " + ((ds.Tables["t"].Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_old_original");
                    writer.WriteValue(((hyperLinkTable.Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Наличие PDF версии: " + ((ds.Tables["t"].Rows[0]["PDF"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_PDF_exists");
                    writer.WriteValue(((hyperLinkTable.Rows[0]["PDF"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Доступ: Заказать через личный кабинет";
                    writer.WritePropertyName("exemplar_access");
                    writer.WriteValue(
                        (hyperLinkTable.Rows[0]["ForAllReader"].ToString() == "1") ?
                        "Для прочтения онлайн необходимо перейти по ссылке" :
                        "Для прочтения онлайн необходимо положить в корзину и заказать через личный кабинет");
                    writer.WritePropertyName("exemplar_carrier");
                    writer.WriteValue("Электронная книга");

                    writer.WritePropertyName("exemplar_id");
                    writer.WriteValue("ebook");//для всех у кого есть электронная копия. АПИ когда это значение встретит, сразу вернёт "доступно"

                }
                writer.WriteEndObject();
                AddField("MethodOfAccess", "Удалённый доступ");
            }
            writer.WriteEndObject();
            writer.Flush();
            writer.Close();

            AddField("Exemplar", sb.ToString());
        }

        public override void ExportCovers()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select IDMAIN from "+this.Fund+"..IMAGES");

            DataTable table = ExecuteQuery(sb.ToString());
            foreach (DataRow row in table.Rows)
            {
                sb.Length = 0;
                sb.AppendFormat("select PIC from " + this.Fund + "..IMAGES where IDMAIN = {0}", row["IDMAIN"].ToString());
                DataTable pics = ExecuteQuery(sb.ToString());
                int i = 0;
                //using (new NetworkConnection(_directoryPath, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
                NetworkCredential theNetworkCredential = new NetworkCredential("BJStor01\\imgview", "Image_123Viewer");
                CredentialCache theNetcache = new CredentialCache();
                theNetcache.Add(@"\\192.168.4.30\VufindCovers", 0, "Basic", theNetworkCredential);
                //then do whatever, such as getting a list of folders:
                string[] theFolders = System.IO.Directory.GetDirectories(@"\\192.168.4.30\VufindCovers");

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
                    img.Save(path + i.ToString() + ".jpg");
                }

            }
        }

        private void AddHierarchyFields(string ParentPIN, string CurrentPIN)
        {
            string query = " select * from " + this.Fund + "..DATAEXT " +
                               " where IDMAIN = " + ParentPIN;
            DataTable table = ExecuteQuery(query);
            string TopHierarchyId = GetTopId( ParentPIN );
            AddField("hierarchy_top_id", this.Fund + "_" + TopHierarchyId);

            query = " select * from " + this.Fund + "..DATAEXT A " +
                    " left join " + this.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                    " where A.IDMAIN = " + TopHierarchyId + " and MNFIELD = 200 and MSFIELD = '$a' ";
            table = ExecuteQuery(query);
            if (table.Rows.Count != 0)
            {
                string hierarchy_top_title = table.Rows[0]["PLAIN"].ToString();
                AddField("hierarchy_top_title", hierarchy_top_title);
            }
            AddField("hierarchy_parent_id", this.Fund + "_" + ParentPIN);

            query = " select * from " + this.Fund + "..DATAEXT A " +
                    " left join " + this.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                    " where A.IDMAIN = " + ParentPIN + " and MNFIELD = 200 and MSFIELD = '$a' ";
            table = ExecuteQuery(query);
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


            query = " select * from " + this.Fund + "..DATAEXT A " +
                    " left join " + this.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                    " where A.IDMAIN = " + CurrentPIN + " and MNFIELD = 200 and MSFIELD = '$a' ";
            table = ExecuteQuery(query);
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
        private string GetTopId(string ParentPIN)
        {
            string query = " select * from " + this.Fund + "..DATAEXT " +
                           " where MNFIELD = 225 and MSFIELD = '$a' and IDMAIN = " + ParentPIN;
            DataTable table = ExecuteQuery(query);
            if (table.Rows.Count == 0)
            {
                return ParentPIN;
            }
            ParentPIN = table.Rows[0]["SORT"].ToString();
            return GetTopId(ParentPIN);
        }
        private int GetMaxIDMAIN()
        {
            string query = "select max(ID) from " + this.Fund + "..MAIN";
            DataTable table = ExecuteQuery(query);
            return int.Parse(table.Rows[0][0].ToString());
        }
        private string GetAFAll( string AFLinkId, string AFTable)
        {
            //NAMES: (1) Персоналии
            //ORGS: (2) Организации
            //HEADER: (3) Унифицированное заглавие
            //DEL: (5) Источник списания
            //GEO: (6) Географическое название

            string query = " select PLAIN from " + this.Fund + ".." + AFTable + " A " +
                               " where IDAF = " + AFLinkId;
            DataTable table = ExecuteQuery(query);
            string Another_author_AF_all = "";
            foreach (DataRow r in table.Rows)
            {
                Another_author_AF_all += r["PLAIN"].ToString() + "; ";
            }
            return Another_author_AF_all;
        }
        private string GetQuery( int idmain )
        {
            return "select A.*,cast(A.MNFIELD as nvarchar(10))+A.MSFIELD code,A.SORT,B.PLAIN, C.IDLEVEL level_id, " +
                    " case when C.IDLEVEL = 1 then 'Монография'  " +
                    "  when C.IDLEVEL = -100 then 'Коллекция'  " +
                    "  when C.IDLEVEL = -2 then 'Сводный уровень многотомного издания'  " +
                    "  when C.IDLEVEL = 2 then 'Том (выпуск) многотомного издания'  " +
                    "  when C.IDLEVEL = -3 then 'Сводный уровень сериального издания'  " +
                    "  when C.IDLEVEL = -33 then 'Сводный уровень подсерии, входящей в серию'  " +
                    "  when C.IDLEVEL = 3 then 'Выпуск сериального издания'  " +
                    "  when C.IDLEVEL = -4 then 'Сводный уровень сборника'  " +
                    "  when C.IDLEVEL = 4 then 'Часть сборника'  " +
                    "  when C.IDLEVEL = -5 then 'Сводный уровень продолжающегося издания'  " +
                    "  when C.IDLEVEL = 5 then 'Выпуск продолжающегося издания' else '' end level " +
                    " from " + this.Fund + "..DATAEXT A" +
                    " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                    " left join " + this.Fund + "..MAIN C on A.IDMAIN = C.ID " +
                //" where A.IDMAIN between " + count + " and " + (count + 999).ToString() +//типа для ускорения, но можно явно пин указать же. не помню.
                    " where A.IDMAIN = " + idmain +
                //" and exists (select 1 from BRIT_SOVET..DATAEXT C where A.IDMAIN = C.IDMAIN and C.MNFIELD = 899 and C.MSFIELD = '$p')" +
                    " order by IDMAIN, IDDATA";
        }
        private DataTable ExecuteQuery(string query)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.SelectCommand.CommandText = query;
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
                    if (ex.Number != -2) throw;
                    //это событиями переделать
                    VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                    args.RecordId = _lastID.ToString();

                    //this.OnDatabaseTimeout
                    
                    //_f1.textBox1.Text += DateTime.Now.ToShortTimeString() + " - " + ex.Message + " ;\r\n ";
                    //Application.DoEvents();
                    Thread.Sleep(5000);
                    continue;
                }
            }
            return ds.Tables["t"];
        }

        private string GetUnifiedLocation(string location)
        {
            string result = location;

            switch (location)
            {
                case "ЦМС Академия Рудомино":
                    result = "Академия \"Рудомино\"";
                    break;
                case "…Выст… КОО Группа справочного-информационного обслуживания":
                    result = "Выставка книг 2 этаж";
                    break;
                case "…ЗалФ… Отдел детской книги и детских программ":
                    result = "Детский зал 2 этаж";
                    break;
                case "ЦМС ОР Дом еврейской книги":
                    result = "Дом еврейской книги 3 этаж";
                    break;
                case "…Зал… КОО Группа абонементного обслуживания":
                    result = "Зал абонементного обслуживания 2 этаж";
                    break;
                case "…Зал… КОО Группа выдачи документов":
                    result = "Зал выдачи документов 2 этаж";
                    break;
                case "…Зал… КНИО Группа искусствоведения":
                    result = "Зал искусствоведения 4 этаж";
                    break;
                case "…Зал… КНИО Группа редкой книги":
                    result = "Зал редкой книги 4 этаж";
                    break;
                case "…ЗалФ… КНИО Группа религоведения":
                    result = "Зал религиоведения 4 этаж";
                    break;
                case "…Хран… Сектор книгохранения - 0 этаж":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - 2 этаж":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - 3 этаж":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - 4 этаж":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - 5 этаж":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - 6 этаж":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - 7 этаж":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - Абонемент":
                    result = "Книгохранение";
                    break;
                case "…Хран… Сектор книгохранения - Новая периодика":
                    result = "Книгохранение";
                    break;
                case "…Хран… КНИО Группа хранения редкой книги":
                    result = "Книгохранение редкой книги";
                    break;
                case "Книжный клуб":
                    result = "Книжный клуб 1 этаж";
                    break;
                case "…ЗалФ… ЦМС ОР Культурный центр Франкотека":
                    result = "Культурный центр \"Франкотека\" 2 этаж";
                    break;
                case "…ЗалФ… ЦМС ОР Лингвистический ресурсный центр Pearson":
                    result = "Лингвистический ресурсный центр Pearson 3 этаж";
                    break;
                case "КНИО - Комплексный научно-исследовательский отдел":
                    result = "Научно-исследовательский отдел";
                    break;
                case "…Обраб… КО КОД Сектор ОД - Группа инвентаризации":
                    result = "Обработка в группе инвентаризации";
                    break;
                case "…Обраб… КО КОД Сектор ОД - Группа каталогизации":
                    result = "Обработка в группе каталогизации";
                    break;
                case "…Обраб… КО ХКРФ Сектор микрофильмирования":
                    result = "Обработка в группе микрофильмирования";
                    break;
                case "…Обраб… ЦИИТ Группа оцифровки":
                    result = "Обработка в группе оцифровки";
                    break;
                case "…Обраб… КО КОД Сектор ОД - Группа систематизации":
                    result = "Обработка в группе систематизации";
                    break;
                case "…Обраб… КО КОД Сектор комплектования":
                    result = "Обработка в секторе комплектования";
                    break;
                case "…Обраб… КО ХКРФ Сектор научной реставрации":
                    result = "Обработка в секторе научной реставрации";
                    break;
                case "…Хран… Сектор книгохранения - Овальный зал":
                    result = "Овальный зал";
                    break;
                case "КО КОД - Комплексный отдел комплектования и обработки документов":
                    result = "Отдел комплектования";
                    break;
                case "КОО - Комплексный отдел обслуживания":
                    result = "Отдел обслуживания";
                    break;
                case "КОО Группа регистрации":
                    result = "Отдел обслуживания";
                    break;
                case "КО ХКРФ - Комплексный отдел хранения, консервации и реставрации фондов":
                    result = "Отдел хранения и реставрации";
                    break;
                case "ЦКПП Редакционно-издательский отдел":
                    result = "Редакционно-издательский отдел";
                    break;
                case "КО ХКРФ Сектор книгохранения":
                    result = "Сектор книгохранения";
                    break;
                case "КО КОД Сектор ОД":
                    result = "Сектор обработки документов";
                    break;
                case "…Хран… ЦИИТ Сервера библиотеки":
                    result = "Сервера библиотеки";
                    break;
                case "Д Бухгалтерия":
                    result = "Служебные подразделения";
                    break;
                case "Д Группа экспедиторского обслуживания":
                    result = "Служебные подразделения";
                    break;
                case "Д Контрактная служба":
                    result = "Служебные подразделения";
                    break;
                case "Д Отдел PR и редакция сайта":
                    result = "Служебные подразделения";
                    break;
                case "Д Отдел безопасности":
                    result = "Служебные подразделения";
                    break;
                case "Д Отдел внутреннего финансового контроля":
                    result = "Служебные подразделения";
                    break;
                case "Д Отдел по работе с персоналом":
                    result = "Служебные подразделения";
                    break;
                case "Д Отдел финансового планирования и сводной отчетности":
                    result = "Служебные подразделения";
                    break;
                case "Д УЭ - Управление по эксплуатации объектов недвижимости и обеспечения деятельности":
                    result = "Служебные подразделения";
                    break;
                case "Д УЭ Служба материально-технического обеспечения":
                    result = "Служебные подразделения";
                    break;
                case "Д УЭ Служба управления инженерными системами":
                    result = "Служебные подразделения";
                    break;
                case "Д УЭ Служба эксплуатации зданий и благоустройства":
                    result = "Служебные подразделения";
                    break;
                case "ЦБИД - Центр библиотечно-информационной деятельности и поддержки чтения":
                    result = "Служебные подразделения";
                    break;
                case "…ЗалФ… ЦМС ОР Центр американской культуры":
                    result = "Центр американской культуры 3 этаж";
                    break;
                case "ЦИИТ - Центр инновационных информационных технологий":
                    result = "Центр инновационных информационных технологий";
                    break;
                case "ЦИИТ Группа IT":
                    result = "Центр инновационных информационных технологий";
                    break;
                case "ЦИИТ Группа автоматизации":
                    result = "Центр инновационных информационных технологий";
                    break;
                case "ЦИИТ Группа развития":
                    result = "Центр инновационных информационных технологий";
                    break;
                case "ЦКПП - Центр культурно-просветительских программ":
                    result = "Центр культурно-просветительских программ";
                    break;
                case "ЦМС - Центр международного сотрудничества":
                    result = "Центр международного сотрудничества";
                    break;
                case "ЦМС ОР - Отдел развития":
                    result = "Центр международного сотрудничества";
                    break;
                case "ЦМС ОР Отдел японской культуры":
                    result = "Центр международного сотрудничества";
                    break;
                case "ЦМС Отдел международного протокола":
                    result = "Центр международного сотрудничества";
                    break;
                case "ЦМРС - Центр межрегионального сотрудничества":
                    result = "Центр межрегионального сотрудничества";
                    break;
                case "…ЗалФ… ЦМС ОР Центр славянских культур":
                    result = "Центр славянских культур 4 этаж";
                    break;
                case "…Зал… КОО Группа читального зала 3 этаж":
                    result = "Читальный зал 3 этаж";
                    break;
                case "…Зал… КОО Группа электронного зала 2 этаж":
                    result = "Электронный зал 2 этаж";
                    break;
                case "American cultural center":
                    result = "Центр американской культуры 3 этаж";
                    break;
                case "American cultural center(!)":
                    result = "Центр американской культуры 3 этаж";
                    break;
                case "Выездная библиотека":
                    result = "Центр американской культуры 3 этаж";
                    break;
                case "Франкотека":
                    result = "Культурный центр \"Франкотека\" 2 этаж";
                    break;
                case "Francothèque":
                    result = "Культурный центр \"Франкотека\" 2 этаж";
                    break;
                case "Центр славянских культур":
                    result = "Центр славянских культур 4 этаж";
                    break;
                case "КО автоматизации":
                    result = "Центр инновационных информационных технологий";
                    break;
                case "КО комплектования и ОД. Сектор комплектования – группа регистрации":
                    result = "Отдел комплектования";
                    break;
                case "КО обслуживания – зал абонементного обслуживания":
                    result = "Зал абонементного обслуживания 2 этаж";
                    break;
                case "Сектор научной реставрации":
                    result = "Отдел хранения и реставрации";
                    break;
                default:
                    result = "нет данных";
                    break;
            }

            return result;
        }
    }
}
