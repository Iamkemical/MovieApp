var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Movie/GetAllMovies",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "subGenres.name", "width": "15%" },
            { "data": "genres.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Movie/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='fa fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/Movie/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='fa fa-trash'></i></a>
                                </div>
                            `;
                }, "width": "10%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}