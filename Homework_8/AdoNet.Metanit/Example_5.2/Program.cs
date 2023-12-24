using System.Data;

DataSet bookStore = new DataSet("BookStore");
DataTable booksTable = new DataTable("Books");
// добавляем таблицу в dataset
bookStore.Tables.Add(booksTable);

// создаем столбцы для таблицы Books
DataColumn idColumn = new DataColumn("Id", Type.GetType("System.Int32"));
idColumn.Unique = true; // столбец будет иметь уникальное значение
idColumn.AllowDBNull = false; // не может принимать null
idColumn.AutoIncrement = true; // будет автоинкрементироваться
idColumn.AutoIncrementSeed = 1; // начальное значение
idColumn.AutoIncrementStep = 1; // приращении при добавлении новой строки

DataColumn nameColumn = new DataColumn("Name", Type.GetType("System.String"));
DataColumn priceColumn = new DataColumn("Price", Type.GetType("System.Decimal"));
priceColumn.DefaultValue = 100; // значение по умолчанию
DataColumn discountColumn = new DataColumn("Discount", Type.GetType("System.Decimal"));
discountColumn.Expression = "Price * 0.2";

booksTable.Columns.Add(idColumn);
booksTable.Columns.Add(nameColumn);
booksTable.Columns.Add(priceColumn);
booksTable.Columns.Add(discountColumn);
// определяем первичный ключ таблицы books
booksTable.PrimaryKey = new DataColumn[] { booksTable.Columns["Id"] };

DataRow row = booksTable.NewRow();
row.ItemArray = new object[] { null, "Война и мир", 200 };
booksTable.Rows.Add(row); // добавляем первую строку
booksTable.Rows.Add(new object[] { null, "Отцы и дети", 170 }); // добавляем вторую строку

Console.Write("\tИд \tНазвание \tЦена \tСкидка");
Console.WriteLine();
foreach (DataRow r in booksTable.Rows)
{
    foreach (var cell in r.ItemArray)
        Console.Write("\t{0}", cell);
    Console.WriteLine();
}

Console.Read();