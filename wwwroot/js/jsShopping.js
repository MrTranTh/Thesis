$(document).ready(function () {
    LoadCart();
    ShowCount();
    
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity').text();
        if (tQuantity != '') {
            quantity = parseInt(tQuantity);
        }
        /*alert(id + " " + quantity);*/
        $.ajax({
            url: "/ShoppingCart/AddToCart",
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.success) {
                    $('#checkout_items').html(rs.count);
                    alert(rs.msg);
                }
            }
        });
    });

    //add to cart trong giỏ hàng
    $('body').on('click', '.btnAddToCartDetail', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var inputQuantity = document.getElementById("quantity");
        var newValue = inputQuantity.value;
        /*alert(id + " " + quantity);*/
        $.ajax({
            url: "/ShoppingCart/AddToCartDetail",
            type: 'POST',
            data: { id: id, quantity: newValue },
            success: function (rs) {
                if (rs.success) {
                    $('#checkout_items').html(rs.count);
                    alert(rs.msg);
                }
            }
        });
    });

    //Xóa từng sản phẩm ko muốn mua nữa
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm("Bạn muốn xóa mặt hàng này khỏi giỏ hàng?");
        if (conf === true) {
            Delete(id);
        }
    });

    //Xóa hết giỏ hàng
    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var conf = confirm("Bạn muốn xóa tất cả mặt hàng này khỏi giỏ hàng?");
        if (conf === true) {
            DeleteAll();

        }
    });

    //Cập nhật số lượng sản phẩm trong giỏ hàng
    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = $('#Quantity_' + id).val();
        Update(id, quantity);
    });
});


//Hàm hỗ trợ đếm số hàng trong giỏ hàng
function ShowCount() {
    $.ajax({
        url: "/ShoppingCart/ShowCount",
        type: 'GET',
        success: function (rs) {
            if (rs.success) {
                $('#checkout_items').html(rs.count);           
            }
        }
    });
}


//hàm hỗ trỡ xóa hết mặt hàng trong giỏ hàng
function DeleteAll() {
    $.ajax({
        url: "/ShoppingCart/DeleteAll",
        type: 'POST',
        success: function (rs) {
            if (rs.success) {
                LoadCart();
                window.location.reload();
            }
        }
    });
}

//hàm hỗ trỡ xóa hết mặt hàng trong giỏ hàng
function Delete(id) {
    $.ajax({
        url: "/ShoppingCart/Delete",
        type: 'POST',
        data: {id:id},
        success: function (rs) {
            if (rs.success) {
                LoadCart();
                window.location.reload();
            }
        }
    });
}

//hàm hỗ trỡ cập nhật mặt hàng trong giỏ hàng
function Update(id, quantity) {
    $.ajax({
        url: "/ShoppingCart/Update",
        type: 'POST',
        data: {id:id, quantity:quantity},
        success: function (rs) {
            if (rs.success) {
                LoadCart();
                window.location.reload();
            }
        }
    });
}


function LoadCart() {
    $.ajax({
        url: "/ShoppingCart/Partial_Item_Cart",
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}

