using CRMEducacional.CreateLeadViewModels;
using CRMEducacional.Data;
using CRMEducacional.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMEducacional.Controllers
{
    /// <summary>
    /// Controlador de API para gerenciar operações relacionadas a Leads.
    /// </summary>
    /// <remarks>
    /// Autor: Marcos Morais
    /// Data: 11 de agosto de 2024
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : ControllerBase
    {
        /// <summary>
        /// Obtém todos os registros de Leads.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Leads.</param>
        /// <returns>
        /// Retorna uma lista de Leads. Se nenhum Lead for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var lead = await context
                .Leads
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho, pois não há necessidade de rastrear mudanças nas entidades.
                .ToListAsync(); // Converte a consulta em uma lista assíncrona.

            if (lead == null)
            {
                return NotFound(new { Message = "Nenhum registro encontrado." }); // Retorna uma resposta 404 se nenhum Lead for encontrado.
            }

            return Ok(lead); // Retorna um status 200 (Ok) com a lista de Leads.
        }

        /// <summary>
        /// Obtém um Lead específico pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Leads.</param>
        /// <param name="id">Identificador único do Lead a ser recuperado.</param>
        /// <returns>
        /// Retorna o Lead correspondente ao ID especificado. Se o Lead não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, int id)
        {
            var lead = await context
                .Leads
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho.
                .FirstOrDefaultAsync(x => x.Id == id); // Recupera o primeiro Lead que corresponde ao ID fornecido.

            if (lead == null)
            {
                return NotFound(new { Message = "Nenhum registro encontrado." }); // Retorna uma resposta 404 se o Lead não for encontrado.
            }

            return Ok(lead); // Retorna um status 200 (Ok) com o Lead encontrado.
        }

        /// <summary>
        /// Cria um novo Lead e o salva no banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Leads.</param>
        /// <param name="lead">Objeto de modelo de visualização que contém os dados do Lead a ser criado.</param>
        /// <returns>
        /// Retorna o Lead criado com um status 201 (Created) e a URL para acessar o Lead criado.
        /// Se a operação falhar, retorna um status 400 (BadRequest).
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateLeadViewModel lead // Modelo de visualização que contém os dados necessários para criar um Lead.
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
            }

            var newLead = new Lead
            {
                Nome = lead.Nome, // Atribui o nome do Lead.
                Email = lead.Email, // Atribui o email do Lead.
                Telefone = lead.Telefone, // Atribui o telefone do Lead.
                CPF = lead.CPF // Atribui o CPF do Lead.
            };

            try
            {
                await context.Leads.AddAsync(newLead); // Adiciona o novo Lead ao contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.

                return CreatedAtAction(nameof(GetByIdAsync), new { id = newLead.Id }, newLead); // Retorna o status 201 (Created) com a URL do novo Lead.
            }
            catch
            {
                return BadRequest(); // Retorna um status 400 (BadRequest) se ocorrer algum erro durante o processo.
            }
        }

        /// <summary>
        /// Atualiza um Lead existente.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Leads.</param>
        /// <param name="id">Identificador único do Lead a ser atualizado.</param>
        /// <param name="lead">Objeto de modelo de visualização contendo os novos dados do Lead.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a atualização for bem-sucedida. 
        /// Se o Lead não for encontrado ou se a operação falhar, retorna um status 400 (BadRequest) ou 404 (NotFound).
        /// </returns>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            int id,
            [FromBody] CreateLeadViewModel lead
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
            }

            var existingLead = await context.Leads.FindAsync(id); // Busca o Lead existente com base no ID fornecido.

            if (existingLead == null)
            {
                return NotFound(new { Message = "Lead não encontrado." }); // Retorna um status 404 (NotFound) se o Lead não for encontrado.
            }

            // Atualiza os dados do Lead existente com os novos dados fornecidos.
            existingLead.Nome = lead.Nome;
            existingLead.Email = lead.Email;
            existingLead.Telefone = lead.Telefone;
            existingLead.CPF = lead.CPF;

            try
            {
                context.Leads.Update(existingLead); // Atualiza o Lead no contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                return NoContent(); // Retorna um status 204 (NoContent) indicando que a atualização foi bem-sucedida.
            }
            catch
            {
                return BadRequest(); // Retorna um status 400 (BadRequest) se ocorrer algum erro durante o processo.
            }
        }

        /// <summary>
        /// Remove um Lead existente pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Leads.</param>
        /// <param name="id">Identificador único do Lead a ser removido.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a remoção for bem-sucedida. 
        /// Se o Lead não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, int id)
        {
            var lead = await context.Leads.FindAsync(id); // Busca o Lead existente com base no ID fornecido.

            if (lead == null)
            {
                return NotFound(new { Message = "Lead não encontrado." }); // Retorna um status 404 (NotFound) se o Lead não for encontrado.
            }

            try
            {
                context.Leads.Remove(lead); // Remove o Lead do contexto do banco de dados.
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