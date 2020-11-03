using CasaDoCodigo.Models;
using CasaDoCodigo.Models.VIewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="produtoRepository">Obejto passado pelo controlador de injeção de dependência</param>
        public PedidoController(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository, IItemPedidoRepository itemPedidoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
        }

        public IActionResult Carrossel()
        {
            return View(produtoRepository.GetProdutos());
        }

        public IActionResult Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
                pedidoRepository.AddItem(codigo);

            Pedido pedido = pedidoRepository.GetPedido();
            //Passado a lista de objetos para a view de carrinho
            List<ItemPedido> itens = pedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }

        public IActionResult Cadastro()
        {
            var pedido = pedidoRepository.GetPedido();

            //Redirecionando o usuário para tela principal caso não haja pedido instanciado
            if (pedido is null)
                return RedirectToAction("Carrossel");

            return View(pedido.Cadastro);
        }

        //O HTTPPost restringe para que a página só seja acessada através de um método e não digitando o endereço
        [HttpPost]
        //Validando token contra ataques maliciosos utilizando token do cliente
        [ValidateAntiForgeryToken]
        public IActionResult Resumo(Cadastro cadastro)
        {
            //Validando modelo no lado do servidor
            if (ModelState.IsValid)
                return View(pedidoRepository.UpdateCadastro(cadastro));
            else
                return RedirectToAction("Cadastro");
        }

        [HttpPost]
        //Validando token contra ataques maliciosos utilizando token do cliente
        [ValidateAntiForgeryToken]

        public UpdateQuantidadeResponse UpdateQuantidade([FromBody] ItemPedido itemPedido)
        {
            return pedidoRepository.UpdateQuantidade(itemPedido);
        }
    }
}
