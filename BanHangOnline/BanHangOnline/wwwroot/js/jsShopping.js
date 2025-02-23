$(document).ready(function () {
	$('body').on('click', '.btnAddToCart', function (e) {
		e.preventDefault();
		var id = $(this).data('id');
		var quantity = 1;
		var txtQuantity = $('#quantity_value').text();
		if (txtQuantity != '') {
			quantity = parseInt(txtQuantity);
		}
		alert(id + " " + quantity);
	});
});