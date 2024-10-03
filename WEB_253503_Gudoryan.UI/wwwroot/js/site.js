// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    if ($('#game-list').length > 0) {
        $(document).on('click', '.page-link', function (event) {
            event.preventDefault();

            var url = $(this).attr('href');
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {

                    $('#game-list').html(data);
                }
            });
        });
    }
});
