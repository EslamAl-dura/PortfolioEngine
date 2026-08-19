$(document).ready(function () {
    const $filterForm = $('#filterForm');
    const token = $('#csrfForm input[name="__RequestVerificationToken"]').val();

    // Sorting columns logic
    $('.sortable-col').on('click', function () {
        const sortField = $(this).data('sort');
        const currentSort = $('input[name="SortBy"]').val();
        const currentIsDesc = $('input[name="IsDescending"]').val() === 'true';

        if (currentSort === sortField) {
            $('input[name="IsDescending"]').val(!currentIsDesc);
        } else {
            $('input[name="SortBy"]').val(sortField);
            $('input[name="IsDescending"]').val('false');
        }

        $filterForm.submit();
    });

    // Pagination helper
    window.goToPage = function (page) {
        $('#pageNumberInput').val(page);
        $filterForm.submit();
    };

    // View Details Modal
    window.viewMessageDetails = function (id) {
        $.ajax({
            url: `/Message/Details/${id}`,
            type: 'GET',
            success: function (res) {
                if (res.success) {
                    $('#msgSender').text(res.data.senderName);
                    $('#msgEmail').text(res.data.senderEmail);
                    $('#msgSubject').text(res.data.subject);
                    $('#msgContent').text(res.data.content);

                    $('#detailsModal').removeClass('opacity-0 pointer-events-none');
                    $('#detailsContainer').removeClass('scale-95').addClass('scale-100');
                }
            }
        });
    };

    window.closeDetailsModal = function () {
        $('#detailsModal').addClass('opacity-0 pointer-events-none');
        $('#detailsContainer').removeClass('scale-100').addClass('scale-95');
    };

    // Toggle Read Status
    window.toggleReadStatus = function (id) {
        $.ajax({
            url: `/Message/ToggleRead/${id}`,
            type: 'POST',
            data: { __RequestVerificationToken: token },
            success: function (res) {
                if (res.success) window.location.reload();
            }
        });
    };

    // Delete Action
    window.openDeleteModal = function (id) {
        if (confirm('Are you sure you want to delete this message?')) {
            $.ajax({
                url: `/Message/Delete/${id}`,
                type: 'POST',
                data: { __RequestVerificationToken: token },
                success: function (res) {
                    if (res.success) window.location.reload();
                }
            });
        }
    };
});