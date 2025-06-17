var dataTable;
$(document).ready(function () {
    loadData();
});

function loadData() {
    dataTable = $("#products-table").DataTable({
        "ajax": { "url": "/Product/GetData" },
        "columns": [
            {"data":"name"},
            {"data":"description"},
            {"data":"price"},
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a href="/Product/Edit/${data}" class="btn btn-success">Edit</a>
                            <a href="/Product/Delete/${data}" class="btn btn-danger">Delete</a>
                    `;
                }
            }
        ]
    });
}