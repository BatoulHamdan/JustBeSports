window.ParsleyConfig = {
    errorsWrapper: '<div class="invalid-feedback"></div>',
    errorTemplate: '<div></div>',
    classHandler: function (ParsleyField) {
        return ParsleyField.$element;
    },
    errorClass: 'is-invalid',
    successClass: 'is-valid',
};


$(document).ready(function () {
    $('#checkoutForm').parsley();

    $.get('/CartItem/GetSessionId', function (data) {
        const sessionId = data.sessionId;
        console.log("Session ID:", sessionId);

        // Fetch cart items after getting session ID
        fetchCartItems(sessionId);

        // Quantity increase
        $(document).on('click', '.increase-quantity', function () {
            const cartItemId = $(this).data('cartitem-id');
            const row = $(this).closest('tr');
            const quantitySpan = $(this).siblings('span');
            const currentQuantity = parseInt(quantitySpan.text());

            // Get max stock from a hidden field or data attribute
            const maxStock = parseInt(row.data('stock'));

            if (currentQuantity >= maxStock) {
                alert('Maximum stock reached.');
                return;
            }

            const newQuantity = currentQuantity + 1;
            updateQuantity(cartItemId, newQuantity, sessionId);
        });

        $(document).on('click', '.decrease-quantity', function () {
            const cartItemId = $(this).data('cartitem-id');
            const quantitySpan = $(this).siblings('span');
            const currentQuantity = parseInt(quantitySpan.text());
            const newQuantity = currentQuantity - 1;
            if (newQuantity < 1) return;
            updateQuantity(cartItemId, newQuantity, sessionId);
        });

        // Delete cart item
        $(document).on('click', '.delete-cart-item', function () {
            const cartItemId = $(this).data('cartitem-id');
            if (confirm('Are you sure you want to remove this item from the cart?')) {
                deleteCartItem(cartItemId, sessionId);
            }
        });
    });
});

// Fetch cart items from the server and populate the table
function fetchCartItems(sessionId) {
    $.get(`/CartItem/GetAllCartItems?sessionId=${sessionId}`, function (data) {
        const cartItems = data;
        let tableBody = '';
        let totalAmount = 0;
        let totalItems = 0;

        if (cartItems.length > 0) {
            cartItems.forEach(item => {
                const itemTotal = item.productPrice * item.quantity;
                totalAmount += itemTotal;
                totalItems += item.quantity;

                tableBody += `
                    <tr data-stock="${item.variantStock}">
                        <td><img src="/assets/${item.productImageUrl}" alt="${item.productName}" width="50" height="50"></td>
                        <td>${item.productName}</td>
                        <td>${item.productSize}</td>
                        <td>
                            <button class="btn btn-sm btn-secondary decrease-quantity" data-cartitem-id="${item.id}" ${item.quantity === 1 ? 'disabled' : ''} title="Decrease Quantity">-</button>
                            <span class="mx-2">${item.quantity}</span>
                            <button class="btn btn-sm btn-secondary increase-quantity" data-cartitem-id="${item.id}" ${item.quantity === item.variantStock ? 'disabled' : ''} title="Increase Quantity">+</button>
                        </td>
                        <td>${item.productPrice}</td>
                        <td>${itemTotal.toFixed(2)}</td>
                        <td>
                            <button class="btn btn-sm btn-danger delete-cart-item" data-cartitem-id="${item.id}" title="Remove Item">Delete</button>
                        </td>
                    </tr>
                `;
            });

            $('#checkoutSection').show();
            $('#totalSummary').html(`
                <div class="alert alert-secondary d-flex justify-content-between align-items-center">
                    <span><strong>Total Items:</strong> ${totalItems}</span>
                    <span><strong>Total Amount:</strong> $${totalAmount.toFixed(2)}</span>
                </div>
            `);
        } else {
            tableBody = '<tr><td colspan="6" class="text-center">No items in cart.</td></tr>';
            $('#checkoutSection').hide();
            $('#totalSummary').html('');
        }

        $('#cartItemsTableBody').html(tableBody);
    });
}

function calculateTotal() {
    let total = 0;
    $('#cartItemsTableBody tr').each(function () {
        const price = parseFloat($(this).find('td').eq(4).text());
        const quantity = parseInt($(this).find('span').text());
        total += price * quantity;
    });
    return total.toFixed(2);
}

// Update item quantity (increment or decrement)
function updateQuantity(cartItemId, quantity, sessionId) {
    $.ajax({
        url: '/CartItem/UpdateQuantity',
        type: 'POST',
        data: { cartItemId: cartItemId, quantity: quantity },
        success: function () {
            fetchCartItems(sessionId);
        },
        error: function () {
            alert('Failed to update quantity.');
        }
    });
}

// Delete an item from the cart
function deleteCartItem(cartItemId, sessionId) {
    $.ajax({
        url: '/CartItem/RemoveFromCart',
        type: 'POST',
        data: { cartItemId: cartItemId, sessionId: sessionId },
        success: function () {
            fetchCartItems(sessionId);
        },
        error: function () {
            alert('Failed to delete item.');
        }
    });
}

// When checkout button is clicked, open the modal
$('#checkoutButton').on('click', function () {
    $('#checkoutModal').modal('show');
});

$('#submitCheckout').on('click', function () {
    const form = $('#checkoutForm');

    // Validate with Parsley
    if (!form.parsley().validate()) {
        return; // stop if validation fails
    }

    // Get form values
    const firstName = $('#firstName').val();
    const lastName = $('#lastName').val();
    const governate = $('#governate').val();
    const instagramAccount = $('#instagramAccount').val();
    const phoneNumber = $('#phoneNumber').val();
    const fullAddress = $('#fullAddress').val();
    const totalPrice = calculateTotal();

    // Proceed to submit
    $.post('/CartItem/Checkout', {
        firstName,
        lastName,
        governate,
        instagramAccount,
        phoneNumber,
        fullAddress,
        totalPrice
    })
        .done(function (response) {
            $('#checkoutModal').modal('hide');
            window.location.href = '/CartItem/OrderConfirmation?orderId=' + response.orderId;
        })
        .fail(function (xhr, status, error) {
            console.error('Checkout failed:', status, error);
            alert('Failed to complete checkout.');
        });
});
