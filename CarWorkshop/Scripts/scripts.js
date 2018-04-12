$(document).ready(function () {
    shoppingCartBinds();
    var xhr;
    //$("#search").autoComplete({
    //    minChar: 1,
    //    source: function (term, response) {
    //        try { xhr.abort(); } catch (e) { }
    //        xhr = $.getJSON('/products/search', { q: term }, function (data) { response(data); });
    //    },
    //    renderItem: function (item, search) {
    //        return '<div class="autocomplete-suggestion" data-id="' + item.Id + '"><a href="#">' + item.Name + '</a></div>';
    //    },
    //    onSelect: function (e, term, item) {
    //        var id = item.attr('data-id');
    //        var url = '/products/details/' + id;
    //        window.location.href = url;
    //    }
    //});
});

function shoppingCartBinds() {
    $('.add-to-cart').click(function () {
        var id = $(this).attr('data-product');
        if (!id) {
            return;
        }
        $.get('/shoppingcart/addtocart/' + id, function () {
            
            updateCartCount();
            toastr.success('Dodano produkt do koszyka');
        });
    });
}

function updateCartCount() {
    $.get('/shoppingcart/GetItemsCount', function (data) {
        $(".cart-count").html(data);
    });
    
}