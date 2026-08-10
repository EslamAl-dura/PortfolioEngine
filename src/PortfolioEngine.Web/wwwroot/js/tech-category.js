$(document).ready(function () {

    // --- CACHED SELECTORS ---
    const $createModal = $('#createCategoryModal');
    const $createContainer = $('#modalContainer');
    const $createForm = $('#createCategoryForm');
    const $createValidation = $('#validationSummary');

    const $editModal = $('#editCategoryModal');
    const $editContainer = $('#editModalContainer');
    const $editForm = $('#editCategoryForm');
    const $editValidation = $('#editValidationSummary');

    const $deleteModal = $('#deleteCategoryModal');
    const $deleteContainer = $('#deleteModalContainer');
    const $deleteForm = $('#deleteCategoryForm');


    // ==========================================
    // 1. CREATE MODAL FUNCTIONS
    // ==========================================
    window.openModal = function () {
        $createModal.removeClass('opacity-0 pointer-events-none');
        $createContainer.removeClass('scale-95').addClass('scale-100');
    };

    window.closeModal = function () {
        $createModal.addClass('opacity-0 pointer-events-none');
        $createContainer.removeClass('scale-100').addClass('scale-95');
        $createForm[0].reset();
        $createValidation.addClass('hidden').empty();
    };

    // AJAX Create Submit
    $createForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeModal();
                    window.location.reload();
                } else {
                    const errorHtml = result.errors.map(err => `<div>• ${err}</div>`).join('');
                    $createValidation.html(errorHtml).removeClass('hidden');
                }
            },
            error: function () {
                $createValidation.html('<div>• An unexpected error occurred.</div>').removeClass('hidden');
            }
        });
    });


    // ==========================================
    // 2. EDIT MODAL FUNCTIONS
    // ==========================================
    window.openEditModal = function (id) {
        $.ajax({
            url: `/TechCategory/GetForEdit/${id}`,
            type: 'GET',
            success: function (data) {
                $('#edit_Id').val(data.id);
                $('#edit_Name').val(data.name);
                $('#edit_Type').val(data.type);
                $('#edit_IconClass').val(data.iconClass);
                $('#edit_Description').val(data.description);

                $editModal.removeClass('opacity-0 pointer-events-none');
                $editContainer.removeClass('scale-95').addClass('scale-100');
            },
            error: function () {
                alert('Could not fetch category details.');
            }
        });
    };

    window.closeEditModal = function () {
        $editModal.addClass('opacity-0 pointer-events-none');
        $editContainer.removeClass('scale-100').addClass('scale-95');
        $editValidation.addClass('hidden').empty();
    };

    // AJAX Edit Submit
    $editForm.on('submit', function (e) {
        e.preventDefault();
        const id = $('#edit_Id').val();

        $.ajax({
            url: `/TechCategory/Edit/${id}`,
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeEditModal();
                    window.location.reload();
                } else {
                    const errorHtml = result.errors.map(err => `<div>• ${err}</div>`).join('');
                    $editValidation.html(errorHtml).removeClass('hidden');
                }
            }
        });
    });


    // ==========================================
    // 3. DELETE MODAL FUNCTIONS
    // ==========================================
    window.openDeleteModal = function (id, name) {
        $('#delete_Id').val(id);
        $('#delete_CategoryName').text(name);

        $deleteModal.removeClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-95').addClass('scale-100');
    };

    window.closeDeleteModal = function () {
        $deleteModal.addClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-100').addClass('scale-95');
    };

    // AJAX Delete Submit
    $deleteForm.on('submit', function (e) {
        e.preventDefault();
        const id = $('#delete_Id').val();

        $.ajax({
            url: `/TechCategory/Delete/${id}`,
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeDeleteModal();
                    window.location.reload();
                }
            }
        });
    });


    // ==========================================
    // 4. GLOBAL EVENTS
    // ==========================================
    // Close active modal on Escape key
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') {
            if (!$createModal.hasClass('opacity-0')) window.closeModal();
            if (!$editModal.hasClass('opacity-0')) window.closeEditModal();
            if (!$deleteModal.hasClass('opacity-0')) window.closeDeleteModal();
        }
    });

});