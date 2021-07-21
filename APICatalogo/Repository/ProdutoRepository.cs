using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto){}

        public async Task<PagedList<Produto>> GetProdutos(ProdutoParameters produtosParameter)
        {
            //return Get()
            //    .OrderBy(on => on.Nome)
            //    .Skip((produtosParameter.PageNumber - 1) * produtosParameter.PageSize)
            //    .Take(produtosParameter.PageSize)
            //    .ToList();

            return await PagedList<Produto>
                .ToPagedList(Get().OrderBy(on => on.ProdutoId), produtosParameter.PageNumber, produtosParameter.PageSize);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }
    }
}
