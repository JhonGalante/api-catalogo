using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using APICatalogo.Repository;
using AutoMapper;
using APICatalogo.DTOs;
using APICatalogo.Pagination;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnityOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        // GET: api/Produtos
        [HttpGet("produtos")]
        [HttpGet("/produtos")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos([FromQuery] ProdutoParameters produtosParameter)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameter);
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return _mapper.Map<List<ProdutoDTO>>(produtos);
        }

        // GET: api/Produtos/5
        //[HttpGet("{id}/{param2?}", Name = "ObterProduto")]
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ProdutoDTO>> GetProduto(/*[FromQuery]*/int id/*[BindRequired]string nome*/)
        {

            //throw new Exception("Exception ao retornar o produto pelo id");
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProdutoDTO>(produto);
        }

        // PUT: api/Produtos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
            {
                return BadRequest();
            }

            _uof.ProdutoRepository.Update(_mapper.Map<Produto>(produtoDTO));

            try
            {
                await _uof.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Produtos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostProduto(ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);
            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return CreatedAtAction("GetProduto", new { id = produtoDto.ProdutoId }, produtoDto);
        }

        // DELETE: api/Produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto == null)
            {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            return NoContent();
        }

        [HttpGet("menorPreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPreco()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            return _mapper.Map<List<ProdutoDTO>>(produtos);
        }

        private bool ProdutoExists(int id)
        {
            return _uof.ProdutoRepository.GetById(e => e.ProdutoId == id) != null;
        }
    }
}
