$(document).ready(function () {
    const table = $('#tblOrders').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        info: true
    });

    loadOrders();

    // When status changes
    $(document).on('change', '.order-status', function () {
        const orderId = $(this).data('order-id');
        const newStatus = $(this).val();

        $.post('/Order/UpdateOrderStatus', { orderId: orderId, newStatus: newStatus }, function () {
            alert('Order status updated successfully!');
            loadOrders(); // reload the table after update
        }).fail(function () {
            alert('Failed to update status.');
        });
    });

    // When view button clicked
    $(document).on('click', '.view-order', function () {
        const orderId = $(this).data('order-id');
        window.location.href = `/Order/ViewOrder/${orderId}`;
    });

    //// When delete button clicked
    //$(document).on('click', '.delete-order', function () {
    //    const orderId = $(this).data('order-id');
    //    if (confirm('Are you sure you want to delete this order?')) {
    //        $.ajax({
    //            url: `/Order/DeleteOrder/${orderId}`,
    //            type: 'POST',
    //            success: function () {
    //                alert('Order deleted successfully!');
    //                loadOrders(); // reload the table after delete
    //            },
    //            error: function () {
    //                alert('Failed to delete order.');
    //            }
    //        });
    //    }
    //});

});

function loadOrders() {
    $.get('/Order/GetAllOrders', function (orders) {
        // Sort by orderDate descending (newest first)
        orders.sort((a, b) => new Date(b.orderDate) - new Date(a.orderDate));

        let tableRows = '';

        orders.forEach(order => {
            tableRows += `
                <tr>
                    <td>${order.firstName} ${order.lastName}</td>
                    <td>$${order.totalPrice.toFixed(2)}</td>
                    <td>${new Date(order.orderDate).toLocaleDateString()}</td>
                    <td>
                        <select class="form-select order-status" data-order-id="${order.id}">
                            <option value="Pending" ${order.status === 'Pending' ? 'selected' : ''}>Pending</option>
                            <option value="Approved" ${order.status === 'Approved' ? 'selected' : ''}>Approved</option>
                            <option value="Processing" ${order.status === 'Processing' ? 'selected' : ''}>Processing</option>
                            <option value="Shipped" ${order.status === 'Shipped' ? 'selected' : ''}>Shipped</option>
                            <option value="Delivered" ${order.status === 'Delivered' ? 'selected' : ''}>Delivered</option>
                            <option value="Canceled" ${order.status === 'Canceled' ? 'selected' : ''}>Canceled</option>
                        </select>
                    </td>
                    <td>
                       <button class="btn btn-dark view-order" data-order-id="${order.id}">View</button>
                    </td>
                </tr>
            `;
        });

        $('#tblOrders').DataTable().clear().destroy(); // clear previous table
        $('#tblOrders tbody').html(tableRows); // fill new data
        $('#tblOrders').DataTable({ // re-initialize DataTable
            paging: true,
            searching: true,
            ordering: true,
            info: true
        });
    });
}
