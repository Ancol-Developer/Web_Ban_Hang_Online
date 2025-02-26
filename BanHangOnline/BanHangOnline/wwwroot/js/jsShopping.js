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
					}
				}
			});
		}
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