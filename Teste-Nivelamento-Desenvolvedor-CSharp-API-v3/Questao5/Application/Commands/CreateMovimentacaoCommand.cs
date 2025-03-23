using Dapper;
using MediatR;
using System.Data;
using Microsoft.Data.Sqlite;

public class CreateMovimentacaoCommand : IRequest<string>
{
    public string IdContaCorrente { get; set; }
    public string TipoMovimento { get; set; }
    public decimal Valor { get; set; }
}

public class CreateMovimentacaoHandler : IRequestHandler<CreateMovimentacaoCommand, string>
{
    private readonly IDbConnection _dbConnection;

    public CreateMovimentacaoHandler()
    {
        _dbConnection = new SqliteConnection("Data Source=database.sqlite");
    }

    public async Task<string> Handle(CreateMovimentacaoCommand request, CancellationToken cancellationToken)
    {
        if (request.Valor <= 0) throw new Exception("INVALID_VALUE");
        if (request.TipoMovimento != "C" && request.TipoMovimento != "D") throw new Exception("INVALID_TYPE");

        var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
            "SELECT * FROM contacorrente WHERE idcontacorrente = @Id", new { Id = request.IdContaCorrente });

        if (conta == null) throw new Exception("INVALID_ACCOUNT");
        if (conta.Ativo == 0) throw new Exception("INACTIVE_ACCOUNT");

        var idMovimento = Guid.NewGuid().ToString();
        await _dbConnection.ExecuteAsync(
            "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
            new { IdMovimento = idMovimento, request.IdContaCorrente, DataMovimento = DateTime.UtcNow.ToString("dd/MM/yyyy"), request.TipoMovimento, request.Valor });

        return idMovimento;
    }
}
