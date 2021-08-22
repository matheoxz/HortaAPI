using Npgsql;
using System.Collections.Generic;
using System;
using HortaIoT.Models;

namespace HortaIoT.Repository{

    public class DataRepository : IDataRepository
    {
        private NpgsqlConnection _db;
        ICultivationRepository _cultivationRepository;
        public DataRepository(IDbContext dbContext, ICultivationRepository cultivationRepository){
            _db = dbContext.GetConnection();
            _cultivationRepository = cultivationRepository;
        }

        public void Add(DataModel data)
        {
            data.Received = DateTimeOffset.Now.ToUnixTimeSeconds();
            try{
                _db.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO public.measurement_data "+
                "(received, ph, ec, temperature, turbidity, illuminance, water_level)"+
                " VALUES (@received, @ph, @ec, @temperature, @turbidity, @illuminance, @water_level)", _db))
                {
                    cmd.Parameters.AddWithValue("received", data.Received);
                    cmd.Parameters.AddWithValue("ph", data.Ph);
                    cmd.Parameters.AddWithValue("ec", data.Ec);
                    cmd.Parameters.AddWithValue("temperature", data.Temperature);
                    cmd.Parameters.AddWithValue("turbidity", data.Turbidity);
                    cmd.Parameters.AddWithValue("illuminance", data.Illuminance);
                    cmd.Parameters.AddWithValue("water_level", data.WaterLevel);
                    cmd.ExecuteNonQuery();
                }
                _db.Close();
            }catch(Exception e){
                _db.Close();
                throw new InvalidOperationException(e.Message);
            }
        }

        public List<DataModel> GetAll()
        {
            List<DataModel> result = new List<DataModel>();
        
            try{
            _db.Open();
            using(var cmd = new NpgsqlCommand("SELECT * from public.measurement_data", _db))
                using(var reader = cmd.ExecuteReader())
                    while(reader.Read()){
                        result.Add(new DataModel{
                            Received = (long)reader["received"],
                            Ph = (double)reader["ph"],
                            Ec= (double)reader["ec"],
                            Temperature = (double)reader["temperature"],
                            Turbidity = (double)reader["turbidity"],
                            Illuminance = (int)reader["illuminance"],
                            WaterLevel = (int)reader["water_level"],
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

        public List<DataModel> GetByCultivation(string cultivationName)
        {
            CultivationModel cultivationModel = _cultivationRepository.GetByName(cultivationName)??
                                    throw new KeyNotFoundException();

            return GetRange(DateTimeOffset.FromUnixTimeSeconds(cultivationModel.Start), DateTimeOffset.FromUnixTimeSeconds(cultivationModel.Finish));
        }

        public List<DataModel> GetRange(DateTimeOffset start, DateTimeOffset end)
        {
            long _start = start.ToUnixTimeSeconds();
            long _end = end.ToUnixTimeSeconds();
            List<DataModel> result = new List<DataModel>();
        
            try{
            _db.Open();
            using(var cmd = new NpgsqlCommand("SELECT * from public.measurement_data WHERE received >= @start and received <= @end", _db)){
                cmd.Parameters.AddWithValue("start", _start);
                cmd.Parameters.AddWithValue("end", _end);
                using(var reader = cmd.ExecuteReader())
                    while(reader.Read()){
                        result.Add(new DataModel{
                            Received = (long)reader["received"],
                            Ph = (double)reader["ph"],
                            Ec= (double)reader["ec"],
                            Temperature = (double)reader["temperature"],
                            Turbidity = (double)reader["turbidity"],
                            Illuminance = (int)reader["illuminance"],
                            WaterLevel = (int)reader["water_level"],
                        });
                    }
            }    
            _db.Close();
            }
            catch(Exception e){
                _db.Close();
                throw new InvalidOperationException(e.Message);
            }
    
            return result;
        }
    }
}