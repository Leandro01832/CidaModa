$(document).ready(function () {

    $("#btn_dados").click(function () {
        $("#DadosPessoais").delay('800').fadeIn("slow");
        //  $("#Pedidos").delay('800').fadeOut("slow");  
    });

    $("#btn_pedidos").click(function () {
        $("#Pedidos").delay('800').fadeIn("slow");
        //  $("#DadosPessoais").delay('800').fadeOut("slow");
    });
});