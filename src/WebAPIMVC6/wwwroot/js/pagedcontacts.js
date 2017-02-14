$(document).ready(function () {
    var baseUrl = '/api/contact/Paged';
    //debugger;


    var contactTable = new TurboTablesLib({
        tableId: 'ContactsTable',
        totalItemsAttribute: 'ctTotalItems',
        page: 1,
        pageSize: 10,
        pagerSizeOptions: [[10, 25, 50, -1], [10, 25, 50, 'All']],
        sortColumn: 'LastName',
        sortDirection: 'asc',
        columnHeaderClass: 'colHeaders',
        spinnerSource: '/images/spinner-128.gif'
    });

    contactTable.setDataBinding(ContactsList);

    ContactsList(contactTable.getPage(), contactTable.getPageSize(), contactTable.getSortColumn(), contactTable.getSortDirection());

    function ContactsList(page, pageSize, sortColumn, direction) {
        var requestString = '?page=' + page + '&pageSize=' + pageSize + '&sortColumn=' + sortColumn + '&direction=' + direction;
        var url = baseUrl + requestString;
        var gridBodyId = 'ContactsList';
        var template = 'contactsListHGrid';
        ajaxGet({
            url: url,
            success: function (result) {
                jsonresult = result;
                if (parseInt(result.totalItems, 10) > 0)
                    bindGrid(gridBodyId, template, result.contacts);
                else
                    bindNoRecords(gridBodyId);
                contactTable.endDataBinding(result.totalItems);
            },
            error: function (result, status, xhr) {
                bindNoRecords(gridBodyId);
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

    function bindNoRecords(gridBodyId) {
        var html = '<tr id="noRecordsFound"><td class="lead text-left text-danger" colspan= "4">No Records Found!</td></tr>';
        $("#" + gridBodyId).html(html);
    }
});
