
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
			var itemPedido = response.itemPedido;
			let linhaDoItem = 
			debugger;
		});
	}
}

var carrinho = new Carrinho();