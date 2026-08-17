$(document).ready(function () {

    const DEFAULT_ICON = 'lightning-charge-fill';

    // --- CACHED SELECTORS ---
    const $skillModal = $('#skillModal');
    const $skillContainer = $('#skillModalContainer');
    const $skillForm = $('#skillForm');
    const $skillValidation = $('#skillValidationSummary');

    const $deleteModal = $('#deleteSkillModal');
    const $deleteContainer = $('#deleteSkillModalContainer');
    const $deleteForm = $('#deleteSkillForm');

    // --- Technologies Select list ---
    const $dropdownBtn = $('#tech-dropdown-btn');
    const $dropdownMenu = $('#tech-dropdown-menu');
    const $dropdownArrow = $('#tech-dropdown-arrow');
    const $searchInput = $('#tech-search-input');
    const $nativeSelect = $('#SelectedTechnologyIds');
    const $selectedText = $('#tech-selected-text');

    // ==========================================
    // 1. ICON PICKER FUNCTIONS
    // ==========================================
    function selectIcon(key) {
        let iconKey = key || DEFAULT_ICON;
        iconKey = iconKey.replace(/^bi\s+bi-/, '').replace(/^bi-/, '');

        $('#skill_IconClass').val(iconKey);
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

    $('#iconGrid').on('click', '.icon-option-btn', function () {
        selectIcon($(this).attr('data-icon-key'));
    });

    $('#iconSearchInput').on('keyup', filterIcons);


    // ==========================================
    // 2. SKILL MODAL (CREATE / EDIT)
    // ==========================================
    window.openCreateSkillModal = function () {
        $skillForm[0].reset();
        $('#skill_Id').val('');
        $skillValidation.addClass('hidden').empty();

        // UI Customization
        $('#skillModalTitleText').text('Add Skill');
        $('#skillModalIcon').attr('class', 'bi bi-award text-blue-600');
        $('#skillSubmitBtn').text('Create Skill').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-xl shadow-sm');
        $skillForm.attr('action', '/Skill/Create');

        // Reset inputs & ranges
        $('#skill_Proficiency').val(80);
        $('#proficiencyValue').text('80%');
        $('#skill_IsSoftSkill').prop('checked', false);

        // Reset Multi-Select Checkboxes
        setSelectedTechnologies([]);

        // Reset Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(DEFAULT_ICON);

        showSkillModal();
    };

    $(document).on('click', '.edit-skill-btn', function () {
        const $btn = $(this);
        const skill = {
            id: $btn.data('id'),
            name: $btn.data('name'),
            description: $btn.data('description'),
            iconClass: $btn.data('icon'),
            proficiency: parseInt($btn.data('proficiency')),
            isSoftSkill: $btn.data('soft') === true || $btn.data('soft') === 'true',
            selectedTechnologyIds: $btn.data('techs') || []
        };

        openEditSkillModal(skill);
    });

    window.openEditSkillModal = function (skill) {
        $skillForm[0].reset();
        $skillValidation.addClass('hidden').empty();

        // UI Customization
        $('#skillModalTitleText').text('Edit Skill');
        $('#skillModalIcon').attr('class', 'bi bi-pencil-square text-amber-500');
        $('#skillSubmitBtn').text('Save Changes').attr('class', 'px-5 py-2 text-sm font-medium text-white bg-amber-600 hover:bg-amber-700 rounded-xl shadow-sm');
        $skillForm.attr('action', `/Skill/Edit/${skill.id}`);

        // Populate Form Fields
        $('#skill_Id').val(skill.id);
        $('#skill_Name').val(skill.name);
        $('#skill_Description').val(skill.description);
        $('#skill_Proficiency').val(skill.proficiency);
        $('#proficiencyValue').text(skill.proficiency + '%');
        $('#skill_IsSoftSkill').prop('checked', skill.isSoftSkill);

        // Populate Multi-Select Checkboxes
        setSelectedTechnologies(skill.selectedTechnologyIds || []);

        // Sync Icon Picker
        $('#iconSearchInput').val('');
        filterIcons();
        selectIcon(skill.iconClass);

        showSkillModal();
    };

    function showSkillModal() {
        $skillModal.removeClass('opacity-0 pointer-events-none');
        $skillContainer.removeClass('scale-95').addClass('scale-100');
    }

    window.closeSkillModal = function () {
        $skillModal.addClass('opacity-0 pointer-events-none');
        $skillContainer.removeClass('scale-100').addClass('scale-95');
        $skillValidation.addClass('hidden').empty();
        closeDropdown();
    };

    // Form Submission
    $skillForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    window.closeSkillModal();
                    window.location.reload();
                } else {
                    const errorHtml = result.errors ? result.errors.map(err => `<div>• ${err}</div>`).join('') : `<div>• ${result.message}</div>`;
                    $skillValidation.html(errorHtml).removeClass('hidden');
                }
            },
            error: function () {
                $skillValidation.html('<div>• An unexpected error occurred.</div>').removeClass('hidden');
            }
        });
    });

    // ==========================================
    // 3. TECHNOLOGIES MULTI-SELECT WITH CHECKBOXES
    // ==========================================
    // Toggle Dropdown Visibility
    $dropdownBtn.on('click', function (e) {
        e.stopPropagation();
        const isHidden = $dropdownMenu.hasClass('hidden');
        if (isHidden) {
            $dropdownMenu.removeClass('hidden');
            $dropdownArrow.addClass('rotate-180');
            $searchInput.focus();
        } else {
            closeDropdown();
        }
    });

    // Close Dropdown on Click Outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#tech-select-container').length) {
            closeDropdown();
        }
    });

    function closeDropdown() {
        $dropdownMenu.addClass('hidden');
        $dropdownArrow.removeClass('rotate-180');
        $searchInput.val('');
        filterTechOptions('');
    }

    // Filter Options Search
    $searchInput.on('input', function () {
        const query = $(this).val().toLowerCase().trim();
        filterTechOptions(query);
    });

    function filterTechOptions(query) {
        $('.tech-option-item').each(function () {
            const labelText = $(this).text().toLowerCase();
            if (labelText.includes(query)) {
                $(this).removeClass('hidden');
            } else {
                $(this).addClass('hidden');
            }
        });
    }

    // Handle Checkbox Selection Change
    $(document).on('change', '.tech-checkbox', function () {
        syncSelectedValues();
    });

    // Sync selected checkboxes with hidden native <select> and trigger button text
    window.syncSelectedValues = function () {
        const selectedValues = [];
        const selectedLabels = [];

        $('.tech-checkbox:checked').each(function () {
            selectedValues.push($(this).val());
            selectedLabels.push($(this).data('label'));
        });

        // Sync Native Select options for MVC form submission
        $nativeSelect.find('option').each(function () {
            $(this).prop('selected', selectedValues.includes($(this).val()));
        });

        // Update Display Text
        if (selectedLabels.length === 0) {
            $selectedText
                .text('Select technologies...')
                .addClass('text-slate-400 dark:text-slate-500')
                .removeClass('text-slate-900 dark:text-slate-100');
        } else if (selectedLabels.length <= 2) {
            $selectedText
                .text(selectedLabels.join(', '))
                .removeClass('text-slate-400 dark:text-slate-500')
                .addClass('text-slate-900 dark:text-slate-100');
        } else {
            $selectedText
                .text(`${selectedLabels.length} technologies selected`)
                .removeClass('text-slate-400 dark:text-slate-500')
                .addClass('text-slate-900 dark:text-slate-100');
        }
    };

    // Helper method to reset or pre-populate selected technology IDs when editing
    window.setSelectedTechnologies = function (techIds) {
        const ids = (techIds || []).map(id => id.toString());

        $('.tech-checkbox').each(function () {
            const isChecked = ids.includes($(this).val().toString());
            $(this).prop('checked', isChecked);
        });

        syncSelectedValues();
    };


    // ==========================================
    // 4. DELETE MODAL FUNCTIONS
    // ==========================================
    window.openDeleteSkillModal = function (id, name) {
        $('#delete_SkillId').val(id);
        $('#delete_SkillName').text(name);

        $deleteModal.removeClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-95').addClass('scale-100');
    };

    window.closeDeleteSkillModal = function () {
        $deleteModal.addClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-100').addClass('scale-95');
    };

    $deleteForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: `/Skill/Delete/${$('#delete_SkillId').val()}`,
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            headers: {
                "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (result) {
                if (result.success) {
                    window.closeDeleteSkillModal();
                    window.location.reload();
                } else {
                    alert(result.message);
                }
            }
        });
    });


    // ==========================================
    // 5. GLOBAL EVENTS
    // ==========================================
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') {
            if (!$skillModal.hasClass('opacity-0')) window.closeSkillModal();
            if (!$deleteModal.hasClass('opacity-0')) window.closeDeleteSkillModal();
        }
    });

});