$(function () {

    if ($(".like-button").size() > 0) {
        var postClient = $.connection.postHub;

        postClient.client.updateLikeCount = function (post) {
            var counter = $(".like-count");
            $(counter).fadeOut(function () {
                $(this).text(post.LikeCount);
                $(this).fadeIn();
            });
        };

        $(".like-button").on("click", function () {
            var code = $(this).attr("data-id");
            postClient.server.like(code);
        });

        $.connection.hub.start();
    }

});

