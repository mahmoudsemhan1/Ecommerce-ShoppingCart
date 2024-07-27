var dtble;
$(document).ready(function () {
    loaddata();
});

function loaddata() {
    $.ajax({
        url: "/Admin/Product/GetData",
        method: "GET",
        dataType: "json",
        success: function (data) {
            console.log("Data received from server:", data); // Log the data for inspection

            dtble = $("#mytable").DataTable({
                data: data, // Assuming the data is an array of objects
                columns: [
                    { data: "name" },
                    { data: "description" }, // Ensure this matches the key in the data
                    { data: "price" },
                    { data: "category.name" },
                    {
                        data: null,
                        render: function (data, type, row) {
                            return `<a href="/Admin/Product/Edit/${data.id}" class="btn btn-warning">Edit</a>
                                    <a  onClick=DeleteItem("/Admin/Product/Delete/${data.id}") class="btn btn-danger">Delete</a>`;
                        }
                    }
                ]
            });
        },
        error: function (xhr, status, error) {
            console.error("Failed to fetch data:", status, error);
        }
    });
}
$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);
        var id = btn.data('id'); // Assuming the data-id attribute holds the game's ID

        const swal = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-danger mx-2",
                cancelButton: "btn btn-light"
            },
            buttonsStyling: false
        });

        swal.fire({
            title: "Are you sure that you need to ddelete this game?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            cancelButtonText: "No, cancel!",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/Admin/Product/Delete/${id}`,
                    method: 'DELETE',
                    success: function () {
                        swal.fire(
                            'Deleted!',
                            'Game has been deleted.',
                            'success'
                        );
                        btn.parents('tr').fadeOut();
                    },
                    error: function () {
                        swal.fire(
                            'Ooops...',
                            'Something went wrong',
                            'error'
                        );
                    }
                });

            }
        });

    });
});
