﻿using APICatalogo.Models;
using APICatalogo.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos (ProdutoParameters produtosParameter);
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
}
