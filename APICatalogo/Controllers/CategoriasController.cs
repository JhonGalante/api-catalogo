using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using APICatalogo.Filter;
using Microsoft.Extensions.Logging;
using APICatalogo.Repository;
using AutoMapper;
using APICatalogo.DTOs;
using APICatalogo.Pagination;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace APICatalogo.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    //[EnableCors("PermitirApiRequest")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnityOfWork uof, /*ILogger<CategoriasController> logger*/ IMapper mapper)
        {
            _uof = uof;
            //_logger = logger;
            _mapper = mapper;
        }

        [HttpGet("/saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        // GET: api/Categorias
        [HttpGet("produtos")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            try
            {
                //_logger.LogInformation("############## GET api/categorias/produtos ##################");
                var categorias = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                //throw new Exception();
                return categoriasDto;
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias([FromQuery] CategoriaParameters categoriaParameters)
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriaParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return _mapper.Map<List<CategoriaDTO>>(categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        /// <summary>
        /// Obtem uma categoria pelo seu ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objetos categoria</returns>
        [HttpGet("{id}")]
        [EnableCors("PermitirApiRequest")]
        //[ProducesResponseType(typeof(ProdutoDTO), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            try
            {
                _logger.LogInformation($"############## GET api/categorias/id = {id} ##################");
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                return _mapper.Map<CategoriaDTO>(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados");
            }
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> PutCategoria(int id, CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.CategoriaId)
            {
                return BadRequest($"Não foi possivel atualizar a categoria com id={id}");
            }

            _uof.CategoriaRepository.Update(_mapper.Map<Categoria>(categoriaDTO));

            try
            {
                await _uof.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao tentar obter as categorias do banco de dados");
                }
            }

            return Ok($"A categoria com id={id} foi atualizada com sucesso!");
        }

        // POST: api/Categorias
        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        ///     Exemplo de request:
        ///         POST api/categorias
        ///         {
        ///             "categoriaId": 1,
        ///             "nome": "categoria1",
        ///             "imagemUrl": "https://teste.com.br/1"
        ///         }
        /// </remarks>
        /// <param name="categoriaDTO"></param>
        /// <returns>Retorna um objeto de categoria incluído</returns>
        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Categoria>> PostCategoria(CategoriaDTO categoriaDTO)
        {
            try
            {
                _uof.CategoriaRepository.Add(_mapper.Map<Categoria>(categoriaDTO));
                await _uof.Commit();

                return CreatedAtAction("GetCategoria", new { id = categoriaDTO.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar nova categoria");
            }
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoria == null)
                {
                    return NotFound();
                }

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao excluir a categoria de id={id}");
            }
        }

        private bool CategoriaExists(int id)
        {
            return _uof.CategoriaRepository.GetById(e => e.CategoriaId == id) != null;
        }
    }
}
