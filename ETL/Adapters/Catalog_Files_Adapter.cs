using System;

namespace ETL
{
    public sealed class Catalog_Files_Adapter : IEntityAdapter
    {
        private readonly ComConnector connector;
        private IComWrapper cursor = null;
        public Catalog_Files_Adapter(ComConnector connector)
        {
            this.connector = connector;
        }
        private string GetQueryText()
        {
            return "ВЫБРАТЬ " + // ПЕРВЫЕ 1000
                "Код          КАК code, " +
                "Наименование КАК fnm, " +
                "ВЫРАЗИТЬ(ВладелецФайла КАК Справочник.ВнутренниеДокументы).Код КАК dog_code " +
                "ИЗ Справочник.Файлы " +
                "ГДЕ ВладелецФайла ССЫЛКА Справочник.ВнутренниеДокументы";
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
                Console.WriteLine("Справочник \"Файлы\" - нет данных для выгрузки.");
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
            return "Справочник \"Файлы\"";
        }
    }
}
