using CRMEducacional.CreateOfertaViewModels;
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
    /// Controlador de API para gerenciar operações relacionadas a Ofertas.
    /// </summary>
    /// <remarks>
    /// Autor: Marcos Morais
    /// Data: 11 de agosto de 2024
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class OfertaController : ControllerBase
    {
        /// <summary>
        /// Obtém todos os registros de Ofertas.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Ofertas.</param>
        /// <returns>
        /// Retorna uma lista de Ofertas. Se nenhum Oferta for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var oferta = await context
                .Ofertas
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho, pois não há necessidade de rastrear mudanças nas entidades.
                .ToListAsync(); // Converte a consulta em uma lista assíncrona.

            if (oferta == null)
            {
                return NotFound(new { Message = "Nenhum registro encontrado." }); // Retorna uma resposta 404 se nenhum Oferta for encontrado.
            }

            return Ok(oferta); // Retorna um status 200 (Ok) com a lista de Ofertas.
        }

        /// <summary>
        /// Obtém um Oferta específico pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Ofertas.</param>
        /// <param name="id">Identificador único do Oferta a ser recuperado.</param>
        /// <returns>
        /// Retorna o Oferta correspondente ao ID especificado. Se o Oferta não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context, int id)
        {
            var oferta = await context
                .Ofertas
                .AsNoTracking() // Desativa o rastreamento para melhorar o desempenho.
                .FirstOrDefaultAsync(x => x.Id == id); // Recupera o primeiro Oferta que corresponde ao ID fornecido.

            if (oferta == null)
            {
                return NotFound(new { Message = "Nenhum registro encontrado." }); // Retorna uma resposta 404 se o Oferta não for encontrado.
            }

            return Ok(oferta); // Retorna um status 200 (Ok) com o Oferta encontrado.
        }

        /// <summary>
        /// Cria um novo Oferta e o salva no banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Ofertas.</param>
        /// <param name="oferta">Objeto de modelo de visualização que contém os dados do Oferta a ser criado.</param>
        /// <returns>
        /// Retorna o Oferta criado com um status 201 (Created) e a URL para acessar o Oferta criado.
        /// Se a operação falhar, retorna um status 400 (BadRequest).
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateOfertaViewModel oferta // Modelo de visualização que contém os dados necessários para criar um Oferta.
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
            }

            var newOferta = new Oferta
            {
                Nome = oferta.Nome, // Atribui o nome da Oferta.
                Descricao = oferta.Descricao, // Atribui uma descricao da Oferta.
                VagasDisponiveis = oferta.VagasDisponiveis, // Atribui a qtde de vaga.
            };

            try
            {
                await context.Ofertas.AddAsync(newOferta); // Adiciona o novo Oferta ao contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newOferta.Id }, newOferta); // Retorna o status 201 (Created) com a URL do novo Oferta.
            }
            catch
            {
                return BadRequest(); // Retorna um status 400 (BadRequest) se ocorrer algum erro durante o processo.
            }
        }


        /// <summary>
        /// Atualiza um Oferta existente.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Ofertas.</param>
        /// <param name="id">Identificador único do Oferta a ser atualizado.</param>
        /// <param name="oferta">Objeto de modelo de visualização contendo os novos dados do Oferta.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a atualização for bem-sucedida. 
        /// Se o Oferta não for encontrado ou se a operação falhar, retorna um status 400 (BadRequest) ou 404 (NotFound).
        /// </returns>
        [HttpPut("id/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            int id,
            [FromBody] CreateOfertaViewModel oferta
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna um status 400 (BadRequest) se o modelo de entrada for inválido.
            }

            var existingOferta = await context.Ofertas.FindAsync(id); // Busca o Oferta existente com base no ID fornecido.

            if (existingOferta == null)
            {
                return NotFound(new { Message = "Oferta não encontrado." }); // Retorna um status 404 (NotFound) se o Oferta não for encontrado.
            }

            // Atualiza os dados do Oferta existente com os novos dados fornecidos.
            existingOferta.Nome = oferta.Nome;
            existingOferta.Descricao = oferta.Descricao;
            existingOferta.VagasDisponiveis = oferta.VagasDisponiveis;

            try
            {
                context.Ofertas.Update(existingOferta); // Atualiza o Oferta no contexto do banco de dados.
                await context.SaveChangesAsync(); // Salva as mudanças no banco de dados de forma assíncrona.
                return NoContent(); // Retorna um status 204 (NoContent) indicando que a atualização foi bem-sucedida.
            }
            catch
            {
                return BadRequest(); // Retorna um status 400 (BadRequest) se ocorrer algum erro durante o processo.
            }
        }

        /// <summary>
        /// Remove um Oferta existente pelo ID.
        /// </summary>
        /// <param name="context">Contexto do banco de dados utilizado para acessar a tabela de Ofertas.</param>
        /// <param name="id">Identificador único do Oferta a ser removido.</param>
        /// <returns>
        /// Retorna um status 204 (NoContent) se a remoção for bem-sucedida. 
        /// Se o Oferta não for encontrado, retorna um status 404 (NotFound).
        /// </returns>
        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, int id)
        {
            var oferta = await context.Ofertas.FindAsync(id); // Busca o Oferta existente com base no ID fornecido.

            if (oferta == null)
            {
                return NotFound(new { Message = "Oferta não encontrado." }); // Retorna um status 404 (NotFound) se o Oferta não for encontrado.
            }

            try
            {
                context.Ofertas.Remove(oferta); // Remove o Oferta do contexto do banco de dados.
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