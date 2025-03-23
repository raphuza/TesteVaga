using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/conta-corrente")]
public class ContaCorrenteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContaCorrenteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("movimentacao")]
    public async Task<IActionResult> MovimentarConta([FromBody] CreateMovimentacaoCommand command)
    {
        try
        {
            var idMovimento = await _mediator.Send(command);
            return Ok(new { idMovimento });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpGet("saldo/{idContaCorrente}")]
    public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
    {
        try
        {
            var response = await _mediator.Send(new GetSaldoQuery { IdContaCorrente = idContaCorrente });
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}
