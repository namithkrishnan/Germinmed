$(function () {
    $("#loaderbody").addClass('hide');
 

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
        });

    //activatejQueryTable();
});

$(document).ready(function () {
  
    $('#productImageEdit').hide();
    $('#imageEditMessage').show();
    //activatejQueryTable();
 });


function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}
function jQueryAjaxPost(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.success) {
                    $("#firstTab").html(response.html);
                    refreshAddNewTab($(form).attr('data-restUrl'), true);
                    $.notify(response.message, "success");
                    if ((typeof activatejQueryTable !== 'undefined') && ($.isFunction(activatejQueryTable)))
                        activatejQueryTable();
                   
                }
                else {
                    $.notify(response.message, "error");
                }
            },
             error: function (xhr) {
            alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
        }
        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;
        }
        $.ajax(ajaxConfig);

    }
    return false;
}

function jQueryAjaxPostAddImage(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.success) {
                   $("#imageList").html(response.html);
                    refreshAddImageTab($(form).attr('data-restUrl1'), false);
                    refreshAddImageTab2($(form).attr('data-restUrl2'), false);
                     $.notify(response.message, "success");
                  
                    if ((typeof activatejQueryTableImage !== 'undefined') && ($.isFunction(activatejQueryTableImage)))
                        activatejQueryTableImage();
                   }
                else {
                    $.notify(response.message, "error");
                }
            }
        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;
        }
        $.ajax(ajaxConfig);

    }
    return false;
}



function jQueryAjaxPostChangePassword(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var ajaxConfig = {
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            success: function (response) {
                if (response.success) {
                  //  $("#divchangepassword").html(response.html);
                    refreshChangePassword($(form).attr('data-restUrl'), false);
                    $.notify(response.message,"success");
                 }
                else {
                    $.notify(response.message, "error");
                }
            }
        }
        if ($(form).attr('enctype') == "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;
        }
        $.ajax(ajaxConfig);

    }
    return false;
}

function refreshChangePassword(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
           // alert(resetUrl);
            $("#secondTab").html(response);
           // $.notify('Password changed Successfully', "success");
        }
    });
}

function refreshAddNewTab(resetUrl, showViewTab)
{
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
            $("#secondTab").html(response);
            $('#imageEditMessage').show();
            $('#productImageEdit').hide();
            $('ul.nav.nav-tabs a:eq(1)').html('Add New');
            if (showViewTab)
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
        }
    });
}
function refreshAddImageTab(resetUrl,showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {
           
            
            $("#imageList").html(response.html);
            $('#ImageUpload').val('');
            $('#DisplayOrder').val('1');
            $('#imagePreview1').attr('src','/AppFiles/Images/Default.png');  
             if (showViewTab)
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
        }
    });
}


function refreshAddImageTab2(resetUrl, showViewTab) {
    $.ajax({
        type: 'GET',
        url: resetUrl,
        success: function (response) {

            $("#imageAdd").html(response.html);
            $('#ImageUpload').val('');
            $('#DisplayOrder').val('1');
            $('#imagePreview1').attr('src', '/AppFiles/Images/Default.png');
            if (showViewTab)
                $('ul.nav.nav-tabs a:eq(0)').tab('show');
        }
    });
}
function Edit(url) {
       
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            
            $('#txtProductId').val(url.substr(url.lastIndexOf('/') + 1));
            $('#productImageEdit').show();
            $('#imageEditMessage').hide();
            $("#secondTab").html(response);
            $('.textarea').wysihtml5();
            $('ul.nav.nav-tabs a:eq(1)').html('Edit');
            $('ul.nav.nav-tabs a:eq(1)').tab("show");
           
        }
      //  error: function (xhr) {
         ///   alert('Request Status: ' + xhr.status + ' Status Text: ' + xhr.statusText + ' ' + xhr.responseText);
       // }
    });
    //$(function () {
    //    if ($("#IsFeatured").is(':checked')) {
    //        $("#txtImage").show();
    //    }
    //    else {
    //        $("#txtImage").hide();
    //    }
    //});
}


function EditImage(url,div) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('#txtProductId').val(url.substr(url.lastIndexOf('/') + 1));
            $(div).html(response);
                      
        }
    });

}

function AddImage(url) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $("#imageAdd").html(response);

        }
    });

}



function Approve(url,id) {
    if (confirm("Are you sure to Approver this account?") == true) {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (response) {
                if (response.success) {
                    $("#firstTab").html(response.html);
                    $.notify(response.message, "success");
                    if ((typeof activatejQueryTable !== 'undefined') && ($.isFunction(activatejQueryTable)))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }

            }
        });
    }
    
}




function Delete(url)
{
    if (confirm("Are you sure to Delete this record?") == true)
    {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (response) {
                if (response.success) {
                    $("#firstTab").html(response.html);
                    $.notify(response.message, "warn");
                    if ((typeof activatejQueryTable !== 'undefined') && ($.isFunction(activatejQueryTable)))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }

            }
        });
    }
}
function DeleteImage(url) {
    if (confirm("Are you sure to Delete this record?") == true) {
        $.ajax({
            type: 'POST',
            url: url,
            success: function (response) {
                if (response.success) {
                    $("#imageList").html(response.html);
                    $.notify(response.message, "warn");
                    if ((typeof activatejQueryTable !== 'undefined') && ($.isFunction(activatejQueryTable)))
                        activatejQueryTable();
                }
                else {
                    $.notify(response.message, "error");
                }

            }
        });
    }
}