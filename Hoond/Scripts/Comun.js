$(function () {
    $("#cmdActualiza").click(function () {
        var msg = $("#txtPregunta").val()
        return confirm(msg);
    });
})

$(window).ready(function () {
    $(".se-pre-con").fadeOut("slow");

    $('#DialogError').dialog({
        autoOpen: false,
        modal: true,
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });
    $('#DialogError').dialog('open');
});

$(function () {
    $("#cmbPaginado").change(function () {
        var paginado = $(this).val();
        var campo = $("#cmbCampo").val();
        var cadena = $("#txtBusqueda").val();
        var loc = location.toString().split('?');
        window.location = loc[0] + '?cmbPaginado=' + encodeURIComponent(paginado) + "&cmbCampo=" + campo + "&txtBusqueda=" + cadena;
        //window.location = '@Url.Action("Index")' + '?cmbPaginado=' + encodeURIComponent(paginado) + "&cmbCampo=" + campo + "&txtBusqueda=" + cadena;
    });
});

$(function () {
    $("#idH").click(function () {
        var checkboxes = document.querySelectorAll('input[type="checkbox"]');
        if ($(this).is(":checked")) {
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].name == "ids")
                    checkboxes[i].checked = true;
            }
        }
        else {
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].name == "ids")
                    checkboxes[i].checked = false;
            }
        }
    });
});

$(function () {
    $("#cmdEliminar").click(function () {
        var checkboxes = document.querySelectorAll('input[type="checkbox"]');
        var count = 0;
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].checked && checkboxes[i].name == "ids")
                count++;
        }
        if (count != 0) {
            var msg = 'Esta seguro de eliminar {0} registros?';
            //var msg = "@Resources.Content.msg_ElimarM";
            msg = String.format(msg, count);
            return confirm(msg);
        }
        else {
            return false;
        }
    });
});

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
}

function OpenDialog(Obj, Campo) {
    $('#Dialog' + Obj).dialog({
        autoOpen: false,
        modal: true,
        buttons: {
            "Guardar": function () {
                var Item = $('#' + Campo).val();
                $.ajax({
                    url: "/Comun/Insert" + Obj,
                    data: { Value: Item },
                    type: 'POST',
                    success: function (data) {
                        llena(data, Obj);
                    }
                });
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            $('#' + Campo).val("");
        }
    });
    $('#Dialog' + Obj).dialog('open');
}

function OpenDialog(Obj, Campo, cPadre) {
    var idPadre = $("#id" + cPadre).val();
    if (idPadre != '') {
        $('#Dialog' + Obj).dialog({
            autoOpen: false,
            modal: true,
            buttons: {
                "Guardar": function () {
                    var Item = $('#' + Campo).val();
                    $.ajax({
                        url: "/Comun/Insert" + Obj,
                        data: { Value: Item, idPadre: idPadre },
                        type: 'POST',
                        success: function (data) {
                            llena(data, Obj, idPadre);
                        }
                    });
                    $(this).dialog("close");
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            },
            close: function () {
                $('#' + Campo).val("");
            }
        });
        $('#Dialog' + Obj).dialog('open');
    }
    else {
        alert("Debe selecionar un(a) " + cPadre);
    }
}

function OpenDialogM(Obj, Campos, cPadre) {
    var idPadre = $("#id" + cPadre).val();
    if (idPadre != '') {
        $('#Dialog' + Obj).dialog({
            autoOpen: false,
            modal: true,
            buttons: {
                "Guardar": function () {
                    var arr = Campos.split(',');
                    var Item = "";
                    for (var i = 0; i < arr.length; i++) {
                        Item = Item + $('#' + arr[i]).val() + ",";
                    }
                    $.ajax({
                        url: "/Comun/Insert" + Obj,
                        data: { Value: Item, idPadre: idPadre },
                        type: 'POST',
                        success: function (data) {
                            llena(data, Obj, idPadre);
                        }
                    });
                    $(this).dialog("close");
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            },
            close: function () {
                var arr = Campos.split(',');
                for (var i = 0; i < arr.length; i++) {
                    $('#' + arr[i]).val("");
                }
            }
        });
        $('#Dialog' + Obj).dialog('open');
    }
    else {
        alert("Debe selecionar un(a) " + cPadre);
    }
}

function llena(Item, Obj) {
    var url = Obj;
    if (Obj == "Color" || Obj == "Talla")
        Obj = "clave" + Obj;
    else
        Obj = "id" + Obj;
    Item = Item.split("|");
    $.ajax({
        type: "POST",
        url: "/Comun/Get" + url,
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#" + Obj).empty();
            var option = $(document.createElement('option'));
            option.val(Item[0]);
            option.text(Item[1]);
            $("#" + Obj).append(option);
            $(data).each(function () {
                if (Item[0] != this.id) {
                    option = $(document.createElement('option'));
                    option.text(this.valor);
                    option.val(this.id);
                    $("#" + Obj).append(option);
                }
            });
        }
    });
}

function llena(Item, Obj, idPadre) {
    var url = Obj;
    if (Obj == "Color" || Obj == "Talla")
        Obj = "clave" + Obj;
    else
        Obj = "id" + Obj;
    Item = Item.split("|");
    $.ajax({
        type: "POST",
        url: "/Comun/Get" + url,
        data: JSON.stringify({ idPadre: idPadre }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#" + Obj).empty();
            var option = $(document.createElement('option'));
            option.val(Item[0]);
            option.text(Item[1]);
            $("#" + Obj).append(option);
            $(data).each(function () {
                if (Item[0] != this.id) {
                    option = $(document.createElement('option'));
                    option.text(this.valor);
                    option.val(this.id);
                    $("#" + Obj).append(option);
                }
            });
        },
        error: function (data, status, jqXHR) {
            alert('There was an error.' + data + status + jqXHR);
        }
    });
}

function LlenaSC(idPadre, Obj) {
    if (idPadre == '') {
        idPadre = 0;
    }
    var Objs = Obj.split(',');
    for (var i = 0; i < Objs.length; i++) {
        LlenaC(idPadre, Objs[i]);
    }
}

function LlenaC(idPadre, Obj) {
    var url = Obj;
    if (Obj == "Color" || Obj == "Talla")
        Obj = "clave" + Obj;
    else
        Obj = "id" + Obj;
    $("#" + Obj).empty();
    $.ajax({
        type: "POST",
        url: "/Comun/Get" + url,
        data: JSON.stringify({ idPadre: idPadre }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var option = $(document.createElement('option'));
            option.text("");
            option.val("");
            $("#" + Obj).append(option);
            $(data).each(function () {
                option = $(document.createElement('option'));
                option.text(this.valor);
                option.val(this.id);
                $("#" + Obj).append(option);
            });
        },
        error: function (data, status, jqXHR) {
            alert('There was an error.' + data + status + jqXHR);
        }
    });
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

