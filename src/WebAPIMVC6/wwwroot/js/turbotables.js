//
//Copyright (c) Seagull Consulting, Inc. All rights reserved. See License.txt in the project root for license information.
//
//Version 0.9.1
//
//Public methods:
//   setDataBinding(function)
//   endDataBinding(totalItems)
//   getPage()
//   getPageSize()
//   getSortColumn()
//   getSortDirection()
//   showSpinner(show)
//
//Private methods:
//   CreateFilterRow(tableId)
//   CreatePagingControls(tableId)
//   EnableRowSorting(tableId)
//   FirstPage(event)
//   PreviousPage(event)
//   NextPage(event)
//   LastPage(event)
//   UpdatePagerControls()
//   ShowTableInfo()
//   FilterTable()
//   ProcessSortEvent(id)

//Define constructor for TurboTables Library
function TurboTablesLib(options) {


     //register the instance
     if (options.tableId )
         this.tableId = options.tableId;
     else
          throw 'NextPage Library requires the Table Id to run!';

     //Create locally scoped variables ($this)
     this.page = 1;
     this.pageSize = 10;
     this.pagerSizeOptions = [];
     this.sortColumn = 'Id';
     this.sortDirection = 'asc';
     this.filterPropertyValue;
     this.columnHeaderClass = 'colHeaders';
     this.spinnerSource = '/images/spinner.gif';
     this.totalItemsAttribute = '';
     this.totalItems = 0;
     this.tableInfoId = 'showing';
     this.filterInputId = 'filter';
     this.selectPgSizerId = 'pageSize';
     this.tableInfoTemplate = 'Showing {0} to {1} of {2} entries';
     this.firstButtonId = 'first';
     this.prevButtonId = 'prev';
     this.nextButtonId = 'next';
     this.lastButtonId = 'last';
     this.spinnerId = 'spinner';
     this.dataBinder = function () { };

     if (options) {
          if (options.page)
               this.page = options.page;
          if (options.pageSize)
               this.pageSize = options.pageSize;
          if (options.pagerSizeOptions)
               this.pagerSizeOptions = options.pagerSizeOptions;
          if (options.sortColumn)
               this.sortColumn = options.sortColumn;
          if (options.sortDirection)
               this.sortDirection = options.sortDirection;
          if (options.columnHeaderClass)
               this.columnHeaderClass = options.columnHeaderClass;
          if (options.spinnerSource)
               this.spinnerSource = options.spinnerSource;
          if (options.totalItemsAttribute)
               this.totalItemsAttribute = options.totalItemsAttribute;
     }

     //Retrieve total items from table (perhaps needs more error checking)
     this.totalItems = parseInt(document.getElementById(this.tableId).getAttribute(this.totalItemsAttribute), 10);

     //Add Showing and Filter row and functionality
     this.CreateFilterRow(this.tableId);

     //Add Paging controls and PageSizer control
     this.CreatePagingControls(this.tableId);

     //Hook events for row sorting
     this.EnableRowSorting(this.tableId);

     //Add spinner
     this.AddSpinner(this.tableId);

     //enableRowSorting();
     this.ShowTableInfo();

     return this;
};

TurboTablesLib.prototype.setDataBinding = function (dataBinder) {
     //Call data binding
     this.dataBinder = dataBinder;
     return this;
};

TurboTablesLib.prototype.endDataBinding = function (totalItems) {
     this.totalItems = totalItems;
     document.getElementById(this.tableId).setAttribute(this.totalItemsAttribute, this.totalItems);
     this.ShowTableInfo();
     //Stop the spinner
     this.showSpinner(false);
};

TurboTablesLib.prototype.getPage = function () {
     return this.page;
};

TurboTablesLib.prototype.getPageSize = function () {
     return this.pageSize;
};


TurboTablesLib.prototype.getSortColumn = function () {
     return this.sortColumn;
};

TurboTablesLib.prototype.getSortDirection = function () {
     return this.sortDirection;
};

//Show / Hide waiting data spinner
TurboTablesLib.prototype.showSpinner = function (show) {
     if (show)
          document.getElementById(this.spinnerId).style.display = 'block';
     else
          document.getElementById(this.spinnerId).style.display = 'none';

};
//Create 'private' Helper functions

//Create the Filter Row elements (Table info and filter input) above the table
TurboTablesLib.prototype.CreateFilterRow = function (tableId) {

     //Create elements in the filter row
     var filterNode = document.createElement('div');
     filterNode.className = 'row';
     var formNode = document.createElement('form');
     formNode.className = 'form-inline';

     var showingLabelNode = document.createElement('label');
     showingLabelNode.innerHTML = this.tableInfoTemplate;
     showingLabelNode.id = this.tableInfoId;
     var infoNode = document.createElement('div');
     infoNode.className = 'col-sm-5 table-info';
     infoNode.appendChild(showingLabelNode);
     formNode.appendChild(infoNode)

     var filterLabelNode = document.createElement('label');
     filterLabelNode.innerHTML = 'Filter:';
     var filterInputNode = document.createElement('input');
     filterInputNode.id = this.filterInputId;
     filterInputNode.setAttribute('type', 'search');
     filterInputNode.className= 'form-control input-sm';
     var filterDivNode = document.createElement('div');
     filterDivNode.className = 'col-sm-7 table-filter';
     filterDivNode.appendChild(filterLabelNode);
     filterDivNode.appendChild(filterInputNode);
     formNode.appendChild(filterDivNode);
      
     filterNode.appendChild(formNode);
     var insertPosition = document.getElementById(tableId).parentNode.parentNode;
     insertPosition.parentNode.insertBefore(filterNode, insertPosition);

     var self = this;
     //Hook events for filter
     filterInputNode.addEventListener("keyup", function () {
          self.FilterTable();
     });
     filterInputNode.onmouseover = function () {
          filterInputNode.setAttribute('cursor', 'pointer');
     };
     filterInputNode.onmouseleave = function () {
          filterInputNode.setAttribute('cursor', 'auto');
     };
};

//Add the Spinner element above the table
TurboTablesLib.prototype.AddSpinner = function (tableId) {

     //Create spinner elements
     var spinnerNode = document.createElement('div');
     spinnerNode.className = 'spinner';
     spinnerNode.id = this.spinnerId;
     var spinnerImageNode = document.createElement('img');
     spinnerImageNode.className = 'img-responsive center-block';
     spinnerImageNode.setAttribute('src', this.spinnerSource);
     spinnerImageNode.setAttribute('alt', "Loading...");

     //Insert spinner above table
     spinnerNode.appendChild(spinnerImageNode);
     var insertPosition = document.getElementById(tableId).parentNode.parentNode;
     insertPosition.parentNode.insertBefore(spinnerNode, insertPosition);
};

TurboTablesLib.prototype.EnableRowSorting = function (tableId) {
     //throw 'Not implemented exception';
     var self = this;

     //Discover the header columns and add event listeners to them
     var thead = document.getElementById(tableId).getElementsByTagName('thead');
     var colRows = thead[0].getElementsByTagName('tr');
     var colHeaders = colRows[0].getElementsByTagName('th');

     for (var idx = 0; idx < colHeaders.length; idx++) {
          //If sorting specified for the column, then hook the click event
          if (colHeaders[idx].className.indexOf('sortable') !== -1) {
               colHeaders[idx].addEventListener('click', function () {
                    self.ProcessSortEvent(this.id);
               });
          };
     };
};

//Create the Paging controls (first, previous, next, last) and Page Sizer dropdown list
TurboTablesLib.prototype.CreatePagingControls = function (tableId) {

     //Create elements in the paging controls row
     var pagingCtrlNode = document.createElement('div');
     pagingCtrlNode.className = 'row';
     var tablePagerNode = document.createElement('div');
     tablePagerNode.className = 'table-pager';

     //Create table-pager control button group
     var bgColumnNode = document.createElement('div');
     bgColumnNode.className = 'col-sm-7';
     var bgGroupNode = document.createElement('div');
     bgGroupNode.className = 'btn-group';
     var firstButtonNode = document.createElement('button');
     firstButtonNode.className = 'btn btn-default disabled';
     firstButtonNode.innerHTML = 'First';
     firstButtonNode.id = this.firstButtonId;
     firstButtonNode.setAttribute('type', 'button');
     var prevButtonNode = document.createElement('button');
     prevButtonNode.className = 'btn btn-default disabled';
     prevButtonNode.innerHTML = 'Previous';
     prevButtonNode.id = this.prevButtonId;
     prevButtonNode.setAttribute('type', 'button');
     var nextButtonNode = document.createElement('button');
     nextButtonNode.className = 'btn btn-default';
     nextButtonNode.innerHTML = 'Next';
     nextButtonNode.id = this.nextButtonId;
     nextButtonNode.setAttribute('type', 'button');
     nextButtonNode.npLib = this;
     var lastButtonNode = document.createElement('button');
     lastButtonNode.className = 'btn btn-default';
     lastButtonNode.innerHTML = 'Last';
     lastButtonNode.id = this.lastButtonId;
     lastButtonNode.setAttribute('type', 'button');
     bgGroupNode.appendChild(firstButtonNode);
     bgGroupNode.appendChild(prevButtonNode);
     bgGroupNode.appendChild(nextButtonNode);
     bgGroupNode.appendChild(lastButtonNode);
     bgColumnNode.appendChild(bgGroupNode);

     //create table-pager pageSizer
     var sizerColumnNode = document.createElement('div');
     sizerColumnNode.className = 'col-sm-5';
     var pgSizerNode = document.createElement('div');
     pgSizerNode.className = 'table-pageSizer';
     var labelPgSizerNode = document.createElement('label');
     labelPgSizerNode.innerHTML = 'Page size:';
     var selectPgSizerNode = document.createElement('select');
     selectPgSizerNode.className = 'input-sm';
     selectPgSizerNode.id = this.selectPgSizerId;
     selectPgSizerNode.setAttribute('name', 'pageSizer');
     pgSizerNode.appendChild(labelPgSizerNode);
     pgSizerNode.appendChild(selectPgSizerNode);
     sizerColumnNode.appendChild(pgSizerNode);

     tablePagerNode.appendChild(bgColumnNode);
     tablePagerNode.appendChild(sizerColumnNode);
     pagingCtrlNode.appendChild(tablePagerNode);
     var insertPosition = document.getElementById(tableId).parentNode.parentNode;
     insertPosition.parentNode.insertBefore(pagingCtrlNode, insertPosition.nextSibling);

     //Initialize the pageSize dropdown list
     var optionValues = this.pagerSizeOptions[0];
     var optionText = this.pagerSizeOptions[1];
     var idx = 0;
     optionValues.forEach(function (opt) {
          var option = document.createElement('option');
          option.text = optionText[idx];
          option.value = opt;
          selectPgSizerNode.appendChild(option);
          idx++;
     });

     //Hook events for paging controls
     var self = this;
     firstButtonNode.addEventListener('click', function () { self.FirstPage(); });
     prevButtonNode.addEventListener('click', function () { self.PreviousPage(); });
     nextButtonNode.addEventListener('click', function () { self.NextPage(); });
     lastButtonNode.addEventListener('click', function () { self.LastPage(); });

     //Hook event for page size change
     selectPgSizerNode.addEventListener('change', function () { self.UpdatePageSize(); });

};

//Move to first display page
TurboTablesLib.prototype.FirstPage = function () {
     this.page = 1;
     //Start the spinner
     this.showSpinner(true);
     //Call data binding
     this.dataBinder(this.page, this.pageSize, this.sortColumn, this.sortDirection);
};

//Move to previous display page
TurboTablesLib.prototype.PreviousPage = function () {
     if (this.page > 1) {
          this.page = this.page - 1;
          //Start the spinner
          this.showSpinner(true);
          //Call data binding
          this.dataBinder(this.page, this.pageSize, this.sortColumn, this.sortDirection);
    }
};

//Move to next display page
TurboTablesLib.prototype.NextPage = function () {
    var lastPage = 0;

    var remainder = this.totalItems % this.pageSize;
    if (remainder > 0) {
         lastPage = ((this.totalItems - remainder) / this.pageSize) + 1;
    }
    else {
         lastPage = this.totalItems / this.pageSize;
    }

    if (this.page < lastPage) {
         this.page = this.page + 1;
         //Start the spinner
         this.showSpinner(true);
         //call data binding
         this.dataBinder(this.page, this.pageSize, this.sortColumn, this.sortDirection);
    }
};

//Move to last display page
TurboTablesLib.prototype.LastPage = function () {
     var remainder = this.totalItems % this.pageSize;
     if (remainder > 0) {
          this.page = ((this.totalItems - remainder) / this.pageSize) + 1;
     }
     else {
          this.page = this.totalItems / this.pageSize;
     }

     //Start the spinner
     this.showSpinner(true);
     //call data binding
     this.dataBinder(this.page, this.pageSize, this.sortColumn, this.sortDirection);
};

//UpdatePageSize
TurboTablesLib.prototype.UpdatePageSize = function () {
    var result = 1;
    //always reset to page 1 on Page Size change
    this.page = 1;

    var pageSizer = document.getElementById(this.selectPgSizerId);
    this.pageSize = parseInt(pageSizer.options[pageSizer.selectedIndex].value, 10);
    if (this.pageSize === -1)
        this.pageSize = this.totalItems;

    //Start the spinner
    this.showSpinner(true);
    //call data binding
    this.dataBinder(this.page, this.pageSize, this.sortColumn, this.sortDirection);
    this.ShowTableInfo();
};

TurboTablesLib.prototype.UpdatePagerControls = function() {
     var lastPage = 0;

     var remainder = this.totalItems % this.pageSize;
     if (remainder > 0) {
          lastPage = ((this.totalItems - remainder) / this.pageSize) + 1;
     }
     else {
          lastPage = this.totalItems / this.pageSize;
     }

     document.getElementById(this.firstButtonId).className = 'btn btn-default';
     document.getElementById(this.prevButtonId).className = 'btn btn-default';
     document.getElementById(this.nextButtonId).className = 'btn btn-default';
     document.getElementById(this.lastButtonId).className = 'btn btn-default';

     if (this.page === 1) {
          document.getElementById(this.firstButtonId).className = 'btn btn-default disabled';
          document.getElementById(this.prevButtonId).className = 'btn btn-default disabled';
     }
     if (this.page === lastPage) {
          document.getElementById(this.nextButtonId).className = 'btn btn-default disabled';
          document.getElementById(this.lastButtonId).className = 'btn btn-default disabled';
     }
};

//show page number
TurboTablesLib.prototype.ShowTableInfo = function () {

     //Update the Table Info label
     var high = this.pageSize * this.page;
     if (high > this.totalItems)
          high = this.totalItems;
     if (this.page === 1)
          low = 1;
     else
          low = ((this.page - 1) * this.pageSize) + 1;
     var tableInfoText = this.tableInfoTemplate.replace('{0}', low).replace('{1}', high).replace('{2}', this.totalItems);
     document.getElementById(this.tableInfoId).innerHTML = tableInfoText;

     //Update the Pager controls
     this.UpdatePagerControls();
};

TurboTablesLib.prototype.FilterTable = function () {
     var input, filter, tableBody, rows, rowData, rowIdx, dataIdx;
     var found = false;

     input = document.getElementById(this.filterInputId);
     filter = input.value.toUpperCase();
     //really need body reference
     tableBody = document.getElementById(this.tableId).getElementsByTagName('tbody');
     rows = tableBody[0].getElementsByTagName('tr');

     for (var rowIdx = 0; rowIdx < rows.length; rowIdx++) {
          rowData = rows[rowIdx].getElementsByTagName('td');
          for (var dataIdx = 0; dataIdx < rowData.length; dataIdx++) {
               if (rowData[dataIdx].innerHTML.toUpperCase().indexOf(filter) > -1) {
                    found = true;
                    break;
               };
          };

          if (found) {
               rows[rowIdx].style.display = '';
               found = false;
          }
          else {
               rows[rowIdx].style.display = 'none';
          }
     };
};

TurboTablesLib.prototype.ProcessSortEvent = function (id) {
     console.log('Processing sort event for col id: ' + id);
     //Find and restore the text of the last sort column
     var text = document.getElementById(this.sortColumn).firstChild.innerHTML;
     var idx = text.indexOf('<');
     if (idx > 0)
         text = text.substring(0, idx);
     document.getElementById(this.sortColumn).firstChild.innerHTML = text;

     //Find the new sort column, make it the new sort column, update it's text, and change the sort direction
     this.sortColumn = id;
     text = document.getElementById(id).firstChild.innerHTML;
     if (this.sortDirection === 'asc') {
          //document.getElementById(id).innerHTML = document.getElementById(id).innerHTML + '&#8595;' // Unicode 'down' arrow
          document.getElementById(id).firstChild.innerHTML = text + '<i class="fa fa-sort-desc"></i>';
          this.sortDirection = 'desc';
     }
     else {
          //document.getElementById(id).innerHTML = document.getElementById(id).innerHTML + '&#8593; ' // Unicode 'up' arrow
          document.getElementById(id).firstChild.innerHTML = text + '<i class="fa fa-sort-asc" ></i >';
          this.sortDirection = 'asc';
     }

     //Start the spinner
     this.showSpinner(true);
     //call data binding
     this.dataBinder(this.page, this.pageSize, this.sortColumn, this.sortDirection);
};