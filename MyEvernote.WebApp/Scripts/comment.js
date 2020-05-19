var noteid = -1;
var modalCommentBodyId = "#modal_comment_body";
$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");
        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    });
});

function doComment(btn, e, commentid, spanid) {
    var button = $(btn);
    var mode = button.data("edit-mode");

    if (e == "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.removeClass("fa-edit");
            btnSpan.addClass("fa-ok");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
            $(spanid).focus();
        } else {
            button.data("edit-mode", false);
            button.removeClass("btn-success");
            button.addClass("btn-warning");
            var btnSpan = button.find("span");
            btnSpan.removeClass("fa-ok");
            btnSpan.addClass("fa-edit");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {
                if (data.result) {
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
                } else {
                    aler("Yorum Güncellenemedi...");
                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı");
            });

        }

    } else if (e == "delete_clicked") {
        var dialog_res = confirm("Yorum Silinsin mi?");
        if (!dialog_res) return false;
        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid
        }).done(function (data) {
            if (data.result) {
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            } else {
                alert("Sunucu il bağlantı kurulamadı..!");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamıyor..!");
        });
    } else if (e == "new_clicked") {
        var txt = $("#new_comment_text").val();
        $.ajax({
            method: "POST",
            url: "/Comment/Create/",
            data: { text: txt, "noteid": noteid }
        }).done(function (data) {
            if (data.result) {
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            } else {
                alert("Yorum Eklenemedi..!");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamıyor..!");
        });
    }

}