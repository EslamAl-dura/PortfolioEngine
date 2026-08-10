// JavaScript for technology Index view
$(document).ready(function () {
    const $modal = $('#techModal');
    const $container = $('#techModalContainer');
    const $form = $('#techForm');
    const $validation = $('#techValidationSummary');

    // Open Modal for Create
    window.openCreateTechModal = function (categoryId) {
        $form[0].reset();
        $('#tech_Id').val('');

        if (categoryId) {
            $('#tech_CategoryId').val(categoryId);
        }

        $('#techModalTitle').html('<i class="bi bi-cpu text-blue-600"></i> Add Technology');
        $modal.removeClass('opacity-0 pointer-events-none');
        $container.removeClass('scale-95').addClass('scale-100');
    };

    // Open Modal for Edit
    window.openEditTechModal = function (id) {
        $.ajax({
            url: `/Technology/GetForEdit/${id}`,
            type: 'GET',
            success: function (data) {
                $('#tech_Id').val(data.id);
                $('#tech_Name').val(data.name);
                $('#tech_Description').val(data.description);
                $('#tech_IconClass').val(data.iconClass);
                $('#tech_CategoryId').val(data.techCategoryId);

                $('#techModalTitle').html('<i class="bi bi-pencil text-amber-500"></i> Edit Technology');
                $modal.removeClass('opacity-0 pointer-events-none');
                $container.removeClass('scale-95').addClass('scale-100');
            }
        });
    };

    window.closeTechModal = function () {
        $modal.addClass('opacity-0 pointer-events-none');
        $container.removeClass('scale-100').addClass('scale-95');
        $validation.addClass('hidden').empty();
    };

    // AJAX Create / Edit Submit Handler
    $form.on('submit', function (e) {
        e.preventDefault();
        const id = $('#tech_Id').val();
        const url = id ? `/Technology/Edit/${id}` : '/Technology/Create';

        $.ajax({
            url: url,
            type: 'POST',
            data: new FormData(this),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.success) {
                    closeTechModal();
                    window.location.reload();
                } else {
                    const errorHtml = result.errors.map(err => `<div>• ${err}</div>`).join('');
                    $validation.html(errorHtml).removeClass('hidden');
                }
            }
        });
    });

    // Delete Technology
    window.deleteTechnology = function (id, name) {
        if (confirm(`Are you sure you want to delete "${name}"?`)) {
            $.ajax({
                url: `/Technology/Delete/${id}`,
                type: 'POST',
                data: {
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                },
                success: function (result) {
                    if (result.success) {
                        window.location.reload();
                    }
                }
            });
        }
    };
});