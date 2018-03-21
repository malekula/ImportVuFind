using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExportBJ_XML.classes.BJ
{
    public static class KeyValueMapping
    {
        //это нужно превратить в словарь
        public static KeyValuePair<int, string> GetUnifiedLocation(string location)
        {
            KeyValuePair<int, string> result = new KeyValuePair<int, string>();
            //Dictionary<int, string> Locations = new Dictionary<int, string>();
            //Locations[2000] = "SDS";
            switch (location)
            {
                case "ЦМС Академия Рудомино":
                    result = new KeyValuePair<int, string>(2000, "Академия \"Рудомино\"");
                    break;
                case "…Выст… КОО Группа справочного-информационного обслуживания":
                    result = new KeyValuePair<int, string>(2001, "Выставка книг 2 этаж");
                    break;
                case "…ЗалФ… Отдел детской книги и детских программ":
                    result = new KeyValuePair<int, string>(2003, "Детский зал 2 этаж");
                    break;
                case "ЦМС ОР Дом еврейской книги":
                    result = new KeyValuePair<int, string>(2005, "Дом еврейской книги 3 этаж");
                    break;
                case "…Зал… КОО Группа абонементного обслуживания":
                    result = new KeyValuePair<int, string>(2006, "Зал абонементного обслуживания 2 этаж");
                    break;
                case "…Зал… КОО Группа выдачи документов":
                    result = new KeyValuePair<int, string>(2007, "Зал выдачи документов 2 этаж");
                    break;
                case "…Зал… КНИО Группа искусствоведения":
                    result = new KeyValuePair<int, string>(2008, "Зал искусствоведения 4 этаж");
                    break;
                case "…Зал… КНИО Группа редкой книги":
                    result = new KeyValuePair<int, string>(2009, "Зал редкой книги 4 этаж");
                    break;
                case "…ЗалФ… КНИО Группа религоведения":
                    result = new KeyValuePair<int, string>(2010, "Зал религиоведения 4 этаж");
                    break;
                case "…Хран… Сектор книгохранения - 0 этаж":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - 2 этаж":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - 3 этаж":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - 4 этаж":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - 5 этаж":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - 6 этаж":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - 7 этаж":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - Абонемент":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… Сектор книгохранения - Новая периодика":
                    result = new KeyValuePair<int, string>(2011, "Книгохранение");
                    break;
                case "…Хран… КНИО Группа хранения редкой книги":
                    result = new KeyValuePair<int, string>(2012, "Книгохранение редкой книги");
                    break;
                case "Книжный клуб":
                    result = new KeyValuePair<int, string>(2013, "Книжный клуб 1 этаж");
                    break;
                case "…ЗалФ… ЦМС ОР Культурный центр Франкотека":
                    result = new KeyValuePair<int, string>(2014, "Культурный центр \"Франкотека\" 2 этаж");
                    break;
                case "…ЗалФ… ЦМС ОР Лингвистический ресурсный центр Pearson":
                    result = new KeyValuePair<int, string>(2015, "Лингвистический ресурсный центр Pearson 3 этаж");
                    break;
                case "КНИО - Комплексный научно-исследовательский отдел":
                    result = new KeyValuePair<int, string>(2016, "Научно-исследовательский отдел");
                    break;
                case "…Обраб… КО КОД Сектор ОД - Группа инвентаризации":
                    result = new KeyValuePair<int, string>(2017, "Обработка в группе инвентаризации");
                    break;
                case "…Обраб… КО КОД Сектор ОД - Группа каталогизации":
                    result = new KeyValuePair<int, string>(2018, "Обработка в группе каталогизации");
                    break;
                case "…Обраб… КО ХКРФ Сектор микрофильмирования":
                    result = new KeyValuePair<int, string>(2019, "Обработка в группе микрофильмирования");
                    break;
                case "…Обраб… ЦИИТ Группа оцифровки":
                    result = new KeyValuePair<int, string>(2020, "Обработка в группе оцифровки");
                    break;
                case "…Обраб… КО КОД Сектор ОД - Группа систематизации":
                    result = new KeyValuePair<int, string>(2021, "Обработка в группе систематизации");
                    break;
                case "…Обраб… КО КОД Сектор комплектования":
                    result = new KeyValuePair<int, string>(2022, "Обработка в секторе комплектования");
                    break;
                case "…Обраб… КО ХКРФ Сектор научной реставрации":
                    result = new KeyValuePair<int, string>(2023, "Обработка в секторе научной реставрации");
                    break;
                case "…Хран… Сектор книгохранения - Овальный зал":
                    result = new KeyValuePair<int, string>(2024, "Овальный зал");
                    break;
                case "КО КОД - Комплексный отдел комплектования и обработки документов":
                    result = new KeyValuePair<int, string>(2025, "Отдел комплектования");
                    break;
                case "КОО - Комплексный отдел обслуживания":
                    result = new KeyValuePair<int, string>(2026, "Отдел обслуживания");
                    break;
                case "КОО Группа регистрации":
                    result = new KeyValuePair<int, string>(2026, "Отдел обслуживания");
                    break;
                case "КО ХКРФ - Комплексный отдел хранения, консервации и реставрации фондов":
                    result = new KeyValuePair<int, string>(2027, "Отдел хранения и реставрации");
                    break;
                case "ЦКПП Редакционно-издательский отдел":
                    result = new KeyValuePair<int, string>(2028, "Редакционно-издательский отдел");
                    break;
                case "КО ХКРФ Сектор книгохранения":
                    result = new KeyValuePair<int, string>(2011, "Сектор книгохранения");
                    break;
                case "КО КОД Сектор ОД":
                    result = new KeyValuePair<int, string>(2029, "Сектор обработки документов");
                    break;
                case "…Хран… ЦИИТ Сервера библиотеки":
                    result = new KeyValuePair<int, string>(2030, "Сервера библиотеки");
                    break;
                case "Д Бухгалтерия":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д Группа экспедиторского обслуживания":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д Контрактная служба":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д Отдел PR и редакция сайта":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д Отдел безопасности":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д Отдел внутреннего финансового контроля":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д Отдел по работе с персоналом":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д Отдел финансового планирования и сводной отчетности":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д УЭ - Управление по эксплуатации объектов недвижимости и обеспечения деятельности":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д УЭ Служба материально-технического обеспечения":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д УЭ Служба управления инженерными системами":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "Д УЭ Служба эксплуатации зданий и благоустройства":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "ЦБИД - Центр библиотечно-информационной деятельности и поддержки чтения":
                    result = new KeyValuePair<int, string>(2031, "Служебные подразделения");
                    break;
                case "…ЗалФ… ЦМС ОР Центр американской культуры":
                    result = new KeyValuePair<int, string>(2032, "Центр американской культуры 3 этаж");
                    break;
                case "ЦИИТ - Центр инновационных информационных технологий":
                    result = new KeyValuePair<int, string>(2033, "Центр инновационных информационных технологий");
                    break;
                case "ЦИИТ Группа IT":
                    result = new KeyValuePair<int, string>(2033, "Центр инновационных информационных технологий");
                    break;
                case "ЦИИТ Группа автоматизации":
                    result = new KeyValuePair<int, string>(2033, "Центр инновационных информационных технологий");
                    break;
                case "ЦИИТ Группа развития":
                    result = new KeyValuePair<int, string>(2033, "Центр инновационных информационных технологий");
                    break;
                case "ЦКПП - Центр культурно-просветительских программ":
                    result = new KeyValuePair<int, string>(2034, "Центр культурно-просветительских программ");
                    break;
                case "ЦМС - Центр международного сотрудничества":
                    result = new KeyValuePair<int, string>(2035, "Центр международного сотрудничества");
                    break;
                case "ЦМС ОР - Отдел развития":
                    result = new KeyValuePair<int, string>(2035, "Центр международного сотрудничества");
                    break;
                case "ЦМС ОР Отдел японской культуры":
                    result = new KeyValuePair<int, string>(2035, "Центр международного сотрудничества");
                    break;
                case "ЦМС Отдел международного протокола":
                    result = new KeyValuePair<int, string>(2035, "Центр международного сотрудничества");
                    break;
                case "ЦМРС - Центр межрегионального сотрудничества":
                    result = new KeyValuePair<int, string>(2035, "Центр межрегионального сотрудничества");
                    break;
                case "…ЗалФ… ЦМС ОР Центр славянских культур":
                    result = new KeyValuePair<int, string>(2036, "Центр славянских культур 4 этаж");
                    break;
                case "…Зал… КОО Группа читального зала 3 этаж":
                    result = new KeyValuePair<int, string>(2037, "Читальный зал 3 этаж");
                    break;
                case "…Зал… КОО Группа электронного зала 2 этаж":
                    result = new KeyValuePair<int, string>(2038, "Электронный зал 2 этаж");
                    break;
                case "American cultural center":
                    result = new KeyValuePair<int, string>(2039, "Центр американской культуры 3 этаж");
                    break;
                case "American cultural center(!)":
                    result = new KeyValuePair<int, string>(2039, "Центр американской культуры 3 этаж");
                    break;
                case "Выездная библиотека":
                    result = new KeyValuePair<int, string>(2039, "Центр американской культуры 3 этаж");
                    break;
                case "Франкотека":
                    result = new KeyValuePair<int, string>(2014, "Культурный центр \"Франкотека\" 2 этаж");
                    break;
                case "Francothèque":
                    result = new KeyValuePair<int, string>(2014, "Культурный центр \"Франкотека\" 2 этаж");
                    break;
                case "Центр славянских культур":
                    result = new KeyValuePair<int, string>(2036, "Центр славянских культур 4 этаж");
                    break;
                case "КО автоматизации":
                    result = new KeyValuePair<int, string>(2033, "Центр инновационных информационных технологий");
                    break;
                case "КО комплектования и ОД. Сектор комплектования – группа регистрации":
                    result = new KeyValuePair<int, string>(2025, "Отдел комплектования");
                    break;
                case "КО обслуживания – зал абонементного обслуживания":
                    result = new KeyValuePair<int, string>(2006, "Зал абонементного обслуживания 2 этаж");
                    break;
                default:
                    result = new KeyValuePair<int, string>(2000, "нет данных");
                    break;
            }

            return result;
        }

        public static readonly Dictionary<string, string> UnifiedLocation = new Dictionary<string, string>()
        {
            { "КО обслуживания – зал абонементного обслуживания", "Зал абонементного обслуживания 2 этаж" }
        };


        //public KeyValueMapping()
        //{
        //    LocationCodes = CreateLocationCodeDictionary();

        //}

        //private Dictionary<int, string> CreateLocationCodeDictionary()
        //{
            
        //    LocationCodes[2000] = "Академия \"Рудомино\"";
        //    LocationCodes[2001] = "Выставка книг 2 этаж";
        //    LocationCodes[2002] = "Группа МБА";
        //    LocationCodes[2003] = "Детский зал 2 этаж";
        //    LocationCodes[2004] = "Дирекция";
        //    LocationCodes[2005] = "Дом еврейской книги 3 этаж";
        //    LocationCodes[2006] = "Зал абонементного обслуживания 2 этаж";
        //    LocationCodes[2007] = "Зал выдачи документов 2 этаж";
        //    LocationCodes[2008] = "Зал искусствоведения 4 этаж";
        //    LocationCodes[2009] = "Зал редкой книги 4 этаж";
        //    LocationCodes[2010] = "Зал религиоведения 4 этаж";
        //    LocationCodes[2011] = "Книгохранение";
        //    LocationCodes[2012] = "Книгохранение редкой книги";
        //    LocationCodes[2013] = "Книжный клуб 1 этаж";
        //    LocationCodes[2014] = "Культурный центр \"Франкотека\" 2 этаж";
        //    LocationCodes[2015] = "Лингвистический ресурсный центр Pearson 3 этаж";
        //    LocationCodes[2016] = "Научно-исследовательский отдел";
        //    LocationCodes[2017] = "Обработка в группе инвентаризации";
        //    LocationCodes[2018] = "Обработка в группе каталогизации";
        //    LocationCodes[2019] = "Обработка в группе микрофильмирования";
        //    LocationCodes[2020] = "Обработка в группе оцифровки";
        //    LocationCodes[2021] = "Обработка в группе систематизации";
        //    LocationCodes[2022] = "Обработка в секторе комплектования";
        //    LocationCodes[2023] = "Обработка в секторе научной реставрации";
        //    LocationCodes[2024] = "Овальный зал";
        //    LocationCodes[2025] = "Отдел комплектования";
        //    LocationCodes[2026] = "Отдел обслуживания";
        //    LocationCodes[2027] = "Отдел хранения и реставрации";
        //    LocationCodes[2028] = "Редакционно-издательский отдел";
        //    LocationCodes[2029] = "Сектор обработки документов";
        //    LocationCodes[2030] = "Электронный доступ";
        //    LocationCodes[2031] = "Служебные подразделения";
        //    LocationCodes[2032] = "Центр американской культуры 3 этаж";
        //    LocationCodes[2033] = "Центр инновационных информационных технологий";
        //    LocationCodes[2034] = "Центр культурно-просветительских программ";
        //    LocationCodes[2035] = "Центр международного сотрудничества";
        //    LocationCodes[2036] = "Центр славянских культур 4 этаж";
        //    LocationCodes[2037] = "Читальный зал 3 этаж";
        //    LocationCodes[2038] = "Электронный зал 2 этаж";
        //    LocationCodes[2039] = "Центр американской культуры 3 этаж";
        //    return LocationCodes;
        //}
    }
}
