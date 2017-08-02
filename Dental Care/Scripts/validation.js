jQuery(function($) {"use strict";
	var Site = {

		initialized : false,

		initialize : function() {

			if (this.initialized)
				return;
			this.initialized = true;

			this.build();
			this.validation();
			//this.events();
		},

		build : function() {
		},
		
		validation : function() {
			var bool = true;

			$('#name,#sub,#email,#message,#pwd_confirm,#tel').blur(function() {
				validateForm2(this);
			});

			$('#submit').click(function() {
				var i = 0;
				var x = $('#name').val();

				if (x == null || x == "" || x == "Name") {

					$('#name').addClass('error')
					bool = false;

				} else {
					i++;
					$('#name').removeClass('error');
					name_val = $('#name').val();

				}

				var x = $('#sub').val();

				if (x == null || x == "" || x == "Name") {
					$('#sub').addClass('error')
					bool = false;

				} else {
					i++;
					$('#sub').removeClass('error');
					comp_val = $('#sub').val();

				}

				var x = $('#email').val();

				var atpos = x.indexOf("@");
				var dotpos = x.lastIndexOf(".");
				if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length || x == 'Email') {
					$('#email').addClass('error')
					bool = false;
				} else {

					i++;
					$('#email').removeClass('error');
					email_val = $('#email').val();

				}

				msg_val = $('#message').val();
				//alert(i);

				if (i == 3) {

					bool = true;
				}

				if (!bool) {

					return false;
				}
				/*else {

				    setTimeout(function () {
				        $('#name').val('');
				        $('#email').val('');
				        $('#sub').val('');
				        $('#message').val('');
				        //$('#name,#sub,#email,#message').next().removeClass("focussed");
				        //$('.ch').css('top', 0)
				        //$('#success').find('div').fadeOut();
				        $('#success').fadeIn(500);

				        $('#success').find('div').fadeIn();
				        setTimeout(function () {
				            $('#success').find('div').fadeOut();

				        }, 2500)
				    }, 500);
				
				}*/

			});

			$('#check-fields').click(function () {

                // email
			    var x = $('#email').val();

			    if (x != "" && x != null && !x.match(emailRegex)) {
			        $('#email').addClass('error')
			        return false;
			    } else {
			        $('#email').removeClass('error');
			    }

			    // password confirmation
			    var x = $('#pwd_confirm').val();

			    if (x == $('#pwd').val()) {
			        $('#pwd_confirm').removeClass('error');
			    } else {
			        $('#pwd_confirm').addClass('error');
			        return false;
			    }

			    // tel
			    var x = $('#tel').val();
			    
			    if (x != "" && x != null && !x.match(numericExpression)) {
			        $('#tel').addClass('error')
			        return false;
			    } else {
			        $('#tel').removeClass('error');
			    }

			});

			function validateForm2(abc) {

				if ($(abc).val() != "") {
					$(abc).removeClass('error');

				} else {
					$(abc).addClass('error');

				}
				// email
				if ($(abc).attr('id') == 'email') {
					if (($(abc).val() != "" || $(abc).val() != null) && ($(abc).val().match(emailRegex))) {
						$(abc).removeClass('error');

					} else {
						$(abc).addClass('error');
					}
				}
			    // password confirmation
				if ($(abc).attr('id') == 'pwd_confirm') {
				    if ($(abc).val() == $('#pwd').val()) {
				        $(abc).removeClass('error');

				    } else {
				        $(abc).addClass('error');
				    }
				}
			    // tel
				if ($(abc).attr('id') == 'tel') {
				    
				    if (($(abc).val() == "" || $(abc).val() == null) || ($(abc).val().match(numericExpression))) {
				        $(abc).removeClass('error');
				    } else {
				        $(abc).addClass('error');
				    }
				}

			}

			var name_val = ''
			var email_val = '';

			var msg_val = '';
			var comp_val = '';
			var emailRegex = /^[a-zA-Z0-9._]+[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,4}$/;
			var numericExpression = /^[(]{0,1}[0-9]{3}[)]{0,1}[-\s\.]{0,1}[0-9]{3}[-\s\.]{0,1}[0-9]{4}$/;
		}
	};

	Site.initialize();
})

