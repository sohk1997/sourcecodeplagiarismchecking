var mainContent;
var titleContent;

$(function () {
    mainContent = $("#MainContent"); /// render partial views.
    titleContent = $("title"); // render titles.
});

var routingApp = $.sammy("#MainContent", function () {
    this.get("#/Dashboard/Index", function (context) {
        titleContent.html("Index");
        $.get("/Dashboard/Index", function (data) {
           context.$element().html(data);
        });
    });

    this.get("#/Demo/Index", function (context) {
        titleContent.html("Demo Index");
        //$("#BigLoader").modal('show'); // If you want to show loader
        $.get("/Demo/Index", function (data) {
            //$("#BigLoader").modal('hide');
            context.$element().html(data);
        });
    });

    //this.get("#/Student/Edit", function (context) {
    //    titleContent.html("Edit Student");
    //    $.get("/Student/Edit", {
    //        studentID: context.params.id // pass student id
    //    }, function (data) {
    //        context.$element().html(data);
    //    });
    //});

    this.get("#/Demo/CompareCode", function (context) {
        titleContent.html("Compare Code");
        $.get("/Demo/CompareCode", function (data) {
            context.$element().html(data);
        });
    });

    //this.get("#/Home/Contact", function (context) {
    //    titleContent.html("Contact");
    //    $.get("/Home/Contact", function (data) {
    //        context.$element().html(data);
    //    });
    //});
});

$(function () {
    routingApp.run("#/Dashboard/Index"); // default routing page.
});

function IfLinkNotExist(type, path) {
    if (!(type != null && path != null))
        return false;

    var isExist = true;

    if (type.toLowerCase() == "get") {
        if (routingApp.routes.get != undefined) {
            $.map(routingApp.routes.get, function (item) {
                if (item.path.toString().replace("/#", "#").replace(/\\/g, '').replace("$/", "").indexOf(path) >= 0) {
                    isExist = false;
                }
            });
        }
    } else if (type.toLowerCase() == "post") {
        if (routingApp.routes.post != undefined) {
            $.map(routingApp.routes.post, function (item) {
                if (item.path.toString().replace("/#", "#").replace(/\\/g, '').replace("$/", "").indexOf(path) >= 0) {
                    isExist = false;
                }
            });
        }
    }
    return isExist;
}