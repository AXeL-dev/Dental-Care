$(document).ready(function() {
  "use strict";
    var video_data;
    var video_frame;
    var top_bar;
    var nav_bar;
    var secondary_bar;
    //===============Mobile nav Function============
    $('.menu').on('click', function() {
        if ($(window).width() <= 767) {
            $('.navigation').slideToggle('normal');
        }
    })
    $('.navigation>ul> li').on('click', function() {
        if ($(window).width() <= 767) {
            $('.navigation>ul> li').removeClass('on');
            $('.navigation>ul> li ul').slideUp('normal');
            if ($(this).find('>ul').is(':hidden') == true) {
                $(this).addClass('on');
                $(this).find('>ul').slideDown('normal');
            }
        }
    });
	/*$("#testimonial").owlCarousel({
		autoPlay : 5000, //Set AutoPlay to 3 seconds
		items : 1,
		itemsDesktop : [1170, 1],
		itemsDesktopSmall : [1024, 1],
		itemsTabletSmall : [768, 1],
		itemsMobile : [480, 1],
		navigation : false,
		pagination : false,
		transitionStyle : "fade"

	});

	$("#team-carousel").owlCarousel({
		autoPlay : 5000,
		items : 4,
		itemsDesktop : [1170, 3],
		itemsDesktopSmall : [1024, 3],
		itemsTabletSmall : [768, 2],
		itemsMobile : [480, 1],
		navigation : true,
		pagination : false,
		navigationText : ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]
	})
	

	if ($("#services-block").length) {

		$("#services-block").owlCarousel({
			items : 1,
			itemsDesktop : [1199, 1],
			itemsDesktopSmall : [979, 1],
			itemsTablet : [768, 1],
			itemsMobile : [600, 1],
			navigation : true,
			pagination : false,
			navigationText : ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]

		});

	}

	
	if ($("#blog-carousel").length) {

		$("#blog-carousel").owlCarousel({
			items : 1,
			itemsDesktop : [1199, 1],
			itemsDesktopSmall : [979, 1],
			itemsTablet : [768, 1],
			itemsMobile : [600, 1],
			navigation : true,
			pagination : false,
			navigationText : ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"]

		});

	}
	$(".accordion-title").on('click',function() {
		$(this).next().slideToggle("easeOut"), $(this).toggleClass("active"), $("accordion-title").toggleClass("active"), $(".accordion-content").not($(this).next()).slideUp("easeIn"), $(".accordion-title").not($(this)).removeClass("active")
	}), $(".accordion-content").addClass("defualt-hidden");
	//==========================================
	//===============Video Function============
	$('.video-btn').on('click', function() {
		var video_data = $(this).next().attr('data-video');
		var video_frame = $(this).after("<iframe src='' class='video_frame'>  </iframe>");
		$('.video_frame').attr('src', video_data);
		$(this).hide();
	});
	//==========================================
	//===============counter Function========
	if ($('.counter').length) {
		$('.counter').appear(function() {
			$(".counter").each(function() {
				var e = $(this),
				    a = e.attr("data-count");
				$({
					countNum : e.text()
				}).animate({
					countNum : a
				}, {
					duration : 8e3,
					easing : "linear",
					step : function() {
						e.text(Math.floor(this.countNum))
					},
					complete : function() {
						e.text(this.countNum)
					}
				})
			})
		})
	}
    */
	//==========================================
	//===============Datepicker Function========
	if ($('.datepicker').length) {
	    $(".datepicker").datepicker({
	        closeText: 'Fermer',
	        prevText: 'Pr&eacute;c&eacute;dent',
	        nextText: 'Suivant',
	        currentText: 'Aujourd\'hui',
	        monthNames: ['Janvier', 'F&eacute;vrier', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Ao&ucirc;t', 'Septembre', 'Octobre', 'Novembre', 'D&eacute;cembre'],
	        monthNamesShort: ['Janv.', 'F&eacute;vr.', 'Mars', 'Avril', 'Mai', 'Juin', 'Juil.', 'Ao&ucirc;t', 'Sept.', 'Oct.', 'Nov.', 'Déc.'],
	        dayNames: ['Dimanche', 'Lundi', 'Mardi', 'Mercredi', 'Jeudi', 'Vendredi', 'Samedi'],
	        dayNamesShort: ['Dim.', 'Lun.', 'Mar.', 'Mer.', 'Jeu.', 'Ven.', 'Sam.'],
	        dayNamesMin: ['D', 'L', 'M', 'M', 'J', 'V', 'S'],
	        weekHeader: 'Sem.',
	        dateFormat: 'dd/mm/yy'
	    });
	}

    //==========================================
    //===============Datatable Function========
	if ($('.apply-datatable').length) {
	    $.fn.dataTable.ext.errMode = 'throw';
	    $('.apply-datatable').DataTable({
	        "language": {
	            "sProcessing": "Traitement en cours...",
	            "sSearch": "Rechercher&nbsp;:",
	            "sLengthMenu": "Afficher _MENU_ &eacute;l&eacute;ments",
	            "sInfo": "Affichage de l'&eacute;l&eacute;ment _START_ &agrave; _END_ sur _TOTAL_ &eacute;l&eacute;ments",
	            "sInfoEmpty": "Affichage de l'&eacute;l&eacute;ment 0 &agrave; 0 sur 0 &eacute;l&eacute;ment",
	            "sInfoFiltered": "(filtr&eacute; de _MAX_ &eacute;l&eacute;ments au total)",
	            "sInfoPostFix": "",
	            "sLoadingRecords": "Chargement en cours...",
	            "sZeroRecords": "Aucun &eacute;l&eacute;ment &agrave; afficher",
	            "sEmptyTable": "Aucune donn&eacute;e disponible dans le tableau",
	            "oPaginate": {
	                "sFirst": "Premier",
	                "sPrevious": "Pr&eacute;c&eacute;dent",
	                "sNext": "Suivant",
	                "sLast": "Dernier"
	            },
	            "oAria": {
	                "sSortAscending": ": activer pour trier la colonne par ordre croissant",
	                "sSortDescending": ": activer pour trier la colonne par ordre d&eacute;croissant"
	            }
	        }
	    });
	}

    //==========================================
    //===============Fullcalendar Function========
	$('#calendar').fullCalendar({
	    lang: 'fr',
	    editable: false,
	    eventLimit: true, // allow "more" link when too many events
	    events: '/Calendar/GetEvents/',
	    eventRender: function (event, element) {
	        element.attr('title', event.tooltip);
	    }
	});

    /*
	//==========================================
	//===============Fancylight box Function========
	if ($('#gallery').length) {
		$(".fancylight").fancybox({
			openEffect : 'elastic',
			closeEffect : 'elastic',

			helpers : {
				media : {}
			}
		});

	}
	//===============header Function============
	var top_bar = $('#top-bar').height();
	var nav_bar = $('.nav-wrap').height();
	var secondary_bar = $('.secondary-header').height();
	if ($('.header-style').hasClass('fix-header')) {
		$('body').addClass('p-top');
	}
	$(window).scroll(function() {
		if ($('.header-2').hasClass('fix-header')) {

			if ($(window).scrollTop() >= secondary_bar) {
				$('.header-2').addClass('fix');

			} else {
				$('.header-2').removeClass('fix');
			}

		}

		if ($('.header-1').hasClass('fix-header')) {
			if ($(window).scrollTop() >= top_bar) {
				$('.header-1').addClass('fix');
			} else {
				$('.header-1').removeClass('fix');
			}
		}

	});
	$(window).load(function() {
		//===============Loader Function========
		$("#preloader").fadeOut();
		//==========================================
		//===============Doctors filter Function========
		if ($('#isotope').length) {
			// init Isotope
			var $grid = $('.isotope').isotope({
				itemSelector : '.item	',
				percentPosition : true,
				layoutMode : 'fitRows',
				fitRows : {
					gutter : 0
				}
			});
			// filter items on button click
			$('.filter-button-group').on('click', 'a', function() {
				var filterValue = $(this).attr('data-filter');
				$grid.isotope({
					filter : filterValue
				});
				var text_value = $(this).text();
				$('.doctor-specialist span').text(text_value);
			});
		}
	})
    */
    /* Map address pin function*/
    /*
	if ($('#map').length) {
		var map = new GMaps({
			div : '#map',
			lat : 41.402619,
			lng : -74.333062,
			disableDefaultUI : true,
			zoom : 10,
			scrollwheel : false
		});
		map.drawOverlay({
			lat : map.getCenter().lat(),
			lng : map.getCenter().lng(),
			content : '<a href="#" class="mapmarker"><i class="ion-ios-location"></i></a>',
			verticalAlign : 'top',
			horizontalAlign : 'center'
		});
	}
    */

}); 