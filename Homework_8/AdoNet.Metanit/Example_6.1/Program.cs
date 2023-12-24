using System.Data;

DataSet ds = new DataSet("Store");

// таблица компаний
DataTable companiesTable = new DataTable("Companies");
// два столбца таблицы Companies
DataColumn compIdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
compIdColumn.Unique = true;
compIdColumn.AllowDBNull = false;
compIdColumn.AutoIncrement = true;
compIdColumn.AutoIncrementSeed = 1;
compIdColumn.AutoIncrementStep = 1;

DataColumn compNameColumn = new DataColumn("Name", Type.GetType("System.String"));
// добавляем столбцы
companiesTable.Columns.Add(compIdColumn);
companiesTable.Columns.Add(compNameColumn);
// добавляем таблицу в dataset
ds.Tables.Add(companiesTable);

// вторая таблица - смартфонов компаний
DataTable phonesTable = new DataTable("Phones");
DataColumn phoneIdColumn = new DataColumn("Id", Type.GetType("System.Int32"));
phoneIdColumn.Unique = true;
phoneIdColumn.AllowDBNull = false;
phoneIdColumn.AutoIncrement = true;
phoneIdColumn.AutoIncrementSeed = 1;
phoneIdColumn.AutoIncrementStep = 1;

DataColumn phoneNameColumn = new DataColumn("Name", Type.GetType("System.String"));
DataColumn phonePriceColumn = new DataColumn("Price", Type.GetType("System.Decimal"));
// столбец-внешний ключ
DataColumn phoneCompanyColumn = new DataColumn("CompanyId", Type.GetType("System.Int32"));
// добавляем столбцы в таблицу смартфонов
phonesTable.Columns.Add(phoneIdColumn);
phonesTable.Columns.Add(phoneNameColumn);
phonesTable.Columns.Add(phonePriceColumn);
phonesTable.Columns.Add(phoneCompanyColumn);
// добавляем таблицу смартфонов
ds.Tables.Add(phonesTable);

// установка отношений между таблицами
ds.Relations.Add("PhonesCompanies", companiesTable.Columns["Id"], phonesTable.Columns["CompanyId"]);

// Добавим ряд данных
DataRow apple = companiesTable.NewRow();
apple.ItemArray = new object[] { null, "Apple" };
companiesTable.Rows.Add(apple);
DataRow samsung = companiesTable.NewRow();
samsung.ItemArray = new object[] { null, "Samsung" };
companiesTable.Rows.Add(samsung);

DataRow iphone5 = phonesTable.NewRow();
iphone5.ItemArray = new object[] { null, "iPhone 5", 400, apple["Id"] };
phonesTable.Rows.Add(iphone5);

DataRow iphone6s = phonesTable.NewRow();
iphone6s.ItemArray = new object[] { null, "iPhone 6S", 600, apple["Id"] };
phonesTable.Rows.Add(iphone6s);

DataRow galaxy6 = phonesTable.NewRow();
galaxy6.ItemArray = new object[] { null, "Samsung Galaxy S6", 500, samsung["Id"] };
phonesTable.Rows.Add(galaxy6);

DataRow galaxyace2 = phonesTable.NewRow();
galaxyace2.ItemArray = new object[] { null, "Samsung Galaxy Ace 2", 200, samsung["Id"] };
phonesTable.Rows.Add(galaxyace2);

// выведем все смартфоны компании Apple
DataRow[] rows = apple.GetChildRows(ds.Relations["PhonesCompanies"]);
Console.WriteLine("Продукция компании Apple");
Console.WriteLine("Id \tСмартфон \tЦена");

foreach (DataRow r in rows)
{
    Console.WriteLine("{0} \t{1} \t{2}", r["Id"], r["Name"], r["Price"]);
}

Console.Read();

ForeignKeyConstraint foreignKey = new ForeignKeyConstraint(companiesTable.Columns["Id"], phonesTable.Columns["CompanyId"])
{
    ConstraintName = "PhonesCompaniesForeignKey",
    DeleteRule = Rule.SetNull,
    UpdateRule = Rule.Cascade
};
// добавляем внешний ключ в dataset
ds.Tables["Phones"].Constraints.Add(foreignKey);
// применяем внешний ключ
ds.EnforceConstraints = true;
 
ds.Relations.Add("PhonesCompanies", companiesTable.Columns["Id"], phonesTable.Columns["CompanyId"]);