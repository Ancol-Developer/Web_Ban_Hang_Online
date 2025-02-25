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