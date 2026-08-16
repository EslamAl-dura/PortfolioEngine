$(document).ready(function () {

    const DEFAULT_ICON = 'code-slash';

    // --- CACHED SELECTORS ---
    const $techModal = $('#techModal');
    const $techContainer = $('#techModalContainer');
    const $techForm = $('#techForm');
    const $techValidation = $('#techValidationSummary');

    const $deleteModal = $('#deleteTechModal');
    const $deleteContainer = $('#deleteTechModalContainer');
    const $deleteForm = $('#deleteTechForm');


    // ==========================================
    // 1. ICON PICKER FUNCTIONS
    // ==========================================
    function selectIcon(key) {
        let iconKey = key || DEFAULT_ICON;
        iconKey = iconKey.replace(/^bi\s+bi-/, '').replace(/^bi-/, '');

        $('#tech_IconClass').val(iconKey);
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
    // 2. TECHNOLOGY MODAL (CREATE / EDIT)
    // ==========================================
    window.openCreateTechModal = function (categoryId) {
        $techForm[0].reset();
        $('#tech_Id').val('');
        $techValidation.addClass('hidden').empty();

        if (categoryId) {
            $('#tech_CategoryId').val(categoryId);
        } else {
            $('#tech_CategoryId').val('');
        }

        // UI Customization for Create
        $('#techModalTitleText').text('Add New Technology');
        $('#techModalIcon').attr('class', 'bi bi-cpu text-blue-600');
        $('#techSubmitBtn').text('Create Technology').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-xl shadow-sm');
        $techForm.attr('action', '/Technology/Create');

        // Reset Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(DEFAULT_ICON);

        showTechModal();
    };

    window.openEditTechModalFromBtn = function (btn) {
        const tech = {
            id: btn.getAttribute('data-id'),
            name: btn.getAttribute('data-name'),
            techCategoryId: btn.getAttribute('data-categoryid'),
            iconClass: btn.getAttribute('data-icon'),
            description: btn.getAttribute('data-description')
        };

        openEditTechModal(tech);
    };

    window.openEditTechModal = function (tech) {
        $techForm[0].reset();
        $techValidation.addClass('hidden').empty();

        // UI Customization for Edit
        $('#techModalTitleText').text('Edit Technology');
        $('#techModalIcon').attr('class', 'bi bi-pencil-square text-amber-500');
        $('#techSubmitBtn').text('Save Changes').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-amber-600 hover:bg-amber-700 rounded-xl shadow-sm');
        $techForm.attr('action', `/Technology/Edit/${tech.id}`);

        // Populate Form Fields
        $('#tech_Id').val(tech.id);
        $('#tech_Name').val(tech.name);
        $('#tech_CategoryId').val(tech.techCategoryId);
        $('#tech_Description').val(tech.description);

        // Sync Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(tech.iconClass);

        showTechModal();
    };

    function showTechModal() {
        $techModal.removeClass('opacity-0 pointer-events-none');
        $techContainer.removeClass('scale-95').addClass('scale-100');
    }

    window.closeTechModal = function () {
        $techModal.addClass('opacity-0 pointer-events-none');
        $techContainer.removeClass('scale-100').addClass('scale-95');
        $techValidation.addClass('hidden').empty();
    };

    // Unified AJAX Form Submission
    $techForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeTechModal();
                    window.location.reload();
                } else {
                    const errorHtml = result.errors.map(err => `<div>• ${err}</div>`).join('');
                    $techValidation.html(errorHtml).removeClass('hidden');
                }
            },
            error: function () {
                $techValidation.html('<div>• An unexpected error occurred.</div>').removeClass('hidden');
            }
        });
    });


    // ==========================================
    // 3. DELETE MODAL FUNCTIONS
    // ==========================================
    window.openDeleteTechModal = function (id, name) {
        $('#delete_TechId').val(id);
        $('#delete_TechName').text(name);

        $deleteModal.removeClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-95').addClass('scale-100');
    };

    window.closeDeleteTechModal = function () {
        $deleteModal.addClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-100').addClass('scale-95');
    };

    $deleteForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: `/Technology/Delete/${$('#delete_TechId').val()}`,
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeDeleteTechModal();
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
            if (!$techModal.hasClass('opacity-0')) window.closeTechModal();
            if (!$deleteModal.hasClass('opacity-0')) window.closeDeleteTechModal();
        }
    });

});