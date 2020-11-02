using System.Collections.Generic;
using System.Linq;

namespace CasaDoCodigo.Models.VIewModels
{
    public class CarrinhoViewModel
    {
        public CarrinhoViewModel(IList<ItemPedido> items)
        {
            Items = items;
        }

        public IList<ItemPedido> Items { get; }

        public decimal Total => Items.Sum(i => i.Quantidade * i.PrecoUnitario);
    }
}
