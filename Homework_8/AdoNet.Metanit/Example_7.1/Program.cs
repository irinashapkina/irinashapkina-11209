using System.Data;

DataSet ds = new DataSet("Store");
// таблица компаний
DataTable companiesTable = new DataTable("Companies");
DataColumn compIdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
DataColumn compNameColumn = new DataColumn("Name", Type.GetType("System.String"));
// добавляем столбцы
companiesTable.Columns.Add(compIdColumn);
companiesTable.Columns.Add(compNameColumn);
// добавляем таблицу в dataset
ds.Tables.Add(companiesTable);
 
// вторая таблица - смартфонов компаний
DataTable phonesTable = new DataTable("Phones");
DataColumn phoneIdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
DataColumn phoneNameColumn = new DataColumn("Name", Type.GetType("System.String"));
DataColumn phonePriceColumn = new DataColumn("Price", Type.GetType("System.Decimal"));
DataColumn phoneCompanyColumn = new DataColumn("CompanyId", Type.GetType("System.Int32"));
// добавляем столбцы в таблицу смартфонов
phonesTable.Columns.Add(phoneIdColumn);
phonesTable.Columns.Add(phoneNameColumn);
phonesTable.Columns.Add(phonePriceColumn);
phonesTable.Columns.Add(phoneCompanyColumn);
// добавляем таблицу смартфонов
ds.Tables.Add(phonesTable);
 
// Добавим ряд данных
companiesTable.Rows.Add(new object[] { 1, "Apple" });
companiesTable.Rows.Add(new object[] { 2, "Samsung" });
 
phonesTable.Rows.Add(new object[] { 1, "iPhone 5", 400, 1 });
phonesTable.Rows.Add(new object[] { 2, "iPhone 6S", 600, 1});
phonesTable.Rows.Add(new object[] { 3, "Samsung Galaxy S6", 500, 2 });
phonesTable.Rows.Add(new object[] { 4, "Samsung Galaxy Ace 2", 200, 2});
 
var query = from phone in ds.Tables["Phones"].AsEnumerable()
            from company in ds.Tables["Companies"].AsEnumerable()
            where (int)phone["CompanyId"] == (int)company["Id"]
            where (decimal)phone["Price"] >200
            select new { Model = phone["Name"], Price = phone["Price"], Company = company["Name"] };
             
foreach (var phone in query)
    Console.WriteLine("{0} ({1}) - {2}", phone.Model, phone.Company, phone.Price);
    