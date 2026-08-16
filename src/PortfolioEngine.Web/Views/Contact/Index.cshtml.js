$(document).ready(function () {

    const DEFAULT_ICON = 'envelope-fill';

    // --- CACHED SELECTORS ---
    const $contactModal = $('#contactModal');
    const $contactContainer = $('#contactModalContainer');
    const $contactForm = $('#contactForm');
    const $contactValidation = $('#contactValidationSummary');

    const $deleteModal = $('#deleteContactModal');
    const $deleteContainer = $('#deleteContactModalContainer');
    const $deleteForm = $('#deleteContactForm');


    // ==========================================
    // 1. ICON PICKER FUNCTIONS
    // ==========================================
    function selectIcon(key) {
        let iconKey = key || DEFAULT_ICON;
        iconKey = iconKey.replace(/^bi\s+bi-/, '').replace(/^bi-/, '');

        $('#contact_IconClass').val(iconKey);
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
    // 2. CONTACT MODAL (CREATE / EDIT)
    // ==========================================
    window.openCreateContactModal = function () {
        $contactForm[0].reset();
        $('#contact_Id').val('');
        $contactValidation.addClass('hidden').empty();

        // UI Customization for Create
        $('#contactModalTitleText').text('Add Contact Channel');
        $('#contactModalIcon').attr('class', 'bi bi-person-plus text-blue-600');
        $('#contactSubmitBtn').text('Create Contact').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-xl shadow-sm');
        $contactForm.attr('action', '/Contact/Create');

        // Reset Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(DEFAULT_ICON);

        showContactModal();
    };
    window.openEditContactModalFromBtn = function (btn) {
        const contact = {
            id: btn.getAttribute('data-id'),
            name: btn.getAttribute('data-name'),
            value: btn.getAttribute('data-value'),
            contactType: btn.getAttribute('data-type'),
            iconClass: btn.getAttribute('data-icon'),
            description: btn.getAttribute('data-description')
        };

        openEditContactModal(contact);
    };
    window.openEditContactModal = function (contact) {
        $contactForm[0].reset();
        $contactValidation.addClass('hidden').empty();

        // UI Customization for Edit
        $('#contactModalTitleText').text('Edit Contact Channel');
        $('#contactModalIcon').attr('class', 'bi bi-pencil-square text-amber-500');
        $('#contactSubmitBtn').text('Save Changes').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-amber-600 hover:bg-amber-700 rounded-xl shadow-sm');
        $contactForm.attr('action', `/Contact/Edit/${contact.id}`);

        // Populate Form Fields
        $('#contact_Id').val(contact.id);
        $('#contact_Name').val(contact.name);
        $('#contact_Value').val(contact.value);
        $('#contact_ContactType').val(contact.contactType);
        $('#contact_Description').val(contact.description);

        // Sync Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(contact.iconClass);

        showContactModal();
    };

    function showContactModal() {
        $contactModal.removeClass('opacity-0 pointer-events-none');
        $contactContainer.removeClass('scale-95').addClass('scale-100');
    }

    window.closeContactModal = function () {
        $contactModal.addClass('opacity-0 pointer-events-none');
        $contactContainer.removeClass('scale-100').addClass('scale-95');
        $contactValidation.addClass('hidden').empty();
    };

    // Unified AJAX Form Submission
    $contactForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeContactModal();
                    window.location.reload();
                } else {
                    const errorHtml = result.errors ? result.errors.map(err => `<div>• ${err}</div>`).join('') : `<div>• ${result.message}</div>`;
                    $contactValidation.html(errorHtml).removeClass('hidden');
                }
            },
            error: function () {
                $contactValidation.html('<div>• An unexpected error occurred.</div>').removeClass('hidden');
            }
        });
    });


    // ==========================================
    // 3. DELETE MODAL FUNCTIONS
    // ==========================================
    window.openDeleteContactModal = function (id, name) {
        $('#delete_ContactId').val(id);
        $('#delete_ContactName').text(name);

        $deleteModal.removeClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-95').addClass('scale-100');
    };

    window.closeDeleteContactModal = function () {
        $deleteModal.addClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-100').addClass('scale-95');
    };

    $deleteForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: `/Contact/Delete/${$('#delete_ContactId').val()}`,
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            headers: {
                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (result) {
                if (result.success) {
                    window.closeDeleteContactModal();
                    window.location.reload();
                } else {
                    alert(result.message);
                }
            }
        });
    });


    // ==========================================
    // 4. GLOBAL EVENTS
    // ==========================================
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') {
            if (!$contactModal.hasClass('opacity-0')) window.closeContactModal();
            if (!$deleteModal.hasClass('opacity-0')) window.closeDeleteContactModal();
        }
    });

});