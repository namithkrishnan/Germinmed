
$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });


});

function getRootURL() {
    var n = null, i, t;
    return location.hostname == "localhost" ? n = location.protocol + "//" + location.hostname + (location.hostname == "localhost" ? ":" + location.port + "/" : "/" + location.pathname.split("/")[1]) : (i = location.pathname.split("/"), t = cleanArray(i), n = location.protocol + "//" + location.host + "/" + t[0] + "/"), n
}

function cleanArray(n) {
    for (var i = [], t = 0; t < n.length; t++)
        n[t] && i.push(n[t]);
    return i
}

var rootURL = getRootURL();

function FilterProductListByBrand(id) {
    debugger;
    //$(".product-right .row").hide();
    //$(".products .tab-content").find('div.active').hide();
    //$(".tab-content").find('div.active').html("");
    $($(".tab-content").find('div.active')[0]).find(".row").html("");
    var categoryId = $('ul.list-inline').find('li.active').attr("id");
    //var data = { brandId: id, categoryId: categoryId };
    //CategorySub
    var url = "";
    var data = {};
    if (id > 0) {
        url = rootURL + "/Product/GetAllProductsByBrand";
        data = { brandId: id, categoryId: categoryId };
    }
    else {
        url = rootURL + "/Product/CategorySub";
        data = { Id: categoryId };
    }
    $.ajax({
        type: 'GET',
        url: url,
        data: data,
        success: function (response) {
            $($(".tab-content").find('div.active')[0]).find(".row").html(response);
        }
    });

}

function FilterOfferListByBrand(id) {
    debugger;
    $($(".tab-content").find('div.active')[0]).find(".row").html("");
    var categoryId = $('ul.list-inline').find('li.active').attr("id");
    var url = "";
    var data = {};
    //if (id > 0) {
    url = "/Offer/ProductAllOffer";
    data = { Id: categoryId, BrandId: id };
    //}
    //else {
    //    url = "/Product/CategorySub";
    //    data = { Id: categoryId };
    //}
    $.ajax({
        type: 'GET',
        url: url,
        data: data,
        success: function (response) {
            $($(".tab-content").find('div.active')[0]).find(".row").html(response);
        }
    });

}
function FilterProductList(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {

            $("#divGetAllProductsByCatAction").html(response);


        }
    });

}

function FilterProductListOffer(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {

            $("#divGetAllProductsByCatActionOffer").html(response);


        }
    });

}

function AddCartQty(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {

            $("#cartTable").html(response);


        }
    });

}
function FilterCategoryList(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {

            $("#divGetAllCatByRootCategory").html(response);


        }
    });

}


$('.top-links ul li > a').click(function () {
    $('.top-links ul li').removeClass('active');
    $(this).parent().addClass('active');
});



