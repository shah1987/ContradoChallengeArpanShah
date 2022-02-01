const apiBaseURL = 'https://localhost:44312';
var totalRecords = 0;
var pageSize = 10;
var curPage = 1;
(function () {
    _productDemo = {
        apiURL: {
            getallproducts: '/api/products/getallproducts',
            deleteproduct: '/api/products/deleteproduct'
        },
        GetAllProducts: function () {
            var objRequest = {
                PageIndex: curPage * 1,
                PageSize: 10
            };
            $.ajax({
                url: apiBaseURL + _productDemo.apiURL.getallproducts,
                type: 'POST',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objRequest),
                success: function (response) {
                    if (response.StatusCode == 1000 && response.Data.length > 0) {
                        $("#gvProduct").empty();
                        $("#tmplProduct").tmpl(response.Data).appendTo("#gvProduct");
                        totalRecords = response.Data[0].TotalRecords;
                        _productDemo.SetPagging();
                    }
                    else {
                        ("#gvProduct").html($('#noDataFound').html());
                        totalRecords = 0;
                    }
                },
                Error: function (d) {
                    alert('Error');
                    totalRecords = 0;
                }
            });
        },
        SetPagging: function () {
            if (totalRecords > 0) {
                let totalPage = Math.ceil(totalRecords / pageSize);
                var paggingArr = [];
                for (var i = 1; i <= totalPage; i++) {
                    paggingArr.push({ pageID : i});
                }
                $("#pgData").empty();
                $("#tmplPagging").tmpl(paggingArr).appendTo("#pgData");
            }
        },
        SetPageIndex: function (index) {
            curPage = index;
            _productDemo.GetAllProducts();
        },
        DeleteProduct: function (pID) {
            $('#dialog-confirm').show();
            $("#dialog-confirm").dialog({
                resizable: false,
                height: "auto",
                width: 300,
                modal: true,
                buttons: {
                    "Yes": function () {
                        let objRequest = {};
                        objRequest.ProductID = pID;
                        $.ajax({
                            url: apiBaseURL + _productDemo.apiURL.deleteproduct,
                            type: 'POST',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(objRequest),
                            success: function (response) {
                                if (response.StatusCode == 1000) {
                                    alert('Product Deleted Successfully.')
                                    $('#dialog-confirm').hide();
                                    _productDemo.GetAllProducts();
                                }
                                else {
                                    $('#dialog-confirm').hide();
                                }
                            },
                            Error: function (d) {
                                alert('Some Error Occured');
                            }
                        });
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    }
})();

$(document).ready(function () {
    _productDemo.GetAllProducts();
});