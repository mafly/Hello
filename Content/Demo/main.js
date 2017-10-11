$(function() {
    $('ul.jsnav a').bind('click',function(event){
        var $anchor = $(this);
 
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top
        }, 1500,'easeInOutExpo');
        /*
        if you don't want to use the easing effects:
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top
        }, 1000);
        */
        event.preventDefault();
    });
});

$(window).scroll(function(){
  if($(window).scrollTop() > 750){
      $("#follownav").slideDown("fast");
	  $('#follownav').removeClass('hide').addClass('show');
	  
  }
});
$(window).scroll(function(){
  if($(window).scrollTop() < 750){
      $("#follownav").slideUp("fast");
  }
});