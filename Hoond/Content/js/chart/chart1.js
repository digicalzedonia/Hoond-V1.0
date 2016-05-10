// Load the Visualization API and the corechart package.
//google.charts.load('current', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.
//google.charts.setOnLoadCallback(drawChart);

// Callback that creates and populates a data table,
// instantiates the pie chart, passes in the data and
// draws it.
//function drawChart() {

//     Create the data table.
//    var data = new google.visualization.DataTable();
//    data.addColumn('string', 'Topping');
//    data.addColumn('number', 'Slices');
//    data.addRows([
//      ['Ventas', 3],
//      ['Ganancias', 2],
//      ['Pérdidas', 1],
//      ['Oportunidades', 2],
//    ]);

//     Set chart options
//    var options = {
//        'title': 'Actividades de ventas',
//        titleTextStyle: { bold: true, fontSize: 15, color: '#505259' },
//        'width': 500,
//        'height': 400,
//        colors: ['#6baf6b', '#84c184', '#8bd790', '#ace8ae'],
//        fontName: 'muli',
//        backgroundColor: { fill: 'transparent' }

//    };

//     Instantiate and draw our chart, passing in some options.
//    var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
//    chart.draw(data, options);
//}