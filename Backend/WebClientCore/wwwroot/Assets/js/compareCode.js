//var data = '@Html.Raw(Json.Encode(ViewBag.Data))';
//var json2js = JSON.parse(data);
//var abc = json2js.map(function (x) {
//    return {
//        label: x.Drink,
//        value: x.Quantity
//    };
//});


new Morris.Donut({
    // ID of the element in which to draw the chart.
    element: 'chart1',
    // Chart data records -- each entry in this array corresponds to a point on
    // the chart.
    data: [
        { label: "Download Sales", value: 12 },
        { label: "In-Store Sales", value: 30 },
        { label: "Mail-Order Sales", value: 20 }
    ]
});

new Morris.Line({
    // ID of the element in which to draw the chart.
    element: 'chart2',
    // Chart data records -- each entry in this array corresponds to a point on
    // the chart.
    data: [
        { y: '2006', a: 100, b: 90 },
        { y: '2007', a: 75, b: 60 },
        { y: '2008', a: 50, b: 40 },
        { y: '2009', a: 75, b: 55 },
        { y: '2010', a: 50, b: 40 },
        { y: '2011', a: 75, b: 85 },
        { y: '2012', a: 100, b: 80 }
    ],
    // The name of the data record attribute that contains x-values
    xkey: 'y',
    // A list of names of data record attributes that contain y-values.
    ykeys: ['a', 'b'],
    // Labels for the ykeys -- will be displayed when you hover over the
    // chart.
    labels: ['Series A', 'Series B']
});

var editor = CodeMirror.fromTextArea(document.getElementById('editor1'), {
    mode: "javascript",
    lineNumbers: true,
    theme: 'icecoder',


});
//editor.addLineClass(10, "background", "text-danger");
editor.getDoc().markText(
    { line: 10 },
    { line: 20 },
    { css: "background-color : red; color : white;" }
);

editor.getDoc().markText(
    { line: 33 },
    { line: 39 },
    { css: "background-color : red; color : white;" }
);

var editor1 = CodeMirror.fromTextArea(document.getElementById('editor2'), {
    mode: "javascript",
    lineNumbers: true,
    theme: 'icecoder',
});
