using PickleAndHope.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace PickleAndHope.DataAccessLayer
{
    public class PickleRepo
    {
        string connectionString;

        public PickleRepo(IConfiguration config)
        {
            connectionString = config.GetConnectionString("PickleAndHope");
        }

        static List<Pickle> _pickles = new List<Pickle>() 
        { 
            new Pickle() 
            { 
                Id = 1, 
                Type = "Dill", 
                NumberInStock = 9, 
                Price = 1.25m, 
                Size = "large" 
            } 
        };

        public Pickle Add(Pickle pickle)
        {
            var sql = @"insert into Pickle (NumberInStock, Price, Size, [Type])
                        output inserted.*
                        values (@NumberInStock,@Price,@Size,@Type)";

            using (var db = new SqlConnection(connectionString))
            {
                // Dapper
                var result = db.QueryFirstOrDefault<Pickle>(sql, pickle);
                return result;

                //ADO
                //db.Open();
                //var cmd = db.CreateCommand();
                //cmd.CommandText = sql;

                //cmd.Parameters.AddWithValue("NumberInStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("Price", pickle.Price);
                //cmd.Parameters.AddWithValue("Size", pickle.Size);
                //cmd.Parameters.AddWithValue("Type", pickle.Type);

                //var reader = cmd.ExecuteReader();

                //if(reader.Read())
                //{
                //    var newPickle = MapReaderToPickle(reader);
                //    return newPickle;
                //}
                //return null;
        }
    }

        public void Remove(string type)
        {
            throw new NotImplementedException();
        }

        public Pickle Update(Pickle pickle)
        {
            var sql = @"update Pickle
                        set numberInStock = numberInStock + @NewStock
                        output inserted.*
                        where id = @Id";

            using (var db = new SqlConnection(connectionString))
            {
                // Dapper
                var parameters = new
                { 
                    NewStock = pickle.NumberInStock,
                    Id = pickle.Id 
                };

                return db.QueryFirstOrDefault<Pickle>(sql, parameters);


                // ADO
                //db.Open();

                //var cmd = db.CreateCommand();
                //cmd.CommandText = sql;

                //cmd.Parameters.AddWithValue("NewStock", pickle.NumberInStock);
                //cmd.Parameters.AddWithValue("Id", pickle.Id);

                //var reader = cmd.ExecuteReader();

                //if(reader.Read())
                //{
                //    var updatedPickle = MapReaderToPickle(reader);
                //    return updatedPickle;
                //}
                //return null;
            }
        }

        public Pickle GetByType(string typeOfPickle)
        {
            using (var db = new SqlConnection(connectionString)) // iDisposable closes connection after closing } without explicitly writing connection.Close(), use when there are multiple return options
            {
                //db.Open();

                //var query = @$"select *
                //          from pickle
                //          where Type = '{type}'"; // string interpolation leaves you vulnerable to sql injection attacks.  Don't do this or Bobby Tables will get you

                // Dapper
                var query = @"select *
                          from pickle
                          where Type = @Type";

                var parameters = new { Type = typeOfPickle };
                var typeResult = db.QueryFirstOrDefault<Pickle>(query, parameters);
                return typeResult;

                // ADO
                //var cmd = db.CreateCommand();
                //cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("Type", typeOfPickle);

                //var reader = cmd.ExecuteReader();

                //if (reader.Read())
                //{
                //    var pickleByType = MapReaderToPickle(reader);
                //    return pickleByType;
                //}
                //return null;
            }
        }

        public List<Pickle> GetAll()
        {   
            // Dappper
            using (var db = new SqlConnection(connectionString))
            { 
                return db.Query<Pickle>("select * from pickle").ToList();
            }

            // ADO
            //// Sql connection
            //var connection = new SqlConnection(connectionString);
            //connection.Open();

            //// Sql command
            //var cmd = connection.CreateCommand();
            //cmd.CommandText = "select * from pickle"; // tsql query

            //// Sql data reader
            //var reader =  cmd.ExecuteReader();
            //var pickles = new List<Pickle>();

            //while (reader.Read())
            //{
            //    //var id = reader.GetInt32(); // requires column #, not advised because if the columns ever change you will have an issue
            //    //var id = (int)reader["id"];
            //    //var type = (string)reader["type"];
            //    var pickle = MapReaderToPickle(reader);
            //    pickles.Add(pickle);
            //}
            //connection.Close(); //super important, don't forget this step

            //return pickles;
        }

        public Pickle GetById(int id)
        {
            using (var db = new SqlConnection(connectionString))
            {
                // Dapper
                var idQuery = @"select * 
                                from Pickle 
                                where id = @id";
                var pickle = db.QueryFirstOrDefault<Pickle>(idQuery, new { Id = id});
                return pickle;

                // ADO
                //db.Open();
                //var cmd = db.CreateCommand();

                //cmd.CommandText = idQuery;
                //cmd.Parameters.AddWithValue("id", id);

                //var reader = cmd.ExecuteReader();
                //if(reader.Read())
                //{
                //    return MapReaderToPickle(reader);
                //}
                //return null;
            }
        }

        Pickle MapReaderToPickle(SqlDataReader reader)
        {
            var pickle = new Pickle
            {
                Id = (int)reader["id"],
                Type = (string)reader["type"],
                Price = (decimal)reader["price"],
                NumberInStock = (int)reader["numberInStock"],
                Size = (string)reader["size"]
            };
            return pickle;
        }
    }
}
