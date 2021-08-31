using Npgsql;
using System.Collections.Generic;
using System;
using HortaIoT.Models;


namespace HortaIoT.Repository{

    public class CultivationRepository : ICultivationRepository
    {
        private NpgsqlConnection _db;
        public CultivationRepository(IDbContext dbContext){
            _db = dbContext.GetConnection();
        }

        public void Add(CultivationModel cultivation)
        {
            try{
                _db.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO public.plant_cultivation "+
                "(name, description, start, finish)"+
                " VALUES (@name, @description, @start, @finish)", _db))
                {
                    cmd.Parameters.AddWithValue("name", cultivation.Name);
                    cmd.Parameters.AddWithValue("description", cultivation.Description);
                    cmd.Parameters.AddWithValue("start", cultivation.Start);
                    cmd.Parameters.AddWithValue("finish", cultivation.Finish);
                    cmd.ExecuteNonQuery();
                }
                _db.Close();
            }catch(Exception e){
                _db.Close();
                throw new InvalidOperationException(e.Message);
            }
        }

        public void Delete(string name)
        {
            try{
                 CultivationModel cultivation = GetByName(name)??
                                    throw new KeyNotFoundException();

                _db.Open();
                using(var cmd = new NpgsqlCommand("DELETE FROM public.plant_cultivation WHERE name = @name", _db)){
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.ExecuteNonQuery();
                }
            }catch(KeyNotFoundException){
                _db.Close();
                throw new KeyNotFoundException();
            }catch (Exception e){
                _db.Close();
                throw new InvalidOperationException(e.Message);
            }
        }

        public List<CultivationModel> GetAll()
        {
            List<CultivationModel> result = new List<CultivationModel>();
        
            try{
            _db.Open();
            using(var cmd = new NpgsqlCommand("SELECT * from public.plant_cultivation", _db))
                using(var reader = cmd.ExecuteReader())
                    while(reader.Read()){
                        result.Add(new CultivationModel{
                            Name = (string)reader["name"],
                            Id = (int)reader["id"],
                            Description = (string)reader["description"],
                            Start = (long)reader["start"],
                            Finish = (long)reader["finish"],
                        });
                    }
            _db.Close();
            }
            catch(Exception e){
                _db.Close();
                throw new InvalidOperationException(e.Message);
            }
    
            return result;
        }

        public CultivationModel GetByName(string name)
        {
            try{
                _db.Open();
                using(var cmd = new NpgsqlCommand("SELECT * from public.plant_cultivation WHERE name = @name", _db)){
                    cmd.Parameters.AddWithValue("name", name);
                    using(var reader = cmd.ExecuteReader()){
                        CultivationModel result = new CultivationModel();
                        while(reader.Read()){
                                result.Name = (string)reader["name"];
                                result.Id = (int)reader["id"];
                                result.Description = (string)reader["description"];
                                result.Start = (long)reader["start"];
                                result.Finish = (long)reader["finish"];
                        }
                        _db.Close();
                        if(result.Id == 0) return null;
                        return result;
                    }
                }
            }
            catch(Exception e){
                _db.Close();
                throw new InvalidOperationException(e.Message);
            }
        
        }

        public void Update(string name, CultivationModel cultivationModel)
        {
            try{
                CultivationModel cultivation = GetByName(name)??
                                    throw new KeyNotFoundException();

                _db.Open();
                using (var cmd = new NpgsqlCommand("Update public.plant_cultivation set "+
                "(name, description, start, finish)"+
                " = (@name, @description, @start, @finish)"+
                "WHERE name = @oldName", _db))
                {
                    cmd.Parameters.AddWithValue("name", cultivationModel.Name);
                    cmd.Parameters.AddWithValue("description", cultivationModel.Description);
                    cmd.Parameters.AddWithValue("start", cultivationModel.Start);
                    cmd.Parameters.AddWithValue("finish", cultivationModel.Finish);
                    cmd.Parameters.AddWithValue("oldName", name);
                    cmd.ExecuteNonQuery();
                }
                _db.Close();
            }catch(KeyNotFoundException){
                _db.Close();
                throw new KeyNotFoundException();
            }catch(Exception e){
                _db.Close();
                throw new InvalidOperationException(e.Message);
            }
        }
    }
}
