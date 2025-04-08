$(document).ready(function () {
	ShowCount();
	$('body').on('click', '.btnAddToCart', function (e) {
		e.preventDefault();
		var id = $(this).data('id');
		var quantity = 1;
		var txtQuantity = $('#quantity_value').text();
		if (txtQuantity != '') {
			quantity = parseInt(txtQuantity);
		}

		$.ajax({
			url: '/shoppingcart/addtocart',
			type: 'POST',
			data: { id: id, quantity: quantity },
			success: function (rs) {
				if (rs.success) {
					$('#checkout_items').html(rs.count);
					alert(rs.msg)
				}
			}
		});
	});

	$('body').on('click', '.btnDeleteCart', function (e) {
		e.preventDefault();
		var id = $(this).data('id');
		var conf = confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng không?");

		if (conf == true) {
			$.ajax({
				url: '/shoppingcart/Delete',
				type: 'POST',
				data: { id: id },
				success: function (rs) {
					if (rs.success) {
						$('#checkout_items').html(rs.count);
						$('#trow_' + id).remove();
						LoadCart();
					}
				}
			});
		}
	});

	$('body').on('click', '.btnDeleteAll', function (e) {
		e.preventDefault();
		var id = $(this).data('id');
		var conf = confirm("Bạn có chắc muốn xóa toàn bộ sản phẩm này khỏi giỏ hàng không?");

		if (conf == true) {
			DeleteAll();
		}
	});

	$('body').on('click', '.btnUpdateCart', function (e) {
		e.preventDefault();
		var id = $(this).data('id');
		var quantity = $('#Quantity_' + id).val();
		Update(id, quantity);
	});
});

function ShowCount() {
	$.ajax({
		url: '/shoppingcart/ShowCount',
		type: 'GET',
		success: function (rs) {
			$('#checkout_items').html(rs.count);
		}
	});
}

function DeleteAll() {
	$.ajax({
		url: '/shoppingcart/DeleteAll',
		type: 'POST',
		success: function (rs) {
			if (rs.success) {
				LoadCart();
			}
		}
	});
}

function Update(id, quantity) {
	$.ajax({
		url: '/shoppingcart/Update',
		data: { id: id , quantity: quantity},
		type: 'POST',
		success: function (rs) {
			if (rs.success) {
				LoadCart();
			}
		}
	});
}

function LoadCart() {
	$.ajax({
		url: '/shoppingcart/Partial_Item_View',
		type: 'GET',
		success: function (rs) {
			$('#load_data').html(rs);
		}
	});
}