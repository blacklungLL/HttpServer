using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace MyORMLibrary;

public class ORMContext<T> where T : class, new()
{
    private readonly IDbConnection _dbConnection;

    public ORMContext(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public T ReadById(int id, string tableName)
    {
        string query = $"SELECT * FROM Users WHERE Id=@id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return Map(reader);
            }
        }
        return new T();
    }
    
    public List<T> ReadAll(string tableName)
    {
        List<T> results = new List<T>();
        string sql = $"SELECT * FROM {tableName}";
        try
        {
            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                _dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(Map(reader));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            _dbConnection.Close();
        }
        return results;
    }
    
    public T Create(T entity)
    {
        var properties = typeof(T).GetProperties();
        var columnNames = string.Join(", ", properties.Select(p => p.Name));
        var parameterNames = string.Join(", ", properties.Select(p => "@" + p.Name));

        string sql = $"INSERT INTO {typeof(T).Name} ({columnNames}) VALUES ({parameterNames})";

        try
        {
            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                foreach (var property in properties)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = $"@{property.Name}";
                    parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }

                _dbConnection.Open();
                var result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int newId))
                {
                    var idProperty = typeof(T).GetProperty("id");
                    idProperty?.SetValue(entity, newId);
                }
            }
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            _dbConnection.Close();
        }

        return entity;
    }
      
    public void Update<T>(int id, T entity, string tableName)
    {
        using (SqlConnection connection = new SqlConnection(_dbConnection.ConnectionString))
        {
            connection.Open();
            string sql = $"UPDATE {tableName} SET Column1 = @value1 WHERE Id = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@value1", "значение");
  
            command.ExecuteNonQuery();
        }
    }
    public List<T> Where(Expression<Func<T, bool>> predicate)
    {
        var sqlQuery = ExpressionParser<T>.BuildSqlQuery(predicate, singleResult: false);
        return ExecuteQueryMultiple(sqlQuery).ToList();
    }
    
    public T FirstOrDefault(Expression<Func<T, bool>> predicate)
    {
        var sqlQuery = ExpressionParser<T>.BuildSqlQuery(predicate, singleResult: true);
        return ExecuteQuerySingle(sqlQuery);
    }
    
    public int Delete(int id, string tableName)
    {
        string query = "DELETE FROM Users WHERE Id=@id";
    
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query; 
            var parameter = command.CreateParameter(); 
            parameter.ParameterName = "@id"; 
            parameter.Value = id; 
            command.Parameters.Add(parameter);
            
            _dbConnection.Open(); 
            return command.ExecuteNonQuery();
        }
    }
    
    private T ExecuteQuerySingle(string query)
    {
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close();
        }

        return null;
    }

    private IEnumerable<T> ExecuteQueryMultiple(string query)
    {
        var results = new List<T>();
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    results.Add(Map(reader));
                }
            }
            _dbConnection.Close();
        }
        return results;
    }
    
    private T Map(IDataReader reader)
    {
        var entity = new T();
        var props = typeof(T).GetProperties();

        foreach (var property in props)
        {
            if (reader[property.Name] != DBNull.Value)
            {
                property.SetValue(entity, reader[property.Name]);
            }
        }
        return entity;
    }

    public T ReadMovieById(int id)
    {
        string query = $"SELECT * FROM films WHERE id=@id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return Map(reader);
            }
        }

        return null;
    }
    
    public void CreateMovie(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name != "id" && p.Name != "Id"); 
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => '@' + p.Name));
        string query = $"INSERT INTO {typeof(T).Name} ({columns}) VALUES ({values})";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    public void CreateMovieData(T entity, string tableName)
    {
        var properties = entity.GetType().GetProperties();
        var columns = string.Join(", ", properties.Select(p => p.Name));
        var values = string.Join(", ", properties.Select(p => '@' + p.Name));
        string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    
    public T GetById(int Id)
    {
        string query = $"SELECT * FROM {typeof(T).Name} WHERE id = @Id";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Id";
            parameter.Value = Id;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }
    
    public void Update(T entity)
    {
        var properties = entity.GetType().GetProperties()
            .Where(p => p.Name != "id");
        var values = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        string query = $"UPDATE {typeof(T).Name} SET {values} WHERE id = @Id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@Id";
            idParameter.Value = entity.GetType().GetProperty("id").GetValue(entity);
            command.Parameters.Add(idParameter);
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    public void Delete(string id, string tableName)
    {
        string query = $"DELETE FROM {tableName} WHERE id = @id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    public void DeleteMovieData(string id, string tableName)
    {
        string query = $"DELETE FROM {tableName} WHERE id = @id";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    public void Delete(T entity)
    {
        var properties = entity.GetType().GetProperties();
        var condition = string.Join(" AND ", properties.Select(p => $"{p.Name} = @{p.Name}"));
        string query = $"DELETE FROM {typeof(T).Name} WHERE {condition}";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            foreach (var property in properties)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = '@' + property.Name;
                parameter.Value = property.GetValue(entity);
                command.Parameters.Add(parameter);
            }
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    public void DeleteMovie(string id, string tableName)
    {
        string query = $"DELETE FROM {tableName} WHERE id = @MovieId";
        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@MovieId";
            parameter.Value = id;
            command.Parameters.Add(parameter);
            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
    
    public List<T> GetAll()
    {
        string query = $"SELECT * FROM {typeof(T).Name}";
        using (var command = _dbConnection.CreateCommand())
        {
            var result = new List<T>();
            command.CommandText = query;
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(Map(reader));
                }
                _dbConnection.Close();
                return result;
            }
        }
    }
    
    public T GetByName(string Name)
    {
        string query = $"SELECT * FROM {typeof(T).Name} WHERE name = @Name";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Name";
            parameter.Value = Name;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }
    
    public T GetByTitle(string title)
    {
        string query = $"SELECT * FROM {typeof(T).Name} WHERE title = @title";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@title";
            parameter.Value = title;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }
    
    public T GetByGenreName(string GenreName)
    {
        string query = $"SELECT * FROM {typeof(T).Name} WHERE name = @GenreName";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@GenreName";
            parameter.Value = GenreName;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }
    
    public T GetByCountryName(string CountryName)
    {
        string query = $"SELECT * FROM {typeof(T).Name} WHERE name = @CountryName";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@CountryName";
            parameter.Value = CountryName;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }
    
    public T GetUserByLogin(string Login)
    {
        string query = $"SELECT * FROM {typeof(T).Name} WHERE Login = @Login";

        using (var command = _dbConnection.CreateCommand())
        {
            command.CommandText = query;
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@Login";
            parameter.Value = Login;
            command.Parameters.Add(parameter);

            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return Map(reader);
                }
            }
            _dbConnection.Close(); // test
        }

        return null;
    }
}

