

$(document).ready(function() {

			
			var boton = $('.accordion > li > a'),
				submenu = $('.accordion li > .sub-menu');


			boton.on('click', function(event) {
				
				event.preventDefault();


				if ($(this).attr('class') != 'active'){
					submenu.slideUp('normal');
					$(this).next().stop(true,true).slideToggle('normal');
					boton.removeClass('active');
					$(this).addClass('active');
				} 

			});

		});

