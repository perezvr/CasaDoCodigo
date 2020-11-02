using CasaDoCodigo.Models;
using CasaDoCodigo.Models.VIewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IPedidoRepository
    {
        Pedido GetPedido();
        void AddItem(string codigo);
        UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido);
    }

    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        /*
         * Esse componente é responsável por acessar o objeto da sessão
         * **/
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IItemPedidoRepository itemPedidoRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="contextAccessor">Está registrado automaticamente e está sendo passado por injeção de dependência</param>
        public PedidoRepository(ApplicationContext contexto, IHttpContextAccessor contextAccessor, IItemPedidoRepository itemPedidoRepository) : base(contexto)
        {
            this.contextAccessor = contextAccessor;
            this.itemPedidoRepository = itemPedidoRepository;

        }

        public void AddItem(string codigo)
        {
            Produto produto = contexto.Set<Produto>()
                .Where(p => p.Codigo.Equals(codigo))
                .SingleOrDefault();

            if (produto is null)
                throw new ArgumentException("Produto nao encontrado");

            var pedido = GetPedido();

            var itemPedido = contexto.Set<ItemPedido>()
                .Where(i => i.Produto.Codigo.Equals(codigo) && i.Pedido.Equals(pedido.Id))
                .SingleOrDefault();

            if (itemPedido is null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
                contexto.Set<ItemPedido>()
                    .Add(itemPedido);

                contexto.SaveChanges();
            }
        }

        public Pedido GetPedido()
        {
            //Verificando se já há um pedido instanciado na sessão
            var pedidoId = GetPedidoId();

            var pedido = dbSet
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .Where(p => p.Id.Equals(pedidoId))
                .SingleOrDefault();

            if (pedido is null)
            {
                pedido = new Pedido();
                dbSet.Add(pedido);
                contexto.SaveChanges();
                SetPedidoId(pedido.Id);
            }

            return pedido;
        }

        private int? GetPedidoId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }

        public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
        {
            var itemPedidoDB = itemPedidoRepository.GetItemPedido(itemPedido.Id);

            if (itemPedidoDB != null)
            {
                itemPedidoDB.AtualizaQuantidade(itemPedido.Quantidade);
                contexto.SaveChanges();

                CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(GetPedido().Itens);

                return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
            }

            throw new ArgumentException("ItemPedido não encontrado");
        }
    }
}