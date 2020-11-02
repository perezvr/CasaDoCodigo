using CasaDoCodigo.Models.VIewModels;

namespace CasaDoCodigo.Models
{
    /// <summary>
    /// Objeto reponsável por devolver dados para uma requisição
    /// </summary>
    public class UpdateQuantidadeResponse
    {
        public UpdateQuantidadeResponse(ItemPedido itemPedido, CarrinhoViewModel carrinhoViewModel)
        {
            ItemPedido = itemPedido;
            CarrinhoViewModel = carrinhoViewModel;
        }
        /// <summary>
        /// Utilizado para atualizar informações na View sem necessitar recarregar a página
        /// </summary>
        public ItemPedido ItemPedido { get; }
        /// <summary>
        /// Utilizado para atualizar informações na View sem necessitar recarregar a página
        /// </summary>
        public CarrinhoViewModel CarrinhoViewModel { get; }
    }
}
