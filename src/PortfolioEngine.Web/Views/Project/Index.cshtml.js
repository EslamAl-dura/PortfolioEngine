$(document).ready(function () {

    // --- CACHED SELECTORS ---
    const $projectModal = $('#projectModal');
    const $projectContainer = $('#projectModalContainer');
    const $projectForm = $('#projectForm');
    const $projectValidation = $('#projectValidationSummary');

    const $deleteModal = $('#deleteProjectModal');
    const $deleteContainer = $('#deleteProjectModalContainer');
    const $deleteForm = $('#deleteProjectForm');
    // is finished items
    const $isFinishedCheckbox = $('#project_IsFinished');
    const $endDateContainer = $('#projectEndDateContainer');
    const $endDateInput = $('#project_EndDate');
    // is live items
    const $isLiveCheckbox = $('#project_IsLive');
    const $liveUrlContainer = $('#liveUrContainer');
    const $liveUrlInput = $('#project_LiveUrl');
    

    // Toggle End Date Field Visibility
    $isFinishedCheckbox.on('change', function () {
        if ($(this).is(':checked')) {
            $endDateContainer.removeClass('opacity-50 pointer-events-none');
        } else {
            $endDateContainer.addClass('opacity-50 pointer-events-none');
            $endDateInput.val('');
        }
    });

    // Toggle Live Url Field Visibility
    $isLiveCheckbox.on('change', function () {
        if ($(this).is(':checked')) {
            $liveUrlContainer.removeClass('opacity-50 pointer-events-none');
        } else {
            $liveUrlContainer.addClass('opacity-50 pointer-events-none');
            $liveUrlInput.val('');
        }
    });

    

    // Open Modal for Create
    window.openCreateProjectModal = function () {
        $projectForm[0].reset();
        $('#project_Id').val('');
        $projectValidation.addClass('hidden').empty();

        $('#projectModalTitleText').text('Add Project');
        $('#projectModalIcon').attr('class', 'bi bi-folder-plus text-blue-600');
        $('#projectSubmitBtn')
            .text('Create Project')
            .attr('class', 'px-5 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-xl shadow-sm transition-all');

        $projectForm.attr('action', '/Project/Create');
        $isFinishedCheckbox.prop('checked', false).trigger('change');

        // Reset multiselects
        $('#project_SelectedColleagueIds, #project_SelectedSkillIds').val([]);

        showProjectModal();
    };

    // Open Modal for Edit
    $(document).on('click', '.edit-project-btn', function () {
        const $btn = $(this);

        const project = {
            id: $btn.data('id'),
            title: $btn.data('title'),
            description: $btn.data('description'),
            liveUrl: $btn.data('liveurl'),
            githubUrl: $btn.data('githuburl'),
            slug: $btn.data('slug'),
            isFinished: $btn.data('finished') === true || $btn.data('finished') === 'true',
            featured: $btn.data('featured') === true || $btn.data('featured') === 'true',
            isLive: $btn.data('islive') === true || $btn.data('islive') === 'true',
            isPublic: $btn.data('ispublic') === true || $btn.data('ispublic') === 'true',
            isTeamWork: $btn.data('isteamwork') === true || $btn.data('isteamwork') === 'true',
            startDate: $btn.data('start'),
            endDate: $btn.data('end'),
            colleagues: $btn.data('colleagues') || [],
            skills: $btn.data('skills') || []
        };

        openEditProjectModal(project);
    });

    window.openEditProjectModal = function (project) {
        $projectForm[0].reset();
        $projectValidation.addClass('hidden').empty();

        $('#projectModalTitleText').text('Edit Project');
        $('#projectModalIcon').attr('class', 'bi bi-pencil-square text-amber-500');
        $('#projectSubmitBtn')
            .text('Save Changes')
            .attr('class', 'px-5 py-2 text-sm font-medium text-white bg-amber-600 hover:bg-amber-700 rounded-xl shadow-sm transition-all');

        $projectForm.attr('action', `/Project/Edit/${project.id}`);

        $('#project_Id').val(project.id);
        $('#project_Title').val(project.title);
        $('#project_Description').val(project.description);
        $('#project_LiveUrl').val(project.liveUrl);
        $('#project_GithubUrl').val(project.githubUrl);
        $('#project_Slug').val(project.slug);

        $('#project_IsLive').prop('checked', project.isLive);
        $('#project_IsPublic').prop('checked', project.isPublic);
        $('#project_Featured').prop('checked', project.featured);
        $('#project_IsTeamWork').prop('checked', project.isTeamWork);

        $isFinishedCheckbox.prop('checked', project.isFinished).trigger('change');
        $('#project_StartDate').val(project.startDate);
        if (project.isFinished && project.endDate) {
            $endDateInput.val(project.endDate);
        }

        $('#project_SelectedColleagueIds').val(project.colleagues);
        $('#project_SelectedSkillIds').val(project.skills);

        showProjectModal();
    };

    function showProjectModal() {
        $projectModal.removeClass('opacity-0 pointer-events-none');
        $projectContainer.removeClass('scale-95').addClass('scale-100');
    }

    window.closeProjectModal = function () {
        $projectModal.addClass('opacity-0 pointer-events-none');
        $projectContainer.removeClass('scale-100').addClass('scale-95');
        $projectValidation.addClass('hidden').empty();
    };

    // AJAX Form Submission
    $projectForm.on('submit', function (e) {
        e.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    window.closeProjectModal();
                    window.location.reload();
                } else {
                    let errorHtml = '';
                    if (result.errors && result.errors.length > 0) {
                        errorHtml = result.errors.map(err => `<div>• ${err}</div>`).join('');
                    } else {
                        errorHtml = `<div>• ${result.message || 'An error occurred while saving.'}</div>`;
                    }
                    $projectValidation.html(errorHtml).removeClass('hidden');
                }
            },
            error: function () {
                $projectValidation.html('<div>• An unexpected server error occurred.</div>').removeClass('hidden');
            }
        });
    });

    // Delete Modal
    window.openDeleteProjectModal = function (id, title) {
        $('#delete_ProjectId').val(id);
        $('#delete_ProjectTitle').text(title);

        $deleteModal.removeClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-95').addClass('scale-100');
    };

    window.closeDeleteProjectModal = function () {
        $deleteModal.addClass('opacity-0 pointer-events-none');
        $deleteContainer.removeClass('scale-100').addClass('scale-95');
    };

    $deleteForm.on('submit', function (e) {
        e.preventDefault();
        const projectId = $('#delete_ProjectId').val();

        $.ajax({
            url: `/Project/Delete/${projectId}`,
            type: 'POST',
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    window.closeDeleteProjectModal();
                    window.location.reload();
                } else {
                    alert(result.message || 'Failed to delete project.');
                }
            },
            error: function () {
                alert('An error occurred while attempting to delete the record.');
            }
        });
    });

    // ESC Key handling
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') {
            if (!$projectModal.hasClass('opacity-0')) window.closeProjectModal();
            if (!$deleteModal.hasClass('opacity-0')) window.closeDeleteProjectModal();
        }
    });

});

$(document).ready(function () {

    // ==========================================
    // 1. DYNAMIC VISIBILITY FOR TEAMWORK / COLLEAGUES
    // ==========================================
    const $teamworkCheckbox = $('#project_IsTeamWork');
    const $colleagueGroup = $('#colleagueSelectionGroup');

    function toggleColleaguesVisibility() {
        if ($teamworkCheckbox.is(':checked')) {
            $colleagueGroup.removeClass('hidden');
        } else {
            $colleagueGroup.addClass('hidden');
            // Uncheck all colleagues and clear selections when set to Solo
            $('.colleague-checkbox').prop('checked', false);
            updatePopoverState('colleague');
        }
    }

    $teamworkCheckbox.on('change', toggleColleaguesVisibility);


    // ==========================================
    // 2. REUSABLE POPOVER MULTI-SELECT ENGINE
    // ==========================================
    function setupPopoverSelect(prefix, hiddenFieldName, defaultPlaceholder) {
        const $btn = $(`#${prefix}-dropdown-btn`);
        const $menu = $(`#${prefix}-dropdown-menu`);
        const $searchInput = $(`#${prefix}-search-input`);
        const $label = $(`#${prefix}-dropdown-label`);
        const $hiddenContainer = $(`#${prefix}-hidden-inputs`);
        const optionClass = `.${prefix}-option-item`;
        const checkboxClass = `.${prefix}-checkbox`;

        // Toggle dropdown display
        $btn.on('click', function (e) {
            e.stopPropagation();
            $('.popover-menu').not($menu).addClass('hidden'); // Close other active dropdowns
            $menu.toggleClass('hidden');
            if (!$menu.hasClass('hidden')) {
                $searchInput.val('').trigger('input').focus();
            }
        });

        // Prevent dropdown click from closing itself
        $menu.on('click', function (e) {
            e.stopPropagation();
        });

        // Search Filter
        $searchInput.on('input', function () {
            const query = $(this).val().toLowerCase().trim();
            $(optionClass).each(function () {
                const text = $(this).find('span').text().toLowerCase();
                $(this).toggleClass('hidden', !text.includes(query));
            });
        });

        // Checkbox Selection Handler
        $(document).on('change', checkboxClass, function () {
            updateState();
        });

        function updateState() {
            const selectedVals = [];
            const selectedLabels = [];

            $(checkboxClass + ':checked').each(function () {
                selectedVals.push($(this).val());
                selectedLabels.push($(this).data('label'));
            });

            // Update Label Display
            if (selectedLabels.length === 0) {
                $label.text(defaultPlaceholder).addClass('text-slate-400').removeClass('text-slate-900 dark:text-white font-medium');
            } else if (selectedLabels.length <= 2) {
                $label.text(selectedLabels.join(', ')).removeClass('text-slate-400').addClass('text-slate-900 dark:text-white font-medium');
            } else {
                $label.text(`${selectedLabels.length} selected`).removeClass('text-slate-400').addClass('text-slate-900 dark:text-white font-medium');
            }

            // Sync hidden inputs for MVC Model Binding (SelectedColleagueIds / SelectedSkillIds)
            $hiddenContainer.empty();
            selectedVals.forEach(id => {
                $hiddenContainer.append(`<input type="hidden" name="${hiddenFieldName}" value="${id}" />`);
            });
        }

        // Export state updater function for programmatic reset/edit load
        window.updatePopoverState = window.updatePopoverState || {};
        window.updatePopoverState[prefix] = updateState;
    }

    // Initialize Popovers
    setupPopoverSelect('colleague', 'SelectedColleagueIds', 'Select colleagues...');
    setupPopoverSelect('tech', 'SelectedSkillIds', 'Select technologies...');

    // Close popovers on outside click
    $(document).on('click', function () {
        $('#colleague-dropdown-menu, #tech-dropdown-menu').addClass('hidden');
    });


    // ==========================================
    // 3. INTEGRATION WITH CREATE / EDIT MODAL
    // ==========================================

    // Reset function helper
    function resetPopovers() {
        $('.colleague-checkbox, .tech-checkbox').prop('checked', false);
        if (window.updatePopoverState) {
            if (window.updatePopoverState.colleague) window.updatePopoverState.colleague();
            if (window.updatePopoverState.tech) window.updatePopoverState.tech();
        }
    }

    // Intercept Modal Open (Create)
    const originalOpenCreate = window.openCreateProjectModal;
    window.openCreateProjectModal = function () {
        if (typeof originalOpenCreate === 'function') originalOpenCreate();

        resetPopovers();
        toggleColleaguesVisibility();
    };

    // Intercept Modal Open (Edit)
    const originalOpenEdit = window.openEditProjectModal;
    window.openEditProjectModal = function (project) {
        if (typeof originalOpenEdit === 'function') originalOpenEdit(project);

        resetPopovers();

        // Populate Colleagues Checkboxes
        if (project.colleagues && project.colleagues.length > 0) {
            $('.colleague-checkbox').each(function () {
                if (project.colleagues.includes($(this).val())) {
                    $(this).prop('checked', true);
                }
            });
        }

        // Populate Tech Skills Checkboxes
        if (project.skills && project.skills.length > 0) {
            $('.tech-checkbox').each(function () {
                if (project.skills.includes($(this).val())) {
                    $(this).prop('checked', true);
                }
            });
        }

        // Refresh UI state
        if (window.updatePopoverState) {
            if (window.updatePopoverState.colleague) window.updatePopoverState.colleague();
            if (window.updatePopoverState.tech) window.updatePopoverState.tech();
        }

        // Dynamic visibility check
        toggleColleaguesVisibility();
    };

});