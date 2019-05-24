﻿
$(function () {
    $("#loaderbody").addClass('hide');
 
    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
        });

  
});




function FilterProductListByBrand(id) {
    $.ajax({
        type: 'GET',
        url: "/Product/GetAllProductsByBrand/"+id,
        success: function (response) {
           
            $("#divGetAllProductsByCatAction").html(response);
            
        
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



