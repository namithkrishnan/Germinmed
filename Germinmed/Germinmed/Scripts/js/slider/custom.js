/*
 *
 *		CUSTOM.JS
 *
 */

(function ($) {

    "use strict";


    // DETECT TOUCH DEVICE //
    function is_touch_device() {
        return !!('ontouchstart' in window) || (!!('onmsgesturechange' in window) && !!window.navigator.maxTouchPoints);
    }


    // SHOW/HIDE MOBILE MENU //
    function show_hide_mobile_menu() {

        $("#mobile-menu-button").on("click", function (e) {

            e.preventDefault();

            $("#mobile-menu").slideToggle(300);

        });

    }


    // MOBILE MENU //
    function mobile_menu() {

        if ($(window).width() < 992) {

            if ($("#menu").length > 0) {

                if ($("#mobile-menu").length < 1) {

                    $("#menu").clone().attr({
                        id: "mobile-menu",
                        class: ""
                    }).insertAfter("#header");

                    $("#mobile-menu .megamenu > a").on("click", function (e) {

                        e.preventDefault();

                        $(this).toggleClass("open").next("div").slideToggle(300);

                    });

                    $("#mobile-menu .dropdown > a").on("click", function (e) {

                        e.preventDefault();

                        $(this).toggleClass("open").next("ul").slideToggle(300);

                    });

                }

            }

        } else {

            $("#mobile-menu").hide();

        }

    }


    // STICKY //
    function sticky() {

        var sticky_point = 190;

        $("#header").clone().attr({
            id: "header-sticky",
            class: ""
        }).insertAfter("header");

        $(window).scroll(function () {

            if ($(window).scrollTop() > sticky_point) {
                $("#header-sticky").fadeIn(300).addClass("header-sticky");
                $("#header .menu ul, #header .menu .megamenu-container").css({ "visibility": "hidden" });
            } else {
                $("#header-sticky").fadeOut(100).removeClass("header-sticky");
                $("#header .menu ul, #header .menu .megamenu-container").css({ "visibility": "visible" });
            }

        });

    }


    // HEADER SEARCH //
    function header_search() {

        $(".search a").on("click", function (e) {

            e.preventDefault();

            if (!$(".search a").hasClass("open")) {

                $("#search-form-container").addClass("open-search-form");

            } else {

                $("#search-form-container").removeClass("open-search-form");

            }

            $(window).resize(function () {
                $("#search-form-container").removeClass("open-search-form");
            })

        });

        $("#search-form").after('<a class="search-form-close" href="#">x</a>');

        $("#search-form-container a.search-form-close").on("click", function (event) {

            event.preventDefault();
            $("#search-form-container").removeClass("open-search-form");

        });

    }


    // PROGRESS BARS // 
    function progress_bars() {

        $(".progress .progress-bar:in-viewport").each(function () {

            if (!$(this).hasClass("animated")) {
                $(this).addClass("animated");
                $(this).animate({
                    width: $(this).attr("data-width") + "%"
                }, 2000);
            }

        });

    }


    // CHARTS //
    function pie_chart() {

        if (typeof $.fn.easyPieChart !== 'undefined') {

            $(".pie-chart:in-viewport").each(function () {

                $(this).easyPieChart({
                    animate: 1500,
                    lineCap: "square",
                    lineWidth: $(this).attr("data-line-width"),
                    size: $(this).attr("data-size"),
                    barColor: function (percent) {
                        var gradient_start = "#74ccff";
                        var gradient_stop = "#096ba3";
                        var ctx = this.renderer.getCtx();
                        var canvas = this.renderer.getCanvas();
                        var gradient = ctx.createLinearGradient(0, 0, canvas.width, 0);
                        gradient.addColorStop(0, gradient_start);
                        gradient.addColorStop(0.5, gradient_stop);
                        return gradient;
                    },
                    trackColor: $(this).attr("data-track-color"),
                    scaleColor: "transparent",
                    onStep: function (from, to, percent) {
                        $(this.el).find(".pie-chart-details .value").text(Math.round(percent));
                    }
                });

            });

        }

    }

    // COUNTER //
    function counter() {

        if (typeof $.fn.jQuerySimpleCounter !== 'undefined') {

            $(".counter .counter-value:in-viewport").each(function () {

                if (!$(this).hasClass("animated")) {
                    $(this).addClass("animated");
                    $(this).jQuerySimpleCounter({
                        start: 0,
                        end: $(this).attr("data-value"),
                        duration: 2000
                    });
                }

            });

        }
    }


    // ANIMATE CHARTS //
    function animate_charts() {

        $(".statistics-container .animate-chart:in-viewport").each(function () {

            if (!$(this).hasClass("animated")) {

                $(this).addClass("animated");

                // LINE CHART //
                var data = {
                    labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    datasets: [
						{
						    fillColor: "transparent",
						    strokeColor: "transparent",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
						},
						{
						    fillColor: "transparent",
						    strokeColor: "transparent",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60]
						},
						{
						    fillColor: "transparent",
						    strokeColor: "#74ccff",
						    pointColor: "#fff",
						    pointStrokeColor: "74ccff",
						    data: [5, 10, 7, 15, 12, 16, 11, 13, 10, 2, 20, 30]
						},
						{
						    fillColor: "transparent",
						    strokeColor: "#45bbff",
						    pointColor: "#fff",
						    pointStrokeColor: "#45bbff",
						    data: [10, 20, 22, 38, 30, 40, 28, 22, 51, 45, 55, 50]
						},
						{
						    fillColor: "transparent",
						    strokeColor: "#209fe8",
						    pointColor: "#fff",
						    pointStrokeColor: "#209fe8",
						    data: [20, 25, 32, 28, 35, 23, 33, 48, 31, 25, 10, 20]
						},
						{
						    fillColor: "transparent",
						    strokeColor: "#c7cd57",
						    pointColor: "#fff",
						    pointStrokeColor: "#c7cd57",
						    data: [15, 20, 30, 20, 40, 28, 38, 28, 41, 35, 15, 25]
						}
                    ]
                }

                if ($("#line-chart").length > 0) {

                    var line_chart = new Chart(document.getElementById("line-chart").getContext("2d")).Line(data, {
                        responsive: true,
                        showTooltips: false,
                        bezierCurve: false,
                        pointDotStrokeWidth: 2
                    });

                }

                // FILL LINE CHART //
                var data = {
                    labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    datasets: [
						{
						    fillColor: "transparent",
						    strokeColor: "transparent",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
						},
						{
						    fillColor: "transparent",
						    strokeColor: "transparent",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60]
						},
						{
						    fillColor: "rgba(116, 204, 255, 0.8)",
						    strokeColor: "rgba(116, 204, 255, 0.8)",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [10, 20, 22, 38, 30, 40, 28, 22, 51, 45, 55, 50]
						},
						{
						    fillColor: "rgba(69, 187, 255, 0.8)",
						    strokeColor: "rgba(69, 187, 255, 0.8)",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [20, 25, 32, 28, 35, 23, 33, 48, 31, 25, 10, 20]
						},
						{
						    fillColor: "rgba(199, 205, 87, 0.8)",
						    strokeColor: "rgba(199, 205, 87, 0.8)",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [15, 20, 30, 20, 40, 28, 38, 28, 41, 35, 15, 25]
						},
						{
						    fillColor: "rgba(32, 159, 232, 0.8)",
						    strokeColor: "rgba(32, 159, 232, 0.8)",
						    pointColor: "transparent",
						    pointStrokeColor: "transparent",
						    data: [5, 10, 7, 15, 12, 16, 11, 13, 10, 2, 20, 30]
						}
                    ]
                }

                if ($("#fill-line-chart").length > 0) {

                    var fill_line_chart = new Chart(document.getElementById("fill-line-chart").getContext("2d")).Line(data, {
                        responsive: true,
                        showTooltips: false,
                        bezierCurve: false,
                        pointDotStrokeWidth: 2
                    });

                }

                // BAR CHARTS //
                var data = {
                    labels: [""],
                    datasets: [
						{
						    label: "",
						    fillColor: "transparent",
						    strokeColor: "transparent",
						    highlightFill: "transparent",
						    highlightStroke: "transparent",
						    data: [100]
						},
						{
						    label: "",
						    fillColor: "#74ccff",
						    strokeColor: "#74ccff",
						    highlightFill: "#74ccff",
						    highlightStroke: "#74ccff",
						    data: [50]
						},
						{
						    label: "",
						    fillColor: "#45bbff",
						    strokeColor: "#45bbff",
						    highlightFill: "#45bbff",
						    highlightStroke: "#45bbff",
						    data: [90]
						},
						{
						    label: "",
						    fillColor: "#209fe8",
						    strokeColor: "#209fe8",
						    highlightFill: "#209fe8",
						    highlightStroke: "#209fe8",
						    data: [70]
						},
						{
						    label: "",
						    fillColor: "#c7cd57",
						    strokeColor: "#c7cd57",
						    highlightFill: "#c7cd57",
						    highlightStroke: "#c7cd57",
						    data: [40]
						}
                    ]
                };

                if ($("#bar-chart").length > 0) {

                    var bar_chart = new Chart(document.getElementById("bar-chart").getContext("2d")).Bar(data, {
                        responsive: true,
                        showTooltips: false,
                        barShowStroke: true,
                        barStrokeWidth: 0,
                        barValueSpacing: 0,
                        barDatasetSpacing: 50
                    });

                }

                // PIE CHART
                var data = [
					{
					    value: 15,
					    color: "#74ccff",
					    highlight: "#74ccff",
					    label: "#74ccff"
					},
					{
					    value: 25,
					    color: "#45bbff",
					    highlight: "#45bbff",
					    label: "#45bbff"
					},
					{
					    value: 30,
					    color: "#209fe8",
					    highlight: "#209fe8",
					    label: "#209fe8"
					},
					{
					    value: 30,
					    color: "#c7cd57",
					    highlight: "#c7cd57",
					    label: "#c7cd57"
					}
                ]

                if ($("#circle-chart").length > 0) {

                    var circle_chart = new Chart(document.getElementById("circle-chart").getContext("2d")).Pie(data, {
                        responsive: true,
                        showTooltips: false,
                        animationSteps: 30
                    });

                }

            }

        });

    }


    // LOAD MORE PROJECTS //
    function load_more_projects() {

        var number_clicks = 0;

        $(".load-more").on("click", function (e) {

            e.preventDefault();

            if (number_clicks == 0) {
                $.ajax({
                    type: "POST",
                    url: $(".load-more").attr("href"),
                    dataType: "html",
                    cache: false,
                    msg: '',
                    success: function (msg) {
                        $(".isotope").append(msg);
                        $(".isotope").imagesLoaded(function () {
                            $(".isotope").isotope("reloadItems").isotope();
                            $(".portfolio-item").hoverdir();
                            $(".fancybox").fancybox({
                                prevEffect: 'none',
                                nextEffect: 'none'
                            });
                        });
                        number_clicks++;
                        $(".load-more").html("Nothing to load");
                    }
                });
            }

        });

    }


    // SHOW/HIDE SCROLL UP //
    function show_hide_scroll_top() {

        if ($(window).scrollTop() > $(window).height() / 2) {
            $("#scroll-up").fadeIn(300);
        } else {
            $("#scroll-up").fadeOut(300);
        }

    }


    // SCROLL UP //
    function scroll_up() {

        $("#scroll-up").on("click", function () {
            $("html, body").animate({
                scrollTop: 0
            }, 800);
            return false;
        });

    }


    // CLICKABLE DIV //
    function clickable_div() {

        $(".service-box.style-5").on("click", function () {
            window.location = $(this).find("a").attr("href");
            return false;
        });

    }


    // INSTAGRAM FEED //
    function instagram_feed() {

        if ($('#instafeed').length > 0) {

            var nr = $('#instafeed').data('number');
            var userid = $('#instafeed').data('user');
            var accesstoken = $('#instafeed').data('accesstoken');

            var feed = new Instafeed({
                target: 'instafeed',
                get: 'user',
                userId: userid,
                accessToken: accesstoken,
                limit: nr,
                resolution: 'thumbnail'
            });

            feed.run();

        }

    }


    // MULTILAYER PARALLAX //
    function multilayer_parallax() {

        $(".parallax-multilayer .parallax-layer").each(function () {

            var x = parseInt($(this).attr("data-x"), 10),
				y = parseInt($(this).attr("data-y"), 10);

            $(this).css({
                "left": x + "%",
                "top": y + "%"
            });

        });

    }


    // ANIMATIONS //
    function animations() {

        animations = new WOW({
            boxClass: 'wow',
            animateClass: 'animated',
            offset: 100,
            mobile: false,
            live: true
        });

        animations.init();

    }

    // DOCUMENT READY //
    $(document).ready(function () {

        // STICKY //
        sticky();


        // MENU //
        if (typeof $.fn.superfish !== 'undefined') {

            $(".menu").superfish({
                delay: 500,
                animation: {
                    opacity: 'show',
                    height: 'show'
                },
                speed: 'fast',
                autoArrows: true
            });

        }


        // SHOW/HIDE MOBILE MENU //
        show_hide_mobile_menu();


        // MOBILE MENU //
        mobile_menu();


        // HEADER SEARCH //
        header_search();


        // FANCYBOX //
        if (typeof $.fn.fancybox !== 'undefined') {

            $(".fancybox").fancybox({
                prevEffect: 'none',
                nextEffect: 'none'
            });

        }


        // REVOLUTION SLIDER //
        if (typeof $.fn.revolution !== 'undefined') {

            $(".rev_slider").revolution({
                sliderType: "standard",
                sliderLayout: "auto",
                delay: 9500,
                parallax: {
                    type: "mouse",
                    levels: [5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85],
                    origo: "enterpoint",
                    speed: 400,
                    bgparallax: "false",
                    disable_onmobile: "off"
                },
                navigation: {
                    arrows: {
                        style: "default",
                        enable: true,
                        hide_onmobile: true,
                        hide_onleave: false,
                        hide_delay: 200,
                        hide_delay_mobile: 1200,
                        hide_under: 0,
                        hide_over: 9999,
                        tmp: '',
                        left: {
                            h_align: "left",
                            v_align: "center",
                            h_offset: 20,
                            v_offset: 0,
                        },
                        right: {
                            h_align: "right",
                            v_align: "center",
                            h_offset: 20,
                            v_offset: 0
                        }
                    },
                    bullets: {
                        style: "default",
                        enable: true,
                        hide_onmobile: false,
                        hide_onleave: false,
                        hide_delay: 200,
                        hide_delay_mobile: 1200,
                        hide_under: 0,
                        hide_over: 9999,
                        tmp: '',
                        direction: "horizontal",
                        space: 5,
                        h_align: "center",
                        v_align: "bottom",
                        h_offset: 0,
                        v_offset: 40
                    },
                    touch: {
                        touchenabled: "on",
                        swipe_treshold: 75,
                        swipe_min_touches: 1,
                        drag_block_vertical: false,
                        swipe_direction: "horizontal"
                    }
                },
                gridwidth: 1170,
                gridheight: 560
            });

            $(".rev_slider_parallax").revolution({
                sliderType: "standard",
                sliderLayout: "auto",
                delay: 9000,
                parallax: {
                    type: "mouse",
                    levels: [5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85],
                    origo: "enterpoint",
                    speed: 400,
                    bgparallax: "false",
                    disable_onmobile: "off"
                },
                navigation: {
                    arrows: {
                        enable: false,
                    },
                    bullets: {
                        enable: false,
                    }
                },
                gridwidth: 1170,
                gridheight: 560
            });

        }


        // OWL Carousel //
        if (typeof $.fn.owlCarousel !== 'undefined') {

            /* PORTFOLIO CAROUSEL */
            var portfolio_carousel = $(".owl-carousel.portfolio-carousel").owlCarousel({
                items: 4,
                itemsDesktop: [1199, 4],
                itemsDesktopSmall: [991, 3],
                itemsTablet: [767, 2],
                itemsMobile: [479, 1],
                slideSpeed: 250,
                autoPlay: true,
                stopOnHover: true,
                navigation: false,
                navigationText: false,
                pagination: false,
                paginationNumbers: false,
                mouseDrag: true,
                touchDrag: true,
                transitionStyle: false
            });

            $(".next").on("click", function () {
                portfolio_carousel.trigger('owl.next');
            });

            $(".prev").on("click", function () {
                portfolio_carousel.trigger('owl.prev');
            });


            /* FEATURES SLIDER */
            var features_carousel = $(".owl-carousel.features-slider").owlCarousel({
                singleItem: true,
                slideSpeed: 100,
                autoPlay: true,
                stopOnHover: true,
                navigation: false,
                navigationText: false,
                pagination: false,
                paginationNumbers: false,
                mouseDrag: false,
                touchDrag: true,
                transitionStyle: "fade"
            });

            $(".next").on("click", function () {
                features_carousel.trigger('owl.next');
            });

            $(".prev").on("click", function () {
                features_carousel.trigger('owl.prev');
            });


            /* IMAGES SLIDER */
            $(".owl-carousel.images-slider").owlCarousel({
                singleItem: true,
                slideSpeed: 100,
                autoPlay: true,
                stopOnHover: true,
                navigation: false,
                navigationText: false,
                pagination: true,
                paginationNumbers: false,
                mouseDrag: true,
                touchDrag: true,
                transitionStyle: "fade"
            });


            // LOGOS SLIDER //
            $(".owl-carousel.logos-slider").owlCarousel({
                items: 5,
                itemsDesktop: [1199, 5],
                itemsDesktopSmall: [991, 4],
                itemsTablet: [767, 2],
                itemsMobile: [479, 1],
                slideSpeed: 250,
                autoPlay: true,
                stopOnHover: true,
                navigation: true,
                navigationText: false,
                pagination: false,
                paginationNumbers: false,
                mouseDrag: true,
                touchDrag: true,
                transitionStyle: false
            });


            // TESTIMONIALS SLIDER //
            $(".owl-carousel.testimonials-slider").owlCarousel({
                singleItem: true,
                slideSpeed: 100,
                autoPlay: true,
                stopOnHover: true,
                navigation: false,
                navigationText: false,
                pagination: true,
                paginationNumbers: false,
                mouseDrag: false,
                touchDrag: true,
                transitionStyle: "goDown"
            });


            /* BLOG SLIDER */
            $(".owl-carousel.blog-slider").owlCarousel({
                singleItem: true,
                slideSpeed: 100,
                autoPlay: true,
                stopOnHover: true,
                navigation: false,
                navigationText: false,
                pagination: true,
                paginationNumbers: false,
                mouseDrag: true,
                touchDrag: true,
                transitionStyle: "goDown"
            });

        }


        // GOOGLE MAPS //
        if (typeof $.fn.gmap3 !== 'undefined') {

            $(".map").each(function () {

                var data_zoom = 15;

                if ($(this).attr("data-zoom") !== undefined) {
                    data_zoom = parseInt($(this).attr("data-zoom"), 10);
                }

                $(this).gmap3({
                    marker: {
                        values: [{
                            address: $(this).attr("data-address"),
                            data: $(this).attr("data-address-details")
                        }],
                        options: {
                            draggable: false
                        },
                        events: {
                            click: function (marker, event, context) {
                                var map = $(this).gmap3("get"),
								infowindow = $(this).gmap3({ get: { name: "infowindow" } });
                                if (infowindow) {
                                    infowindow.open(map, marker);
                                    infowindow.setContent(context.data);
                                } else {
                                    $(this).gmap3({
                                        infowindow: {
                                            anchor: marker,
                                            options: { content: context.data }
                                        }
                                    });
                                }
                            }
                        }
                    },
                    map: {
                        options: {
                            mapTypeId: google.maps.MapTypeId.ROADMAP,
                            zoom: data_zoom,
                            scrollwheel: false
                        }
                    }
                });

            });

        }


        // ISOTOPE //
        if ((typeof $.fn.imagesLoaded !== 'undefined') && (typeof $.fn.isotope !== 'undefined')) {

            $(".isotope").imagesLoaded(function () {

                var container = $(".isotope");

                container.isotope({
                    itemSelector: '.isotope-item',
                    layoutMode: 'masonry',
                    transitionDuration: '0.8s'
                });

                $(".filter li a").on("click", function () {
                    $(".filter li a").removeClass("active");
                    $(this).addClass("active");

                    var selector = $(this).attr("data-filter");
                    container.isotope({
                        filter: selector
                    });

                    return false;
                });

                $(window).resize(function () {
                    container.isotope();
                });

            });

            $(".isotope.masonry").imagesLoaded(function () {

                var container_masonry = $(".isotope.masonry");

                container_masonry.isotope({
                    itemSelector: '.isotope-item',
                    layoutMode: 'masonry',
                    masonry: {
                        columnWidth: 1,
                        gutter: 0
                    },
                    transitionDuration: '0.8s'
                });

                $(".filter li a").on("click", function () {
                    $(".filter li a").removeClass("active");
                    $(this).addClass("active");

                    var selector = $(this).attr("data-filter");
                    container_masonry.isotope({
                        filter: selector
                    });

                    return false;
                });

                $(window).resize(function () {
                    container_masonry.isotope();
                });

            });

            $(".isotope.masonry.gutter").imagesLoaded(function () {

                var container_masonry_gutter = $(".isotope.masonry.gutter");

                container_masonry_gutter.isotope({
                    itemSelector: '.isotope-item',
                    layoutMode: 'masonry',
                    masonry: {
                        columnWidth: 1,
                        gutter: 2
                    },
                    transitionDuration: '0.8s'
                });

                $(".filter li a").on("click", function () {
                    $(".filter li a").removeClass("active");
                    $(this).addClass("active");

                    var selector = $(this).attr("data-filter");
                    container_masonry_gutter.isotope({
                        filter: selector
                    });

                    return false;
                });

                $(window).resize(function () {
                    container_masonry_gutter.isotope();
                });

            });

        }


        // LOAD MORE PORTFOLIO ITEMS //
        load_more_projects();


        // PLACEHOLDER //
        if (typeof $.fn.placeholder !== 'undefined') {

            $("input, textarea").placeholder();

        }


        // CONTACT FORM VALIDATE & SUBMIT //
        // VALIDATE //
        if (typeof $.fn.validate !== 'undefined') {

            $("#contact-form").validate({
                rules: {
                    name: {
                        required: true
                    },
                    email: {
                        required: true,
                        email: true
                    },
                    subject: {
                        required: true
                    },
                    message: {
                        required: true,
                        minlength: 10
                    }
                },
                messages: {
                    name: {
                        required: "Please enter your name!"
                    },
                    email: {
                        required: "Please enter your email!",
                        email: "Please enter a valid email address"
                    },
                    subject: {
                        required: "Please enter the subject!"
                    },
                    message: {
                        required: "Please enter your message!"
                    }
                },

                // SUBMIT //
                submitHandler: function (form) {
                    var result;
                    $(form).ajaxSubmit({
                        type: "POST",
                        data: $(form).serialize(),
                        url: "assets/php/send.php",
                        success: function (msg) {

                            if (msg == 'OK') {
                                result = '<div class="alert alert-success">Your message was successfully sent!</div>';
                                $("#contact-form").clearForm();
                            } else {
                                result = msg;
                            }

                            $("#alert-area").html(result);

                        },
                        error: function () {

                            result = '<div class="alert alert-danger">There was an error sending the message!</div>';
                            $("#alert-area").html(result);

                        }
                    });
                }
            });

        }


        // MULTILAYER PARALLAX //
        multilayer_parallax();


        // PARALLX //
        if (typeof $.fn.stellar !== 'undefined') {

            if (!is_touch_device()) {

                $(window).stellar({
                    horizontalScrolling: false,
                    verticalScrolling: true,
                    responsive: true
                });

            }

        }


        // SHOW/HIDE SCROLL UP
        show_hide_scroll_top();


        // SCROLL UP //
        scroll_up();


        // YOUTUBE PLAYER //
        if (typeof $.fn.mb_YTPlayer !== 'undefined') {

            $(".youtube-player").mb_YTPlayer();

        }


        // CLICKABLE DIV //
        clickable_div();


        // HOVERDIR //
        if (typeof $.fn.hoverdir !== 'undefined') {

            $(".portfolio-item").hoverdir();

        }


        // MAGNIFY //
        if (typeof $.fn.magnify !== 'undefined') {

            $(".magnify-img").each(function () {
                $(this).magnify();
            });

        }


        // INSTAGRAM FEED //
        instagram_feed();


        // ANIMATIONS //
        animations();


        // COUNTDOWN //
        if (typeof $.fn.countdown !== 'undefined') {

            $(".countdown").countdown('2016/03/01 22:30', function (event) {
                $(this).html(event.strftime(
					'<div>%-D <span>Days</span></div>' +
					'<div>%-H <span>Hours</span></div>' +
					'<div>%-M <span>Minutes</span></div>' +
					'<div>%S <span>Seconds</span></div>'
				));
            });

        }


    });


    // WINDOW SCROLL //
    $(window).scroll(function () {

        progress_bars();
        pie_chart();
        counter();
        animate_charts();
        show_hide_scroll_top();

    });


    // WINDOW RESIZE //
    $(window).resize(function () {

        mobile_menu();

    });

})(window.jQuery);