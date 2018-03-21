using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.classes.DB;

namespace ExportBJ_XML.QueriesText
{
    public class Bibliojet
    {
        public static readonly string SELECT_RECORD_QUERY =
            "select A.*,cast(A.MNFIELD as nvarchar(10))+A.MSFIELD code,A.SORT,B.PLAIN, C.IDLEVEL level_id, " +
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
                "  ,A.MNFIELD, A.MSFIELD , F.NAME RusFieldName" +
                " from " + DatabaseWrapper.Fund + "..DATAEXT A" +
                " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                " left join " + DatabaseWrapper.Fund + "..MAIN C on A.IDMAIN = C.ID " +
                " left join " + DatabaseWrapper.Fund + "..FIELDS F on A.MNFIELD = F.MNFIELD and A.MSFIELD = F.MSFIELD " +
                " where A.IDMAIN = @idmain " +
                " order by IDMAIN, IDDATA";

        public static readonly string IMPORT_CLARIFY_10a = " select * from " + DatabaseWrapper.Fund + "..DATAEXT A " +
                           " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                           " where A.MNFIELD = 10 and A.MSFIELD = '$b' and A.IDDATA = @iddata";


        public static readonly string IMPORT_CLARIFY_517a = " select B.PLAIN from " + DatabaseWrapper.Fund + "..DATAEXT A " +
                                " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where MNFIELD = 517 and MSFIELD = '$b' and A.IDDATA = @iddata";

        public static readonly string IMPORT_CLARIFY_205a_1 = " select * from " + DatabaseWrapper.Fund + "..DATAEXT A " +
                                " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where A.MNFIELD = 205 and A.MSFIELD = '$b' and A.IDDATA = @iddata";

        public static readonly string IMPORT_CLARIFY_205a_2 = " select * from " + DatabaseWrapper.Fund + "..DATAEXT A " +
                        " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 205 and A.MSFIELD = '$f' and A.IDDATA = @iddata";

        public static readonly string IMPORT_CLARIFY_205a_3 = " select * from " + DatabaseWrapper.Fund + "..DATAEXT A " +
                        " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 205 and A.MSFIELD = '$g' and A.IDDATA = @iddata";

        public static readonly string IMPORT_CLARIFY_606a = "select * " +
                                " from " + DatabaseWrapper.Fund + "..TPR_CHAIN A " +
                                " left join " + DatabaseWrapper.Fund + "..TPR_TES B on A.IDTES = B.ID " +
                                " where A.IDCHAIN = @idchain" +
                                " order by IDORDER";

        public static readonly string GET_ALL_EXEMPLARS = " select distinct A.IDMAIN, A.IDDATA from " + DatabaseWrapper.Fund + "..DATAEXT A" +
                            " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                            " where A.IDMAIN = @idmain and (A.MNFIELD = 899 and A.MSFIELD = '$p' or A.MNFIELD = 899 and A.MSFIELD = '$a' or A.MNFIELD = 899 and A.MSFIELD = '$w') " +
                            " and not exists (select 1 from BJVVV..DATAEXT C where C.IDDATA = A.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c' and C.SORT = 'Списано')";

        public static readonly string GET_EXEMPLAR = " select * from " + DatabaseWrapper.Fund + "..DATAEXT A" +
                        " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                        " where A.IDDATA = @iddata";

        public static readonly string GET_HYPERLINK = " select * from " + DatabaseWrapper.Fund + "..DATAEXT A" +
                    " left join " + DatabaseWrapper.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                    " where A.MNFIELD = 940 and A.MSFIELD = '$a' and A.IDMAIN = @idmain";

        public static readonly string GET_BOOK_SCAN_INFO = " select * from BookAddInf..ScanInfo A" +
                    " where A.IDBase = @idbase and A.IDBook = @idmain" ;

        public static readonly string GET_ALL_IDMAIN_WITH_IMAGES = "select IDMAIN from " + DatabaseWrapper.Fund + "..IMAGES";

        public static readonly string GET_IMAGE = "select IDMAIN, PIC from " + DatabaseWrapper.Fund + "..IMAGES where IDMAIN = @idmain";

        public static readonly string GET_PARENT_PIN = " select * from " + DatabaseWrapper.Fund + "..DATAEXT " +
                           " where MNFIELD = 225 and MSFIELD = '$a' and IDMAIN = @ParentPIN";

        public static readonly string GET_MAX_IDMAIN = "select max(ID) from " + DatabaseWrapper.Fund + "..MAIN";

    }



}
