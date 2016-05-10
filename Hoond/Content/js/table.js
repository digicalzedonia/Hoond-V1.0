$(function tables() {

    $('table').footable();
    //$('table').data('footable').reset();

    $('.sort-column').click(function (e) {
        e.preventDefault();
        var footableSort = $('table').data('footable-sort');
        var index = $(this).data('index');
        footableSort.doSort(index, 'toggle');
    });


    $('table').footable().bind('footable_filtering', function (e) {
        var selected = $('.filter-status').find(':selected').text();
        if (selected && selected.length > 0) {
            e.filter += (e.filter && e.filter.length > 0) ? ' ' + selected : selected;
            e.clear = !e.filter;
        }
    });


    $('.clear-filter').click(function (e) {
        e.preventDefault();
        $('table').trigger('footable_clear_filter');
    });


});

    $(function () {
        var buttonhide1 = document.getElementById('show1');
        var contenthide1 = document.getElementById('hide1');
        var buttonhide2 = document.getElementById('show2');
        var contenthide2 = document.getElementById('hide2');
        

        //buttonhide1.addEventListener('click', function (e) {
        //    console.log(e);
        //    contenthide1.classList.toggle('open');
        //});

        //buttonhide2.addEventListener('click', function (e) {
        //    console.log(e);
        //    contenthide2.classList.toggle('open');
        //});


    }());
