using CRMEducacional.CreateProcessoSeletivoViewModels;
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
    /// Controlador de API para gerenciar operações relacionadas a ProcessoSeletivos.
    /// </summary>
    /// <remarks>
    /// Autor: Marcos Morais
    /// Data: 11 de agosto de 2024
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessoSeletivoController : ControllerBase
    {
        /// <summary>
        /// Obtém todos os registros de ProcessosSeletivos.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de ProcessosSeletivos.</param>
        /// <returns>
        /// Retorna uma lista de ProcessosSeletivos. Se nenhum ProcessoSeletivo for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var processoseletivo = await context
                .ProcessosSeletivos
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho, pois não há necessidade de rastrear mudanças nas entidades.
                .ToListAsync(); // Converte a consulta em uma lista assíncrona.

            if (processoseletivo == null)
            {
                return NotFound(new { Message = "Nenhum registro encontrado." }); // Retorna uma resposta 404 se nenhum ProcessoSeletivo for encontrado.
            }

            return Ok(processoseletivo); // Retorna um status 200 (Ok) com a lista de ProcessoSeletivos.
        }

        /// <summary>
        /// Obtém um ProcessoSeletivo específico pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de ProcessosSeletivos.</param>
        /// <param name="id">Identificador único do ProcessoSeletivo a ser recuperado.</param>
        /// <returns>
        /// Retorna o ProcessoSeletivo correspondente ao ID especificado. Se o ProcessoSeletivo não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, int id)
        {
            var processoseletivo = await context
                .ProcessosSeletivos
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho.
                .FirstOrDefaultAsync(x => x.Id == id); // Recupera o primeiro ProcessoSeletivo que corresponde ao ID fornecido.

            if (processoseletivo == null)
            {
                return NotFound(new { Message = "Nenhum registro encontrado." }); // Retorna uma resposta 404 se o ProcessoSeletivo não for encontrado.
            }

            return Ok(processoseletivo); // Retorna um status 200 (Ok) com o ProcessoSeletivo encontrado.
        }

        /// <summary>
        /// Cria um novo ProcessoSeletivo e o salva no banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de ProcessosSeletivos.</param>
        /// <param name="processoseletivo">Objeto de modelo de visualização que contém os dados do ProcessoSeletivo a ser criado.</param>
        /// <returns>
        /// Retorna o ProcessoSeletivo criado com um status 201 (Created) e a URL para acessar o ProcessoSeletivo criado.
        /// Se a operação falhar, retorna um status 400 (BadRequest).
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateProcessoSeletivoViewModel processoseletivo // Modelo de visualização que contém os dados necessários para criar um ProcessoSeletivo.
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
            }

            var newProcessoSeletivo = new ProcessoSeletivo
            {
                Nome = processoseletivo.Nome, // Atribui o nome do ProcessoSeletivo.
                DataInicio = processoseletivo.DataInicio, // Atribui a DataInicio do ProcessoSeletivo.
                DataTermino = processoseletivo.DataTermino // Atribui a DataTermino do ProcessoSeletivo.
            };

            try
            {
                await context.ProcessosSeletivos.AddAsync(newProcessoSeletivo); // Adiciona o novo ProcessoSeletivo ao contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newProcessoSeletivo.Id }, newProcessoSeletivo);
            }
            catch (Exception ex)
            {
                // Loga o erro ou trata exceção específica se necessário.
                // Retorna um status 500 (InternalServerError) em caso de erro inesperado.
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro ao criar o processo seletivo.", detail = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um ProcessoSeletivo existente.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de ProcessosSeletivos.</param>
        /// <param name="id">Identificador único do ProcessoSeletivo a ser atualizado.</param>
        /// <param name="processoseletivo">Objeto de modelo de visualização contendo os novos dados do ProcessoSeletivo.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a atualização for bem-sucedida. 
        /// Se o ProcessoSeletivo não for encontrado ou se a operação falhar, retorna um status 400 (BadRequest) ou 404 (NotFound).
        /// </returns>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context, int id,
            [FromBody] CreateProcessoSeletivoViewModel processoseletivo
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
            }

            var existingProcessoSeletivo = await context.ProcessosSeletivos.FindAsync(id); // Busca o ProcessoSeletivo existente com base no ID fornecido.

            if (existingProcessoSeletivo == null)
            {
                return NotFound(new { Message = "ProcessoSeletivo não encontrado." }); // Retorna um status 404 (NotFound) se o ProcessoSeletivo não for encontrado.
            }

            // Atualiza os dados do ProcessoSeletivo existente com os novos dados fornecidos.
            existingProcessoSeletivo.Nome = processoseletivo.Nome;
            existingProcessoSeletivo.DataInicio = processoseletivo.DataInicio;
            existingProcessoSeletivo.DataTermino = processoseletivo.DataTermino;



            try
            {
                context.ProcessosSeletivos.Update(existingProcessoSeletivo); // Atualiza o ProcessoSeletivo no contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                return NoContent(); // Retorna um status 204 (NoContent) indicando que a atualização foi bem-sucedida.
            }
            catch
            {
                return BadRequest(); // Retorna um status 400 (BadRequest) se ocorrer algum erro durante o processo.
            }
        }

        /// <summary>
        /// Remove um ProcessoSeletivo existente pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de ProcessosSeletivos.</param>
        /// <param name="id">Identificador único do ProcessoSeletivo a ser removido.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a remoção for bem-sucedida. 
        /// Se o ProcessoSeletivo não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, int id)
        {
            var processoseletivo = await context.ProcessosSeletivos.FindAsync(id); // Busca o ProcessoSeletivo existente com base no ID fornecido.

            if (processoseletivo == null)
            {
                return NotFound(new { Message = "ProcessoSeletivo não encontrado." }); // Retorna um status 404 (NotFound) se o ProcessoSeletivo não for encontrado.
            }

            try
            {
                context.ProcessosSeletivos.Remove(processoseletivo); // Remove o ProcessoSeletivo do contexto do banco de dados.
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