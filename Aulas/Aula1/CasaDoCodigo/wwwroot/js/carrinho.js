
class Carrinho {
	clickIncremento(button) {
		let data = this.getData(button);
		data.Quantidade++;
		this.postQuantidade(data);
	}

	clickDecremento(button) {
		let data = this.getData(button);
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
		var novaQuantidade = $(linhaDoItem).find('input').val();

		return {
			id: itemId,
			Quantidade: novaQuantidade
		};
	}

	postQuantidade(data) {

		//Criando um objeto de cabeçalho para enviar o token para o servidor
		let token = $('[name=__RequestVerificationToken]').val();
		let headers = {};
		headers['RequestVerificationToken'] = token;

		$.ajax({
			url: '/pedido/updatequantidade',
			type: 'POST',
			contentType: 'application/json',
			data: JSON.stringify(data),
			headers: headers
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
		});
	}
}

var carrinho = new Carrinho();

Number.prototype.duasCasas = function () {
	return this.toFixed(2).replace('.', ',');
}