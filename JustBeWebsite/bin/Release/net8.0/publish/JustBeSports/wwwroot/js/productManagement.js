let products = [];
const urlParams = new URLSearchParams(window.location.search);
const selectedCategoryId = urlParams.get('categoryId');

// Load all categories for both the category filter and selection options
function loadCategoryOptions(selectedId = null) {
    $.ajax({
        url: '/Category/GetAllCategories',
        type: 'GET',
        success: function (categories) {
            const cmb = $('#cmbCategoryList');
            cmb.empty().append('<option></option>');
            categories.forEach(cat => {
                cmb.append(`<option value="${cat.id}">${cat.name}</option>`);
            });
            if (selectedId !== null) {
                cmb.val(selectedId).trigger('change');
            }
        },
        error: function () {
            alert("Error loading categories.");
        }
    });
}

// Initialize the category filter and load products
$(document).ready(function () {
    // Initialize Select2 for category filter
    $('#cmbCategoryFilter').select2({
        placeholder: "Select a Category",
        allowClear: true
    });

    // Load the category options
    loadCategoryOptions();

    // Load filter categories and apply any selected category filter
    loadFilterCategoryOptions().then(() => {
        if (selectedCategoryId) {
            $('#cmbCategoryFilter').val(selectedCategoryId).trigger('change');
        } else {
            loadProducts();
        }
    });

    // Search bar functionality
    document.getElementById("searchBar").addEventListener("input", function () {
        const searchQuery = this.value.toLowerCase();
        const filteredProducts = products.filter(product =>
            product.name.toLowerCase().includes(searchQuery) ||
            product.description.toLowerCase().includes(searchQuery) ||
            product.color.toLowerCase().includes(searchQuery)
        );
        renderProducts(filteredProducts);
    });

    // Category filter change event
    $('#cmbCategoryFilter').on('change', function () {
        const selectedCategoryId = $(this).val();
        loadProducts(selectedCategoryId);
    });

    // Load filter category options
    function loadFilterCategoryOptions() {
        return $.ajax({
            url: '/Category/GetAllCategories',
            type: 'GET',
            success: function (categories) {
                const cmb = $('#cmbCategoryFilter');
                cmb.empty().append('<option></option>');
                categories.forEach(cat => {
                    cmb.append(`<option value="${cat.id}">${cat.name}</option>`);
                });
            },
            error: function () {
                alert("Error loading filter categories.");
            }
        });
    }

    // Load products with optional category filter
    function loadProducts(categoryId = '') {
        let url = '/Product/GetAllProducts';
        if (categoryId) {
            url += `?categoryId=${categoryId}`;
        }

        fetch(url)
            .then(response => response.json())
            .then(data => {
                products = data;
                renderProducts(data);
            })
            .catch(error => {
                console.error('Error loading products:', error);
                alert('Error loading products.');
            });
    }

    // Render products to the grid
    function renderProducts(products) {
        const grid = document.getElementById("productsGrid");
        grid.innerHTML = "";

        if (products.length === 0) {
            grid.innerHTML = "<p>No products found.</p>";
            return;
        }

        products.forEach(product => {
            const imageUrl = product.images?.length > 0
                ? `/assets/${product.images[0].url}`
                : null;

            const card = document.createElement("div");
            card.className = "col-md-3 mb-4";

            const imagesCarousel = product.images?.length > 0
                ? `<div id="carousel-${product.id}" class="carousel slide" data-bs-ride="carousel">
                <div class="carousel-inner">
                  ${product.images.map((img, index) => `
                    <div class="carousel-item ${index === 0 ? 'active' : ''}">
                        <img src="/assets/${img.url}" class="d-block w-100" alt="Image of ${product.name}" />
                    </div>`).join('')}
                </div>
                <button class="carousel-control-prev" type="button" data-bs-target="#carousel-${product.id}" data-bs-slide="prev">
                    <span aria-hidden="true"><i class="fas fa-chevron-left fa-2x text-dark"></i></span>
                    <span class="visually-hidden">Previous</span>
                </button>
                <button class="carousel-control-next" type="button" data-bs-target="#carousel-${product.id}" data-bs-slide="next">
                    <span aria-hidden="true"><i class="fas fa-chevron-right fa-2x text-dark"></i></span>
                    <span class="visually-hidden">Next</span>
                </button>
              </div>`
                : `<div class="card-img-top bg-light d-flex align-items-center justify-content-center" style="height: 200px;">
                  <span class="text-muted">No Image</span>
              </div>`;

            card.innerHTML = `
            <div class="card h-100 d-flex flex-column">
                ${imagesCarousel}
                <div class="card-body d-flex flex-column">
                    <h5 class="card-title">${product.name}</h5>
                    <p class="card-text mb-1"><strong>Color:</strong> ${product.color}</p>
                    <p class="card-text mb-1"><strong>Price:</strong> ${product.price}</p>
                    <p class="card-text">${product.description}</p>
                    ${
                                    product.variants.some(v => v.stock > 0)
                                        ? `<div class="mb-2">
                                    <strong>Available Sizes:</strong>
                                    ${product.variants
                                            .filter(v => v.stock > 0)
                                            .map(v => `<span class="text-danger">${v.size}</span>`)
                                            .join(', ')
                                    }
                                    <div class="mt-auto">
                                        <button class="btn btn-sm btn-dark mr-2 add-to-cart-btn" data-product='${JSON.stringify(product)}'>Add to Cart</button>
                                    </div>
                               </div>`
                                        : '<div class="mb-2"><span class="text-danger">Out of stock</span></div>'
                    }
                </div>
            </div>
        `;

            grid.appendChild(card);
        });

        document.querySelectorAll('.carousel').forEach(carousel => {
            new bootstrap.Carousel(carousel);
        });
    }

    document.addEventListener("click", function (e) {
        if (e.target.classList.contains("add-to-cart-btn")) {
            const btn = e.target;
            const productData = btn.getAttribute("data-product");
            const product = JSON.parse(productData);
            addToCart(product);
        }
    });

    $('#cmbVarientList').select2({
        dropdownParent: $('#cartItemModal'),
        placeholder: "Select Size",
        allowClear: true
    });

    window.addToCart = function (product) {
        $('#txtProductName').val(product.name);
        $('#cmbVarientList').empty();

        const availableVariants = product.variants.filter(v => v.stock > 0);

        if (availableVariants.length > 0) {
            availableVariants.forEach(v => {
                $('#cmbVarientList').append(`<option value="${v.id}" data-stock="${v.stock}">${v.size}</option>`);
            });
        } else {
            $('#cmbVarientList').append(`<option disabled>No sizes available</option>`);
        }

        $('#cmbVarientList').off('change').on('change', updateStockLimit);
        $('#cmbVarientList').trigger('change');

        $('#cartItemModal').modal('show');
        $('#addToCartButton').data('product', product);

        function updateStockLimit() {
            const selectedOption = $('#cmbVarientList').find('option:selected');
            const selectedVariantId = selectedOption.val();
            const maxStock = selectedOption.data('stock');

            // Get the quantity already in the cart for this selected variant
            $.get(`/CartItem/GetQuantityInCart?productVariantId=${selectedVariantId}`, function (existingQtyInCart) {
                existingQtyInCart = parseInt(existingQtyInCart || 0);
                const availableStockForCart = maxStock - existingQtyInCart;

                // Update the stock display
                $('#stockNote').text(`Available stock: ${availableStockForCart}`);

                // Set the min and max values for the quantity input field
                $('#txtQuantity')
                    .val(1)
                    .attr('min', 1)
                    .attr('max', availableStockForCart);

                // Enable or disable the increase/decrease buttons based on available stock
                $('#increaseQuantity').off('click').on('click', function () {
                    let currentQty = parseInt($('#txtQuantity').val());
                    if (currentQty < availableStockForCart) {
                        $('#txtQuantity').val(currentQty + 1);
                    }
                });

                $('#decreaseQuantity').off('click').on('click', function () {
                    let currentQty = parseInt($('#txtQuantity').val());
                    if (currentQty > 1) {
                        $('#txtQuantity').val(currentQty - 1);
                    }
                });
            });
        }

        updateStockLimit();
    };

    $('#cartItemModal .btn-secondary, #productModal .close').on('click', function () {
        $('#cartItemModal').modal('hide');
        $('#cartItemForm').parsley().reset();
        $('#cartItemForm')[0].reset();
        $('#addToCartButton').removeData('productId');
    });

    $('#addToCartButton').on('click', function () {
        const selectedVariantId = $('#cmbVarientList').val();

        if (!selectedVariantId) {
            alert("Please select a size.");
            return;
        }

        const selectedOption = $('#cmbVarientList').find('option:selected');
        const maxStock = selectedOption.data('stock');
        const quantity = parseInt($('#txtQuantity').val(), 10);
        const product = $('#addToCartButton').data('product');

        // Get quantity already in cart for this variant
        $.get(`/CartItem/GetQuantityInCartproductVariantId=${selectedVariantId}`, function (existingQtyInCart) {
            existingQtyInCart = parseInt(existingQtyInCart || 0);
            const totalRequested = quantity + existingQtyInCart;

            if (totalRequested > maxStock) {
                alert(`You already have ${existingQtyInCart} in the cart. Only ${maxStock} available.`);
                return;
            }

            const data = {
                productId: product.id,
                productVariantId: selectedVariantId,
                quantity: quantity
            };

            $.post('/CartItem/AddToCart', data)
                .done(response => {
                    alert("Item added to cart!");
                })
                .fail(() => {
                    alert("Failed to add to cart.");
                });

            $('#cartItemModal').modal('hide');
        });
    });

    $('#cartItemModal .btn-secondary, #productModal .close').on('click', function () {
        $('#cartItemModal').modal('hide');
        $('#cartItemForm').parsley().reset();
        $('#cartItemForm')[0].reset();
        $('#addToCartButton').removeData('productId');
    });

    $('#addToCartButton').on('click', function () {
        const selectedVariantId = $('#cmbVarientList').val();

        if (!selectedVariantId) {
            alert("Please select a size.");
            return;
        }

        const selectedOption = $('#cmbVarientList').find('option:selected');
        const maxStock = selectedOption.data('stock');
        const quantity = parseInt($('#txtQuantity').val(), 10);

        if (quantity > maxStock) {
            alert(`Cannot add more than ${maxStock} items for this size.`);
            return;
        }

        const product = $('#addToCartButton').data('product');

        const data = {
            productId: product.id,
            productVariantId: selectedVariantId,
            quantity: quantity
        };

        console.log("Sending to cart:", data);

        $.post('/CartItem/AddToCart', data)
            .done(response => {
                alert("Item added to cart!");
            })
            .fail(() => {
                alert("Failed to add to cart.");
            });

        $('#cartItemModal').modal('hide');
    });
});
