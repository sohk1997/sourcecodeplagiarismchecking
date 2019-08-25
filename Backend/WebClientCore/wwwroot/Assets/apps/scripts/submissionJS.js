var table = $('#submissionDatatable');
function getCookie(name) {
    var value = "; " + document.cookie;
    console.log("cookie " + value);
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
};
// begin first table
table.dataTable({

    // Internationalisation. For more info refer to http://datatables.net/manual/i18n
    serverSide: true,
    "searching": false,
    "ordering": false,
    lengthChange: false,
    ajax: {
        url: API_URL + '/api/submission',
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

//var tableWrapper = jQuery('#submission_datatable_1_wrapper');
//
//    table.find('.group-checkable').change(function () {
//        var set = jQuery(this).attr("data-set");
//        var checked = jQuery(this).is(":checked");
//        jQuery(set).each(function () {
//            if (checked) {
//                $(this).prop("checked", true);
//                $(this).parents('tr').addClass("active");
//            } else {
//                $(this).prop("checked", false);
//                $(this).parents('tr').removeClass("active");
//            }
//        });
//        jQuery.uniform.update(set);
//    });

//    table.on('change', 'tbody tr .checkboxes', function () {
//        $(this).parents('tr').toggleClass("active");
//    });

