using CRMEducacional.CreateInscricaoViewModels;
using CRMEducacional.Data;
using CRMEducacional.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMEducacional.Controllers
{
    /// <summary>
    /// Controlador de API para gerenciar operações relacionadas a Inscricoes.
    /// </summary>
    /// <remarks>
    /// Autor: Marcos Morais
    /// Data: 11 de agosto de 2024
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class InscricaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InscricaoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os registros de Inscricoes.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Inscricoes.</param>
        /// <returns>
        /// Retorna uma lista de Inscricoes. Se nenhum Inscricao for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context)
        {
            var inscricao = await context
                .Inscricoes
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho, pois não há necessidade de rastrear mudanças nas entidades.
                .ToListAsync(); // Converte a consulta em uma lista assíncrona.

            if (inscricao == null)
            {
                return NotFound(new { Message = "Nenhum registro encontrado." }); // Retorna uma resposta 404 se nenhum Inscricao for encontrado.
            }

            return Ok(inscricao); // Retorna um status 200 (Ok) com a lista de Inscricoes.
        }

        /// <summary>
        /// Obtém um Inscricao específico pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Inscricoes.</param>
        /// <param name="id">Identificador único do Inscricao a ser recuperado.</param>
        /// <returns>
        /// Retorna o Inscricao correspondente ao ID especificado. Se o Inscricao não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context, int id)
        {
            var inscricao = await context
                .Inscricoes
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho.
                .FirstOrDefaultAsync(x => x.Id == id); // Recupera o primeiro Inscricao que corresponde ao ID fornecido.

            if (inscricao == null)
            {
                return NotFound($"Registro Id={id} não localizado."); // Retorna uma resposta 404 se o Inscricao não for encontrado.
            }

            return Ok(inscricao); // Retorna um status 200 (Ok) com o Inscricao encontrado.
        }

        /// <summary>
        /// Obtém todas as inscrições associadas a um CPF específico.
        /// </summary>
        [HttpGet("cpf/{cpf}")]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoesPorCpf(string cpf)
        {
            var inscricoes = await _context.Inscricoes
                                           .Include(i => i.Lead) // Inclui a entidade Lead na consulta
                                           .Where(i => i.Lead.CPF == cpf) // Filtra pelo CPF
                                           .ToListAsync();

            if (inscricoes == null || !inscricoes.Any())
            {
                return NotFound($"Registro CPF={cpf} não localizado.");
            }

            return Ok(inscricoes);
        }

        /// <summary>
        /// Obtém todas as inscrições associadas a um CPF específico.
        /// </summary>
        [HttpGet("oferta/{id}")]
        public async Task<ActionResult<IEnumerable<Inscricao>>> GetInscricoesPorOferta(int id)
        {
            var inscricoes = await _context.Inscricoes
                                           .Include(i => i.Oferta) // Inclui a entidade Lead na consulta
                                           .Where(i => i.Oferta.Id == id) // Filtra pelo CPF
                                           .ToListAsync();

            if (inscricoes == null || !inscricoes.Any())
            {
                return NotFound($"Registro Id={id} não localizado.");
            }

            return Ok(inscricoes);
        }


        /// <summary>
        /// Cria um novo Inscricao e o salva no banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Inscricoes.</param>
        /// <param name="inscricao">Objeto de modelo de visualização que contém os dados do Inscricao a ser criado.</param>
        /// <returns>
        /// Retorna o Inscricao criado com um status 201 (Created) e a URL para acessar o Inscricao criado.
        /// Se a operação falhar, retorna um status 400 (BadRequest).
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateInscricaoViewModel inscricao // Modelo de visualização que contém os dados necessários para criar um Inscricao.
        )
        {
            if (!ModelState.IsValid)
            {
                // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
                return BadRequest(ModelState);
            }

            var newInscricao = new Inscricao
            {
                NumeroDeInscricao = inscricao.NumeroDeInscricao, // Atribui o número da inscrição.
                Status = inscricao.Status, // Atribui o status da inscrição.
                ProcessoSeletivoId = inscricao.ProcessoSeletivoId, // Atribui o Processo Seletivo ID da inscrição.
                OfertaId = inscricao.OfertaId, // Atribui o Oferta ID da inscrição.
                LeadId = inscricao.LeadId // Atribui o Lead ID da inscrição.
            };

            try
            {
                await context.Inscricoes.AddAsync(newInscricao); // Adiciona o novo Inscricao ao contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                // Retorna o status 201 (Created) com a URL do novo Inscricao.
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newInscricao.Id }, newInscricao);
            }
            catch (Exception ex)
            {
                // Loga o erro ou trata exceção específica se necessário.
                // Retorna um status 500 (InternalServerError) em caso de erro inesperado.
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro ao criar a inscrição.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um Inscricao existente.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Inscricoes.</param>
        /// <param name="id">Identificador único do Inscricao a ser atualizado.</param>
        /// <param name="inscricao">Objeto de modelo de visualização contendo os novos dados do Inscricao.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a atualização for bem-sucedida. 
        /// Se o Inscricao não for encontrado ou se a operação falhar, retorna um status 400 (BadRequest) ou 404 (NotFound).
        /// </returns>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context, int id,
            [FromBody] CreateInscricaoViewModel inscricao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
            }

            var existingInscricao = await context.Inscricoes.FindAsync(id); // Busca o Inscricao existente com base no ID fornecido.

            if (existingInscricao == null)
            {
                return NotFound(new { Message = "Inscricao não encontrado." }); // Retorna um status 404 (NotFound) se o Inscricao não for encontrado.
            }

            // Atualiza os dados do Inscricao existente com os novos dados fornecidos.
            existingInscricao.NumeroDeInscricao = inscricao.NumeroDeInscricao;
            existingInscricao.Status = inscricao.Status;
            existingInscricao.LeadId = inscricao.LeadId;
            existingInscricao.ProcessoSeletivoId = inscricao.ProcessoSeletivoId;
            existingInscricao.OfertaId = inscricao.OfertaId;

            try
            {
                context.Inscricoes.Update(existingInscricao); // Atualiza o Inscricao no contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                return NoContent(); // Retorna um status 204 (NoContent) indicando que a atualização foi bem-sucedida.
            }
            catch
            {
                return BadRequest(); // Retorna um status 400 (BadRequest) se ocorrer algum erro durante o processo.
            }
        }

        /// <summary>
        /// Remove um Inscricao existente pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Inscricoes.</param>
        /// <param name="id">Identificador único do Inscricao a ser removido.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a remoção for bem-sucedida. 
        /// Se o Inscricao não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, int id)
        {
            var inscricao = await context.Inscricoes.FindAsync(id); // Busca o Inscricao existente com base no ID fornecido.

            if (inscricao == null)
            {
                // Retorna um status 404 (NotFound) se o Inscricao não for encontrado.
                return NotFound($"Registro Id={id} não localizado.");
            }

            try
            {
                context.Inscricoes.Remove(inscricao); // Remove o Inscricao do contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                return NoContent(); // Retorna um status 204 (NoContent) indicando que a remoção foi bem-sucedida.
            }
            catch
            {
                return BadRequest(); // Retorna um status 400 (BadRequest) se ocorrer algum erro durante o processo.
            }
        }
    }
}