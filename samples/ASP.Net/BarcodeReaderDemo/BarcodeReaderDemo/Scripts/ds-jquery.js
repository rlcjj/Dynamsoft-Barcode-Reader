// JavaScript Document
(function ($) {
    $.fn.clickhide = function (options) {
        //default configuration properities
		var defaults = {
            _toggleSpeed: "normal",
            _needSwitchContent: false,
            _hasSlideArrow: false,
			_slideArrow: false,
			_slideArrowClass: "tab-arrow",
            _clickOrHover: "click",
			_clickToUndo: false,
            _rootObjID: "#subNav",
            _tabNavNode1: ".dwt-add-on",
			_tabNavNode2: ".subNav-ct-more",
            _tabContent: "tabcontent",
            _currentTabClass: "tabOn",
            handler: null
        };
        var options = $.extend(defaults, options);
        this.each(function () {
            $(this).find(options._tabNavNode1).bind(options._clickOrHover, function (e) {
               $(options._tabNavNode2).toggle();
			   if($(options._tabNavNode2).is(':visible')){
				   $(options._tabNavNode1).css({"border-bottom":"solid 3px #fe8e14","color":"#fe8e14"});
				   }else {
				    $(options._tabNavNode1).css({"border-bottom":"","color":""});
					   }
			
        });
		});
	}
})(jQuery);



$(function() {
    /* navigation */   
    var timeouts;		
    $("#overall-nav a.dropdown-toggle").hover(function() {
            var targetToggler = $(this);
            var targetSubNav = $(this).next(".nav-secondary");

            if ($("#overall-nav .active").length == 0) {
                $(".nav-secondary").hide();
                targetSubNav.show();
                targetSubNav.addClass("active");
                targetToggler.addClass("active");
            } else {
                timeouts = setTimeout(function() {
                    $(".nav-secondary").hide();
                    $("#overall-nav .active").removeClass("active");
                    targetSubNav.show();
                    targetSubNav.addClass("active");
                    targetToggler.addClass("active");
                }, 300);
            }
        },
        function() {
            clearTimeout(timeouts);
        });
    $("#overall-nav").mouseleave(function() {
        setTimeout(function() {
            $(".nav-secondary").hide();
            $("#overall-header a").removeClass("active");
        }, 300);
    });
	
	
	/*subNav-add-on*/
	$("#subNav").clickhide({	
		_clickOrHover: "click",
		_tabNavNode1: ".dwt-add-on",
		_tabNavNode2: ".subNav-ct-more",
	});

    //event bubble
	function stopPropagation(e) {
		if (e.stopPropagation)
			e.stopPropagation();
		else
			e.cancelBubble = true;
	}
	$(document).bind('click',function(){
		$('.subNav-ct-more').css('display','none');
		$(".dwt-add-on").css({"border-bottom":"","color":""});
	});

	$('.dwt-add-on').bind('click',function(e){
		stopPropagation(e);
	});
	
	//sunNav--scroll fixed
	//toTop
    $("#toTop").hide();
	$(window).scroll(function(){
		   if ($(window).scrollTop()>90){
				var subNavTop=$("#subNav").css('position');
				if(subNavTop=='relative'){
				$("#subNav").css({"position":"fixed","top":"-100px","box-shadow":"0 0 6px rgba(0, 0, 0, .3)"});
				$("#header").css({"padding-bottom":"60px"});
				$("#subNav").animate({top:0},800);
				}
				$("#toTop").fadeIn();
			}else{
                $("#header").css({"padding-bottom":""});
				$("#subNav").css({"position":"","top":"","box-shadow":""});
				$("#toTop").fadeOut();
			}
		});
	$("#toTop").click(function(){
			$('body,html').animate({scrollTop:0},500);
			return false;		
         });
	
})


// popup div
function popup(id, e) {
	//Cancel the link behavior
    if ( e && e.preventDefault ){
					 e.preventDefault();
                   } else {
					 window.event.returnValue = false;
                   }
	//Get the screen height and width
    var maskHeight = $(document).height();
    var maskWidth = $(window).width();
	//Set heigth and width to mask to fill up the whole screen
    $("#mask").css({
        "width": maskWidth,
        "height": maskHeight
    });
	//Get the window height and width
    var winH = $(window).height();
    var winW = $(window).width();
	//Set the popup window to center
    $(id).css("top", winH / 2 - $(id).height() / 2);
    $(id).css("left", winW / 2 - $(id).width() / 2);
	//transition effect
    $(id).fadeIn(500);
	$("#mask").removeClass("hidden");
}
function resizeWindow() {
    var box = $(".window");
	//Get the window height and width
    var winH = $(window).height();
    var winW = $(window).width();
	//Set the popup window to center
    box.css("top", winH/2 - box.height()/2);
    box.css("left",winW /2 - box.width()/2);
}