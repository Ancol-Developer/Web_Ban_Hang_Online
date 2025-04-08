using BanHangOnline.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BanHangOnline.Common;

public class ThongKeTruyCap
{
	private readonly string? _connectionString;
	private readonly IConfiguration _configuration;

	public ThongKeTruyCap(IConfiguration configuration)
    {
		this._configuration = configuration;
		_connectionString = _configuration.GetConnectionString("DefaultConnection");
	}

    public ThongKeViewModel? ThongKe()
	{
		using (var connect = new SqlConnection(_connectionString))
		{
			var item = connect.QueryFirstOrDefault<ThongKeViewModel>("sp_ThongKe", commandType: CommandType.StoredProcedure);
			return item;
		}
	}
}

