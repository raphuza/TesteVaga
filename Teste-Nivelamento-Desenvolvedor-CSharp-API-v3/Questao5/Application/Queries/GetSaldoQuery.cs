using Dapper;
using MediatR;
using System.Data;
using Microsoft.Data.Sqlite;

public class GetSaldoQuery : IRequest<SaldoResponse>
{
    public string IdContaCorrente { get; set; }
}

public class SaldoResponse
{
    public int Numero { get; set; }
    public string Nome { get; set; }
    public DateTime DataHora { get; set; }
    public decimal Saldo { get; set; }
}

public class GetSaldoHandler : IRequestHandler<GetSaldoQuery, SaldoResponse>
{
    private readonly IDbConnection _dbConnection;

    public GetSaldoHandler()
    {
        _dbConnection = new SqliteConnection("Data Source=database.sqlite");
    }

    public async Task<SaldoResponse> Handle(GetSaldoQuery request, CancellationToken cancellationToken)
    {
        var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
            "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = request.IdContaCorrente });

        if (conta == null) throw new Exception("INVALID_ACCOUNT");
        if (conta.Ativo == 0) throw new Exception("INACTIVE_ACCOUNT");

        var saldo = await _dbConnection.ExecuteScalarAsync<decimal>(
            "SELECT COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END), 0) FROM movimento WHERE idcontacorrente = @IdContaCorrente",
            new { IdContaCorrente = request.IdContaCorrente });

        return new SaldoResponse { Numero = conta.Numero, Nome = conta.Nome, DataHora = DateTime.UtcNow, Saldo = saldo };
    }
}
