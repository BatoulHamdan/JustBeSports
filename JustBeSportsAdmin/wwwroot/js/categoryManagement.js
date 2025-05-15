window.ParsleyConfig = {
    errorsWrapper: '<div class="invalid-feedback"></div>',
    errorTemplate: '<div></div>',
    classHandler: function (ParsleyField) {
        return ParsleyField.$element;
    }
};

let table;

$(document).ready(function () {
    $('#categoryForm').parsley();

    table = $('#tblCategories').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        info: true
    });

    loadCategories();

    $('#saveCategoryButton').off('click').on('click', function () {
        const form = $('#categoryForm');
        if (form.parsley().validate()) {
            var id = $(this).data('categoryId');
            var categoryData = {
                Id: id || 0,
                Name: $('#txtName').val(),
                Description: $('#txtDescription').val()
            };

            $.ajax({
                url: '/Category/Save',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(categoryData),
                success: function (response) {
                    alert(response.message);
                    $('#categoryModal').modal('hide');
                    form.parsley().reset();
                    form[0].reset();
                    $('#saveCategoryButton').removeData('categoryId');
                    form.find('.is-invalid').removeClass('is-invalid');
                    loadCategories();
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                    alert("Error saving category.");
                }
            });
        }
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

    window.loadCategories = loadCategories;
});

function loadCategories() {
    table.clear().draw();

    $.ajax({
        url: '/Category/GetAllCategories',
        type: 'GET',
        success: function (categories) {
            categories.forEach(function (category) {
                table.row.add([
                    `<a href="/Product/Index?categoryId=${category.id}">${category.name}</a>`,
                    category.description,
                    `<button class="btn btn-success" onclick="editCategory(${category.id})">Edit</button>
                     <button class="btn btn-danger" onclick="deleteCategory(${category.id})">Delete</button>`
                ]).draw(false);
            });
        },
        error: function () {
            alert("Error loading categories.");
        }
    });
}

function editCategory(id) {
    $.ajax({
        url: '/Category/GetCategory/' + id,
        type: 'GET',
        success: function (category) {
            $('#txtName').val(category.name);
            $('#txtDescription').val(category.description);

            $('#saveCategoryButton').data('categoryId', id);
            $('#categoryModalLabel').text('Edit Category');
            $('#categoryModal').modal('show');
        },
        error: function () {
            alert("Error loading category data.");
        }
    });
}

function deleteCategory(id) {
    if (confirm("Are you sure you want to delete this category?")) {
        $.ajax({
            url: '/Category/Delete/' + id,
            type: 'DELETE',
            success: function () {
                alert("Category deleted successfully!");
                loadCategories();
            },
            error: function () {
                alert("Error deleting category.");
            }
        });
    }
}

function addCategory() {
    $('#txtName').val('');
    $('#txtDescription').val('');
    $('#saveCategoryButton').removeData('categoryId');
    $('#categoryModalLabel').text('Add New Category');
    $('#categoryModal').modal('show');

    setTimeout(() => { $('#txtName').focus(); }, 500);
}
