using System;

namespace ETL
{
    public sealed class Catalog_InternalDocument_Adapter : IEntityAdapter
    {
        private readonly ComConnector connector;
        private IComWrapper cursor = null;
        public Catalog_InternalDocument_Adapter(ComConnector connector)
        {
            this.connector = connector;
        }
        private string GetQueryText()
        {
            return "ВЫБРАТЬ " + // ПЕРВЫЕ 1000
                "Код                                   КАК code, " +
                "РегистрационныйНомер                  КАК regnom, " +
                "ДатаСогласованияФАСС                  КАК dt, " +
                "Папка.Код                             КАК folder_code, " +
                "Папка.Наименование                    КАК folder_dsc, " +
                "Подготовил.Наименование               КАК Auth_FIO, " +
                "Подразделение.Код                     КАК dep_code, " +
                "Подразделение.Наименование            КАК dep_dsc, " +
                "Корреспондент.Код                     КАК crsp_code, " +
                "Корреспондент.Наименование            КАК crsp_dsc, " +
                "ДатаНачалаДействия                    КАК from_dt, " +
                "ДатаОкончанияДействия                 КАК to_dt, " +
                "ОтветственныйЗаСделку.Наименование    КАК otw_dsc, " +
                "ОбъектДоговора.Код                    КАК dobj_code, " +
                "ОбъектДоговора.Наименование           КАК dobj_dsc, " +
                "Заказчик.Код                          КАК cust_code, " +
                "Заказчик.Наименование                 КАК customer, " +
                "ПРЕДСТАВЛЕНИЕ(СтатусСогласованияФАСС) КАК sost_dsc, " +
                "НомерСоглаованияФАСС                  КАК nomer, " +
                "Заголовок                             КАК naimen, " +
                "ГрифДоступа.Наименование              КАК grif, " +
                "ВидДокумента.Наименование             КАК wid_dsc, " +
                "ДатаРегистрации                       КАК ot_dt, " +
                "Организация.Код                       КАК org_code, " +
                "Организация.Наименование              КАК org_dsc, " +
                "ВопросДеятельности.Код                КАК ask_code, " +
                "ВопросДеятельности.Наименование       КАК ask_dsc, " +
                "Сумма                                 КАК summa, " +
                "НеДействует                           КАК flg_rast, " +
                "Рарус_НомерСделки                     КАК SdlNom, " +
                "Рарус_КодУСО                          КАК uso, " +
                "Рарус_НаличиеЖивогоДоговора           КАК flg_est, " +
                "ДопСоглашение                         КАК flg_dop, " +
                "Комментарий                           КАК descrip " +
                "ИЗ Справочник.ВнутренниеДокументы";
        }
        public IComWrapper GetCursor()
        {
            IComWrapper query = connector.NewObject("Запрос");
            query.Set("Текст", GetQueryText());
            //query.Call("УстановитьПараметр", "Дата1", new DateTime(2010, 3, 1, 0, 0, 0));
            //query.Call("УстановитьПараметр", "Дата2", new DateTime(2010, 10, 1, 0, 0, 0));
            IComWrapper result = query.CallAndWrap("Выполнить");
            if ((bool)result.Call("Пустой"))
            {
                result.Dispose();
                query.Dispose();
                return null;
            }
            cursor = result.CallAndWrap("Выбрать");

            result.Dispose();
            query.Dispose();

            return cursor;
        }
        public override string ToString()
        {
            return "Справочник \"Внутренние документы\"";
        }
    }
}

