$(document).ready(function () {

    // --- CACHED SELECTORS ---
    const $colleagueModal = $('#colleagueModal');
    const $colleagueContainer = $('#colleagueModalContainer');
    const $colleagueForm = $('#colleagueForm');
    const $colleagueValidation = $('#colleagueValidationSummary');

    const $deleteModal = $('#deleteColleagueModal');
    const $deleteContainer = $('#deleteColleagueModalContainer');
    const $deleteForm = $('#deleteColleagueForm');

    const $avatarInput = $('#colleague_AvatarFile');
    const $avatarPreview = $('#avatarPreview');
    const $avatarPlaceholderIcon = $('#avatarPlaceholderIcon');
    const $untilNowCheckbox = $('#colleague_UntilNow');
    const $endDateContainer = $('#endDateContainer');
    const $endDateInput = $('#colleague_EndDate');


    // ==========================================
    // 1. UI TOGGLES & AVATAR PREVIEW
    // ==========================================

    // Handle "Currently working together" (Until Now) checkbox change
    $untilNowCheckbox.on('change', function () {
        if ($(this).is(':checked')) {
            $endDateContainer.addClass('opacity-50 pointer-events-none');
            $endDateInput.val('').prop('required', false);
        } else {
            $endDateContainer.removeClass('opacity-50 pointer-events-none');
        }
    });

    // File Upload Preview Handler
    $avatarInput.on('change', function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $avatarPreview.attr('src', e.target.result).removeClass('hidden');
                $avatarPlaceholderIcon.addClass('hidden');
            };
            reader.readAsDataURL(file);
        }
    });

    function resetAvatarPreview(existingUrl = '') {
        $avatarInput.val('');
        if (existingUrl) {
            $avatarPreview.attr('src', existingUrl).removeClass('hidden');
            $avatarPlaceholderIcon.addClass('hidden');
        } else {
            $avatarPreview.attr('src', '').addClass('hidden');
            $avatarPlaceholderIcon.removeClass('hidden');
        }
    }


    // ==========================================
    // 2. CREATE & EDIT MODAL HANDLERS
    // ==========================================

    window.openCreateColleagueModal = function () {
        $colleagueForm[0].reset();
        $('#colleague_Id').val('');
        $colleagueValidation.addClass('hidden').empty();

        // Modal Headers & Buttons Setup
        $('#colleagueModalTitleText').text('Add Colleague');
        $('#colleagueModalIcon').attr('class', 'bi bi-people text-blue-600');
        $('#colleagueSubmitBtn')
            .text('Create Colleague')
            .attr('class', 'px-5 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-xl shadow-sm transition-all');
        $colleagueForm.attr('action', '/Colleague/Create');

        // Reset Date Controls & Preview
        $untilNowCheckbox.prop('checked', false).trigger('change');
        resetAvatarPreview('');

        showColleagueModal();
    };

    $(document).on('click', '.edit-colleague-btn', function () {
        const $btn = $(this);
        const colleague = {
            id: $btn.data('id'),
            name: $btn.data('name'),
            role: $btn.data('role'),
            profileUrl: $btn.data('profile'),
            avatarUrl: $btn.data('avatar'),
            startDate: $btn.data('start'),
            untilNow: $btn.data('until') === true || $btn.data('until') === 'true',
            endDate: $btn.data('end')
        };

        openEditColleagueModal(colleague);
    });

    window.openEditColleagueModal = function (colleague) {
        $colleagueForm[0].reset();
        $colleagueValidation.addClass('hidden').empty();

        // Modal Headers & Buttons Setup
        $('#colleagueModalTitleText').text('Edit Colleague');
        $('#colleagueModalIcon').attr('class', 'bi bi-pencil-square text-amber-500');
        $('#colleagueSubmitBtn')
            .text('Save Changes')
            .attr('class', 'px-5 py-2 text-sm font-medium text-white bg-amber-600 hover:bg-amber-700 rounded-xl shadow-sm transition-all');
        $colleagueForm.attr('action', `/Colleague/Edit/${colleague.id}`);

        // Populate Input Fields
        $('#colleague_Id').val(colleague.id);
        $('#colleague_Name').val(colleague.name);
        $('#colleague_Role').val(colleague.role);
        $('#colleague_ProfileUrl').val(colleague.profileUrl);
        $('#colleague_StartDate').val(colleague.startDate);

        // Populate Checkbox & Dates
        $untilNowCheckbox.prop('checked', colleague.untilNow).trigger('change');
        if (!colleague.untilNow && colleague.endDate) {
            $endDateInput.val(colleague.endDate);
        }

        // Preview Existing Avatar
        resetAvatarPreview(colleague.avatarUrl);

        showColleagueModal();
    };

    function showColleagueModal() {
        $colleagueModal.removeClass('opacity-0 pointer-events-none');
        $colleagueContainer.removeClass('scale-95').addClass('scale-100');
    }

    window.closeColleagueModal = function () {
        $colleagueModal.addClass('opacity-0 pointer-events-none');
        $colleagueContainer.removeClass('scale-100').addClass('scale-95');
        $colleagueValidation.addClass('hidden').empty();
    };

    // AJAX Form Submission for Create / Edit
    $colleagueForm.on('submit', function (e) {
        e.preventDefault();

        const formData = new FormData(this);

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeColleagueModal();
                    window.location.reload();
                } else {
                    let errorHtml = '';
                    if (result.errors && result.errors.length > 0) {
                        errorHtml = result.errors.map(err => `<div>• ${err}</div>`).join('');
                    } else {
                        errorHtml = `<div>• ${result.message || 'An error occurred while saving.'}</div>`;
                    }
                    $colleagueValidation.html(errorHtml).removeClass('hidden');
                }
            },
            error: function () {
                $colleagueValidation.html('<div>• An unexpected server error occurred.</div>').removeClass('hidden');
            }
        });
    });


    // ==========================================
    // 3. DELETE MODAL HANDLERS
    // ==========================================

    window.openDeleteColleagueModal = function (id, name) {
        $('#delete_ColleagueId').val(id);
        $('#delete_ColleagueName').text(name);

        $deleteModal.removeClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-95').addClass('scale-100');
    };

    window.closeDeleteColleagueModal = function () {
        $deleteModal.addClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-100').addClass('scale-95');
    };

    // AJAX Submission for Delete
    $deleteForm.on('submit', function (e) {
        e.preventDefault();
        const colleagueId = $('#delete_ColleagueId').val();

        $.ajax({
            url: `/Colleague/Delete/${colleagueId}`,
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeDeleteColleagueModal();
                    window.location.reload();
                } else {
                    alert(result.message || 'Failed to delete colleague.');
                }
            },
            error: function () {
                alert('An error occurred while attempting to delete the record.');
            }
        });
    });


    // ==========================================
    // 4. GLOBAL SHORTCUTS
    // ==========================================

    // Close modals on Escape key press
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') {
            if (!$colleagueModal.hasClass('opacity-0')) window.closeColleagueModal();
            if (!$deleteModal.hasClass('opacity-0')) window.closeDeleteColleagueModal();
        }
    });

});