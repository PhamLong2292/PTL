tinymce.PluginManager.add('imagedesc', function (editor, url) {
    // Add a button that opens a window
    editor.addButton('imagedesc', {
        image: '/tinymce/plugins/imagedesc/imagedesc.png',
        icon: true,
        onclick: function () {
            editor.dom.setAttrib(tinyMCE.activeEditor.dom.select('p'), 'class', 'CssImageDescription');

            // Open window
            //            editor.windowManager.open({
            //                title: 'Chèn mô tả ảnh',
            //                width: 1000,
            //                height: 500,
            //                body: [{
            //                    type: 'container',
            //                    html:
            //                        "<div style=\"margin-bottom:10px\">Nhập mô tả ảnh</div>"+
            //                        "<div><input style=\"border:solid 1px;padding:4px;width:100%\" type=textbox id=txtImageDesc class=\"form-control\"></div>\r\n"
            //                        
            //                }],
            //                
            //                onsubmit: function (e) {
            //                    // Insert content when the window form is submitted
            //                    CallProcessInsertImageComment(editor);
            //                }
            //            });
        }
    });

});

function CallProcessInsertImageComment(editor) {
    var Desc = document.getElementById("txtImageDesc").value.trim();
    if (Desc == "") {
        return;
    }
    var HtmlContent = "<div class=\"CssImageDescription\">" + Desc + "</div><div></div>";
    editor.insertContent(HtmlContent);
}