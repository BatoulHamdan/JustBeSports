let products = [];
let currentProductId = null;

const urlParams = new URLSearchParams(window.location.search);
const selectedCategoryId = urlParams.get('categoryId');

window.ParsleyConfig = {
    errorsWrapper: '<div class="invalid-feedback"></div>',
    errorTemplate: '<div></div>',
    classHandler: function (ParsleyField) {
        return ParsleyField.$element;
    }
};

$(document).ready(function () {
    $('#categoryForm').parsley();
    $('#variantForm').parsley();

    $('#cmbCategoryFilter').select2({
        placeholder: "Select a Category",
        allowClear: true
    });

    loadCategoryOptions();
    loadFilterCategoryOptions().then(() => {
        if (selectedCategoryId) {
            $('#cmbCategoryFilter').val(selectedCategoryId).trigger('change');
        } else {
            loadProducts();
        }
    });

    $('#searchBar').on('input', function () {
        const searchQuery = this.value.toLowerCase();
        const filtered = products.filter(p => p.name.toLowerCase().includes(searchQuery));
        renderProducts(filtered);
    });

    $('#cmbCategoryFilter').on('change', function () {
        const catId = $(this).val();
        loadProducts(catId);
    });

    $('#saveProductButton').off('click').on('click', function () {
        if ($('#productForm').parsley().validate()) {
            const id = $(this).data('productId') || 0;
            const productData = {
                id: id,
                name: $('#txtProductName').val(),
                description: $('#txtProductDescription').val(),
                color: $('#txtProductColor').val(),
                price: parseFloat($('#txtProductPrice').val()) || 0,
                categoryId: parseInt($('#cmbCategoryList').val())
            };

            $.ajax({
                url: '/Product/Save',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(productData),
                success: function (res) {
                    alert(res.message);

                    // Reset form
                    $('#productForm').parsley().reset();
                    $('#productForm')[0].reset();
                    $('#saveProductButton').removeData('productId');
                    $('#productForm').find('.is-invalid').removeClass('is-invalid');

                    $('#productModal').one('hidden.bs.modal', function () {
                        const filteredCategoryId = $('#cmbCategoryFilter').val();
                        loadProducts(filteredCategoryId); // reapply filter if needed
                    }).modal('hide');
                },
                error: function (xhr) {
                    console.error(xhr.responseText);
                    alert("Error saving product.");
                }
            });
        }
    });

    // Initialize category dropdown inside product modal
    $('#productModal').on('shown.bs.modal', function () {
        $('#cmbCategoryList').select2({
            dropdownParent: $('#productModal'),
            placeholder: "Select a Category",
            allowClear: true
        });
    });

    // Close modal cleanup
    $('#productModal .btn-secondary, #productModal .close').on('click', function () {
        $('#productModal').modal('hide');
        $('#productForm').parsley().reset();
        $('#productForm')[0].reset();
        $('#saveProductButton').removeData('productId');
    });

    $('#categoryForm').parsley().on('field:validated', function () {
        const field = this.$element;
        if (this.isValid()) {
            field.removeClass('is-invalid');
        } else {
            field.addClass('is-invalid');
        }
    });

    $('#categoryModal').on('hidden.bs.modal', function () {
        const form = $('#categoryForm');
        form[0].reset();
        form.parsley().reset();
        form.find('.is-invalid').removeClass('is-invalid');
        $('#saveCategoryButton').removeData('categoryId');
        $('#categoryModalLabel').text('Add New Category');
    });
});

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

function loadFilterCategoryOptions() {
    return $.ajax({
        url: '/Category/GetAllCategories',
        type: 'GET',
        success: function (categories) {
            const cmb = $('#cmbCategoryFilter');
            cmb.empty().append('<option></option>');
            categories.forEach(cat => cmb.append(`<option value="${cat.id}">${cat.name}</option>`));
        },
        error: function () {
            alert("Error loading filter categories.");
        }
    });
}

function loadProducts(categoryId = '') {
    let url = '/Product/GetAllProducts';
    if (categoryId) url += `?categoryId=${categoryId}`;

    fetch(url)
        .then(r => r.json())
        .then(data => {
            products = data;
            renderProducts(data);
        })
        .catch(e => console.error('Error loading products:', e));
}

function renderProducts(list) {
    const grid = $('#productsGrid').empty();

    list.forEach(product => {
        const imageUrl = product.images?.length > 0
            ? `/assets/${product.images[0].url}`
            : null;

        const card = $(
            `<div class="col-md-3 mb-4">
                <div class="card h-100 d-flex flex-column">
                    ${imageUrl
                ? `<img src="${imageUrl}" class="card-img-top" alt="${product.name}" />`
                : `<div class="card-img-top bg-light d-flex align-items-center justify-content-center" style="height:200px;"><span class="text-muted">No Image</span></div>`}
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title">${product.name}</h5>
                        <p class="card-text mb-1"><strong>Color:</strong> ${product.color}</p>
                        <p class="card-text mb-1"><strong>Price:</strong> ${product.price}</p>
                        <p class="card-text">${product.description}</p>
                        <div class="mb-2">
                            <strong>Sizes:</strong>
                            ${product.variants.map(v => `<span class="badge badge-secondary mr-1">${v.size}</span>`).join('')}
                        </div>
                        <ul class="list-unstyled small text-muted">
                            ${product.variants.map(v => `<li>Size: ${v.size} | Stock: ${v.stock}</li>`).join('')}
                        </ul>
                        <div class="mt-auto">
                            <button type="button" class="btn btn-sm btn-info" onclick="openVariantModal(${product.id}, '${product.name}')">Variants</button>
                            <a href="/Product/Images?id=${product.id}" class="btn btn-sm btn-info">Images</a>
                            <button class="btn btn-sm btn-primary mr-2" onclick="editProduct('${product.id}')">Edit</button>
                            <button class="btn btn-sm btn-danger" onclick="deleteProduct('${product.id}')">Delete</button>
                        </div>
                    </div>
                </div>
            </div>`
        );
        grid.append(card);
    });
}

function addProduct() {
    $('#txtProductName').val('');
    $('#txtProductDescription').val('');
    $('#txtProductColor').val('');
    $('#txtProductPrice').val('');
    $('#variantContainer').empty();
    $('#saveProductButton').removeData('productId');
    $('#productModalLabel').text('Add New Product');

    loadCategoryOptions();
    $('#productModal').modal('show');
}

function editProduct(id) {
    $.ajax({
        url: '/Product/GetProduct/' + id,
        type: 'GET',
        success: function (product) {
            $('#txtProductName').val(product.name);
            $('#txtProductDescription').val(product.description);
            $('#txtProductColor').val(product.color);
            $('#txtProductPrice').val(product.price);
            loadCategoryOptions(product.categoryId);
            setTimeout(() => $('#cmbCategoryList').val(product.categoryId), 200);

            $('#saveProductButton').data('productId', id);
            $('#productModalLabel').text('Edit Product');
            $('#productModal').modal('show');
        },
        error: function () {
            alert("Error loading product data.");
        }
    });
}

function deleteProduct(id) {
    if (!confirm("Are you sure you want to delete this product?")) return;
    $.ajax({
        url: '/Product/Delete/' + id, type: 'DELETE',
        success: function () { alert("Product deleted!"); loadProducts(); },
        error: function () { alert("Error deleting product."); }
    });
}

// Variant management
function openVariantModal(productId, productName) {
    currentProductId = productId;
    $('#variantProductName').text(productName);
    $('#variantList').empty();
    $('#variantModal').modal('show');
    loadVariants(productId);
}

function loadVariants(productId) {
    const url = `/Product/GetAllVariants?productId=${productId}`;

    fetch(url)
        .then(r => {
            if (!r.ok) throw new Error('Network response was not ok');
            return r.json();
        })
        .then(variants => renderVariantList(variants))
        .catch(e => console.error('Error loading variants:', e));
}

function renderVariantList(variants) {
    const ul = $('#variantList').empty();
    if (!variants.length) {
        ul.append('<li class="list-group-item text-muted">No variants yet.</li>');
        return;
    }
    variants.forEach(v => {
        ul.append(
            `<li class="list-group-item d-flex justify-content-between align-items-center">
                <div>
                    Size: ${v.size} — Stock: ${v.stock}
                </div>
                <div>
                    <button class="btn btn-sm btn-outline-primary me-2" onclick="editVariant(${v.id}, '${v.size}', ${v.stock})">✏️</button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteVariant(${v.id})">&times;</button>
                </div>
            </li>`
        );
    });
}

let editingVariantId = null;

function editVariant(id, size, stock) {
    $('#variantSize').val(size);
    $('#variantStock').val(stock);
    editingVariantId = id;

    $('#variantForm button[type="submit"]').text('Update Variant');
}


$('#variantForm').on('submit', function (e) {
    e.preventDefault();
    const variantPayload = {
        id: editingVariantId || 0,  
        productId: currentProductId,
        size: $('#variantSize').val(),
        stock: parseInt($('#variantStock').val(), 10)
    };

    $.ajax({
        url: '/Product/SaveVariant',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(variantPayload),
        success: res => {
            alert(res.message);
            loadVariants(currentProductId);
            $('#variantSize, #variantStock').val('');
            editingVariantId = null;
            $('#variantForm button[type="submit"]').text('Add Variant');
        },
        error: () => alert('Error saving variant.')
    });
});

function deleteVariant(id) {
    if (!confirm('Delete this variant?')) return;
    fetch(`/Product/DeleteVariant?id=${id}`, { method: 'DELETE' })
        .then(r => r.ok ? loadVariants(currentProductId) : alert('Error deleting variant'))
        .catch(() => alert('Error deleting variant'));
}


// Image upload/delete functions 
function openImageModal(productId) {
    $('#productIdForImage').val(productId);
    $('#selectedFileName').text('');
    $('#imageFileInput').val('');
    $('#productImageModal').modal('show');
}

document.getElementById('imageFileInput').addEventListener('change', function (e) {
    $('#selectedFileName').text(e.target.files[0]?.name || '');
});

const dropZone = document.getElementById('dropZone');
['dragover', 'dragleave', 'drop'].forEach(evt => {
    dropZone.addEventListener(evt, function (e) {
        e.preventDefault();
        if (evt === 'dragover') dropZone.classList.add('bg-light');
        else dropZone.classList.remove('bg-light');
        if (evt === 'drop') {
            document.getElementById('imageFileInput').files = e.dataTransfer.files;
            $('#selectedFileName').text(e.dataTransfer.files[0]?.name || '');
        }
    });
});

function addImage() {
    const productId = $('#productIdForImage').val();
    const file = document.getElementById('imageFileInput').files[0];
    if (!file) { alert('Please select an image.'); return; }

    const fd = new FormData();
    fd.append('ProductId', productId);
    fd.append('ImageFile', file);

    fetch('/Product/UploadImage', { method: 'POST', body: fd })
        .then(r => r.ok ? location.reload() : alert('Upload failed.'))
        .catch(() => alert('Error uploading image.'));
}

function deleteImage(id) {
    if (!confirm('Delete this image?')) return;
    $.ajax({
        url: '/Product/DeleteImage/' + id, type: 'DELETE',
        success: () => location.reload(),
        error: () => alert('Error deleting image.')
    });
}

$('#productImageModal').on('hidden.bs.modal', function () {
    $('#productIdForImage').val('');
    $('#selectedFileName').text('');
    $('#imageFileInput').val('');
});
