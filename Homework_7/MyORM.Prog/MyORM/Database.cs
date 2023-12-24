using System.Data;
using System.Reflection;
using Npgsql;

namespace MyORM;

public class Database
{
    public IDbConnection _connection = null;
    public IDbCommand _cmd = null;

    public Database(string connectionString)
    {
        _connection = new NpgsqlConnection(connectionString);
        _cmd = _connection.CreateCommand();
    }
    public void Insert<T>(T model)
    {
        
        using (_connection)
        {
            PropertyInfo[] modelFields = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            List<string> parametrs = modelFields.Select(x => $"@{x.Name}").ToList();

            string sqlExpression = $"INSERT INTO {typeof(T).Name}s ({string.Join(", ", modelFields.Select(p => p.Name))}) VALUES ({string.Join(", ", parametrs)})";

            foreach (PropertyInfo parametr in modelFields)
            {
                _cmd.Parameters.Add(new NpgsqlParameter($"@{parametr.Name}", parametr.GetValue(model)));
            }

            _cmd.CommandText = sqlExpression;

            _connection.Open();
            _cmd.ExecuteNonQuery();
        }
    }

    public IEnumerable<T> Select<T>()
    {
        IList<T> list = new List<T>();
        Type t = typeof(T);

        using (_connection)
        {
            string sqlExpression = $"SELECT * FROM {typeof(T).Name}";

            _cmd.CommandText = sqlExpression;
    
            _connection.Open();
            var reader = _cmd.ExecuteReader();
            while (reader.Read())
            {
                T obj = (T)Activator.CreateInstance(t);
                t.GetProperties().ToList().ForEach(x => x.SetValue(obj, reader[x.Name]));
        
                list.Add(obj);
            }
        }

        return list;
    }
    public T SelectById<T>(int id)
    {
        Type type = typeof(T);
        T obj = default(T);
        using (_connection)
        {
            string sqlExpression = $"SELECT * FROM {typeof(T).Name}s WHERE id = {id}";
            _cmd.CommandText = sqlExpression;
            
            _connection.Open();
            var reader = _cmd.ExecuteReader();
            while (reader.Read())
            {
                obj = (T)Activator.CreateInstance(type);
                type.GetProperties().ToList().ForEach(x => x.SetValue(obj, reader[type.Name]));
                break;
            }

            return obj;
        }
    }

    public bool DeleteById<T>(int id)
    {
        Type type = typeof(T);
        T obj = default(T);
        using (_connection)
        {
            string sqlExpression = $"DELETE FROM {typeof(T).Name}s WHERE id = {id}";
            _cmd.CommandText = sqlExpression;
            
            _connection.Open();
            var reader = _cmd.ExecuteNonQuery();
            return reader > 0;
        }
        return true;
    }
    public void Update<T>(T t)
    {
        using (_connection)
        {
            PropertyInfo[] modelFields = t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            List<string> parametrs = modelFields.Select(x => $"@{x.Name}").ToList();

            string sqlExpression = $"UPDATE {typeof(T).Name}s SET ({string.Join(", ", modelFields
                .Select((p,i) => p.Name + '=' + parametrs[i]))}) WHERE id = @Id";

            foreach (PropertyInfo parametr in modelFields)
            {
                _cmd.Parameters.Add(new NpgsqlParameter($"@{parametr.Name}", parametr.GetValue(t)));
            }

            _cmd.CommandText = sqlExpression;

            _connection.Open();
            _cmd.ExecuteNonQuery();
        }
    }
}