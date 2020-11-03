
class Carrinho {
	clickIncremento(btn) {
		let data = this.getData(btn);
		data.Quantidade++;
		this.postQuantidade(data);
	}

	clickDecremento(btn) {
		let data = this.getData(btn);
		data.Quantidade--;
		this.postQuantidade(data);
	}

	updateQuantidade(input) {
		let data = this.getData(input);
		this.postQuantidade(data);
	}

	getData(elemento) {
		//Pegando o parent do btn que possui a informação do item-id
		var linhaDoItem = $(elemento).parents('[item-id]');
		var itemId = $(linhaDoItem).attr('item-id');
		var novaQtde = $(linhaDoItem).find('input').val();

		return {
			id: itemId,
			Quantidade: novaQtde
		};
	}

	postQuantidade(data) {
		$.ajax({
			url: '/pedido/updatequantidade',
			type: 'POST',
			contentType: 'application/json',
			data: JSON.stringify(data)
		}).done(function (response) {
			//Extraindo o itemPedido do response da requisição
			var itemPedido = response.itemPedido;

			//Pegando o elemento HTML do item do pedido
			let linhaDoItem = $('[item-id=' + itemPedido.id + ']');
			//alterando o valor do input de qtde
			linhaDoItem.find('input').val(itemPedido.quantidade);
			//alterando o valor do subtotal
			linhaDoItem.find('[subtotal]').html((itemPedido.subTotal).duasCasas());

			//Entraindo o carrinhoVIewModel do response da requisição
			let carrinhoViewModel = response.carrinhoViewModel;
			//alterando o total de itens
			$('[numero-itens]').html('Total:' + carrinhoViewModel.items.length + 'item(s)');
			//alterando o total do pedido
			$('[total]').html((carrinhoViewModel.total).duasCasas());

			//removendo o elemento do itemPedido caso esteja com a quantidade zerada
			if (itemPedido.quantidade == 0)
				linhaDoItem.remove();

			debugger;
		});
	}
}

var carrinho = new Carrinho();

Number.prototype.duasCasas = function () {
	return this.toFixed(2).replace('.', ',');
}