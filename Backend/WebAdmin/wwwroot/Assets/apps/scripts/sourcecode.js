
var table = $('#sourceCodeDatatable');

function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
};
// begin first table
table.dataTable({
    serverSide: true,
    "searching": false,
    "ordering": false,
    lengthChange: false,
    ajax: {
        url: API_URL + '/api/submission/admin',
        headers: {
            "Authorization": decodeURIComponent(getCookie("token"))
        },
        method: 'GET',

        data: function (d) {
            delete d.order;
            delete d.columns;
            delete d.search;
        },
        dataFilter: function (data) {
            data = JSON.parse(data);
            data.data.forEach(function (d) {
                if (d.status == '1') {
                    d.status = "PROCESSING";
                }
                if (d.status == '2') {
                    d.status = "SIMILAR";
                }
                if (d.status == '3') {
                    d.status = "NO SIMILAR";
                }
                return d;
            })

            return JSON.stringify(data); // return JSON string
        }
    },
    "columns": [
        {
            "data": "name",
            render: function (data, type, row, meta) {
                return meta.row + meta.settings._iDisplayStart + 1;
            }
        },
        {
            "data": "name",
            render: function (data, type, full, meta) {
                if (full.status == 'SIMILAR') {
                    return '<a href="/compare/' + full.id + '">' + data + '</a>';
                }
                return data;
            }
        },
        { "data": "uploadDate" },
        {
            "data": "status",
            "render": function (data, type, full, meta) {
                if (full.status == 'PROCESSING') {
                    return '<span class="badge badge-warning"> PROCESSING </span>';
                }
                if (full.status == 'SIMILAR') {

                    return '<span class="badge badge-success"> SIMILAR </span>';
                }
                if (full.status == 'NO SIMILAR') {
                    return ' <span class="badge badge-primary"> NO SIMILAR </span>';
                }
            },
        },
    ]
});

