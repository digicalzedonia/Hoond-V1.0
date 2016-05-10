  var toggles = document.querySelectorAll(".c-hamburger");
  
  for (var i = toggles.length - 1; i >= 0; i--) {
    var toggle = toggles[i];
    toggleHandler(toggle);
  };

  function toggleHandler(toggle) {
    toggle.addEventListener( "click", function(e) {
      e.preventDefault();
      (this.classList.contains("is-active") === true) ? this.classList.remove("is-active") : this.classList.add("is-active");
    });
  } 

  $(function() {
    var boton = document.getElementById('menu-btn');
    var menu = document.getElementById('menu');

    boton.addEventListener('click', function (e) {
        console.log(e);
        menu.classList.toggle('open');
    });
  });


$(function () {

    var boton = $('.accordion > li > a'),
        submenu = $('.accordion li > .sub-menu');
    boton2 = $('.sub-menu > li > a'),
    submenu2 = $('.submenu2');

    boton.on('click', function (event) {

        if ($(this).attr('class') != 'active') {
            submenu.slideUp('normal');
            boton2.removeClass('active');
            submenu2.slideUp('normal');
            $(this).next().stop(true, true).slideToggle('normal');
            boton.removeClass('active');
            $(this).addClass('active');
        } else {
            $(this).next().stop(true, true).slideToggle('normal');
            boton.removeClass('active');
        };
    });

    boton2.on('click', function (event) {

        if ($(this).attr('class') != 'active') {
            submenu2.slideUp('normal');
            $(this).next().stop(true, true).slideToggle('normal');
            boton2.removeClass('active');
            $(this).addClass('active');
        } else {
            $(this).next().stop(true, true).slideToggle('normal');
            boton2.removeClass('active');
        };
    });

});


