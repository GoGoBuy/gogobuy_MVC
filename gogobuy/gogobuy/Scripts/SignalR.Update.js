$(function () {
	var chat = $.connection.chat;

	chat.client.update = function (ProductKey, ProductAlternatekey, EnglishProductName, Color) {
		var updateTr = $("#DataTable tr").eq(ProductKey).find("td input");
		updateTr.eq(0).val(ProductAlternatekey);
		updateTr.eq(1).val(EnglishProductName);
		updateTr.eq(2).val(Color);
	};

	$("input:text").change(function () {
		var data = $(this).closest("tr").find("td input"); chat.server.updatedata({ ProductKey: $(this).closest("tr").find("td:eq(0)").text(), ProductAlternatekey: data.eq(0).val(), EnglishProductName: data.eq(1).val(), Color: data.eq(2).val() });
	});

	$.connection.hub.start()
		.done(function () {
			chat.server.hello();
		})
		.fail(function () {
			alert("Error connecting to realtime service");
		});
});