using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    public partial class Startup
    {
        class DataService : IDataService
        {
            private readonly ApplicationContext contexto;

            private readonly IProdutoRepository produtoRepository;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="contexto">Obejto passado pelo controlador de injeção de dependência</param>
            /// <param name="produtoRepository">Obejto passado pelo controlador de injeção de dependência</param>
            public DataService(ApplicationContext contexto, IProdutoRepository produtoRepository)
            {
                this.contexto = contexto;
                this.produtoRepository = produtoRepository;
            }

            public void InicializaDB()
            {
                contexto.Database.Migrate();

                List<Produto> livros = GetLivros();
                produtoRepository.SaveProdutos(livros);
            }

            private static List<Produto> GetLivros()
            {
                var json = File.ReadAllText("livros.json");
                var livros = JsonConvert.DeserializeObject<List<Produto>>(json);
                return livros;
            }
        }
    }
}
