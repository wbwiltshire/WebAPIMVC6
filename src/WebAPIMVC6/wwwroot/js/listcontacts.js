$(document).ready(function () {
    var baseUrl = '/api/contact';
    //debugger;

    CustomersList();

    function CustomersList() {
        var url = baseUrl;
        var gridBodyId = 'ContactsList';
        var template = 'contactsListHGrid';
        ajaxGet({
            url: url,
            success: function (result) {
                jsonresult = result;
                bindGrid(gridBodyId, template, result);
            }
        });
    }

    function bindGrid(grid, src, data) {
        var result = '{"' + grid + '":' + JSON.stringify(data) + "}";
        var source = $('#' + src).html();
        var template = Handlebars.compile(source);
        var html = template(JSON.parse(result));
        $("#" + grid).html(html);
    }
});
