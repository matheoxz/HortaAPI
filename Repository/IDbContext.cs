using Npgsql;

namespace HortaIoT.Repository{

    public interface IDbContext{
        
        NpgsqlConnection GetConnection();

    }
}