
$(function () {
    $("#loaderbody").addClass('hide');
 
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
        });

  
});




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
        url = "/Product/GetAllProductsByBrand";
        data = { brandId: id, categoryId: categoryId };
    }
    else {
        url = "/Product/CategorySub";
        data = { Id: categoryId };
    }
    $.ajax({
        type: 'GET',
        url: url,
        data : data,
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



