using CR.Models;
using Npgsql;
using System.Xml.Linq;
namespace CR.Clients
{
    public class DatabaseClient
    {
        NpgsqlConnection _Connection = new NpgsqlConnection(Const.Connection);
        public async Task InsertNewPreference(SkinInfo skininfo)
        {
            var sql = "insert into public.\"PreferenceList\"(\"SkinName\",\"Price\",\"Weapon\",\"InsertionTime\")"
                + $"values (@SkinName, @Price, @Weapon, @InsertionTime)";
            using (NpgsqlCommand _Command = new NpgsqlCommand(sql, _Connection))
            {
                _Command.Parameters.AddWithValue("SkinName", skininfo.name);
                _Command.Parameters.AddWithValue("Price", skininfo.price);
                _Command.Parameters.AddWithValue("Weapon", skininfo.weapon);
                _Command.Parameters.AddWithValue("InsertionTime", DateTime.Now);
                await _Connection.OpenAsync();
                await _Command.ExecuteNonQueryAsync();
                await _Connection.CloseAsync();
            }
        }
        public async Task DeletePrefence(string name)
        {
            var sql = "delete from public.\"PreferenceList\" where \"SkinName\" = @SkinName";
            using (NpgsqlCommand _Command = new NpgsqlCommand(sql, _Connection))
            {
                _Command.Parameters.AddWithValue("SkinName", name);
                await _Connection.OpenAsync();
                await _Command.ExecuteNonQueryAsync();
                await _Connection.CloseAsync();
            }
        }
        public async Task DeleteAllPreference()
        {
            var sql = "delete from public.\"PreferenceList\"";
            using (NpgsqlCommand _Command = new NpgsqlCommand(sql, _Connection))
            {
                await _Connection.OpenAsync();
                await _Command.ExecuteNonQueryAsync();
                await _Connection.CloseAsync();
            }
        }
        public async Task<List<SkinInfo>> GetPreferenceList()
        {
            List<SkinInfo> exerciseList = new List<SkinInfo>();
            await _Connection.OpenAsync();
            var sql = "select \"SkinName\", \"Price\", \"Weapon\" from public.\"PreferenceList\"";
            NpgsqlCommand _Command = new NpgsqlCommand(sql, _Connection);
            NpgsqlDataReader reader = await _Command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                exerciseList.Add(new SkinInfo { name = reader.GetString(0), price = reader.GetString(1), img = "", weapon = reader.GetString(2) });
            }
            await _Connection.CloseAsync();
            return exerciseList;
        }
    }
}
