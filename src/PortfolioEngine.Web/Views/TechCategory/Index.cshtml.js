$(document).ready(function () {

    const DEFAULT_ICON = 'folder';

    // --- CACHED SELECTORS ---
    const $categoryModal = $('#categoryModal');
    const $categoryContainer = $('#categoryModalContainer');
    const $categoryForm = $('#categoryForm');
    const $categoryValidation = $('#categoryValidationSummary');

    const $deleteModal = $('#deleteCategoryModal');
    const $deleteContainer = $('#deleteModalContainer');
    const $deleteForm = $('#deleteCategoryForm');


    // ==========================================
    // 1. ICON PICKER FUNCTIONS
    // ==========================================
    function selectIcon(key) {
        let iconKey = key || DEFAULT_ICON;
        iconKey = iconKey.replace(/^bi\s+bi-/, '').replace(/^bi-/, '');

        $('#category_IconClass').val(iconKey);
        $('#selectedIconName').text(iconKey);
        $('#selectedIconPreview').attr('class', 'bi bi-' + iconKey + ' text-sm');

        const activeClasses = 'ring-2 ring-blue-500 border-transparent bg-blue-50 dark:bg-blue-950/40 text-blue-600';

        $('.icon-option-btn').each(function () {
            const $btn = $(this);
            if ($btn.attr('data-icon-key') === iconKey) {
                $btn.addClass(activeClasses);
            } else {
                $btn.removeClass(activeClasses);
            }
        });
    }

    function filterIcons() {
        const query = $('#iconSearchInput').val().toLowerCase();

        $('.icon-option-btn').each(function () {
            const $btn = $(this);
            const key = ($btn.attr('data-icon-key') || '').toLowerCase();
            const label = ($btn.attr('data-icon-label') || '').toLowerCase();

            if (key.includes(query) || label.includes(query)) {
                $btn.css('display', 'flex');
            } else {
                $btn.hide();
            }
        });
    }

    // Grid Event Delegation & Search Input
    $('#iconGrid').on('click', '.icon-option-btn', function () {
        const key = $(this).attr('data-icon-key');
        selectIcon(key);
    });

    $('#iconSearchInput').on('keyup', filterIcons);


    // ==========================================
    // 2. CATEGORY MODAL (CREATE / EDIT)
    // ==========================================
    window.openCreateModal = function () {
        $categoryForm[0].reset();
        $('#category_Id').val('');
        $categoryValidation.addClass('hidden').empty();

        // UI Customization for Create
        $('#categoryModalTitleText').text('Add New Category');
        $('#categoryModalIcon').attr('class', 'bi bi-folder-plus text-blue-600');
        $('#categorySubmitBtn').text('Create Category').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-xl shadow-sm');
        $categoryForm.attr('action', '/TechCategory/Create');

        // Reset Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(DEFAULT_ICON);

        showCategoryModal();
    };
    window.openEditModalFromButton = function (button) {
        const category = {
            id: button.getAttribute('data-id'),
            name: button.getAttribute('data-name'),
            type: parseInt(button.getAttribute('data-type')),
            iconClass: button.getAttribute('data-icon'),
            description: button.getAttribute('data-description')
        };

        // Calls your original modal opening function
        openEditModal(category);
    }
    window.openEditModal = function (category) {
        $categoryForm[0].reset();
        $categoryValidation.addClass('hidden').empty();

        // UI Customization for Edit
        $('#categoryModalTitleText').text('Edit Technology Category');
        $('#categoryModalIcon').attr('class', 'bi bi-pencil-square text-amber-500');
        $('#categorySubmitBtn').text('Save Changes').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-amber-600 hover:bg-amber-700 rounded-xl shadow-sm');
        $categoryForm.attr('action', '/TechCategory/Edit');

        // Populate Form Fields
        $('#category_Id').val(category.id);
        $('#category_Name').val(category.name);
        $('#category_Type').val(category.type);
        $('#category_Description').val(category.description);

        // Sync Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(category.iconClass);

        showCategoryModal();
    };

    function showCategoryModal() {
        $categoryModal.removeClass('opacity-0 pointer-events-none');
        $categoryContainer.removeClass('scale-95').addClass('scale-100');
    }

    window.closeCategoryModal = function () {
        $categoryModal.addClass('opacity-0 pointer-events-none');
        $categoryContainer.removeClass('scale-100').addClass('scale-95');
        $categoryValidation.addClass('hidden').empty();
    };

    // Unified AJAX Form Submission
    $categoryForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeCategoryModal();
                    window.location.reload();
                } else {
                    const errorHtml = result.errors.map(err => `<div>• ${err}</div>`).join('');
                    $categoryValidation.html(errorHtml).removeClass('hidden');
                }
            },
            error: function () {
                $categoryValidation.html('<div>• An unexpected error occurred.</div>').removeClass('hidden');
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

    $deleteForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: `/TechCategory/Delete/${$('#delete_Id').val()}`,
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
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') {
            if (!$categoryModal.hasClass('opacity-0')) window.closeCategoryModal();
            if (!$deleteModal.hasClass('opacity-0')) window.closeDeleteModal();
        }
    });

});