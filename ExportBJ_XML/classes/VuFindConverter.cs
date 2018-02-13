using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security;

namespace ExportBJ_XML.classes
{

    public abstract class VuFindConverter
    {
        public string Fund
        { 
            get 
            {
                return this._fund; 
            }
            protected set
            {
                this._fund = value;
            }
        }
        private string _fund = "unknown";
        protected XmlWriter _objXmlWriter;
        protected XmlDocument _exportDocument;
        protected XmlNode _doc;
        protected XmlNode _root;

        public Dictionary<int, string> LocationCodes;

        public event EventHandler RecordExported;
        //public event EventHandler<VuFindConverterEventArgs> OnDatabaseTimeout;
        //event EventHandler<VuFindConverterEventArgs> OnConvertError;

        public VuFindConverter() 
        {
            LocationCodes = new Dictionary<int, string>();
            LocationCodes[2000] = "Академия \"Рудомино\"";
            LocationCodes[2001] = "Выставка книг 2 этаж";
            LocationCodes[2002] = "Группа МБА";
            LocationCodes[2003] = "Детский зал 2 этаж";
            LocationCodes[2004] = "Дирекция";
            LocationCodes[2005] = "Дом еврейской книги 3 этаж";
            LocationCodes[2006] = "Зал абонементного обслуживания 2 этаж";
            LocationCodes[2007] = "Зал выдачи документов 2 этаж";
            LocationCodes[2008] = "Зал искусствоведения 4 этаж";
            LocationCodes[2009] = "Зал редкой книги 4 этаж";
            LocationCodes[2010] = "Зал религиоведения 4 этаж";
            LocationCodes[2011] = "Книгохранение";
            LocationCodes[2012] = "Книгохранение редкой книги";
            LocationCodes[2013] = "Книжный клуб 1 этаж";
            LocationCodes[2014] = "Культурный центр \"Франкотека\" 2 этаж";
            LocationCodes[2015] = "Лингвистический ресурсный центр Pearson 3 этаж";
            LocationCodes[2016] = "Научно-исследовательский отдел";
            LocationCodes[2017] = "Обработка в группе инвентаризации";
            LocationCodes[2018] = "Обработка в группе каталогизации";
            LocationCodes[2019] = "Обработка в группе микрофильмирования";
            LocationCodes[2020] = "Обработка в группе оцифровки";
            LocationCodes[2021] = "Обработка в группе систематизации";
            LocationCodes[2022] = "Обработка в секторе комплектования";
            LocationCodes[2023] = "Обработка в секторе научной реставрации";
            LocationCodes[2024] = "Овальный зал";
            LocationCodes[2025] = "Отдел комплектования";
            LocationCodes[2026] = "Отдел обслуживания";
            LocationCodes[2027] = "Отдел хранения и реставрации";
            LocationCodes[2028] = "Редакционно-издательский отдел";
            LocationCodes[2029] = "Сектор обработки документов";
            LocationCodes[2030] = "Электронный доступ";
            LocationCodes[2031] = "Служебные подразделения";
            LocationCodes[2032] = "Центр американской культуры 3 этаж";
            LocationCodes[2033] = "Центр инновационных информационных технологий";
            LocationCodes[2034] = "Центр культурно-просветительских программ";
            LocationCodes[2035] = "Центр международного сотрудничества";
            LocationCodes[2036] = "Центр славянских культур 4 этаж";
            LocationCodes[2037] = "Читальный зал 3 этаж";
            LocationCodes[2038] = "Электронный зал 2 этаж";
            LocationCodes[2039] = "Центр американской культуры 3 этаж";
            


        }

        public abstract void Export();
        public abstract void ExportSingleRecord(string idRecord);
        public abstract void ExportCovers();

        protected virtual void OnRecordExported(EventArgs e)
        {
            EventHandler handler = RecordExported;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void AddField(string name, string val)
        {
            XmlNode field = _exportDocument.CreateElement("field");
            XmlAttribute att = _exportDocument.CreateAttribute("name");
            att.Value = name;
            field.Attributes.Append(att);
            field.InnerText = SecurityElement.Escape(val);
            val = Extensions.XmlCharacterWhitelist(val);
            field.InnerText = val;
            _doc.AppendChild(field);
        }

        public static string GetFundId(string fund)
        {

            switch (fund)
            {
                case "BJVVV":
                    return "5000";
                case "REDKOSTJ":
                    return "5001";
                case "BJACC":
                    return "5003";
                case "BJFCC":
                    return "5004";
                case "BJSCC":
                    return "5005";
                case "BRIT_SOVET":
                    return "5002";
                case "litres":
                    return "5007";
                case "period":
                    return "5006";
                case "HJB":
                    return "5009";
                case "pearson":
                    return "5008";
            }
            return "<неизвестный фонд>";
        }


    }
}
