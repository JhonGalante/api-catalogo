﻿using APICatalogo.Context;
using APICatalogo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests
{
    public class DBUnitTestsMockInitialize
    {
        public DBUnitTestsMockInitialize()
        {

        }
        public void Seed(AppDbContext context)
        {
            context.Categorias.Add
            (new Categoria { CategoriaId = 999, Nome = "Bebidas999", ImagemUrl = "bebidas999.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 2, Nome = "Sucos", ImagemUrl = "sucos1.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 3, Nome = "Doces", ImagemUrl = "doces1.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 4, Nome = "Salgados", ImagemUrl = "Salgados1.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 5, Nome = "Tortas", ImagemUrl = "tortas1.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 6, Nome = "Bolos", ImagemUrl = "bolos1.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 7, Nome = "Lanches", ImagemUrl = "lanches1.jpg" });

            context.SaveChanges();
        }
    }
}
