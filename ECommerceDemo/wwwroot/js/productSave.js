const apiBaseURL = 'https://localhost:44312';
(function () {
    _productSave = {
        apiURL: {
            saveproduct: '/api/products/saveproduct',
            getallcategories: '/api/category/getallcategories',
            getallproductattribute: '/api/productattribute/getallproductattribute',
            getproductbyid: '/api/products/getproductbyid'
        },
        ValidateProductInput: function () {
            let validationMessage = [];
            let pName = $('#product_name').val();
            let pDesc = $('#product_description').val();
            let pAttr = $('#product_attribute').val();
            let pCat = $('#product_category').val();
            if (pName == '') {
                validationMessage.push('Product Name Require');
            }
            if (pCat == '' || pCat == 0) {
                validationMessage.push('Category Require');
            }
            if (pDesc == '') {
                validationMessage.push('Product Description Require');
            }
            if (pAttr == '' || pAttr == 0) {
                validationMessage.push('Product Attribute Require');
            }

            return validationMessage;
        },
        SaveProduct: function () {
            let validation = _productSave.ValidateProductInput();
            if (validation.length > 0) {
                htmlMsg = '';
                for (var i = 0; i < validation.length; i++) {
                    htmlMsg += '<p>' + validation[i] + '</p>';
                }
                $('#validationMsg').html(htmlMsg).addClass('show');
            }
            else {
                let pName = $('#product_name').val();
                let pDesc = $('#product_description').val();
                let pAttr = $('#product_attribute').val();
                let pCat = $('#product_category').val();
                let pId = $('#product_id').val();
                let pAttValue = $("#product_attribute option:selected").text();
                let objRequest = {};
                objRequest.ProductId = parseInt(pId);
                objRequest.ProdCatId = parseInt(pCat);
                objRequest.AttributeId = parseInt(pAttr);
                objRequest.ProdName = pName;
                objRequest.ProdDescription = pDesc;
                objRequest.AttributeValue = pAttValue;
                    $.ajax({
                        url: apiBaseURL + _productSave.apiURL.saveproduct,
                        type: 'POST',
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(objRequest),
                        success: function (response) {
                            if (response.StatusCode == 1000) {
                                _productSave.ResetProduct();
                                alert('Product Saved Successfully.');
                            }
                            else {
                                alert(response.Message);
                            }
                        },
                        Error: function (d) {
                            alert('Some Error Occured');
                        }
                    });
            }
        },
        GetAllCategories: function () {
            $.ajax({
                url: apiBaseURL + _productSave.apiURL.getallcategories,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    if (response.StatusCode == 1000 && response.Data.length > 0) {
                        $('#product_category').append($('<option>').val(0).text('--- Select Category ---'));
                        for (var i = 0; i < response.Data.length; i++) {
                            $('#product_category').append($('<option>').val(response.Data[i].ProdCatId).text(response.Data[i].CategoryName));
                        }
                    }
                },
                Error: function (d) {
                    alert('Some Error Occured');
                }
            });
        },
        GetAllAttributeByCatID: function (catID) {
            let objRequest = {};
            objRequest.ProdCatId = parseInt(catID);
            $.ajax({
                url: apiBaseURL + _productSave.apiURL.getallproductattribute,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objRequest),
                success: function (response) {
                    if (response.StatusCode == 1000 && response.Data.length > 0) {
                        $('#product_attribute').empty().append($('<option>').val(0).text('--- Select Attribute ---')).val(0);
                        for (var i = 0; i < response.Data.length; i++) {
                            $('#product_attribute').append($('<option>').val(response.Data[i].AttributeID).text(response.Data[i].AttributeName));
                        }
                        if ($('#attr_id').val() > 0) {
                            $('#product_attribute').val($('#attr_id').val());
                        }
                    }
                },
                Error: function (d) {
                    alert('Some Error Occured');
                }
            });
        },
        ResetProduct: function () {
            $('#product_name').val('');
            $('#product_description').val('');
            $('#product_category').val(0);
            $('#product_attribute').empty().append($('<option>').val(0).text('--- Select Attribute ---')).val(0);
            $('#validationMsg').html('').removeClass('show');
        },
        GetProductByID: function () {
            let objRequest = {};
            objRequest.ProductID = parseInt($('#product_id').val());
            $.ajax({
                url: apiBaseURL + _productSave.apiURL.getproductbyid,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objRequest),
                success: function (response) {
                    if (response.StatusCode == 1000) {
                        $('#product_name').val(response.Data.ProdName);
                        $('#product_description').val(response.Data.ProdDescription);
                        $('#attr_id').val(response.Data.AttributeId);
                        $('#product_category').val(response.Data.ProdCatId).trigger('change');
                    }
                },
                Error: function (d) {
                    alert('Some Error Occured');
                }
            });
        }
    }
})();
$(document).ready(function () {
    _productSave.GetAllCategories();
    $('#product_attribute').append($('<option>').val(0).text('--- Select Attribute ---'));
    $("#product_category").change(function () {
        var sele = $('option:selected', this);
        if (sele.val() != 0) {
            _productSave.GetAllAttributeByCatID(sele.val());
        }
    });
    $('#productSave').submit(function (e) {
        e.preventDefault();
        _productSave.SaveProduct();
    });
    if ($('#product_id').val() > 0) {
        _productSave.GetProductByID();
    }
});