using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto){}

        public PagedList<Produto> GetProdutos(ProdutoParameters produtosParameter)
        {
            //return Get()
            //    .OrderBy(on => on.Nome)
            //    .Skip((produtosParameter.PageNumber - 1) * produtosParameter.PageSize)
            //    .Take(produtosParameter.PageSize)
            //    .ToList();

            return PagedList<Produto>
                .ToPagedList(Get().OrderBy(on => on.ProdutoId), produtosParameter.PageNumber, produtosParameter.PageSize);
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}
